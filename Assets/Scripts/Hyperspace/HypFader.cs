using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class HypFader : MonoBehaviour {

	public  float fadeTime = 3f;  //secs to fade in/out
	//public  float advanceTime = 3f;  //secs to leave screen automatically (-1 = don't)
	private Image fadeImage;
	private Color currentColor;
	private float timer = 0f;
	private bool  fadeOut = false;

	void Start () {
		fadeImage = GetComponent<Image>();
		currentColor = fadeImage.color;
	}

	/// <summary>
	/// Resets the timer.
	/// </summary>
	/// <param name="fadeOutTime">Fade out time (-1 = same as fadeIn default).</param>
	/// <param name="bReverse">If set to <c>true</c>, fade to white.</param>
	public void ResetTimer(bool bReverse = false, float fadeOutTime = -1f) {
		fadeOut = bReverse;
		if (fadeOutTime != -1f)  { fadeTime = fadeOutTime; } 
		timer = 0;
	}

	void Update () {
		if (timer < fadeTime) {
			float alphaChange = Time.deltaTime / fadeTime;
			if (!fadeOut) 
				{ currentColor.a -= alphaChange; }
			else
				{ currentColor.a += alphaChange; }
			fadeImage.color = currentColor;
		} else if (!fadeOut) {
			gameObject.SetActive (false); 
		}

		if (!fadeOut) {
			timer += Time.deltaTime;
		} else {
			fadeTime -= Time.deltaTime;
		}
	}
}

