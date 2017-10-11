using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HypGameOverMenu : MonoBehaviour {
	private bool bActive = false;    //is gameOverCanvas visible (is game over)
	private bool bFinished = false;  //finished darkening and slow down
	public GameObject gameOverCanvas;
	private Image darkBackground;
	private Text txtGameOver;
	private float maxAlpha = 165f;
	private Color bgColor;
	private float currentTime = 0f;

	private float timeSlowDown = 4f;

	void Start () {
		gameOverCanvas.SetActive(true);
		darkBackground = GameObject.Find("DarkBackground").GetComponent<Image>();
		txtGameOver = GameObject.Find("txtGameOver").GetComponent<Text>();
		maxAlpha = darkBackground.color.a;
		bgColor = darkBackground.color;
		gameOverCanvas.SetActive(false);
	}
	
	public void LaunchGameOver () {
		gameOverCanvas.SetActive(true);
		bActive = true;
	}

	void Update() {
		if (bActive) {
			currentTime += Time.deltaTime;
			if (currentTime > timeSlowDown) { 
				currentTime = timeSlowDown; 
				bFinished = true;
			}
			Time.timeScale = (timeSlowDown - currentTime) / timeSlowDown;
			if (Time.timeScale < 0.02f) { Time.timeScale = 0.02f; }
			float alpha = (1f - Time.timeScale) * maxAlpha;
			darkBackground.color = new Color(bgColor.r, bgColor.g, bgColor.b, alpha);
			Color c = txtGameOver.color;
			c.a = alpha / maxAlpha;
			txtGameOver.color = c;

			if (bFinished) {
				//TODO add panel w/button and text here and activate?
				if (Input.GetButtonDown("Primary") || Input.GetButtonDown("Cancel")) {
					Time.timeScale = 1f;
					GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadScene("Title");
				}
			}
		}
	}

}
