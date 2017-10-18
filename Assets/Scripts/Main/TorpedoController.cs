using UnityEngine;
using System.Collections;

public class TorpedoController : MonoBehaviour {

	public float fireSpeed = 10.0f;
	public int damage = 100;
	public float lifetime = 2.0f;
	[SerializeField] private GameObject pre_Explosion;
	[SerializeField] private GameObject pre_LaserTimedExplosion;
	private Vector3 shipVel;
	private Rigidbody rb;
	private float lifeSpent = 0f;
	private bool bMissile = false;
	private Quaternion rot;
	private float missVel;
	static private Transform parEff;  //for empty parent container
	static private GameManager gm;
	static private SoundManager aud;

	void Start () {
		Destroy(gameObject, lifetime);
		Rigidbody shipRb = GameObject.Find("PlayerShip").GetComponent<Rigidbody>();
		rb = GetComponent<Rigidbody>();
		shipVel = shipRb.velocity;
		if (gm == null) 
			{ gm = GameObject.Find("GameManager").GetComponent<GameManager>(); }
		if (parEff == null) 
			{ parEff = GameObject.Find("Effects").transform; }
		if (!aud) 
			{ aud = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }

		if (gameObject.name == "Torpedo" || gameObject.name == "Laser" || gameObject.name == "Missile") {   //torpedo1 has rigidbody for movement
			rb.MoveRotation(shipRb.rotation);
			Vector3 f = fireSpeed * transform.up;
			rb.AddForce(f + shipVel, ForceMode.VelocityChange);
		}

		if (gameObject.name == "UFOLaser" || gameObject.name == "UFOTorp") {
			//rb.rotation = Quaternion.LookRotation(shipRb.transform.position - transform.position);
			Vector3 f = fireSpeed * transform.up;
			rb.AddForce(f, ForceMode.VelocityChange);
		}

		if (gameObject.name == "Missile") {
			StartCoroutine(Drift());
		} else {     //sound for missile played from ShipController
			if (gameObject.name == "Laser" || gameObject.name == "Torpedo") {
				aud.PlaySoundImmediate(gameObject.name);   
			} else {    //UFOLaser and UFOTorp
				aud.PlaySoundVisible(gameObject.name, gameObject.transform);
			}
		}
	}

	IEnumerator Drift() {        //for missiles
		bool bLoop = true;
		while(bLoop) {
			yield return new WaitForSeconds(0.2f);
			if ((Random.Range(0f, 100f)) < (100 * lifeSpent/lifetime)) {
				missVel = rb.velocity.sqrMagnitude;
				AlterCourse();
				lifeSpent = 0f;
				bMissile = true;
			}
		}
	}

	void AlterCourse() {         //for missiles
//		Vector3 v1 = rb.velocity.normalized;
//		float v2 = rb.velocity.magnitude;
//		v1 = v1 + new Vector3(0.1f,0.1f,0.0f);
//		v1 = v1 * v2;
//		rb.velocity = v1;
//		rot = Quaternion.LookRotation(v1);
//		transform.rotation.SetLookRotation(transform.position);
//		bMissile = false;
//		return;

		float mag = 40f;
		Vector3 v = transform.localRotation.eulerAngles + (new Vector3(0f, 0f, 1f) * Random.Range(-mag, mag));
		rot = Quaternion.Euler(v);
		//transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, 3f*Time.deltaTime);
		//rb.AddForce((transform.up * fireSpeed) - rb.velocity, ForceMode.Impulse);

		//rb.AddForce(transform.up * fireSpeed / 2, ForceMode.Force);
	}

	void FixedUpdate() {
		if (bMissile) {
			transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, 3f*Time.deltaTime);
			rb.AddForce((transform.up * fireSpeed - rb.velocity) * Time.fixedDeltaTime, ForceMode.Impulse);
//			rb.AddForce(transform.up * fireSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

			if ((missVel + 2f) < rb.velocity.sqrMagnitude) {
				rb.velocity = rb.velocity.normalized * Mathf.Sqrt(missVel);
			}

			//transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.1f);
			//rb.AddForceAtPosition(transform.up * fireSpeed * 0.2f, new Vector3(transform.position.x + rb.velocity.normalized.x,transform.position.y + rb.velocity.normalized.y,0f), ForceMode.Force); 
			//rb.AddForceAtPosition(transform.up * fireSpeed * 0.2f, new Vector3(transform.position.x + Random.Range(-1f, 1f),transform.position.y + Random.Range(-1f, 1f),0f), ForceMode.Force); 
			//rb.AddForce(transform.up * fireSpeed * 0.02f, ForceMode.VelocityChange);
		}
	}

	void Update () {
		lifeSpent += Time.deltaTime;
	}

	public int GetDamage() {
		return damage;
	}

	Quaternion MakeInverse(Quaternion q) {
		Vector3 v = q.eulerAngles;
		if (v.z >= 180f) 
			{ v.z -= 180f; }
		else
			{ v.z += 180f; }
		return Quaternion.Euler(v);
	}

	void OnDestroy() {
		if (gm.bGameOver) { return; }
		Vector3 pos = transform.position;
		if (gameObject.name != "Missile") 
			{ pos -= 0.4f * (rb.velocity.normalized); }  //for correcting explosion location
		GameObject go;
		if (((gameObject.name == "Laser") || (gameObject.name == "UFOLaser")) && ((lifetime - 0.015f) <= lifeSpent)) {
			go = Instantiate(pre_LaserTimedExplosion, pos, Quaternion.identity) as GameObject;
		} else {  //for everything but timed laser exp
			go = Instantiate(pre_Explosion, pos, Quaternion.identity) as GameObject;
		}
		go.transform.SetParent(parEff);
		if ((gameObject.name == "Laser") || (gameObject.name == "UFOLaser"))
			{ go.transform.rotation = MakeInverse(transform.rotation); }
		Destroy (go, 2.0f);

		if (gameObject.name == "Missile") { aud.PlaySoundVisible("expMissile", gameObject.transform); }
		else if (gameObject.name == "Laser") { aud.PlaySoundVisible("expLaser", gameObject.transform); }
		else if (gameObject.name == "Torpedo") { aud.PlaySoundVisible("expTorpedo", gameObject.transform); }
		else if (gameObject.name == "UFOLaser") { aud.PlaySoundVisible("expUFOLaser", gameObject.transform); }
		else if (gameObject.name == "UFOTorp") { aud.PlaySoundVisible("expUFOTorp", gameObject.transform); }
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "EnemyLaser" && gameObject.tag == "Laser") {
			Destroy(coll.gameObject);
			Destroy(gameObject);
		}

		if (coll.gameObject.tag == "Player" && gameObject.name == "UFOLaser") {
			coll.gameObject.GetComponent<ShipHealth>().DamageHealth(gm.enemyFireL);
			Destroy(gameObject);
		}

		if (coll.gameObject.tag == "Player" && gameObject.name == "UFOTorp") {
			coll.gameObject.GetComponent<ShipHealth>().DamageHealth(gm.enemyFireT);
			Destroy(gameObject);
		}
	}
}
