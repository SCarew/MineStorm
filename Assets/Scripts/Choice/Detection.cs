using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Detection : MonoBehaviour {

	private PrefsControl prefs;
	private string str;
	private string choice1, choice2, choice3;
	private string cValue1, cValue2, cValue3;
	private Text c1, c2, c3, title, cv1, cv2, cv3;
	private RectTransform[] panels;
	private float[] coords;
	private bool fComplete = false;

	void Start() {
		//detect image choices from preferences
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		title = GameObject.Find("TitleText").GetComponent<Text>();
		c1 = GameObject.Find("ChoiceText0").GetComponent<Text>();
		c2 = GameObject.Find("ChoiceText1").GetComponent<Text>();
		c3 = GameObject.Find("ChoiceText2").GetComponent<Text>();
		cv1 = GameObject.Find("ChoiceValue0").GetComponent<Text>();
		cv2 = GameObject.Find("ChoiceValue1").GetComponent<Text>();
		cv3 = GameObject.Find("ChoiceValue2").GetComponent<Text>();

		str = prefs.GetChoices();
		if (str == "") { str = "Primary Weapon"; }

		choice1 = prefs.GetChoice(1);
		choice2 = prefs.GetChoice(2);
		choice3 = prefs.GetChoice(3);
		cValue1 = prefs.GetChoiceValue(1);
		cValue2 = prefs.GetChoiceValue(2);
		cValue3 = prefs.GetChoiceValue(3);

		string temp, tempv;
		int rnd = 0;
		for (int i=0; i<2; i++) {
			rnd = Random.Range(0, 4);
			if (rnd == 0) { 
				temp  = choice1; choice1 = choice2; choice2 = temp; 
				tempv = cValue1; cValue1 = cValue2; cValue2 = tempv; 
			}  
			if (rnd == 1) { 
				temp  = choice1; choice1 = choice3; choice3 = temp; 
				tempv = cValue1; cValue1 = cValue3; cValue3 = tempv; 
			}  
			if (rnd == 2) { 
				temp  = choice2; choice2 = choice3; choice3 = temp; 
				tempv = cValue2; cValue2 = cValue3; cValue3 = tempv; 
			}  
		}

		title.text = str;
		c1.text = choice1;
		c2.text = choice2;
		c3.text = choice3;
		cv1.text = cValue1;
		cv2.text = cValue2;
		cv3.text = cValue3;

		panels = new RectTransform[4];
		panels[0] = GameObject.Find("Panel0").GetComponent<RectTransform>();
		panels[1] = GameObject.Find("Panel1").GetComponent<RectTransform>();
		panels[2] = GameObject.Find("Panel2").GetComponent<RectTransform>();
		panels[3] = GameObject.Find("TitlePanel").GetComponent<RectTransform>();

		coords = new float[4];
		coords[0] = panels[0].anchoredPosition.y;
		coords[1] = panels[1].anchoredPosition.y;
		coords[2] = panels[2].anchoredPosition.y;
		coords[3] = panels[3].anchoredPosition.y;
		Debug.Log(coords[0] + "," + coords[1] + "," + coords[2] + "," + coords[3] );
		Debug.Log(panels[0].localPosition.x + " " + panels[0].localPosition.y);
		for (int i=0; i<3; i++) {
			panels[i].anchoredPosition = new Vector2(0f, -344f);
		}
		panels[3].anchoredPosition = new Vector2(0f, 140f);

	}

	void Update() {
		if (fComplete) {return;}
		float rate = 0.05f;
		for (int i = 0; i<4; i++) {
			panels[i].anchoredPosition = Vector2.Lerp(panels[i].anchoredPosition, new Vector2(0, coords[i]), rate);
		}
		if (Mathf.Abs((int) panels[0].anchoredPosition.y) == Mathf.Abs((int) coords[0])) {
			fComplete = true;
			Debug.Log("Time elapsed: " + Time.time);
		}
	}
}
