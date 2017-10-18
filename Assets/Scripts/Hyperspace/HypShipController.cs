﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HypShipController : MonoBehaviour {
	[SerializeField] private GameObject launcher;
	[SerializeField] private GameObject pre_Laser;
	[SerializeField] private GameObject pre_panCancel;
	private Camera cam;
	public  float targetMoveSpeed = 15f;
	private RectTransform targetObj;
	private RectTransform instObj;
	//private Transform quad;
	private Transform pre_Effects;
	private PrefsControl prefs;
	private Vector2 target = new Vector2(0f, 0f);
	private bool bPaused = false;
	private bool bInvertY = false;
	private bool bFreeze = false;    //for end of turn
	private float max_x, max_y;
	private float original_x, original_y, original_z;

	private float deadZone = 0.25f;
	private float laserRechargeRate = 1f;
	private float laserCurrentCharge = 0f;  
	private float return_x = 0.9f, return_y = 0.6f;

	void Start () {
		//quad = GameObject.Find("Background").transform;
		pre_Effects = GameObject.Find("Effects").transform;
		targetObj = GameObject.Find("Crosshair").GetComponent<RectTransform>();
		instObj = GameObject.Find("Instruments").GetComponent<RectTransform>();
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		max_x = 750f;   //X = 1600 / 2
		max_y = 410f;   //Y =  900 / 2
		laserCurrentCharge = laserRechargeRate;
		original_x = transform.position.x;
		original_y = transform.position.y;
		original_z = transform.position.z;

		bInvertY = prefs.GetHyperY();
	}

	void FireLaser() {
		GameObject go = Instantiate(pre_Laser, launcher.transform.position, transform.rotation, pre_Effects) as GameObject;
		go.name = "HypLaser";

	}

	void TurnRight() {
		target.x += targetMoveSpeed;
	}

	void TurnLeft() {
		target.x -= targetMoveSpeed;
	}

	void TurnUp() {
		target.y += targetMoveSpeed;
	}

	void TurnDown() {
		target.y -= targetMoveSpeed;
	}

	void ReturnXY() {   //moves target back toward center
		if (target.x != 0) 
			{ target.x = Mathf.MoveTowards(target.x, original_x, return_x); }
		if (target.y != 0) 
			{ target.y = Mathf.MoveTowards(target.y, original_y, return_y); }
		
	}

	public void EnterWarp() {
		bFreeze = true;
		GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySoundImmediate("EngineBurst");
		gameObject.transform.Find("Main Camera").SetParent(pre_Effects);
		gameObject.GetComponentInChildren<MeshCollider>().enabled = false;
		GameObject.Find("Instruments").SetActive(false);
		GameObject.Find("Crosshair").SetActive(false);
		gameObject.transform.Find("PS_EngineFireL").GetComponent<ParticleSystem>().Play();
		gameObject.transform.Find("PS_EngineFireR").GetComponent<ParticleSystem>().Play();
		Rigidbody rb = gameObject.GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.None;
		rb.AddRelativeForce(0f, 0f, 100f, ForceMode.Impulse);
		rb.AddRelativeTorque(0f, 0f, Random.Range(-0.5f, 0.5f), ForceMode.Impulse);
	}

	void Update () {
		if (bPaused || bFreeze) { return; }

		laserCurrentCharge += Time.deltaTime;
		if (laserCurrentCharge > laserRechargeRate)  { laserCurrentCharge = laserRechargeRate; }

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		if (bInvertY) { v = -v; }
		//bool bS = Input.GetButtonDown("Secondary");
		bool bP = Input.GetButtonDown("Primary");

		if (bP && (laserCurrentCharge >= laserRechargeRate)) {
			laserCurrentCharge = 0f;		
			FireLaser(); 
		}
		bool bMovement = false;
		if (h > deadZone) { TurnRight(); bMovement = true; }
		if (h < -deadZone) { TurnLeft(); bMovement = true; }
		if (v > deadZone) { TurnUp(); bMovement = true; }
		if (v < -deadZone) { TurnDown(); bMovement = true; }
		if (!bMovement) { ReturnXY(); }

		if (target.x > max_x) { target.x = max_x; }
		if (target.x < -max_x) { target.x = -max_x; }
		if (target.y > max_y) { target.y = max_y; }
		if (target.y < -max_y) { target.y = -max_y; }

		targetObj.anchoredPosition = target;
		float x = target.x / max_x;
		float y = target.y / max_y;
		transform.rotation = Quaternion.Euler(y * -7f, x * 30f, 0f);
		//cam.transform.rotation = Quaternion.Euler(y * 2f, x * -6f, 0f);
		cam.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		transform.position = new Vector3(original_x + x * 10f, original_y + y * 8f, original_z);
		instObj.rotation = Quaternion.Euler(0f, 0f, x * -10f);
		instObj.anchoredPosition = new Vector2(0 + x * 50f, 150 + y * 20f);
		targetObj.rotation = Quaternion.Euler(0f, 0f, x * -10f);

		if (Input.GetButtonDown("Cancel")) {
			if (!GameObject.Find("panCancel") && !prefs.bHypGameOver) {
				Transform t_Can = GameObject.Find("Canvas").transform;
				GameObject go = Instantiate(pre_panCancel, t_Can) as GameObject;
				go.name = "panCancel";
				go.GetComponent<RectTransform>().sizeDelta = new Vector2(1600f, 200f);
			}
		}

	}
}
