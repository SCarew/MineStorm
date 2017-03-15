using System.Collections;
using UnityEngine;

public class HypEnder : MonoBehaviour {

	void OnTriggerEnter(Collider obj) {
		//Debug.Log("Collided with " + obj.tag);
		if (obj.tag == "Meteor") {
			Destroy(obj.transform.parent.gameObject);
		} else if (obj.tag == "MeteorParent") {
			Destroy(obj);
		}
	}
}
