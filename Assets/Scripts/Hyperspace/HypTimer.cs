using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HypTimer : MonoBehaviour {
	private Text txtTime;
	private float fTime = 0f;
	public  GameObject fadeinPanel;
	private LevelManager lm;

	void Start () {
		txtTime = gameObject.GetComponent<Text>();
		//fadeinPanel = GameObject.Find("Fadein Panel");
		fadeinPanel.SetActive(true);
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		fTime = GetHypTime();
		StartCoroutine(Countdown());
	}

	IEnumerator Countdown() {
		bool bLoop = true;
		while (bLoop) {
			if (fTime < 100f) 
				{ txtTime.text = ((int)fTime).ToString("00"); }
			else
				{ txtTime.text = "**"; }
			yield return new WaitForSeconds(1.0f);
			if (fTime <= 0f) { bLoop = false; }
		}
	}

	float GetHypTime() {
		//TODO get f somewhere dep on level
		float f = float.Parse(txtTime.text.Trim());
		return f;
	}

	void ExitHyperspace() {
		fadeinPanel.SetActive(true);
		HypFader h = fadeinPanel.GetComponent<HypFader>();
		h.ResetTimer(true);
		Invoke("LoadMainScene", h.fadeTime + 0.5f );
	}

	void LoadMainScene() {
		lm.LoadScene("Main");
	}

	void Update () {
		fTime -= Time.deltaTime;
		if (fTime <= 0f) { ExitHyperspace(); }
	}
}
