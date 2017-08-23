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
		if ((round/4f) == (int)(round/4f)) {    
			ChooseUpgradeOptions();
			lm.LoadScene("Choice", "Main");
		} else {
			lm.LoadScene("Main");
		}
	}

	void ChooseUpgradeOptions() {
		//TODO choose upgrades
		int r;
		int maxUpgrades = 30;
		string s1 = "%/#/$/^";
		string s2 = "000/001/002";
		for (int i=1; i<4; i++) {
			r = Random.Range(0, maxUpgrades) + 1;
			if (r==1) { }
		}

		s1.Replace("%", "Upgrade Available");
		prefs.SetChoice(s1, s2, 3);
	}

	void Update () {
		fTime -= Time.deltaTime;
		if (fTime <= 0f) { 
			ExitHyperspace(); 
			fTime = 10f;
		}
	}
}
