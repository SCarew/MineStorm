using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HypTimer : MonoBehaviour {
	Text txtTime;
	float fTime = 0f;

	void Start () {
		txtTime = gameObject.GetComponent<Text>();
		fTime = GetHypTime();
		StartCoroutine(Countdown());
	}

	IEnumerator Countdown() {
		bool bLoop = true;
		while (bLoop) {
			txtTime.text = ((int)fTime).ToString("00");
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
		//TODO add this
	}

	void Update () {
		fTime -= Time.deltaTime;
		if (fTime <= 0f) { ExitHyperspace(); }
	}
}
