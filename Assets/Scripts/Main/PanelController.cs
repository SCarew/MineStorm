﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {

	private Slider sPrimaryWeapon, sSecondaryWeapon, sHealth, sEngine, sLifeSupport;
	private GameObject panLifeSupport, panGauges, panMapCam;
	private ShipController sc;
	private ShipHealth sh;
	[SerializeField] private Image fillPrimaryWeapon;
	[SerializeField] private Image fillSecondaryWeapon;
	[SerializeField] private Image fillHealth;
	[SerializeField] private Image fillEngine;
	[SerializeField] private Image fillLifeSupport;
	[SerializeField] private GameObject txtSector;
	[SerializeField] private Material[] Gauges;
	private Color level0, level1; 
	// private Color levelminus;
	private float fadeinTime = 0f, totalTime = 0f;

	void Start () {
		sc = GameObject.Find("PlayerShip").GetComponent<ShipController>();
		sh = GameObject.Find("PlayerShip").GetComponent<ShipHealth>();
		sPrimaryWeapon = GameObject.Find("sldPrimaryWeapon").GetComponent<Slider>();
		sSecondaryWeapon = GameObject.Find("sldSecondaryWeapon").GetComponent<Slider>();
		sHealth = GameObject.Find("sldHealth").GetComponent<Slider>();
		sEngine = GameObject.Find("sldEngine").GetComponent<Slider>();
		sLifeSupport = GameObject.Find("sldLifeSupport").GetComponent<Slider>();
		panLifeSupport = GameObject.Find("panLifeSupport");
		panLifeSupport.SetActive(false);
		panGauges = GameObject.Find("panGauges");
		panGauges.SetActive(true);
		panMapCam = GameObject.Find("panMapCam");
		panMapCam.SetActive(true);
		txtSector.SetActive(true);
		level0 = new Color(255/255,  26/255,  26/255, 255/255);      //empty bar
		level1 = new Color( 58/255, 255/255, 133/255, 255/255);      //full bar
		//levelminus = new Color(0, 0, 0, 0);                          //negative bar
		FixOverlay();
	}
	
	void Update () {
		sPrimaryWeapon.value = sc.priCurrentCharge / sc.priRechargeRate;
		sSecondaryWeapon.value = sc.secCurrentCharge / sc.secRechargeRate;
		sHealth.value = (float)sh.GetHealth() / (float)sh.maxHealth;
		sEngine.value = sc.engCurrentCharge / sc.engRechargeRate;
		sLifeSupport.value = sc.lifeCurrentCharge / sc.lifeRechargeRate;

		fillPrimaryWeapon.color = Color.Lerp(level0, level1, sPrimaryWeapon.value);
		fillSecondaryWeapon.color = Color.Lerp(level0, level1, sSecondaryWeapon.value);
		fillHealth.color = Color.Lerp(level0, level1, sHealth.value);
		fillEngine.color = Color.Lerp(level0, level1, sEngine.value);
		fillLifeSupport.color = Color.Lerp(level0, level1, sLifeSupport.value);

		if (sc.isEscaping()) {
			panLifeSupport.SetActive(true);
			panGauges.SetActive(false);
			panMapCam.SetActive(false);
		} else {
			panLifeSupport.SetActive(false);
			panGauges.SetActive(true);
			panMapCam.SetActive(true);
		}

		if (fadeinTime > 0f) {   //used with ShowMessage()
			fadeinTime -= Time.deltaTime;
			if (fadeinTime < 0f) { fadeinTime = 0f; }
			Color c = txtSector.transform.Find("txtSector").GetComponent<Text>().color;
			c.a = (totalTime - fadeinTime) / totalTime;
			txtSector.transform.Find("txtSector").GetComponent<Text>().color = c;
		}
	}

	private void FixOverlay() {
		bool bArcadeMode = GameObject.Find("GameManager").GetComponent<GameManager>().bArcadeMode;
		PrefsControl prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		Image img1 = GameObject.Find("Primary_Gauge").GetComponent<Image>();
		Image img2 = GameObject.Find("Secondary_Gauge").GetComponent<Image>();
		int weap1 = prefs.GetPrimaryWeapon(bArcadeMode);
		int weap2 = prefs.GetSecondaryWeapon(bArcadeMode);
		img1.material = Gauges[weap1];
		img2.material = Gauges[weap2 + 3];
	}

	public void ShowMessage(string s) {   //called from GameManager
		txtSector.SetActive(true);
		txtSector.transform.Find("txtStaticSector").GetComponent<Text>().text = "";
		txtSector.transform.Find("txtSector").GetComponent<Text>().text = s;
		fadeinTime = 1.25f;
		totalTime = fadeinTime;
	}
}
