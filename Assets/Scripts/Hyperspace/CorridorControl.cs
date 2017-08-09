using System.Collections;
using UnityEngine;

public class CorridorControl : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {
		//Debug.Log(coll.gameObject.name + " entered");
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponentInParent<HypUFO>().NowInCorridor(true);
		}
	} 

	void OnTriggerExit(Collider coll) {
		//Debug.Log(coll.gameObject.name + " exited");
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponentInParent<HypUFO>().NowInCorridor(false);
		}
	}
}

//Changed corridor from 24X21 to 30X30 on Aug 7 '17
//Also changed MaxDistance in HypUFO.cs from 25 to 30