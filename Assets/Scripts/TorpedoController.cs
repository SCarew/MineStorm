using UnityEngine;
using System.Collections;

public class TorpedoController : MonoBehaviour {

	private float fireSpeed = 10.0f;
	private int damage = 100;
	private Vector3 shipVel;

	void Start () {
		Destroy(gameObject, 2.0f);
		shipVel = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity;
	}
	
	void Update () {
		transform.position += (shipVel * Time.deltaTime) + (transform.up * Time.deltaTime * fireSpeed);
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
