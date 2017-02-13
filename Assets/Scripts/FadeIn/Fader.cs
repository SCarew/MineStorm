using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour {

	public  float fadeTime = 3f;  //secs to fade in
	public  float advanceTime = 3f;  //secs to leave screen automatically (-1 = don't)
	private Image fadeImage;
	private Color currentColor;

	//TODO put SCEC image on FadeIn screen
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
		//Debug.Log("Loading Start scene");
		advanceTime = -1f;
		//SceneManager.LoadScene("Start");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
