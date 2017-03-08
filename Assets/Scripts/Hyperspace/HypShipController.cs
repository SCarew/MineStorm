using System.Collections;
using UnityEngine;

public class HypShipController : MonoBehaviour {
	[SerializeField] private GameObject launcher;
	[SerializeField] private GameObject pre_Laser;
	private Transform quad;
	private Transform pre_Effects;

	void Start () {
		quad = GameObject.Find("Background").transform;
		pre_Effects = GameObject.Find("Effects").transform;
	}

	void FireLaser() {
		GameObject go = Instantiate(pre_Laser, launcher.transform.position, transform.rotation, pre_Effects) as GameObject;
		go.name = "HypLaser";

	}

	void Update () {
		if (Input.GetButtonDown("Primary")) {
			FireLaser();
		}
	}
}
