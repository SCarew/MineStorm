﻿using System.Collections;
using UnityEngine;

public class CorridorControl : MonoBehaviour {

	void OnTriggerEnter(Collider coll) {
		Debug.Log(coll.gameObject.name + " entered");
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<HypUFO>().NowInCorridor(true);
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<HypUFO>().NowInCorridor(false);
		}
	}
}
