﻿using UnityEngine;
using System.Collections;

public class MeteorControl : MonoBehaviour {

	private GameManager gm;
	private Rigidbody rb;
	private float moveSpeed;
	private float rotTime = 1f / 3f;  // denom = num of secs
	private float x,y,v,h,w;
	private float zDepth = 0f;

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = GetComponentInChildren<Rigidbody>();

		x = Random.Range(1f, gm.level_width) - (gm.level_width/2);
		y = Random.Range(1f, gm.level_height) - (gm.level_height/2);

		transform.position = new Vector3(x, y, zDepth);

		h = Random.Range(-1f, 1f);
		v = Random.Range(-1f, 1f);
		w = Random.Range(-1f, 1f);
		moveSpeed = Random.Range(0.1f, 10f);

	}

	void Update () {
		float h0 = h * 360f * Time.deltaTime * rotTime;
		float v0 = v * 360f * Time.deltaTime * rotTime;
		float w0 = w * 360f * Time.deltaTime * rotTime;
		float h1 = h * Time.deltaTime * moveSpeed;
		float v1 = v * Time.deltaTime * moveSpeed;

		transform.Rotate(h0, v0, w0, Space.Self);
		transform.Translate(v1, h1, 0f, Space.World);
	}
}
