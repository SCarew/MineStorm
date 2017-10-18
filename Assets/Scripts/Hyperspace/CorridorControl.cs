using System.Collections;
using UnityEngine;

public class CorridorControl : MonoBehaviour {

	public GameObject ps_SmokeExplosion;
	private Transform parEffects;

	void Start() {
		parEffects = GameObject.Find("Effects").transform;
	}

	void MakeSmoke(Vector3 loc) {
		GameObject go;
		go = Instantiate(ps_SmokeExplosion, loc + new Vector3(0f, 0f, -2.0f), Quaternion.identity, parEffects) as GameObject;
		Destroy(go, 1f);
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponentInParent<HypUFO>().NowInCorridor(true);
		}
		if (coll.gameObject.tag == "Meteor") {
			MakeSmoke(coll.transform.position);
		}
	} 

	void OnTriggerExit(Collider coll) {
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponentInParent<HypUFO>().NowInCorridor(false);
		}
	}
}

//Changed corridor from 24X21 to 30X30 on Aug 7 '17
//Also changed MaxDistance in HypUFO.cs from 25 to 30