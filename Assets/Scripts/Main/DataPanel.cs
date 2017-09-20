using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataPanel : MonoBehaviour {

	public  Text txtData;
	private string sCRLF = System.Environment.NewLine;   // \r\n
	private Transform tEnemies, tMeteors;
	private ShipHealth sh;
	private PrefsControl prefs;
	private GameManager gm;
	private LevelManager lm;
	private InfoControl info;

	private string data;
	private int lastInfo = 0;

	private float letterRate = 0.025f;
	private float waitTime = 8f;		//after showing info and blanking it

	void Start () {
		tEnemies = GameObject.Find("Enemies").transform; 
		tMeteors = GameObject.Find("Meteors").transform;
		sh = GameObject.Find("PlayerShip").GetComponent<ShipHealth>();
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		prefs = lm.gameObject.GetComponent<PrefsControl>();
		info = lm.gameObject.GetComponent<InfoControl>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		txtData.text = "";
		StartCoroutine(ChooseData());

		//*******Testing********
		string[] st = info.GetInfoArray(13, false);
		for (int i=0; i<st.Length; i++) {
			Debug.Log((i+1) + "===[" + st[i] + "]<" + st[i].Length + ">");
		}
		//**********************
	}

	IEnumerator ChooseData() {
		bool bLoop = true;
		while (bLoop) {
			lastInfo++;
			yield return new WaitForSeconds(waitTime);
			if (gm.bArcadeMode) {   //====Arcade Mode====
				if (lastInfo > 4)  { lastInfo = 1; }
				//Debug.Log("Last info(A) = " + lastInfo);
				if (lastInfo == 1) { data = MakeSectorScan(); }
				if (lastInfo == 2) { data = MakeDamageReport(); }
				if (lastInfo == 3) { data = MakeSensorScan(); }
				if (lastInfo == 4) { data = MakeBestScores(); }
			} else {    			//====Story Mode=====
				if (lastInfo > 6)  { lastInfo = 1; }
				//Debug.Log("Last info(S) = " + lastInfo);
				if (lastInfo == 1) { data = MakeSectorScan(); }
				if (lastInfo == 2) { data = MakeDamageReport(); }
				if (lastInfo == 3) { data = MakeUpgradesReport(); }
				if (lastInfo == 4) { data = MakeSensorScan(); }
				if (lastInfo == 5) { data = MakeSectorDetails(); }
				if (lastInfo == 6) { data = MakeInfo(); }
			}						//===================

			int j = data.Length;
			for (int i=0; i<j; i++) {
				txtData.text = data.Substring(0, i+1) + "_";
				yield return new WaitForSeconds(letterRate);
			}
			txtData.text = txtData.text.Substring(0, j);  //erase the _	
			yield return new WaitForSeconds(waitTime);
			txtData.text = "";
			data = "";
		}
	}

	string MakeSectorScan() {
		string s = "  <Sector Scan>" + sCRLF + sCRLF;
		int small = 0, medium = 0, big = 0;
		foreach ( Transform t in tMeteors ) {
			if (t.name.Contains(".B.")) {
				big++;
			} else if (t.name.Contains(".M.")) {
				medium++;
			} else if (t.name.Contains(".S.")) {
				small++;
			}
		}
		s = s + "Total Objects: " + (tMeteors.childCount + tEnemies.childCount) + sCRLF + sCRLF;
		s = s + "Large: " + big + sCRLF;
		s = s + "Medium: " + medium + sCRLF;
		s = s + "Small: " + small + sCRLF;
		s = s + "Enemies: " + tEnemies.childCount;

		return s;
	}

	string MakeSensorScan() {
		string s = "  <Sensor Scan>" + sCRLF + sCRLF;
		int meteor = 0, electric = 0, magnetic = 0; 
		int elecmag = 0, dense = 0, blackhole = 0;
		foreach ( Transform t in tMeteors ) {
			if 		(t.name.StartsWith("Meteor")) { meteor++; }
			else if (t.name.StartsWith("Electric")) { electric++; }
			else if (t.name.StartsWith("Magnet")) { magnetic++; }
			else if (t.name.StartsWith("Electro")) { elecmag++; }
			else if (t.name.StartsWith("Dense")) { dense++; }
			else if (t.name.StartsWith("Black")) { blackhole++; }
		}
		s = s + "Asteroids: " + meteor;
		if (!gm.bArcadeMode) {      //story mode
			if (gm.currentLevel > 1)
				{ s = s + sCRLF + "Electric mines: " + electric; }
			if (gm.currentLevel > 3)
				{ s = s + sCRLF + "Magnetic mines: " + magnetic; }
			if (gm.currentLevel > 8)
				{ s = s + sCRLF + "Electromag mines: " + elecmag; }
			if (gm.currentLevel > 13)
				{ s = s + sCRLF + "Dense asteroids: " + dense; }
			if (gm.currentLevel > 17)
				{ s = s + sCRLF + "Black hole mines: " + blackhole; }
		} else {					//arcade mode
			if (gm.currentLevel > 1)
				{ s = s + sCRLF + "Electric mines: " + electric; }
			if (gm.currentLevel > 4)
				{ s = s + sCRLF + "Magnetic mines: " + magnetic; }
			if (gm.currentLevel > 7)
				{ s = s + sCRLF + "Electromag mines: " + elecmag; }
			if (gm.currentLevel > 10)
				{ s = s + sCRLF + "Dense asteroids: " + dense; }
			if (gm.currentLevel > 14)
				{ s = s + sCRLF + "Black hole mines: " + blackhole; }
		}
		return s;
	}

	string MakeDamageReport() {
		string s = "  <Damage Report>" + sCRLF + sCRLF;
		sh = GameObject.Find("PlayerShip").GetComponent<ShipHealth>();
		int a = sh.GetHealth();
		int b = sh.maxHealth;
		ShipController sc = sh.GetComponent<ShipController>();
		int pri = prefs.GetPrimaryWeapon();
		int sec = prefs.GetSecondaryWeapon();
		s = s + "Hull Integrity: " + a + "/" + b + sCRLF;
		if      (pri == 0) { s = s + "Torpedoes: Functional" + sCRLF; } 
		else if (pri == 1) { s = s + "Lasers: Functional" + sCRLF; } 
		else if (pri == 2) { s = s + "Missiles: Functional" + sCRLF; }

		if 		(sec == 0) { s = s + "Hyperwarp: "; } 
		else if (sec == 1) { s = s + "Forcefield: "; } 
		else if (sec == 2) { s = s + "Shockwave: "; }
		if (sc.secCurrentCharge >= sc.secRechargeRate)
			{ s = s + "Functional" + sCRLF; }
		else
			{ s = s + "Recharging" + sCRLF; }

		a = gm.shipsRemaining;
		s = s + sCRLF + "Reserve Meteor Busters: " + a;
		return s;
	}

	string MakeUpgradesReport() {
		string s = "  <Upgrades Obtained>" + sCRLF + sCRLF;
		string up = prefs.GetUpgrades();
		int lev = 0;
		if (up.Length >= 3) {
			for (int i=0; i<(up.Length/3); i++) {
				s = s + prefs.UpgradeText(up.Substring(i*3, 3)) + sCRLF;
			}
		} else {
			s = s + " -----" + sCRLF;
		}
		lev = ((int)(gm.currentLevel/4)) * 4 + 4;
		if (lev < gm.finalLevel) {
			s = s + sCRLF + "Next Upgrade at Sector " + lm.SectorName(lev); 
		}
		return s;
	}

	string MakeSectorDetails() {
		string s = "  <Job Assignments>" + sCRLF + sCRLF;
		int a = gm.currentLevel;
		s = s + "Clear Sector " + lm.SectorName(a) + sCRLF + sCRLF;
		if (a < gm.finalLevel) {
			s = s + "Proceed to Sector " + lm.SectorName(a+1) + sCRLF;
		}
		return s;
	}

	string MakeInfo() {
		string s = "  <Info>";
		string i1, i2;
		int big = 0;
		foreach ( Transform t in tMeteors ) {
			if (t.name.Contains(".B."))  { big++; }
		}
		if (big > 0) {
			i1 = info.GetInfo(gm.currentLevel);
			i2 = info.GetInfo(gm.currentLevel - 1);
			if (i1 != i2 && big > 3) {
				s = "  <INFO> <NEW>";
			}
		} else {
			i1 = info.GetInfo(gm.currentLevel, true);
			i2 = info.GetInfo(gm.currentLevel - 1, true);
			if (i1 != i2) {
				s = "  <INFO> <NEW>";
			} else {    //not a new hyperspace message
				i1 = info.GetInfo(gm.currentLevel);
			}
		}
		s = s + sCRLF + sCRLF + i1;
		return s;
	}

	string MakeBestScores() {
		int i, j, score = 0;
		j = prefs.GetTopScore(-1);
		string s = "  <Top " + j.ToString() + " Scores>" + sCRLF + sCRLF;
		string name = "";
		for (i=1; i<(j+1); i++) {
			score = prefs.GetTopScore(i);
			name = prefs.GetTopScoreName(i);
			s = s + i.ToString() + "\t" + name + "\t" + score.ToString() + sCRLF;
		}
		return s;
	}

}
