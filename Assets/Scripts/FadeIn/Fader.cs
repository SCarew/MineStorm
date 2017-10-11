using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour {

	[Tooltip ("Seconds to fade in")]
	public  float fadeTime = 3f;
	[Tooltip ("Seconds until leaving screen automatically after fade in (-1 = don't)")]
	public  float advanceTime = 3f;  
	private Image fadeImage;
	private Color currentColor;

	void Start () {
		fadeImage = GetComponent<Image>();
		currentColor = fadeImage.color;
	}
	
	void Update () {
		if (Time.timeSinceLevelLoad < fadeTime) {
			float alphaChange = Time.deltaTime / fadeTime;
			currentColor.a -= alphaChange;
			fadeImage.color = currentColor;
		} else {
			if (Input.GetButtonDown("Primary")) {
				LoadNextScene();
			}
			if (advanceTime < 0f) {
				gameObject.SetActive (false); 
			}
		}
		if ((advanceTime >= 0f) && (Time.timeSinceLevelLoad > (fadeTime + advanceTime)) ) {
			LoadNextScene();
		} 
	}

	void LoadNextScene() {
		advanceTime = -1f;
		//SceneManager.LoadScene("Start");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
