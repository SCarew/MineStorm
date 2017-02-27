using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelController : MonoBehaviour {

	private Slider sPrimaryWeapon, sSecondaryWeapon, sHealth, sEngine, sLifeSupport;
	private GameObject panLifeSupport, panGauges, panMapCam;
	private ShipController sc;
	private ShipHealth sh;
	[SerializeField] private Image fillPrimaryWeapon, fillSecondaryWeapon;
	[SerializeField] private Image fillHealth, fillEngine, fillLifeSupport;
	private Color level0, level1, levelminus;
//	private Image[] imgArray;

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
		level0 = new Color(255/255,  26/255,  26/255, 255/255);      //empty bar
		level1 = new Color( 58/255, 255/255, 133/255, 255/255);      //full bar
		levelminus = new Color(0, 0, 0, 0);                          //negative bar

//		imgArray = new Image[8];
//		imgArray[0] = sPrimaryWeapon.GetComponentInChildren<Image>();
//		imgArray[1] = fillPrimaryWeapon;
//		imgArray[2] = sSecondaryWeapon.GetComponentInChildren<Image>();
//		imgArray[3] = fillSecondaryWeapon;
//		imgArray[4] = sHealth.GetComponentInChildren<Image>();
//		imgArray[5] = fillHealth;
//		imgArray[6] = sEngine.GetComponentInChildren<Image>();
//		imgArray[7] = fillEngine;
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

		//GameObject.Find("TextSecondary").GetComponent<Text>().text = sc.priCurrentCharge.ToString();
//		if (sc.priCurrentCharge < 0f) {
//			imgArray[0].color = levelminus; 
//			imgArray[1].color = levelminus;
//		}
//		if (sc.secCurrentCharge < 0f) {
//			imgArray[2].color = levelminus; 
//			imgArray[3].color = levelminus;
//		}
//		if (sc.engCurrentCharge < 0f) {
//			imgArray[6].color = levelminus; 
//			imgArray[7].color = levelminus;
//		}

	}
}
