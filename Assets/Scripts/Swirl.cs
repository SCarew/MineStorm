﻿using UnityEngine;
using System.Collections;

public class Swirl : MonoBehaviour {

	public Vector3 euler = new Vector3(0f, 0f, 1f);
	private ParticleSystem[] ps;

	public float countdown = 2f;  //time until part systems begin cooldown

	void Start () {
		ps = gameObject.GetComponentsInChildren<ParticleSystem>();
		Invoke("StartVanish", countdown);
	}

	public void StartVanish() {
		if (ps != null) {
			foreach (ParticleSystem ps1 in ps) 
				{ ps1.Stop(); }
		}
		Destroy(gameObject, 3f);
	}

	void Update () {
		transform.Rotate(euler);	
//		if (Input.GetButtonDown("Secondary") == true)   
//			{ startVanish(); }
	}
}
