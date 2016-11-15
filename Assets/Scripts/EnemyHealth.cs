using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	private int health;
	private bool isAlive = true;   //fix for multiple collider problem
	private GameManager gm;
	public GameManager.mine myType = GameManager.mine.Meteor;
	public GameObject ps_Pieces;
	public GameObject pre_Torpedo;

	private float fireSpeed = 7f;  //for Electric mines' torps

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();	
	}

	public void SetType(GameManager.mine type) {
		myType = type;
	}

	public void SetHealth(int hp) {
		health = hp;
	}

	public void DamageHealth(int dmg) {
		health -= dmg;
		if (isAlive && health <= 0) {
			KillMeteor();
			isAlive = false;
		}
	}

	private void KillMeteor() {
		int size;
		//TODO this if statement must be removed
		if (GetComponentInChildren<MeteorControl2>() == null) {
			size = GetComponentInChildren<MeteorControl>().GetSize();
		}
		else {
			size = GetComponentInChildren<MeteorControl2>().GetSize();
		}

		gm.AddScore(myType, size);
		if (size == 3) {
			gm.SpawnMeteor(myType, 2, 2, gameObject.transform.position);
			Debug.Log(gameObject.name + " spawning med " + myType);
		} else if (size == 2) {
			gm.SpawnMeteor(myType, 1, 2, gameObject.transform.position);
			Debug.Log(gameObject.name + " spawning sma " + myType);
		}
		ExplodeIntoPieces();
		if (myType == GameManager.mine.Electric)
			{ FireElecTorpedo(); }
		Destroy(gameObject, 0.1f);
	}

	private void ExplodeIntoPieces() {
		ParticleSystem ps;
		GameObject go;
		go = Instantiate(ps_Pieces, gameObject.transform.position, Quaternion.identity) as GameObject;
		ps = go.GetComponent<ParticleSystem>();
		ps.Play();
		Destroy(go, ps.duration);
	}

	private void FireElecTorpedo() {
		if (pre_Torpedo != null) {
			GameObject go;
			Rigidbody rbt;
			Vector3 v3_ship, v3_mine;
			v3_mine = gameObject.transform.position;
			v3_ship = GameObject.FindGameObjectWithTag("Player").transform.position;
			go = Instantiate(pre_Torpedo, v3_mine, Quaternion.identity) as GameObject;
			rbt = go.GetComponent<Rigidbody>();
			Vector3 direction = Vector3.Normalize(v3_ship - v3_mine);
			rbt.AddForce(direction * fireSpeed, ForceMode.VelocityChange);
			Destroy(go, 2f);
		}
	}

}
