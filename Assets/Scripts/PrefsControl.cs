using UnityEngine;
using System.Collections;

public class PrefsControl : MonoBehaviour {

	public string GetChoices() {
		string value;
		value = PlayerPrefs.GetString("ChoiceTitle", "");
		return value;
	}

	public string GetChoice(int i) {
		return PlayerPrefs.GetString("Choice" + i.ToString(), "");
	}

	public string GetChoiceValue(int i) {
		return PlayerPrefs.GetString("ChoiceValue" + i.ToString(), "");
	}

	public void SetChoice(string s, string c, int num = 3) {
		//format of s: "title/choice1/choice2/choice3"
		//format of c: "101/102/103"

		string sub;
		int j;

		if (!s.EndsWith("/")) 
			{ s = string.Concat(s, "/"); }
		for (int i=0; i<(num+1); i++) {
			j = s.IndexOf("/");
			sub = s.Substring(0, j);
			if (i == 0) 
				{ PlayerPrefs.SetString("ChoiceTitle", sub); }
			else { 
				PlayerPrefs.SetString("Choice" + i.ToString(), sub); 
			}
			s = s.Substring(j + 1);
		}

		if (!c.EndsWith("/")) 
			{ c = string.Concat(c, "/"); }
		for (int i=1; i<(num+1); i++) {
			j = c.IndexOf("/");
			sub = c.Substring(0, j);
			PlayerPrefs.SetString("ChoiceValue" + i.ToString(), sub);
			c = c.Substring(j + 1);
		}
	}

	public void SetChosenValue(int value) {
		if (value == 100) {  //primary+secondary > NONE
			PlayerPrefs.SetInt("Primary", -1); 
			PlayerPrefs.SetInt("Secondary", -1); }

		if (value == 101)   //primary > torpedo
			{ PlayerPrefs.SetInt("Primary", 0); }
		if (value == 102)   //primary > laser
			{ PlayerPrefs.SetInt("Primary", 1); }
		if (value == 103)   //primary > missile
			{ PlayerPrefs.SetInt("Primary", 2); }
		if (value == 201)   //secondary > hyperjump
			{ PlayerPrefs.SetInt("Secondary", 0); }
		if (value == 202)   //secondary > forcefield
			{ PlayerPrefs.SetInt("Secondary", 1); }
		if (value == 203)   //secondary > shockwave
			{ PlayerPrefs.SetInt("Secondary", 2); }
	}

	public int GetPrimaryWeapon() {
		return PlayerPrefs.GetInt("Primary", 0);
	}

	public int GetSecondaryWeapon() {
		return PlayerPrefs.GetInt("Secondary", 0);
	}
}
