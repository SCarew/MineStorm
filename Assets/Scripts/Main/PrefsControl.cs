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

	public void SetChoice2(string s, string c, int num = 3) {
		SetChoice(s, c, num, true);
	}

	public void UpdateSetChoice() {
		if (PlayerPrefs.GetString("NextChoice1", "") == "") {
			Debug.LogWarning("UpdateSetChoice() called with no update");
			//return;
		}
		Debug.Log(PlayerPrefs.GetString("ChoiceTitle", "*") + " <- " + PlayerPrefs.GetString("NextChoiceTitle", "*"));

		for (int i=1; i<4; i++) {
			PlayerPrefs.SetString("Choice" + i.ToString(), PlayerPrefs.GetString("NextChoice" + i.ToString(), ""));
			PlayerPrefs.SetString("ChoiceValue" + i.ToString(), PlayerPrefs.GetString("NextChoiceValue" + i.ToString(), ""));
			PlayerPrefs.SetString("NextChoice" + i.ToString(), "");
			PlayerPrefs.SetString("NextChoiceValue" + i.ToString(), "");
		}
		PlayerPrefs.SetString("ChoiceTitle", PlayerPrefs.GetString("NextChoiceTitle", ""));
		PlayerPrefs.SetString("NextChoiceTitle", "");
	}

	public void SetChoice(string s, string c, int num = 3, bool bNext=false) {
		//format of s: "title/choice1/choice2/choice3"
		//format of c: "101/102/103"

		string sub;
		int j;
		string sChoice = "Choice";
		string sChoiceTitle = "ChoiceTitle";
		string sChoiceValue = "ChoiceValue";
		if (bNext) {
			sChoice = "NextChoice";
			sChoiceTitle = "NextChoiceTitle";
			sChoiceValue = "NextChoiceValue";
		}

		if (!s.EndsWith("/")) 
			{ s = string.Concat(s, "/"); }
		for (int i=0; i<(num+1); i++) {
			j = s.IndexOf("/");
			sub = s.Substring(0, j);
			if (i == 0) 
				{ PlayerPrefs.SetString(sChoiceTitle, sub); }
			else { 
				PlayerPrefs.SetString(sChoice + i.ToString(), sub); 
			}
			s = s.Substring(j + 1);
		}

		if (!c.EndsWith("/")) 
			{ c = string.Concat(c, "/"); }
		for (int i=1; i<(num+1); i++) {
			j = c.IndexOf("/");
			sub = c.Substring(0, j);
			PlayerPrefs.SetString(sChoiceValue + i.ToString(), sub);
			c = c.Substring(j + 1);
		}
	}

	public void SetChosenValue(int value, bool arcadeMode = false) {
		string sPrimary = "Primary";
		string sSecondary = "Secondary";
		if (GetGameType() == "Arcade") {
			sPrimary = "Arc_Primary";
			sSecondary = "Arc_Secondary";
		}

		if (value == 100) {  //primary+secondary > NONE
			PlayerPrefs.SetInt(sPrimary, -1); 
			PlayerPrefs.SetInt(sSecondary, -1); }

		if (value == 101)   //primary > torpedo
			{ PlayerPrefs.SetInt(sPrimary, 0); }
		if (value == 102)   //primary > laser
			{ PlayerPrefs.SetInt(sPrimary, 1); }
		if (value == 103)   //primary > missile
			{ PlayerPrefs.SetInt(sPrimary, 2); }
		if (value == 201)   //secondary > hyperjump
			{ PlayerPrefs.SetInt(sSecondary, 0); }
		if (value == 202)   //secondary > forcefield
			{ PlayerPrefs.SetInt(sSecondary, 1); }
		if (value == 203)   //secondary > shockwave
			{ PlayerPrefs.SetInt(sSecondary, 2); }
	}

	public int GetPrimaryWeapon(bool arcadeMode = false) {
		if (arcadeMode) {
			return PlayerPrefs.GetInt("Arc_Primary", 0);
		}
		return PlayerPrefs.GetInt("Primary", 0);
	}

	public int GetSecondaryWeapon(bool arcadeMode = false) {
		if (arcadeMode) {
			return PlayerPrefs.GetInt("Arc_Secondary", 0);
		}
		return PlayerPrefs.GetInt("Secondary", 0);
	}

	public string GetGameType() {
		return PlayerPrefs.GetString("GameType", "Arcade");
	}

	public void SetGameType(string s) {
		if (s=="Story" || s=="Continue") {
			PlayerPrefs.SetString("GameType", s);
		} else {
			PlayerPrefs.SetString("GameType", "Arcade");
		}
	}
}
