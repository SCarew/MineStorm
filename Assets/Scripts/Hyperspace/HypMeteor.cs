using System.Collections;
using UnityEngine;

public class HypMeteor : MonoBehaviour {

	private Vector3 target = new Vector3(0f, 0f, 0f);
	private Rigidbody rb;
	private Transform parEff;
	private ScoreManager sm;
	private SoundManager aud;
	private HypShipHealth pShipHealth;
	[SerializeField] private GameObject ps_Pieces;

	private float moveSpeed = 1f;
	private float rotTime = 1f / 6f;
	private int value = 50;  //score
	private int health = 1;

	public enum mineSize {big, medium, small};
	public mineSize mySize = mineSize.big;

	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddTorque(new Vector3(Random.Range(-1f, 1f) * 360 * rotTime, Random.Range(-1f, 1f) * 360 * rotTime, Random.Range(-1f, 1f) * 360 * rotTime), ForceMode.Force);
		parEff = GameObject.Find("Effects").transform;
		sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		pShipHealth = GameObject.Find("Hyp_PlayerShip").GetComponent<HypShipHealth>();
	}

	public void SetTarget (Vector3 location, float speed = 0f) {
		target = location;
		if (speed == 0f) 
			{ moveSpeed += Random.Range(0f, moveSpeed); }
		else
			{ moveSpeed = speed; }
	}

	void Explode() {
		if (mySize == mineSize.big) 
			{ sm.AddScore(value * 2); }
		else if (mySize == mineSize.medium) 
			{ sm.AddScore(value); }
		else 
			{ sm.AddScore(value / 2); }

		ParticleSystem ps;
		GameObject go;
		go = Instantiate(ps_Pieces, gameObject.transform.position, Quaternion.identity) as GameObject;
		go.transform.SetParent(parEff);
		ps = go.GetComponent<ParticleSystem>();
		ps.Play();
		aud.PlaySoundImmediate("explosionhypmeteor");
		Destroy(go, ps.main.duration);
		Destroy(gameObject);
	}

	void Update () {
		//transform.Translate(target * moveSpeed * Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
	}

	void OnCollisionEnter (Collision coll) {
		//Debug.Log(coll.gameObject.name + " was hit");
		if (coll.gameObject.name == "HypLaser") {
			Destroy(coll.gameObject);
			health -= 1;
			if (health <= 0) {
				Explode();
			}
		}
		if (coll.gameObject.name == "Hyp_PlayerShip") {
			pShipHealth.TakeDamage();
			Explode();
		}
	}
}
