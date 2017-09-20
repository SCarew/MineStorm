using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour {

	private PrefsControl prefs;
	private LevelManager lm;
	private SoundManager aud;
	private RawImage[] img;
	private Text[] txt, txtValue;
	private Color colUnselected, colSelected, colMouseover;
	private int numSelected = 0;
	private float fChange = 0f;   //check for delaying input

	private float offset = 0.2f;

	void Start () {
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		img = new RawImage[3];
		img[0] = GameObject.Find("ChoiceImage0").GetComponent<RawImage>();
		img[1] = GameObject.Find("ChoiceImage1").GetComponent<RawImage>();
		img[2] = GameObject.Find("ChoiceImage2").GetComponent<RawImage>();
		txt = new Text[3];
		txt[0] = GameObject.Find("ChoiceText0").GetComponent<Text>();
		txt[1] = GameObject.Find("ChoiceText1").GetComponent<Text>();
		txt[2] = GameObject.Find("ChoiceText2").GetComponent<Text>();
		txtValue = new Text[3];
		txtValue[0] = GameObject.Find("ChoiceValue0").GetComponent<Text>();
		txtValue[1] = GameObject.Find("ChoiceValue1").GetComponent<Text>();
		txtValue[2] = GameObject.Find("ChoiceValue2").GetComponent<Text>();
		colUnselected = new Color(255/255f, 255/255f, 255/255f);
		colMouseover  = new Color(187/255f, 238/255f, 188/255f);
		colSelected   = new Color(207/255f, 232/255f, 249/255f);
		for (int i=0; i<3; i++) {
			img[i].color = colUnselected;
		}

	}
	
	void Update () {
		if (Input.GetButtonDown("Primary") && (numSelected > 0)) {
			Debug.Log("Button " + numSelected + " => " + txtValue[numSelected-1].text);
			img[numSelected - 1].color = colSelected;
			txt[numSelected - 1].fontStyle = FontStyle.BoldAndItalic;
			aud.PlaySoundImmediate("choiceSelect");
			Invoke("ButtonSelected", 0.5f);
		}

		if (fChange > 0) {
			fChange -= Time.deltaTime;
			return;
		}

		bool bChange = false;    //flag for button select change
		if (Input.GetAxis("Vertical") - offset > 0) {
			numSelected -= 1;
			bChange = true;
		}
		if (Input.GetAxis("Vertical") + offset < 0) {
			numSelected += 1;
			bChange = true;
		}
		if (bChange) {
			fChange += 0.5f;    //delay to next button change
			if (numSelected < 1) { numSelected = 1; }
			if (numSelected > 3) { numSelected = 3; }
			aud.PlaySoundImmediate("choiceMove");
			for (int i=0; i<3; i++) {
				img[i].color = colUnselected;
				txt[i].fontStyle = FontStyle.Normal;
			}
			img[numSelected - 1].color = colMouseover;
			//txt[numSelected - 1].fontStyle = FontStyle.Italic;
		}
	}

	void ButtonSelected() {
		int sel = int.Parse(txtValue[numSelected - 1].text);
		prefs.SetChosenValue(sel);
		prefs.UpdateSetChoice();
		lm.LoadNextScene();
	}
}
