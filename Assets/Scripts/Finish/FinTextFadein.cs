using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FinTextFadein : MonoBehaviour {

	[SerializeField] private Text txtVict;
	[SerializeField] private Text txtQues;
	private float fadeTime = 4f;
	private float startTime;
	private Color c;

	void Start () {
		startTime = fadeTime;
	}
	
	void Update () {
		fadeTime -= Time.deltaTime;
		if (fadeTime < 0f) { fadeTime = 0f; Destroy(this); }
		c = txtVict.color;
		c.a = (startTime - fadeTime) / startTime;
		txtVict.color = c;
		txtQues.color = c;
	}
}
