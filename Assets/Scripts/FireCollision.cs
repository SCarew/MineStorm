using UnityEngine;
using System.Collections;

public class FireCollision : MonoBehaviour {

	private ShipHealth sh;
	private int damage;

	void Start() {
		sh = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<ShipHealth>();
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (gameObject.tag == "MineLaser") {
			damage = gm.mineFire;
		} else {
			damage = gm.enemyFire;
		}
	}

	void OnCollisionEnter(Collision coll) {
		//Debug.Log(coll.gameObject.name + " hit for " + damage);
	
		if (coll.gameObject.tag == "Player") {
			sh.DamageHealth(damage);
			Debug.Log(gameObject.name + " hit for " + damage + " by " + coll.gameObject.name);
			Destroy(gameObject);
		}
		if (coll.gameObject.tag == "Laser") {
			Debug.Log(gameObject.name + " hit for " + damage + " by " + coll.gameObject.name);
			Destroy(coll.gameObject);
			Destroy(gameObject);
		}
	}

}
