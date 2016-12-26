using UnityEngine;
using System.Collections;

public class FireCollision : MonoBehaviour {

	private ShipHealth sh;
	private int damage;
	public GameObject pre_ElecExplosion;
	static private Transform parEff;

	void Start() {
		sh = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<ShipHealth>();
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (parEff == null) 
			{ parEff = GameObject.Find("Effects").transform; }

		if (gameObject.tag == "MineLaser") {
			damage = gm.mineFire;
		} else {
			damage = gm.enemyFireT;
		}
	}

	void OnCollisionEnter(Collision coll) {
		//Debug.Log(coll.gameObject.name + " hit for " + damage);
	
		if (coll.gameObject.tag == "Player") {
			sh.DamageHealth(damage);
			Debug.Log(gameObject.name + " hit for " + damage + " by " + coll.gameObject.name);
			Destroy(gameObject);
		}
		if (coll.gameObject.tag == "Laser" || coll.gameObject.tag == "EnemyLaser") {
			Debug.Log(gameObject.name + " hit for " + damage + " by " + coll.gameObject.name);
			Destroy(coll.gameObject);
			Destroy(gameObject);
		}
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<EnemyHealth>().DamageHealth(damage);
			Destroy(gameObject);
		}
	}

	void OnDestroy() {
		GameObject go = Instantiate(pre_ElecExplosion, transform.position, Quaternion.identity) as GameObject;
		go.transform.SetParent(parEff);
		Destroy(go, 2.0f);
	}
}
