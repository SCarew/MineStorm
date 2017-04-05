using System.Collections;
using UnityEngine;

public class HypUFO : MonoBehaviour {

	public  enum UFOType {Pink, Gray};
	public  UFOType myType = UFOType.Gray;
	[SerializeField] private GameObject ps_Pieces;
	[SerializeField] private GameObject pre_SmallMeteor;
	private Transform parEff, parMet;
	private SoundManager aud;
	private ScoreManager sm;
	private HypShipHealth pShipHealth;
	private Rigidbody rb;
	private Vector3 myTarget, myGravity;
	private bool bInCorridor = false;
	private bool bNewlySpawned = false;
	private int health = 4;
	private int pinkScore = 225, grayScore = 150;
	private float weaponTime = 0.25f;
	private float maxDistance = 21f;   //for remaining in play

	private Vector3 velocity, gravity;

	void Start () {
		if (myType == UFOType.Gray) { 
			health = 2; 
			weaponTime = 0.5f;
		}
				
		parEff = GameObject.Find("Effects").transform;
		parMet = GameObject.Find("Meteors").transform;
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		pShipHealth = GameObject.Find("Hyp_PlayerShip").GetComponent<HypShipHealth>();
		rb = GetComponent<Rigidbody>();
		bNewlySpawned = true;

		StartCoroutine(FireUpdate());

		velocity = InitialVelocity(2);
		rb.velocity = velocity;
		Debug.Log("init vel = " + velocity);
	}

	void TakeHit() {
		ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
		ps.Play();
	}

	void Explode () {
		Instantiate(ps_Pieces, transform.position, Quaternion.identity, parEff);
		if (myType == UFOType.Pink) {
			aud.PlaySoundVisible("explosionUFO1", transform);
			sm.AddScore(pinkScore);
		} else {
			aud.PlaySoundVisible("explosionUFO2", transform);
			sm.AddScore(grayScore);
		}
		Destroy(gameObject);
	}

	public void NowInCorridor(bool bCorr) {
		bInCorridor = bCorr;
	}

	void FixedUpdate() {
		if (OutOfBounds()) {
			Destroy(gameObject);
			return;
		}
		if (myType == UFOType.Gray && bNewlySpawned) {
			SetGrayTarget();
		}
//		float t = Time.deltaTime;
//		Vector3 v = transform.position - myTarget;
//		v = v * t + 0.5f * t * t * myGravity;
//		transform.position = v;

//		float speed = 2f;
//		Vector3 v = (transform.position - myTarget).normalized * speed;
//		rb.velocity += v * Time.deltaTime;
		velocity = velocity - gravity * Time.deltaTime;
		//Debug.Log("vel=" + velocity);
		rb.velocity = velocity;

		//transform.position = Vector3.MoveTowards(transform.position, myTarget, Time.deltaTime);
	}

	Vector3 InitialVelocity(int spawner = 2) {
		Vector3 v0;
		v0 = transform.position;
		Vector3 v1 = v0;
		v1.x += Random.Range(1, 20);
		v1.y += Random.Range(1, 5);
		v1.z += Random.Range(1, 5);
		Vector3 dir = v1 - v0;
		float range = dir.magnitude;
		dir = dir.normalized;
		Vector3 grav = dir * Random.Range(0.1f, 2.0f);
		Debug.Log("grav=" + grav);
		gravity = grav;
		Vector3 newVel = new Vector3(Mathf.Sqrt(2f * grav.x * (v1.x - v0.x)), Mathf.Sqrt(2f * grav.y * (v1.y - v0.y)), Mathf.Sqrt(2f * grav.z * (v1.z - v0.z)));
		newVel = new Vector3(newVel.x * 2, newVel.y * 2, newVel.z * 2);
		return newVel;
	}

	bool OutOfBounds() {
		if (Mathf.Abs(transform.position.x) > (maxDistance + 1) || Mathf.Abs(transform.position.y) > (maxDistance + 1)) {
			return true;
		} 
		return false;
	}

	void SetGrayTarget() {
		bNewlySpawned = false;
		Vector3 v3 = transform.position;
		float x, y, z;
		if (v3.x <= 0f && v3.y <= 0f) {
			x = Random.Range(v3.x, maxDistance);
			y = Random.Range(v3.y, maxDistance);
		}
		else if (v3.x > 0f && v3.y <= 0f) {
			x = Random.Range(-maxDistance, v3.x);
			y = Random.Range(v3.y, maxDistance);
		}
		else if (v3.x <= 0f && v3.y > 0f) {
			x = Random.Range(v3.x, maxDistance);
			y = Random.Range(-maxDistance, v3.y);
		}
		else {   //x>0 && y>0
			x = Random.Range(-maxDistance, v3.x);
			y = Random.Range(-maxDistance, v3.y);
		}
		z = Random.Range(1f, 37f);
		myTarget = new Vector3(x, y, z);
		x = Random.Range(-5f, 5f);
		y = Random.Range(-5f, 5f);
		z = Random.Range(-5f, 5f);
		myGravity = new Vector3(x, y, z);
	}

	private IEnumerator FireUpdate() {
		bool bLoop = true;
		while (bLoop) {
			if (bInCorridor) {
				Instantiate(pre_SmallMeteor, transform.position, transform.rotation, parMet);
			}
			yield return new WaitForSeconds(weaponTime);
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.name == "HypLaser") {
			Destroy(coll.gameObject);
			health -= 1;
			if (health <= 0) {
				Explode();
			} else {
				TakeHit();
			}
		}
		if (coll.gameObject.name == "Hyp_PlayerShip") {
			pShipHealth.TakeDamage();
			Explode();
		}
	}

}
