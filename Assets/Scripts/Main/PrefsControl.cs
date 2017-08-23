using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefsControl : MonoBehaviour {

	public enum stats {Ships, Score, Level};
	public enum option {Camera, Sound, Music};
	private static List<string> lstUpgrade;
	private static List<int>    lstUpgradeValue;
	private static List<string> lstUpgradeFormula;

	private void Start() {
		if (lstUpgrade == null || lstUpgrade.Count < 1) {
			CreateListUpgrade();
		}
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
		if (type == stats.Ships) { returnValue = PlayerPrefs.GetInt("ShipsRemaining", 0); }
		if (type == stats.Score) { returnValue = PlayerPrefs.GetInt("StoryScore", 0); }
		if (type == stats.Level) { returnValue = PlayerPrefs.GetInt("CurrentLevel", 0); }

		return returnValue;
	}

	public void SetGameStats(stats type, int value) {
		if (type == stats.Ships) { PlayerPrefs.SetInt("ShipsRemaining", value); }
		if (type == stats.Score) { PlayerPrefs.SetInt("StoryScore", value); }
		if (type == stats.Level) { PlayerPrefs.SetInt("CurrentLevel", value); }

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

	public void SetUpgrade (string up) {
		//if (up.Length % 3 != 0)  { up = ""; }
		//PlayerPrefs.SetString("Upgrades", up);
		if (up.Length != 3) { up = ""; }
		string s = GetUpgrades() + up;
		PlayerPrefs.SetString("Upgrades", s);
	}

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

	private void AddAdditionalShip() {
		lstUpgrade.Add("Additional Ship");
		lstUpgradeValue.Add(999);
		lstUpgradeFormula.Add("***");
	}

	private void CreateListUpgrade() {
		string s = GetUpgrades();
		// may not need all these booleans
		bool bMisLau1 = s.Contains("M+1");
		bool bMisLau2 = s.Contains("M+2");
		bool bMisDam1 = s.Contains("M+A");
		bool bMisDam2 = s.Contains("M+B");
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
		int pri = GetPrimaryWeapon();
		int sec = GetSecondaryWeapon();


		AddAdditionalShip();
		if (pri != 0) {
			lstUpgrade.Add("Set Primary Weapon to Torpedo");
			lstUpgradeValue.Add(910);
			lstUpgradeFormula.Add("P=T");
		}
		if (pri != 1) {
			lstUpgrade.Add("Set Primary Weapon to Laser");
			lstUpgradeValue.Add(911);
			lstUpgradeFormula.Add("P=L");
		}
		if (pri != 2) {
			lstUpgrade.Add("Set Primary Weapon to Missiles");
			lstUpgradeValue.Add(912);
			lstUpgradeFormula.Add("P=M");
		}
		if (sec != 0) {
			lstUpgrade.Add("Set Secondary to Hyperjump");
			lstUpgradeValue.Add(920);
			lstUpgradeFormula.Add("S=H");
		}
		if (sec != 1) {
			lstUpgrade.Add("Set Secondary to Forcefield");
			lstUpgradeValue.Add(921);
			lstUpgradeFormula.Add("S=F");
		}
		if (sec != 2) {
			lstUpgrade.Add("Set Secondary to Shockwave");
			lstUpgradeValue.Add(922);
			lstUpgradeFormula.Add("S=S");
		}
		if (!bMisLau1 && !bMisLau2) {
			lstUpgrade.Add("Missile Launcher II: Increase Loadout by 25%");
			lstUpgradeValue.Add(321);
			lstUpgradeFormula.Add("M+1");
		} else if (!bMisLau2) {
			lstUpgrade.Add("Missile Launcher III: Increase Loadout by 20%");
			lstUpgradeValue.Add(322);
			lstUpgradeFormula.Add("M+2");
		} else { AddAdditionalShip(); }
		if (!bMisDam1 && !bMisDam2) {
			lstUpgrade.Add("Missile Mk2: Increase Damage 25%");
			lstUpgradeValue.Add(325);
			lstUpgradeFormula.Add("M+A");
		} else if (!bMisDam2) {
			lstUpgrade.Add("Missile Mk3: Increase Damage 25%");
			lstUpgradeValue.Add(326);
			lstUpgradeFormula.Add("M+B");
		} else { AddAdditionalShip(); }
		if (!bMisSpe1) {
			lstUpgrade.Add("Missile Rev.A: Increase Speed by 25%");
			lstUpgradeValue.Add(328);
			lstUpgradeFormula.Add("M+S");
		} else { AddAdditionalShip(); }
		if (!bMisRef1) {
			lstUpgrade.Add("Missile Power Generation: Refresh Rate 25% Faster");
			lstUpgradeValue.Add(330);
			lstUpgradeFormula.Add("M+G");
		} else { AddAdditionalShip(); }
		if (!bLasDam1) {
			lstUpgrade.Add("Laser Generator II: Increase Damage 25%");
			lstUpgradeValue.Add(310);
			lstUpgradeFormula.Add("L+1");
		} else if (!bLasDam2) {
			lstUpgrade.Add("Laser Generator III: Increase Damage 20%");
			lstUpgradeValue.Add(311);
			lstUpgradeFormula.Add("L+2");
		} else if (!bLasDam3) {
			lstUpgrade.Add("Laser Generator IV: Increase Damage 25%");
			lstUpgradeValue.Add(312);
			lstUpgradeFormula.Add("L+3");
		} else { AddAdditionalShip(); }
		if (!bLasRef1) {
			lstUpgrade.Add("Laser Power Generation Rev.A: Refresh Rate 20% Faster");
			lstUpgradeValue.Add(315);
			lstUpgradeFormula.Add("L+A");
		} else if (!bLasRef2) {
			lstUpgrade.Add("Laser Power Generation Rev.B: Refresh Rate 20% Faster");
			lstUpgradeValue.Add(316);
			lstUpgradeFormula.Add("L+B");
		} else { AddAdditionalShip(); }
		if (!bLasRng1) {
			lstUpgrade.Add("Laser Stabilizer Mk2: Increase Range 25%");
			lstUpgradeValue.Add(318);
			lstUpgradeFormula.Add("L+R");
		} else { AddAdditionalShip(); }
		if (!bTorDam1) {
			lstUpgrade.Add("Torpedo Mk2: Increase Damage 25%");
			lstUpgradeValue.Add(300);
			lstUpgradeFormula.Add("T+1");
		} else if (!bTorDam2) {
			lstUpgrade.Add("Torpedo Mk3: Increase Damage 20%");
			lstUpgradeValue.Add(301);
			lstUpgradeFormula.Add("T+2");
		} else { AddAdditionalShip(); }
		if (!bTorRef1) {
			lstUpgrade.Add("Torpedo Launcher II: Refresh Rate 25% Faster");
			lstUpgradeValue.Add(304);
			lstUpgradeFormula.Add("T+A");
		} else if (!bTorRef2) {
			lstUpgrade.Add("Torpedo Launcher III: Refresh Rate 20% Faster");
			lstUpgradeValue.Add(305);
			lstUpgradeFormula.Add("T+B");
		} else { AddAdditionalShip(); }
		if (!bTorSpe1) {
			lstUpgrade.Add("Torpedo Casing Rev.A: Increase Speed 20%");
			lstUpgradeValue.Add(308);
			lstUpgradeFormula.Add("T+S");
		} else { AddAdditionalShip(); }
		if (!bHypEff1) {
			lstUpgrade.Add("Hyperwarp Mk2 Power Efficiency: 30% Lower Power Consumption");
			lstUpgradeValue.Add(400);
			lstUpgradeFormula.Add("H+1");
		} else if (!bHypEff2) {
			lstUpgrade.Add("Hyperwarp Mk3 Power Efficiency: 43% Lower Power Consumption");
			lstUpgradeValue.Add(401);
			lstUpgradeFormula.Add("H+2");
		} else { AddAdditionalShip(); }
		if (!bHypRec1) {
			lstUpgrade.Add("Hyperwarp Power Generator A: 25% Faster Recharge");
			lstUpgradeValue.Add(405);
			lstUpgradeFormula.Add("H+A");
		} else if (!bHypRec2) {
			lstUpgrade.Add("Hyperwarp Power Generator B: 25% Faster Recharge");
			lstUpgradeValue.Add(406);
			lstUpgradeFormula.Add("H+B");
		} else { AddAdditionalShip(); }
		if (!bForEff1) {
			lstUpgrade.Add("Forcefield Mk2 Power Efficiency: 30% Lower Power Consumption");
			lstUpgradeValue.Add(410);
			lstUpgradeFormula.Add("F+1");
		} else if (!bForEff2) {
			lstUpgrade.Add("Forcefield Mk3 Power Efficiency: 43% Lower Power Consumption");
			lstUpgradeValue.Add(411);
			lstUpgradeFormula.Add("F+2");
		} else { AddAdditionalShip(); }
		if (!bForDur1) {
			lstUpgrade.Add("Forcefield Power Generator A: 25% Longer Duration");
			lstUpgradeValue.Add(415);
			lstUpgradeFormula.Add("F+A");
		} else if (!bForDur2) {
			lstUpgrade.Add("Forcefield Power Generator B: 20% Longer Duration");
			lstUpgradeValue.Add(416);
			lstUpgradeFormula.Add("F+B");
		} else { AddAdditionalShip(); }
		if (!bShoRec1) {
			lstUpgrade.Add("Shockwave Power Generator A: 20% Faster Recharge");
			lstUpgradeValue.Add(420);
			lstUpgradeFormula.Add("S+A");
		} else if (!bShoRec2) {
			lstUpgrade.Add("Shockwave Power Generator B: 25% Faster Recharge");
			lstUpgradeValue.Add(421);
			lstUpgradeFormula.Add("S+B");
		} else { AddAdditionalShip(); }
		if (!bShoDam1) {
			lstUpgrade.Add("Shockwave Null Point Emitter Mk1: Increase Damage 25%");
			lstUpgradeValue.Add(425);
			lstUpgradeFormula.Add("S+1");
		} else if (!bShoDam2) {
			lstUpgrade.Add("Shockwave Null Point Emitter Mk2: Increase Damage 20%");
			lstUpgradeValue.Add(426);
			lstUpgradeFormula.Add("S+2");
		} else { AddAdditionalShip(); }
//		lstUpgrade.Add("");
//		lstUpgradeValue.Add();
//		lstUpgradeFormula.Add("");

	}

}
