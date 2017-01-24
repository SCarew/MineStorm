using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Detection : MonoBehaviour {

	private PrefsControl prefs;
	private string str;
	private string choice1, choice2, choice3;
	private string cValue1, cValue2, cValue3;
	private Text c1, c2, c3, title, cv1, cv2, cv3;

	void Start() {
		//detect image choices from preferences
		prefs = GetComponent<PrefsControl>();
		title = GameObject.Find("TitleText").GetComponent<Text>();
		c1 = GameObject.Find("ChoiceText0").GetComponent<Text>();
		c2 = GameObject.Find("ChoiceText1").GetComponent<Text>();
		c3 = GameObject.Find("ChoiceText2").GetComponent<Text>();
		cv1 = GameObject.Find("ChoiceValue0").GetComponent<Text>();
		cv2 = GameObject.Find("ChoiceValue1").GetComponent<Text>();
		cv3 = GameObject.Find("ChoiceValue2").GetComponent<Text>();

		// ***Testing***
		prefs.SetChoice("Select Primary Weapon/Torpedo/Laser/Missile", "101/102/103", 3); 
		// ***Testing End***

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
	}
}
