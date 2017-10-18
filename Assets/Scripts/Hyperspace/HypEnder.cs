using System.Collections;
using UnityEngine;

public class HypEnder : MonoBehaviour {

	void OnTriggerEnter(Collider obj) {
		if (obj.tag == "Meteor") {
			Destroy(obj.transform.parent.gameObject);
		} else if (obj.tag == "MeteorParent") {
			Destroy(obj);
		}
	}
}
