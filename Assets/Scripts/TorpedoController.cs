using UnityEngine;
using System.Collections;

public class TorpedoController : MonoBehaviour {

	[SerializeField] private float fireSpeed = 10.0f;
	[SerializeField] private int damage = 100;
	public float lifetime = 2.0f;
	[SerializeField] private GameObject pre_TorpExplosion;
	private Vector3 shipVel;
	private Rigidbody rb;
	private float lifeSpent = 0f;
	private bool bMissile = false;
	private Quaternion rot;
	private float missVel;

	void Start () {
		Destroy(gameObject, lifetime);
		Rigidbody shipRb = GameObject.Find("PlayerShip").GetComponent<Rigidbody>();
		rb = GetComponent<Rigidbody>();
		shipVel = shipRb.velocity;

		if (gameObject.name == "Torpedo" || gameObject.name == "Laser" || gameObject.name == "Missile") {   //torpedo1 has rigidbody for movement
			rb.MoveRotation(shipRb.rotation);
			Vector3 f = fireSpeed * transform.up;
			rb.AddForce(f + shipVel, ForceMode.VelocityChange);
			//Debug.Log(shipVel + "+" + f);
		}

		if (gameObject.name == "Missile") {
			StartCoroutine(Drift());
		}
	}

	IEnumerator Drift() {
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

	void AlterCourse() {
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

		//Debug.Log(transform.localRotation.eulerAngles + " -> " + v);
		//rb.AddForce(transform.up * fireSpeed / 2, ForceMode.Force);
	}

	void FixedUpdate() {
		if (bMissile) {
			transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, 3f*Time.deltaTime);
			rb.AddForce((transform.up * fireSpeed - rb.velocity) * Time.fixedDeltaTime, ForceMode.Impulse);
//			rb.AddForce(transform.up * fireSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

			if ((missVel + 2f) < rb.velocity.sqrMagnitude) {
				rb.velocity = rb.velocity.normalized * Mathf.Sqrt(missVel);
				Debug.Log("*" + missVel);
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

//	void Update () {
//		if (gameObject.name == "Torpedo2(Clone)") {
//			transform.position += (shipVel * Time.deltaTime) + (transform.up * Time.deltaTime * fireSpeed);
//		}
//	}

	public int GetDamage() {
		return damage;
	}

	void OnDestroy() {
//		Vector3 pos = transform.position + 0.5f * (transform.position.normalized);
		Vector3 pos = transform.position - 0.4f * (rb.velocity.normalized);
		//Debug.Log(transform.position + " -> " + pos);
		GameObject go = Instantiate(pre_TorpExplosion, pos, Quaternion.identity) as GameObject;
		Destroy (go, 2.0f);
	}

//	void OnCollisionEnter(Collision coll) {
//		Debug.Log(coll.gameObject.name + " hit for " + damage);
//	
//		if (coll.gameObject.tag == "Meteor") {
//			coll.gameObject.GetComponent<EnemyHealth>().DamageHealth(damage);
//			Debug.Log(coll.gameObject.name + " hit for " + damage);
//		}
//		if (coll.gameObject.tag == "Enemy") {
//			coll.gameObject.GetComponent<EnemyHealth>().DamageHealth(damage);
//			Debug.Log("Enemy " + coll.gameObject.name + " hit for " + damage);
//		}
//		Destroy(gameObject);
//	}

}
