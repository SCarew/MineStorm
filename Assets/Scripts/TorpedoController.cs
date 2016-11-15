using UnityEngine;
using System.Collections;

public class TorpedoController : MonoBehaviour {

	private float fireSpeed = 10.0f;
	private int damage = 100;
	private Vector3 shipVel;

	void Start () {
		Destroy(gameObject, 2.0f);
		Rigidbody shipRb = GameObject.Find("PlayerShip").GetComponent<Rigidbody>();
		shipVel = shipRb.velocity;

		//Testing   //TODO decide on Torpedo1 or Torpedo2 (in Update)
		if (gameObject.name == "Torpedo1(Clone)") {   //torpedo1 has rigidbody for movement
			Rigidbody rb = GetComponent<Rigidbody>();
			rb.MoveRotation(shipRb.rotation);
			Vector3 f = fireSpeed * transform.up;
			rb.AddForce(f + shipVel, ForceMode.VelocityChange);
			//Debug.Log(shipVel + "+" + f);
		}
		//End Testing
	}
	
	void Update () {
		if (gameObject.name == "Torpedo2(Clone)") {
			transform.position += (shipVel * Time.deltaTime) + (transform.up * Time.deltaTime * fireSpeed);
		}
	}

	public int GetDamage() {
		return damage;
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
