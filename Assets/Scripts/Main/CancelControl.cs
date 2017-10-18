using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CancelControl : MonoBehaviour {

	private LevelManager lm;
	private float disappearTime = 5f;   //time till object vanishes
	private float fadeTime = 4.0f;		//time till text starts fading
	private float timeElapsed = 0f;
	private Color c;
	private Text txtCancel;

	void Start () {
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		txtCancel = transform.Find("txtCancel").GetComponent<Text>();
		c = txtCancel.color;
	}
	
	void Update () {
		if (Input.GetButtonDown("Cancel")) {
			lm.LoadScene("Start");
			return;
		}
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= disappearTime) {
			Destroy(gameObject);
		}
		if (timeElapsed >= fadeTime) {
			c.a = 1 - ((timeElapsed - fadeTime) / (disappearTime - fadeTime));
			txtCancel.color = c;
		}
	}
}
