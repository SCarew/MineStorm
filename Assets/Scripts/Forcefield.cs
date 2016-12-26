﻿using UnityEngine;
using System.Collections;

public class Forcefield : MonoBehaviour {

	private int damage = 100;  //damage from striking forcefield
	private float startupTime = 0.1f;
	private float currentTime = 0f;
	private Collider childCol;
	private Material[] childRend;
	private float alpha;
	private bool bWarmup = true;
	private bool firstTime = true;
	private Transform pShip;

	void Start () {
		pShip = GameObject.Find("PlayerShip").transform;
		childCol = GetComponentInChildren<MeshCollider>();
		childCol.enabled = false;
		childRend = GetComponentInChildren<MeshRenderer>().materials;
	}

	void Update () {
		if (firstTime) {
			alpha = childRend[0].color.a;
			firstTime = false;
			//Debug.Log(childRend.Length + ": " + alpha);
		}
		if (bWarmup) {
			Color c;
			float dTime = Time.deltaTime;
			currentTime += dTime;
			if (currentTime >= startupTime) {
				bWarmup = false;
				childCol.enabled = true;
				currentTime = startupTime;
				firstTime = true;  //TODO remove this testing line
			}
			c = childRend[0].color;
			c.a = alpha * currentTime / startupTime;
			childRend[0].color = c;
		}
		transform.position = pShip.position;
		transform.rotation = pShip.rotation;
		if (Input.GetButtonUp("Secondary")) {
			childCol.enabled = false;
			GetComponentInChildren<MeshRenderer>(true).enabled = false;
			GetComponentInChildren<ParticleSystem>().Play();
			Destroy(gameObject, 0.7f);
		}
	}

	void OnCollisionEnter(Collision coll) {
		Debug.Log("FF hit " + coll.gameObject.name + "/" + coll.gameObject.tag);
		if (coll.gameObject.tag == "EnemyLaser" || coll.gameObject.tag == "MineLaser") {
			Destroy(coll.gameObject);
		}
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponentInParent<EnemyHealth>().DamageHealth(damage);
		}
		if (coll.gameObject.GetComponent<MeteorControl2>() != null) {
			coll.gameObject.GetComponent<EnemyHealth>().DamageHealth(damage);
		}

	}
}
