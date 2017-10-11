using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefsControl : MonoBehaviour {

	public enum stats {Ships, Score, Level};
	public enum option {Camera, Sound, Music};
	private static List<string> lstUpgrade;
	//private static List<int>    lstUpgradeValue;
	private static List<string> lstUpgradeFormula;
	private static bool bDeveloperMode = false;

	public bool isHypDead = false;  //flag for Hyperspace death
	public bool bHypGameOver = false;  //flag for Hyperspace end of game

	private int maxScores = 5;  //# of top scores for arcade mode

	private void Start() {
		if (lstUpgrade == null || lstUpgrade.Count < 1) {
			lstUpgrade = new List<string>();
			lstUpgradeFormula = new List<string>();
			//CreateListUpgrade();
		}
	}

	public bool GetDevMode() {
		return bDeveloperMode;
	}

	public void SetDevMode(bool b) {
		bDeveloperMode = b;
		Debug.Log("Dev reached!!!"); 
	}

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

	public void SetChoice2(string s, string c, int num = 3) {
		SetChoice(s, c, num, true);
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
		bool bSG = false;   //starting new game
		string sPrimary = "Primary";
		string sSecondary = "Secondary";
		if (GetGameType() == "Arcade") {
			sPrimary = "Arc_Primary";
			sSecondary = "Arc_Secondary";
		} else {
			ClearNextUpgrade();
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
			{ PlayerPrefs.SetInt(sSecondary, 0); bSG = true; }
		if (value == 202)   //secondary > forcefield
			{ PlayerPrefs.SetInt(sSecondary, 1); bSG = true; }
		if (value == 203)   //secondary > shockwave
			{ PlayerPrefs.SetInt(sSecondary, 2); bSG = true; }

		if (value == 291)   //upgrade > 1
			{ SetUpgrade(PlayerPrefs.GetString("ChoiceFormula1", "")); }
		if (value == 292)   //upgrade > 2
			{ SetUpgrade(PlayerPrefs.GetString("ChoiceFormula2", "")); }
		if (value == 293)   //upgrade > 3
			{ SetUpgrade(PlayerPrefs.GetString("ChoiceFormula3", "")); }

		if (bSG && !arcadeMode)   //new story mode game
			{ PlayerPrefs.DeleteKey("Upgrades"); }

		Debug.Log("Chosen: " + value + " --> " + PlayerPrefs.GetString("ChoiceFormula" + (value-290).ToString(), ""));
	}

//	public void SetChosenValue(string value) {
//		SetUpgrade(value);
//		ClearNextUpgrade();
//	}

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

	public bool GetHyperY() {
		if (PlayerPrefs.GetInt("InvertYAxis", 0) == 0)
			{ return false; }
		else 
			{ return true; }
	}

	public void SetHyperY(bool bInvert) {
		if (bInvert)
			{ PlayerPrefs.SetInt("InvertYAxis", 1); }
		else
			{ PlayerPrefs.SetInt("InvertYAxis", 0); }
	}

	public int GetControlLayout() {
		return PlayerPrefs.GetInt("ControlScheme", 0);
	}

	public void SetControlLayout(int i) {
		if (i < 0 || i > 2) 
			{ i = 0; }
		PlayerPrefs.SetInt("ControlScheme", i);
	}

	public float GetMainVolume() {
		return PlayerPrefs.GetFloat("SoundEffectsVolume", 0.5f);
	}

	public void SetMainVolume(float f) {
		PlayerPrefs.SetFloat("SoundEffectsVolume", f);
	}

	public float GetMusicVolume() {
		return PlayerPrefs.GetFloat("MusicVolume", 0.5f);
	}

	public void SetMusicVolume(float f) {
		PlayerPrefs.SetFloat("MusicVolume", f);
	}

	public int GetSoundSet() {
		return PlayerPrefs.GetInt("SoundEffectsSet", 1);
	}

	public void SetSoundSet(int i) {
		PlayerPrefs.SetInt("SoundEffectsSet", i);
	}

	public bool GetCameraMode() {
		int n = PlayerPrefs.GetInt("CameraMode", 0);
		if (n == 1) 
			{ return true; } 
		else 
			{ return false; }
	}

	public void SetCameraMode(bool b) {
		if (b) { PlayerPrefs.SetInt("CameraMode", 1); }
		else   { PlayerPrefs.SetInt("CameraMode", 0); }
	}

	public int GetGameStats(stats type) {
		int returnValue = 0;
		if (GetGameType() != "Arcade") {
			if (type == stats.Ships) { returnValue = PlayerPrefs.GetInt("ShipsRemaining", 0); }
			if (type == stats.Score) { returnValue = PlayerPrefs.GetInt("StoryScore", 0); }
			if (type == stats.Level) { returnValue = PlayerPrefs.GetInt("CurrentLevel", 0); }
		} else {
			if (type == stats.Ships) { returnValue = PlayerPrefs.GetInt("Arc_ShipsRemaining", 0); }
			if (type == stats.Score) { returnValue = PlayerPrefs.GetInt("ArcadeScore", 0); }
			if (type == stats.Level) { returnValue = PlayerPrefs.GetInt("Arc_CurrentLevel", 0); }
		}

		return returnValue;
	}

	public void SetGameStats(stats type, int value) {
		if (GetGameType() != "Arcade") {
			if (type == stats.Ships) { PlayerPrefs.SetInt("ShipsRemaining", value); }
			if (type == stats.Score) { PlayerPrefs.SetInt("StoryScore", value); }
			if (type == stats.Level) { PlayerPrefs.SetInt("CurrentLevel", value); }
		} else {
			if (type == stats.Ships) { PlayerPrefs.SetInt("Arc_ShipsRemaining", value); }
			if (type == stats.Score) { PlayerPrefs.SetInt("ArcadeScore", value); }
			if (type == stats.Level) { PlayerPrefs.SetInt("Arc_CurrentLevel", value); }
		}
	}

	/// <summary>
	/// Gets the top score at num.  Get # in top if num = -1.
	/// </summary>
	/// <returns>The top score at num.</returns>
	/// <param name="num">Number.</param>
	public int GetTopScore(int num) {
		if (num < 1 || num > maxScores) { 
			if (num == -1) { return maxScores; }
			return 0;
		}
		int returnValue = PlayerPrefs.GetInt("TopScore" + num.ToString(), 0);
		if (returnValue <= 0) {
			returnValue = (maxScores + 1 - num) * 2000;
			PlayerPrefs.SetInt("TopScore" + num.ToString(), returnValue);
			PlayerPrefs.SetString("TopScoreName" + num.ToString(), "SC" + num.ToString());
		}
		return returnValue;
	}

	public string GetTopScoreName(int num) {
		if (num < 1 || num > maxScores) { return ""; }
		return PlayerPrefs.GetString("TopScoreName" + num.ToString(), "---");
	}

	public void SetTopScore(int score, string name = "---") {
		string oldName;
		int oldScore = PlayerPrefs.GetInt("TopScore" + maxScores.ToString(), 0);
		if (score < oldScore) { return; }
		int i, j = 1;
		for (i=maxScores-1; i>0; i--) {
			oldScore = PlayerPrefs.GetInt("TopScore" + i.ToString(), 0);
			if (oldScore > score) {
				j = i + 1;
				break;
			}
		}
		i = maxScores;
		while (i > j) {
			i--;
			oldScore = PlayerPrefs.GetInt("TopScore" + i.ToString(), 0);
			oldName = PlayerPrefs.GetString("TopScoreName" + i.ToString(), "---");
			PlayerPrefs.SetInt("TopScore" + (i+1).ToString(), oldScore);
			PlayerPrefs.SetString("TopScoreName" + (i+1).ToString(), oldName);
		} 
		PlayerPrefs.SetInt("TopScore" + j.ToString(), score);
		PlayerPrefs.SetString("TopScoreName" + j.ToString(), name);
	}

	private void ClearNextUpgrade() {         //used only in Story Mode
		PlayerPrefs.DeleteKey("NextUpgrade");
		PlayerPrefs.DeleteKey("NextUpgradeValue");
	}

	public string GetUpgrades() {
		string s = PlayerPrefs.GetString("Upgrades", "");
		if (s.Length % 3 != 0) { s = ""; }
		return s;
	}

	private void SetUpgrade (string up) {
		if (up.Length % 3 != 0) { up = ""; }
		if (up == "P=T") {
			up = "";
			PlayerPrefs.SetInt("Primary", 0);
		}
		if (up == "P=L") {
			up = "";
			PlayerPrefs.SetInt("Primary", 1);
		}
		if (up == "P=M") {
			up = "";
			PlayerPrefs.SetInt("Primary", 2);
		}
		if (up == "S=H") {
			up = "";
			PlayerPrefs.SetInt("Secondary", 0);
		}
		if (up == "S=F") {
			up = "";
			PlayerPrefs.SetInt("Secondary", 1);
		}
		if (up == "S=S") {
			up = "";
			PlayerPrefs.SetInt("Secondary", 2);
		}

		PlayerPrefs.SetString("Upgrades", GetUpgrades() + up);
	}

	public void ReplaceUpgrade(string upList) {
		if (upList.Length % 3 != 0) { 
			Debug.LogWarning("Incorrect call to ReplaceUpgrade(" + upList + ")");
			return; 
		}
		PlayerPrefs.SetString("Upgrades", upList);
	}

	public string FindNextUpgrade() {
		string s = "", sv = "";
		CreateListUpgrade();
		if (lstUpgradeFormula.Count < 3) { return s; }
		int iNum;
		for (int i=1; i<4; i++) {
			iNum = Random.Range(0, lstUpgradeFormula.Count);
			s = s + lstUpgrade[iNum] + "/";
			sv = sv + lstUpgradeFormula[iNum] + "/";
			lstUpgrade.RemoveAt(iNum);
			lstUpgradeFormula.RemoveAt(iNum);
		}
		PlayerPrefs.SetString("NextUpgrade", s);
		PlayerPrefs.SetString("NextUpgradeValue", sv);

		PlayerPrefs.SetString("ChoiceFormula1", sv.Substring(0, 3));
		PlayerPrefs.SetString("ChoiceFormula2", sv.Substring(4, 3));
		PlayerPrefs.SetString("ChoiceFormula3", sv.Substring(8, 3));
		return s;
	}

	//public string FindNextUpgradeValue() {
	//	return PlayerPrefs.GetString("NextUpgradeValue", "");
	//}

//	private void SetNextUpgrade(string s1, string s2) {
//		PlayerPrefs.SetString("NextUpgrade", s1);
//		PlayerPrefs.SetString("NextUpgradeValue", s2);
//	}
//
//	private string GetNextUpgrade() {
//		return PlayerPrefs.GetString("NextUpgrade", "");
//	}
//
//	private string GetNextUpgradeValue() {
//		return PlayerPrefs.GetString("NextUpgradeValue", "");
//	}

	public string UpgradeText(string formula, bool bMultiLine = false) {
		string s = "";
		if (formula == "P=T") { s = "Set Primary Weapon to Torpedo"; }
		if (formula == "P=L") { s = "Set Primary Weapon to Laser"; }
		if (formula == "P=M") { s = "Set Primary Weapon to Missiles"; }
		if (formula == "S=H") { s = "Set Secondary to Hyperjump"; }
		if (formula == "S=F") { s = "Set Secondary to Forcefield"; }
		if (formula == "S=S") { s = "Set Secondary to Shockwave"; }
		if (formula == "M+A") { s = "Missile Launcher II: Increase Loadout by 25%"; }
		if (formula == "M+B") { s = "Missile Launcher III: Increase Loadout by 20%"; }
		if (formula == "M+1") { s = "Missile Mk2: Increase Damage 25%"; }
		if (formula == "M+2") { s = "Missile Mk3: Increase Damage 25%"; }
		if (formula == "M+S") { s = "Missile Rev.A: Increase Speed by 25%"; }
		if (formula == "M+G") { s = "Missile Power Generation: Refresh Rate 25% Faster"; }
		if (formula == "L+1") { s = "Laser Generator II: Increase Damage 35%"; }
		if (formula == "L+2") { s = "Laser Generator III: Increase Damage 25%"; }
		if (formula == "L+3") { s = "Laser Generator IV: Increase Damage 20%"; }
		if (formula == "L+A") { s = "Laser Power Generation Rev.A: Refresh Rate 20% Faster"; }
		if (formula == "L+B") { s = "Laser Power Generation Rev.B: Refresh Rate 20% Faster"; }
		if (formula == "L+R") { s = "Laser Stabilizer Mk2: Increase Range 20%"; }
		if (formula == "T+1") { s = "Torpedo Mk2: Increase Damage 25%"; }
		if (formula == "T+2") { s = "Torpedo Mk3: Increase Damage 20%"; }
		if (formula == "T+A") { s = "Torpedo Launcher II: Refresh Rate 25% Faster"; }
		if (formula == "T+B") { s = "Torpedo Launcher III: Refresh Rate 20% Faster"; }
		if (formula == "T+S") { s = "Torpedo Casing Rev.A: Increase Speed 20%"; }
		if (formula == "H+1") { s = "Hyperwarp Mk2 Power Efficiency: 30% Lower Power Consumption"; }
		if (formula == "H+2") { s = "Hyperwarp Mk3 Power Efficiency: 43% Lower Power Consumption"; }
		if (formula == "H+A") { s = "Hyperwarp Power Generator A: 20% Faster Recharge"; }
		if (formula == "H+B") { s = "Hyperwarp Power Generator B: 25% Faster Recharge"; }
		if (formula == "F+1") { s = "Forcefield Mk2 Power Efficiency: 30% Lower Power Consumption"; }
		if (formula == "F+2") { s = "Forcefield Mk3 Power Efficiency: 43% Lower Power Consumption"; }
		if (formula == "F+A") { s = "Forcefield Power Generator A: 25% Longer Duration"; }
		if (formula == "F+B") { s = "Forcefield Power Generator B: 20% Longer Duration"; }
		if (formula == "S+1") { s = "Shockwave Null Point Emitter Mk1: Increase Damage 25%"; }
		if (formula == "S+2") { s = "Shockwave Null Point Emitter Mk2: Increase Damage 20%"; }
		if (formula == "S+A") { s = "Shockwave Power Generator A: 20% Faster Recharge"; }
		if (formula == "S+B") { s = "Shockwave Power Generator B: 25% Faster Recharge"; }
		if (formula == "U+1") { s = "Hull Fortification I: Increase Durability 20%"; }
		if (formula == "U+2") { s = "Hull Fortification II: Increase Durability 25%"; }
		if (formula == "U+3") { s = "Hull Fortification III: Increase Durability 20%"; }
		if (formula == "***") { s = "Additional Ship"; }

		if (bMultiLine)  { s = s.Replace(": ", ":\r\n"); }

		return s;
	}

	private void AddAdditionalShip() {
		lstUpgradeFormula.Add("***");
		//lstUpgrade.Add("Additional Ship");
	}

	private void CreateListUpgrade() {   //compare with CheckUpgrades() in ShipController
		string s = GetUpgrades();

		bool bMisDam1 = s.Contains("M+1");
		bool bMisDam2 = s.Contains("M+2");
		bool bMisLau1 = s.Contains("M+A");
		bool bMisLau2 = s.Contains("M+B");
		bool bMisSpe1 = s.Contains("M+S");
		bool bMisRef1 = s.Contains("M+G");
		bool bLasDam1 = s.Contains("L+1");
		bool bLasDam2 = s.Contains("L+2");
		bool bLasDam3 = s.Contains("L+3");
		bool bLasRef1 = s.Contains("L+A");
		bool bLasRef2 = s.Contains("L+B");
		bool bLasRng1 = s.Contains("L+R");
		bool bTorDam1 = s.Contains("T+1");
		bool bTorDam2 = s.Contains("T+2");
		bool bTorRef1 = s.Contains("T+A");
		bool bTorRef2 = s.Contains("T+B");
		bool bTorSpe1 = s.Contains("T+S");
		bool bHypEff1 = s.Contains("H+1");
		bool bHypEff2 = s.Contains("H+2");
		bool bHypRec1 = s.Contains("H+A");
		bool bHypRec2 = s.Contains("H+B");
		bool bForEff1 = s.Contains("F+1");
		bool bForEff2 = s.Contains("F+2");
		bool bForDur1 = s.Contains("F+A");
		bool bForDur2 = s.Contains("F+B");
		bool bShoDam1 = s.Contains("S+1");
		bool bShoDam2 = s.Contains("S+2");
		bool bShoRec1 = s.Contains("S+A");
		bool bShoRec2 = s.Contains("S+B");
		bool bHull1 = s.Contains("U+1");
		bool bHull2 = s.Contains("U+2");
		bool bHull3 = s.Contains("U+3");
		int pri = GetPrimaryWeapon();
		int sec = GetSecondaryWeapon();

		lstUpgrade.Clear();
		lstUpgradeFormula.Clear();


		AddAdditionalShip();
		if (pri != 0) {
			lstUpgradeFormula.Add("P=T");
		}
		if (pri != 1) {
			lstUpgradeFormula.Add("P=L");
		}
		if (pri != 2) {
			lstUpgradeFormula.Add("P=M");
		}
		if (sec != 0) {
			lstUpgradeFormula.Add("S=H");
		}
		if (sec != 1) {
			lstUpgradeFormula.Add("S=F");
		}
		if (sec != 2) {
			lstUpgradeFormula.Add("S=S");
		}
		if (pri == 2) {
			if (!bMisLau1 && !bMisLau2) {
				lstUpgradeFormula.Add("M+A");
			} else if (!bMisLau2) {
				lstUpgradeFormula.Add("M+B");
			} else { AddAdditionalShip(); }
			if (!bMisDam1 && !bMisDam2) {
				lstUpgradeFormula.Add("M+1");
			} else if (!bMisDam2) {
				lstUpgradeFormula.Add("M+2");
			} else { AddAdditionalShip(); }
			if (!bMisSpe1) {
				lstUpgradeFormula.Add("M+S");
			} else { AddAdditionalShip(); }
			if (!bMisRef1) {
				lstUpgradeFormula.Add("M+G");
			} else { AddAdditionalShip(); }
		}
		if (pri == 1) {
			if (!bLasDam1) {
				lstUpgradeFormula.Add("L+1");
			} else if (!bLasDam2) {
				lstUpgradeFormula.Add("L+2");
			} else if (!bLasDam3) {
				lstUpgradeFormula.Add("L+3");
			} else { AddAdditionalShip(); }
			if (!bLasRef1) {
				lstUpgradeFormula.Add("L+A");
			} else if (!bLasRef2) {
				lstUpgradeFormula.Add("L+B");
			} else { AddAdditionalShip(); }
			if (!bLasRng1) {
				lstUpgradeFormula.Add("L+R");
			} else { AddAdditionalShip(); }
		}
		if (pri == 0) {
			if (!bTorDam1) {
				lstUpgradeFormula.Add("T+1");
			} else if (!bTorDam2) {
				lstUpgradeFormula.Add("T+2");
			} else { AddAdditionalShip(); }
			if (!bTorRef1) {
				lstUpgradeFormula.Add("T+A");
			} else if (!bTorRef2) {
				lstUpgradeFormula.Add("T+B");
			} else { AddAdditionalShip(); }
			if (!bTorSpe1) {
				lstUpgradeFormula.Add("T+S");
			} else { AddAdditionalShip(); }
		}

		if (sec == 0) {
			if (!bHypEff1) {
				lstUpgradeFormula.Add("H+1");
			} else if (!bHypEff2) {
				lstUpgradeFormula.Add("H+2");
			} else { AddAdditionalShip(); }
			if (!bHypRec1) {
				lstUpgradeFormula.Add("H+A");
			} else if (!bHypRec2) {
				lstUpgradeFormula.Add("H+B");
			} else { AddAdditionalShip(); }
		}
		if (sec == 1) {
			if (!bForEff1) {
				lstUpgradeFormula.Add("F+1");
			} else if (!bForEff2) {
				lstUpgradeFormula.Add("F+2");
			} else { AddAdditionalShip(); }
			if (!bForDur1) {
				lstUpgradeFormula.Add("F+A");
			} else if (!bForDur2) {
				lstUpgradeFormula.Add("F+B");
			} else { AddAdditionalShip(); }
		}
		if (sec == 2) {
			if (!bShoRec1) {
				lstUpgradeFormula.Add("S+A");
			} else if (!bShoRec2) {
				lstUpgradeFormula.Add("S+B");
			} else { AddAdditionalShip(); }
			if (!bShoDam1) {
				lstUpgradeFormula.Add("S+1");
			} else if (!bShoDam2) {
				lstUpgradeFormula.Add("S+2");
			} else { AddAdditionalShip(); }
		}
		if (!bHull1) {
			lstUpgradeFormula.Add("U+1");
		} else if (!bHull2) {
			lstUpgradeFormula.Add("U+2");
		} else if (!bHull3) {
			lstUpgradeFormula.Add("U+3");
		} else { AddAdditionalShip(); }

//		lstUpgrade.Add("");
//		lstUpgradeValue.Add();
//		lstUpgradeFormula.Add("");

		for (int i=0; i<lstUpgradeFormula.Count; i++) {
			lstUpgrade.Add(UpgradeText(lstUpgradeFormula[i], true));
		}

	}

}
