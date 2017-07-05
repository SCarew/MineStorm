using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartSelect : MonoBehaviour {

	private int currentButton = -1;
	private bool disableContinue = false;
	private Color colorUnselected, colorSelected, colorFinal, colorDisabled;
	public GameObject startPanel, optionPanel, controlsPanel;
	public Text[] textOptions;
	public Text[] textMini;
	public Slider[] sldMini;
	public GameObject[] panelLayout;
	public Text txtLayout, txtMiniLayout;
	private int sliderSound = 8, sliderMusic = 9;   //locations of sliders
	private PrefsControl prefs;
	private LevelManager lm;
	private SoundManager aud;

	private float offset = 0.2f;
	private float fChange = 0f;   //check for delaying input
	private bool menuStart = true;  //on Start(t) or Option(f) menu
	public int startMenuOptions = 5;  
	public int optionMenuOptions = 6;

	void Start () {
		colorUnselected = new Color(151/255f, 244/255f, 248/255f, 255/255f);
		colorSelected   = new Color(231/255f, 242/255f,  43/255f, 255/255f);
		colorFinal      = new Color(249/255f, 160/255f,  48/255f, 255/255f);
		colorDisabled   = new Color(121/255f, 214/255f, 218/255f, 127/255f);
		ResetMenu();
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();

		optionPanel.SetActive(false);
		controlsPanel.SetActive(false);
		startPanel.SetActive(true);
		LoadOptions();

		if (prefs.GetGameStats(PrefsControl.stats.Level) < 1) {
			DisableOption ("txtContinue");
			disableContinue = true;
		}
		foreach (Slider sld0 in sldMini) {
			//sld0.enabled = false;
			sld0.gameObject.SetActive(false);
		}

		//play music theme
		MusicManager music = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		music.PlayMusic(0);	
	}

	void ResetMenu() {
		foreach (Text txt in textOptions) {
			txt.color = colorUnselected;
		}

	}

	void Update () {
		bool bChange = false;
		if (Input.GetButtonDown("Primary") && (currentButton >= 0)) {
			if (controlsPanel.activeSelf) {
				txtLayout.color = colorFinal;
				aud.PlaySoundImmediate("startSelect");
				Invoke("LayoutSelected", 0.4f);
				return;
			}
			textOptions[currentButton].color = colorFinal;
			aud.PlaySoundImmediate("startSelect");
			Invoke("ButtonSelected", 0.4f);
			return;
		}

		if (!menuStart && Input.GetButtonDown("Cancel")) {   //cancel Options menu
			if (optionPanel.activeSelf) {
				startPanel.SetActive(true);
				optionPanel.SetActive(false);
				currentButton = -1;
				menuStart = true;
				LoadOptions();   //reset changed options to previously saved
				return;
			} else {    //cancel Controls menu
				optionPanel.SetActive(true);
				controlsPanel.SetActive(false);
				currentButton = startMenuOptions + 1;
				bChange = true;
			}
		}

		if (fChange > 0) {
			fChange -= Time.deltaTime;
			return;
		}

		float v = Input.GetAxis("Vertical");
		int nextButton = -1;
		if (v - offset > 0) {
			currentButton -= 1;
			nextButton = currentButton - 1;
			bChange = true;
		}
		if (v + offset < 0) {
			currentButton += 1;
			nextButton = currentButton + 1;
			bChange = true;
		}

		if (bChange) {
			fChange = 0.5f;  //length of delay to next option select
			if (currentButton < 0) { currentButton = 0; }
			if (currentButton > (textOptions.Length - 1)) { 
				currentButton = textOptions.Length - 1; 
			}
			if (disableContinue && textOptions[currentButton].name == "txtContinue") {
				currentButton = nextButton;
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
			foreach (Slider sld0 in sldMini) {
				sld0.gameObject.SetActive(false);
			}
			if (disableContinue) { DisableOption("txtContinue"); }
			textOptions[currentButton].color = colorSelected;
			textMini[currentButton].enabled = true;
			if (currentButton == sliderSound) { sldMini[0].gameObject.SetActive(true); }
			if (currentButton == sliderMusic) { sldMini[1].gameObject.SetActive(true); }
			aud.PlaySoundImmediate("startMove");
		}

		float h = Input.GetAxis("Horizontal");
		if (currentButton == sliderSound || currentButton == sliderMusic) {
			if (h - offset > 0) {
				sldMini[currentButton - sliderSound].value += 0.01f;
			}
			if (h + offset < 0) {
				sldMini[currentButton - sliderSound].value -= 0.01f;
			}
		} 

	}

	void StartGame() {
		//TODO launch Choice and game here
		if (textOptions[currentButton].name == "txtStory") {
			prefs.SetChoice("Select Primary Weapon/Torpedo/Laser/Missile", "101/102/103", 3); 
			prefs.SetChoice2("Select Secondary Weapon/Hyperjump/Forcefield/Shockwave", "201/202/203", 3); 
			lm.LoadScene("Choice", "Choice", "Main");
		}
		if (textOptions[currentButton].name == "txtContinue") {
			lm.LoadScene("Main");
		}
		if (textOptions[currentButton].name == "txtArcade") {
			prefs.SetChoice("Select Primary Weapon/Torpedo/Laser/Missile", "101/102/103", 3); 
			prefs.SetChoice2("Select Secondary Weapon/Hyperjump/Forcefield/Shockwave", "201/202/203", 3); 
			lm.LoadScene("Choice", "Choice", "Main");
		}
	}

	void UpdateControlLayout() {
		for (int i=0; i < panelLayout.Length; i++) {
			panelLayout[i].SetActive(false);
		}

		if (txtMiniLayout.text == "Layout 1") {
			panelLayout[0].SetActive(true);
		} else if (txtMiniLayout.text == "Layout 2") {
			panelLayout[1].SetActive(true);
		} else {   //Layout 3
			panelLayout[2].SetActive(true);
		}
	}

	void LayoutSelected() {
		if (txtMiniLayout.text == "Layout 1") 
			{ txtMiniLayout.text = "Layout 2"; }
		else if (txtMiniLayout.text == "Layout 2") 
			{ txtMiniLayout.text = "Layout 3"; }
		else 
			{ txtMiniLayout.text = "Layout 1"; }
		UpdateControlLayout();
		txtLayout.color = colorUnselected;
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
			if (textMini[currentButton].text == "Chasing") {
				textMini[currentButton].text = "Fixed";
			} else {
				textMini[currentButton].text = "Chasing";
			}
		}
		if (textOptions[currentButton].name == "txtControls") {
			controlsPanel.SetActive(true);
			optionPanel.SetActive(false);
			UpdateControlLayout();
		}
		if (textOptions[currentButton].name == "txtHyperY") {
			if (textMini[currentButton].text == "Normal") {
				textMini[currentButton].text = "Inverted";
			} else {
				textMini[currentButton].text = "Normal";
			}
		}
		if (textOptions[currentButton].name == "txtSound") {
			if (textMini[currentButton].text == "Set 1") {
				textMini[currentButton].text = "Set 1";  //not set up for second sound set yet
			} else {
				textMini[currentButton].text = "Set 1";
			}
		}
		if (textOptions[currentButton].name == "txtMusic") {
			
		}
		if (textOptions[currentButton].name == "txtStart") {
			SaveOptions();
			startPanel.SetActive(true);
			optionPanel.SetActive(false);
			currentButton = -1;
			menuStart = true;
			return;
		}

		if (!menuStart) {
			textOptions[currentButton].color = colorSelected;
		}
	}

	void DisableOption (string s)
	{
		foreach (Text txt in textOptions) {
			if (txt.name == s) {
				txt.color = colorDisabled;
			}
		}
	}

	void LoadOptions() {
		//TODO read Controller Setup from Prefs
		sldMini[0].value = prefs.GetMainVolume();
		sldMini[1].value = prefs.GetMusicVolume();
		foreach (Text txt in textMini) {
			if (txt.name == "txtMiniCamera") { 
				if (prefs.GetCameraMode() == true) 
					{ txt.text = "Fixed"; }
				else
					{ txt.text = "Chasing"; }
			}
			if (txt.name == "txtMiniSound") { 
				txt.text = "Set " + prefs.GetSoundSet();
			}
			if (txt.name == "txtMiniHyperY") {
				if (prefs.GetHyperY() == true) {
					txt.text = "Inverted";
				} else {
					txt.text = "Normal";
				}
			}
		}
		int con = prefs.GetControlLayout() + 1;
		txtMiniLayout.text = "Layout " + con;
	}

	void SaveOptions() {
		//TODO write Controller Setup to prefs
		prefs.SetMainVolume(sldMini[0].value);
		prefs.SetMusicVolume(sldMini[1].value);
		foreach (Text txt in textMini) {
			if (txt.name == "txtMiniCamera") { 
				if (txt.text == "Fixed") 
					{ prefs.SetCameraMode(true); } 
				else
					{ prefs.SetCameraMode(false); } 
			}
			if (txt.name == "txtMiniSound") { 
				prefs.SetSoundSet(int.Parse(txt.text.Substring(txt.text.Length - 1, 1)));
			}
			if (txt.name == "txtMiniHyperY") {
				if (txt.text == "Normal") 
					{ prefs.SetHyperY(false); }
				else 
					{ prefs.SetHyperY(true); }
			}
		}
		prefs.SetControlLayout(int.Parse(txtMiniLayout.text.Substring(txtMiniLayout.text.Length - 1, 1)) - 1);
	}
}
