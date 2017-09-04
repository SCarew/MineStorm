﻿//used on Main screen
using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

	public float parallax = 6.0f;
	private Material mat;
	private Transform trans;

	void Start () {
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mat = mr.material;
		trans = transform.parent.transform;   //camera transform

		Invoke("ResetMaterial", 0.5f);
	}

	void ResetMaterial() {    //fixes bug with SectorDisplay.Start()
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mat = mr.material;
	}

	void Update () {
		Vector2 offset = mat.mainTextureOffset;
		offset.x = trans.position.x / transform.localScale.x / parallax;
		offset.y = trans.position.y / transform.localScale.y / parallax;
		mat.mainTextureOffset = offset;
	}
}
