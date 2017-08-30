using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HypTimer : MonoBehaviour {
	private Text txtTime;
	private float fTime = 110f;   //should be set externally by SetHypTime() below
	public  GameObject fadeinPanel;
	private LevelManager lm;
	private int round;
	private PrefsControl prefs;

	void Start () {
		txtTime = gameObject.GetComponent<Text>();
		//fadeinPanel = GameObject.Find("Fadein Panel");
		fadeinPanel.SetActive(true);
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();

		Text txtNext = GameObject.Find("txtNext").GetComponent<Text>();
		round = prefs.GetGameStats(PrefsControl.stats.Level);
		txtNext.text = lm.SectorName(round);

		StartCoroutine(Countdown());
	}

	IEnumerator Countdown() {
		bool bLoop = true;
		while (bLoop) {
			if (fTime < 100f) 
				{ txtTime.text = ((int)fTime).ToString("00"); }
			else
				{ txtTime.text = "--"; }
			yield return new WaitForSeconds(1.0f);
			if (fTime <= 0f) { bLoop = false; }
		}
	}

	public void SetHypTime(float hypTime) {
		fTime = hypTime;
	}

	void ExitHyperspace() {
		fadeinPanel.SetActive(true);
		HypFader h = fadeinPanel.GetComponent<HypFader>();
		h.ResetTimer(true);
		GameObject.Find("Hyp_PlayerShip").GetComponent<HypShipController>().EnterWarp();
		Invoke("LoadMainScene", h.fadeTime + 0.5f );
	}

	void LoadMainScene() {
		//*****Testing - remove*****
		ChooseUpgradeOptions();
		lm.LoadScene("Choice", "Main");
		return;
		//**************************

		if ((round/4f) == (int)(round/4f)) {    
			ChooseUpgradeOptions();
			lm.LoadScene("Choice", "Main");
		} else {
			lm.LoadScene("Main");
		}
	}

	void ChooseUpgradeOptions() {
		string s1;

		s1 = prefs.FindNextUpgrade();
		//s2 = prefs.FindNextUpgradeValue();
		s1 = "Upgrade Available/" + s1;
		Debug.Log(s1);
		prefs.SetChoice(s1, "291/292/293", 3);
	}

	void Update () {
		fTime -= Time.deltaTime;
		if (fTime <= 0f) { 
			ExitHyperspace(); 
			fTime = 10f;
		}
	}
}
