using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSelect : MonoBehaviour {

	private int currentButton = -1;
	private bool disableOptions = false;
	private Color colorUnselected, colorSelected, colorFinal, colorDisabled;
	public GameObject startPanel, optionPanel;
	public Text[] textOptions;
	public Text[] textMini;
	private PrefsControl prefs;
	private LevelManager lm;

	private float offset = 0.2f;
	private float fChange = 0f;   //check for delaying input
	private bool menuStart = true;  //on Start(t) or Option(f) menu
	public int startMenuOptions = 5;  

	void Start () {
		colorUnselected = new Color(151/255f, 244/255f, 248/255f, 255/255f);
		colorSelected   = new Color(231/255f, 242/255f,  43/255f, 255/255f);
		colorFinal      = new Color(249/255f, 160/255f,  48/255f, 255/255f);
		colorDisabled   = new Color(121/255f, 214/255f, 218/255f, 127/255f);
		ResetMenu();
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		optionPanel.SetActive(false);
		startPanel.SetActive(true);
	}

	void ResetMenu() {
		foreach (Text txt in textOptions) {
			txt.color = colorUnselected;
		}

	}

	void Update () {
		if (Input.GetButtonDown("Primary") && (currentButton >= 0)) {
			textOptions[currentButton].color = colorFinal;
			Invoke("ButtonSelected", 0.4f);
			return;
		}

		if (fChange > 0) {
			fChange -= Time.deltaTime;
			return;
		}

		float v = Input.GetAxis("Vertical");
		bool bChange = false;
		if (v - offset > 0) {
			currentButton -= 1;
			bChange = true;
		}
		if (v + offset < 0) {
			currentButton += 1;
			bChange=true;
		}

		if (bChange) {
			fChange = 0.5f;  //length of delay to next option select
			if (currentButton < 0) { currentButton = 0; }
			if (currentButton > (textOptions.Length - 1)) { 
				currentButton = textOptions.Length - 1; 
			}
			if (menuStart) {
				if (currentButton > (startMenuOptions - 1)) {
					currentButton = startMenuOptions - 1;
				}
			} else {
				if (currentButton < startMenuOptions) {
					currentButton = startMenuOptions;
				}
			}
			foreach (Text txt in textOptions) {
				txt.color = colorUnselected;
			}
			foreach (Text txt0 in textMini) {
				txt0.enabled = false;
			}
			textOptions[currentButton].color = colorSelected;
			textMini[currentButton].enabled = true;
		}
	}

	void StartGame() {
		//TODO launch Choice and game here
		if (textOptions[currentButton].name == "txtStory") {
			prefs.SetChoice("Select Primary Weapon/Torpedo/Laser/Missile", "101/102/103", 3); 
			prefs.SetChoice2("Select Secondary Weapon/Hyperjump/Forcefield/Shockwave", "201/202/203", 3); 
			lm.LoadScene("Choice", "Choice", "Main");
		}
	}

	void ButtonSelected() {
		if (textOptions[currentButton].name == "txtStory") {
			prefs.SetGameType("Story");
			StartGame();
		}
		if (textOptions[currentButton].name == "txtContinue") {
			prefs.SetGameType("Continue");
			StartGame();
		}
		if (textOptions[currentButton].name == "txtArcade") {
			prefs.SetGameType("Arcade");
			StartGame();
		}
		if (textOptions[currentButton].name == "txtOptions") {
			optionPanel.SetActive(true);
			startPanel.SetActive(false);
			currentButton = -1;
			menuStart = false;
			return;
		}
		if (textOptions[currentButton].name == "txtExit") {
			Application.Quit();
		}

		//****************** Options Menu below ***********************
		if (textOptions[currentButton].name == "txtCamera") {
			
		}
		if (textOptions[currentButton].name == "txtStart") {
			startPanel.SetActive(true);
			optionPanel.SetActive(false);
			currentButton = -1;
			menuStart = true;
			return;
		}
	}
}
