using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinCredits : MonoBehaviour {

	private bool bRoll = false;			//true to roll credits
	private bool bFade = false;			//true to fade out "asteroids cleared"
	private RectTransform rt;
	private Color c;
	private Text txtQuestion, txtVictory;
	private float scrollRate = 30f;		//speed of credit scroll
	private float timeFade;				//time to fade out "asteroids cleared"
	private float maxTimeFade;
	private float endPoint = 1200f;		//pos of credit rect.y when scene goes to title

	public void StartCredits() {
		bRoll = true;
		bFade = true;
		txtQuestion = GameObject.Find("txtQuestion").GetComponent<Text>();
		txtVictory = GameObject.Find("txtVictory").GetComponent<Text>();
		timeFade = 3f;
		maxTimeFade = timeFade;
		endPoint = 1300f;
		//endPoint = -gameObject.GetComponent<RectTransform>().position.y;
	}

	void Update () {
		if (bRoll) {
			rt = gameObject.GetComponent<RectTransform>();
			rt.position = new Vector3(rt.position.x, rt.position.y + (scrollRate * Time.deltaTime), rt.position.z);
			gameObject.GetComponent<RectTransform>().position = rt.position;
			if (rt.position.y > endPoint) {
				GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadScene("Title");
				return;
			}
		}	

		if (bFade) {
			timeFade -= Time.deltaTime;
			if (timeFade < 0) { 
				bFade = false; 
				timeFade = 0; 
			}
			c = txtVictory.color;
			c.a = timeFade/maxTimeFade;
			txtVictory.color = c;
			txtQuestion.color = c;
		}
	}
}
