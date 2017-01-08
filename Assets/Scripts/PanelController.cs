using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {

	private Slider sPrimaryWeapon, sSecondaryWeapon, sHealth;
	private ShipController sc;
	private ShipHealth sh;

	void Start () {
		sc = GameObject.Find("PlayerShip").GetComponent<ShipController>();
		sh = GameObject.Find("PlayerShip").GetComponent<ShipHealth>();
		sPrimaryWeapon = GameObject.Find("sldPrimaryWeapon").GetComponent<Slider>();
		sSecondaryWeapon = GameObject.Find("sldSecondaryWeapon").GetComponent<Slider>();
		sHealth = GameObject.Find("sldHealth").GetComponent<Slider>();
	}
	
	void Update () {
		sPrimaryWeapon.value = sc.priCurrentCharge / sc.priRechargeRate;
		sSecondaryWeapon.value = sc.secCurrentCharge / sc.secRechargeRate;
		sHealth.value = sh.GetHealth() / sh.maxHealth;
	}
}
