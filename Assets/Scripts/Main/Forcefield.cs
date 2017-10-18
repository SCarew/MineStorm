using UnityEngine;
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
	private bool bTerminateField = false;    //used for ending field on ship blowing up
	private bool bFieldDropped = false;      //true when forcefield dropped
	private Transform pShip;
	private ShipController sc;
	private ShipHealth sh;
	private SoundManager aud;
	private int conLayout = 0;

	void Start () {
		pShip = GameObject.Find("PlayerShip").transform;
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		sc = pShip.gameObject.GetComponent<ShipController>();
		sh = pShip.gameObject.GetComponent<ShipHealth>();
		childCol = GetComponentInChildren<MeshCollider>();
		childCol.enabled = false;
		childRend = GetComponentInChildren<MeshRenderer>().materials;
		aud.PlaySoundImmediate("forcefield");
		conLayout = sc.GetConLayout();
	}

	void Update () {
		if (firstTime) {
			alpha = childRend[0].color.a;
			firstTime = false;
		}
		if (bWarmup) {
			Color c;
			float dTime = Time.deltaTime;
			currentTime += dTime;
			if (currentTime >= startupTime) {
				bWarmup = false;
				childCol.enabled = true;
				currentTime = startupTime;
				//firstTime = true;   //was here to test
			}
			c = childRend[0].color;
			c.a = alpha * currentTime / startupTime;
			childRend[0].color = c;
		}
		transform.position = pShip.position;
		transform.rotation = pShip.rotation;

		bool bS = true;
		if (conLayout == 0 || conLayout == 2) {
			bS = !Input.GetButtonUp("Secondary");
		} else if (conLayout == 1) {
			if (Input.GetAxis("Vertical") < -0.25f) {
				bS = true;
			} else {
				bS = false;
			}
		}
		if (bS == false) { conLayout = 3; }       //to fix forcefield dismissal bug
		if (bFieldDropped) { return; }
		if (!bS || bTerminateField) {
			childCol.enabled = false;
			aud.PlaySoundImmediate("forcefieldoff");
			GetComponentInChildren<MeshRenderer>(true).enabled = false;
			transform.Find("PS_Forcefield1").GetComponent<ParticleSystem>().Play();
			Destroy(gameObject, 0.7f);
			bTerminateField = false;
			bFieldDropped = true;
			sc.priCurrentCharge = sc.priRechargeRate - ((sc.priRechargeRate + 2.5f) * sc.upForCon);
		} else {
			sc.secCurrentCharge -= Time.deltaTime * ((sc.secRechargeRate / 1.5f) / sc.upForDur);
			if (sc.secCurrentCharge <= 0f && sh.GetHealth() > 0) {
				sh.DamageHealth(1);   //start damage if over charge
			}
		}

		if (sh.GetHealth() <= 0) { bTerminateField = true; }
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag == "EnemyLaser" || coll.gameObject.tag == "MineLaser") {
			Destroy(coll.gameObject);
		}
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponentInParent<EnemyHealth>().DamageHealth(damage);
		}
		if (coll.gameObject.GetComponent<MeteorControl>() != null) {
			coll.gameObject.GetComponent<EnemyHealth>().DamageHealth(damage);
		}

	}
}
