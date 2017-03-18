using System.Collections;
using UnityEngine;

public class HypUFO : MonoBehaviour {

	public  enum UFOType {Pink, Gray};
	public  UFOType myType = UFOType.Gray;

	private int health = 4;

	void Start () {
		if (myType == UFOType.Gray) { health = 2; }	
	}

	void TakeHit() {
		ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
		ps.Play();
	}

	void Explode () {
		
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
	}
}
