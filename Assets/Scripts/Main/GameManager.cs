using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private float spawnRange = 1.5f;  //of child mines
	public int enemyFireL = 50;  //from enemy UFOs
	public int enemyFireT = 100;  //from enemy UFOs
	public int mineFire = 50;  //from electro mines
	public int mineBHit = 100;  //from hitting a mine/meteor
	public int mineMHit = 70;  //from hitting a mine/meteor
	public int mineSHit = 35;  //from hitting a mine/meteor

	private PrefsControl prefs;
	private LevelManager lm;
	public int currentLevel = 0;
	public int finalLevel = 26;
	public int shipsRemaining = 0;
	public float level_width, level_height;
	public GameObject[] pre_Meteor;  
	public GameObject[] pre_Test;       // [0]=big  [1]=med  [2]=sma
	public GameObject[] pre_Magnet;
	public GameObject[] pre_Electric;
	public GameObject[] pre_ElectroMagnet;
	public GameObject[] pre_Dense;
	public GameObject[] pre_BlackHole;
	public GameObject[] pre_UFO;
	private int metBig = 0, metMed = 0, metSma = 0;
	private int denBig = 0, denMed = 0, denSma = 0;

	private Transform parMeteor, parTextScores, parEnemy;  //parents for children
	private int score = 0;
	private Text txtScore; //, txtScorePlus; 
	private Text[] txtPlus;
	private string scoreFormat;   //sets leading zeroes, set in Start()
	//public GameObject pre_ScorePlus;
	private LayerMask myLayerMask;
	private float spawnRateUfo1 = 0f, spawnRateUfo2 = 0f;
	private float spawnTimeUfo1 = 0f, spawnTimeUfo2 = 0f;

	private GameOverMenu gameOverMenu;
	public bool bGameOver = false;
	public bool bArcadeMode = false;
	[SerializeField] private GameObject panFadeIn;

	public enum mine {Test, Meteor, Magnet, Electric, ElectroMagnet, Dense, BlackHole, UFO01, UFO02};

	void Awake () {
		level_width = 100f;
		level_height = 100f;
	}

	void Start() {
		panFadeIn.SetActive(true);
		parMeteor = GameObject.Find("Meteors").gameObject.transform;
		parTextScores = GameObject.Find("parScorePlus").gameObject.transform;
		parEnemy = GameObject.Find("Enemies").gameObject.transform;
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		gameOverMenu = GameObject.Find("GameOverMenu").GetComponent<GameOverMenu>();
		txtScore = GameObject.Find("txtScore").GetComponent<Text>();
		//txtScorePlus = parTextScores.GetComponent<Text>();
		scoreFormat = txtScore.text;
		txtPlus = new Text[3];
		txtPlus[0] = parTextScores.Find("txtScorePlus0").GetComponent<Text>();
		txtPlus[1] = parTextScores.Find("txtScorePlus1").GetComponent<Text>();
		txtPlus[2] = parTextScores.Find("txtScorePlus2").GetComponent<Text>();

		myLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Meteor")) | (1 << LayerMask.NameToLayer("Enemy"));
		//Debug.Log(myLayerMask + "=" + myLayerMask.value);
		myLayerMask = myLayerMask | (1 << LayerMask.NameToLayer("MapPlayer")) | (1 << LayerMask.NameToLayer("MapMeteor")) | (1 << LayerMask.NameToLayer("MapEnemy"));
		//Debug.Log(myLayerMask + "=" + myLayerMask.value);
		//mine mineType;  
		for (int i=0; i < pre_Meteor.Length; i++) {   //set up for PickMeteor()
			if (pre_Meteor[i].name.Contains(".B.")) { metBig++; }
			if (pre_Meteor[i].name.Contains(".M.")) { metMed++; }
			if (pre_Meteor[i].name.Contains(".S.")) { metSma++; }
		}
		for (int i=0; i < pre_Dense.Length; i++) {   //set up for PickMeteor()
			if (pre_Dense[i].name.Contains(".B.")) { denBig++; }
			if (pre_Dense[i].name.Contains(".M.")) { denMed++; }
			if (pre_Dense[i].name.Contains(".S.")) { denSma++; }
		}
		if (prefs.GetGameType() == "Arcade")  { bArcadeMode = true; }
		if (prefs.GetGameType() == "Continue")  { SetupContinueMode(); }
		if (prefs.GetCameraMode() == true) { Invoke("CameraModeSet", 0.1f); }

		NextLevel();
	}

	void NextLevel() {
		int numMeteors = 0, numElecMines = 0, numMagMines = 0;
		int numElecMagMines = 0, numDenseMines = 0, numBHMines = 0;
		spawnRateUfo1 = 0f;    //purple mothership 
		spawnRateUfo2 = 0f;    //gray ship
		//int i = 0;

		UpdateForSceneReload();
		if (currentLevel < 1) { currentLevel = 1; }
		UpdateForContinueMode();
		if (currentLevel==1) {
			numMeteors = 8;
			spawnRateUfo2 = 65f;
		}
		if (currentLevel==2) {
			numMeteors = 6;
			numElecMines = 2;
			spawnRateUfo2 = 65f;
		}
		if (currentLevel==3) {
			numMeteors = 7;
			numElecMines = 4;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==4) {
			numMeteors = 6;
			numElecMines = 3;
			numMagMines = 1;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==5) {
			numMeteors = 4;
			numMagMines = 4;
			spawnRateUfo1 = 115f;
		}
		if (currentLevel==6) {     //beta starts
			numMeteors = 3;
			numElecMines = 3;
			numMagMines = 3;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==7) {
			numMeteors = 3;
			numElecMines = 4;
			numMagMines = 4;
			spawnRateUfo1 = 120f;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==8) {
			numMeteors = 3;
			numElecMines = 5;
			numMagMines = 5;
			spawnRateUfo1 = 110f;
			spawnRateUfo2 = 55f;
		}
		if (currentLevel==9) {
			numMeteors = 7;
			numElecMagMines = 2;
			spawnRateUfo2 = 50f;
		}
		if (currentLevel==10) {
			numMeteors = 3;
			numElecMagMines = 3;
			numMagMines = 3;
			spawnRateUfo1 = 120f;
		}
		if (currentLevel==11) {
			numMeteors = 4;
			numElecMines = 3;
			numElecMagMines = 3;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==12) {     //delta begins
			numMeteors = 5;
			numElecMagMines = 5;
			spawnRateUfo1 = 120f;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==13) {
			numMeteors = 3;
			numElecMines = 3;
			numMagMines = 3;
			numElecMagMines = 3;
			spawnRateUfo1 = 110f;
		}
		if (currentLevel==14) {
			numMeteors = 3;
			numDenseMines = 5;
			spawnRateUfo2 = 50f;
		}
		if (currentLevel==15) {
			numMeteors = 1;
			numElecMines = 1;
			numMagMines = 1;
			numElecMagMines = 1;
			numDenseMines = 7;
			spawnRateUfo1 = 100f;
			spawnRateUfo2 = 50f;
		}
		if (currentLevel==16) {     //gamma begins
			numMeteors = 2;
			numElecMines = 2;
			numMagMines = 3;
			numElecMagMines = 3;
			numDenseMines = 4;
			spawnRateUfo1 = 100f;
			spawnRateUfo2 = 50f;
		}
		if (currentLevel==17) {
			numMeteors = 1;
			numElecMines = 3;
			numMagMines = 3;
			numElecMagMines = 3;
			numDenseMines = 6;
			spawnRateUfo1 = 115f;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==18) {
			numMeteors = 5;
			numDenseMines = 5;
			numBHMines = 2;
			spawnRateUfo1 = 120f;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==19) {
			numDenseMines = 6;
			numBHMines = 4;
			spawnRateUfo1 = 100f;
			spawnRateUfo2 = 50f;
		}
		if (currentLevel==20) {
			numMeteors = 4;
			numElecMines = 4;
			numMagMines = 2;
			numElecMagMines = 2;
			numDenseMines = 2;
			numBHMines = 2;
			spawnRateUfo1 = 80f;
			spawnRateUfo2 = 45f;
		}
		if (currentLevel==21) {
			numMeteors = 3;
			numElecMines = 3;
			numMagMines = 3;
			numElecMagMines = 3;
			numDenseMines = 3;
			numBHMines = 3;
			spawnRateUfo1 = 100f;
			spawnRateUfo2 = 50f;
		}
		if (currentLevel==22) {     //omega begins
			numMagMines = 4;
			numElecMagMines = 4;
			numBHMines = 6;
			spawnRateUfo1 = 150f;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==23) {
			numMagMines = 4;
			numElecMagMines = 3;
			numDenseMines = 5;
			numBHMines = 4;
			spawnRateUfo1 = 120f;
			spawnRateUfo2 = 60f;
		}
		if (currentLevel==24) {
			numMeteors = 3;
			numElecMines = 3;
			numMagMines = 3;
			numElecMagMines = 4;
			numDenseMines = 4;
			numBHMines = 5;
			spawnRateUfo1 = 110f;
			spawnRateUfo2 = 75f;
		}
		if (currentLevel==25) {
			numMeteors = 1;
			numElecMines = 6;
			numMagMines = 4;
			numElecMagMines = 4;
			numDenseMines = 4;
			numBHMines = 4;
			spawnRateUfo1 = 100f;
			spawnRateUfo2 = 50f;
		}
		if (currentLevel==26) {
			numElecMines = 5;
			numMagMines = 5;
			numElecMagMines = 5;
			numDenseMines = 3;
			numBHMines = 6;
			spawnRateUfo1 = 80f;
			spawnRateUfo2 = 40f;
		}

		//============ Redo Arcade Level ============
		if (bArcadeMode) {
			int numTotal = 6 + (int)(currentLevel/5) + Random.Range(0, 1 + (int)(currentLevel / 2));
			float maxType = 1f, minType = 0f;
			int thisMeteor = 0;
			level_width = 100f;
			level_height = 100f;
			numMeteors = 0;
			numElecMines = 0;
			numMagMines = 0;
			numElecMagMines = 0;
			numDenseMines = 0;
			numBHMines = 0;
			spawnRateUfo1 = 0f;
			spawnRateUfo2 = 0f;	
			if (currentLevel < 2)   { maxType = 0.99f; }
			if (currentLevel == 2)  { maxType = 1.34f; }
			if (currentLevel == 3)  { maxType = 1.75f; }
			if (currentLevel == 4)  { maxType = 2.00f; }
			if (currentLevel == 5)  { maxType = 2.40f; }
			if (currentLevel == 6)  { maxType = 2.80f; }
			if (currentLevel == 7)  { maxType = 3.00f; }
			if (currentLevel == 8)  { maxType = 3.40f; }
			if (currentLevel == 9)  { maxType = 3.75f; }
			if (currentLevel == 10) { maxType = 3.99f; }
			if (currentLevel > 10)  { maxType = 4.00f + ((currentLevel-10) / 3f); }
			if (currentLevel == 14) { maxType = 5.00f; }
			if (currentLevel > 14)  { maxType = 5.00f + ((currentLevel-14) / 4f); }
			if (currentLevel >= 18) { maxType = 5.99f; }
			minType = maxType / 5f;
			for (int i=0; i<numTotal; i++) {
				thisMeteor = (int)(1 + Random.Range(minType, maxType));
				if      (thisMeteor == 1) { numMeteors++; }
				else if (thisMeteor == 2) { numElecMines++; }
				else if (thisMeteor == 3) { numMagMines++; }
				else if (thisMeteor == 4) { numElecMagMines++; }
				else if (thisMeteor == 5) { numDenseMines++; }
				else if (thisMeteor >= 6) { numBHMines++; }
			}
			if      (currentLevel < 6)  { spawnRateUfo1 = 250f;  spawnRateUfo2 = 65f; }
			else if (currentLevel < 11) { spawnRateUfo1 = 200f;  spawnRateUfo2 = 60f; }
			else if (currentLevel < 16) { spawnRateUfo1 = 160f;  spawnRateUfo2 = 55f; }
			else if (currentLevel < 21) { spawnRateUfo1 = 130f;  spawnRateUfo2 = 50f; }
			else                        { spawnRateUfo1 = 100f;  spawnRateUfo2 = 50f; }

			//Debug.Log("Level:" + currentLevel + "  UFO1:" + spawnRateUfo1 + "  UFO2:" + spawnRateUfo2);
			//Debug.Log("Meteors:" + numTotal + "  minType:" + minType + "  maxType:" + maxType);
			//Debug.Log("Met/Ele/Mag/ElM/Den/BHM: " + numMeteors + "/" + numElecMines + "/" + numMagMines + "/" + numElecMagMines + "/" + numDenseMines + "/" + numBHMines);
		}
		//===========================================


		if (numMeteors > 0) {
			SpawnMeteor(mine.Meteor, 3, numMeteors);
		}
		if (numMagMines > 0) {
			SpawnMeteor(mine.Magnet, 3, numMagMines);
		}
		if (numElecMines > 0) {
			SpawnMeteor(mine.Electric, 3, numElecMines);
		}
		if (numElecMagMines > 0) {
			SpawnMeteor(mine.ElectroMagnet, 3, numElecMagMines);
		}
		if (numDenseMines > 0) {
			SpawnMeteor(mine.Dense, 3, numDenseMines);
		}
		if (numBHMines > 0) {
			SpawnMeteor(mine.BlackHole, 3, numBHMines);
		}
		//add other Mines instantiation

		if (spawnRateUfo1 > 0f) {
			spawnTimeUfo1 = Random.Range(spawnRateUfo1, spawnRateUfo1 * 2);
		}
		if (spawnRateUfo2 > 0f) {
			spawnTimeUfo2 = Random.Range(spawnRateUfo2, spawnRateUfo2 * 2);
		}
		StartCoroutine(CheckForLevelEnd());
	}

	void SetupContinueMode()
	{
		currentLevel = prefs.GetGameStats(PrefsControl.stats.Level);
		score = prefs.GetGameStats(PrefsControl.stats.Score);
		shipsRemaining = prefs.GetGameStats(PrefsControl.stats.Ships);
	}

	void UpdateForContinueMode() {
		if (bArcadeMode) { return; }
		prefs.SetGameStats(PrefsControl.stats.Level, currentLevel);
		prefs.SetGameStats(PrefsControl.stats.Score, score);
		prefs.SetGameStats(PrefsControl.stats.Ships, shipsRemaining);
	}

	void UpdateForSceneReload() {
		currentLevel = prefs.GetGameStats(PrefsControl.stats.Level);
		score = prefs.GetGameStats(PrefsControl.stats.Score);
		shipsRemaining = prefs.GetGameStats(PrefsControl.stats.Ships);
		txtScore.text = score.ToString(scoreFormat);
	}

	void CameraModeSet() {
		GameObject.Find("Main Camera").GetComponent<CameraController>().fixedCam = true;
	}

	void LevelClear() {
		if (bArcadeMode) {
			GameObject.FindObjectOfType<PanelController>().ShowMessage("Level Clear");
			currentLevel++;
			panFadeIn.GetComponent<HypFader>().ResetTimer(true, 2.5f);
			prefs.SetGameStats(PrefsControl.stats.Level, currentLevel);
			prefs.SetGameStats(PrefsControl.stats.Score, score);
			prefs.SetGameStats(PrefsControl.stats.Ships, shipsRemaining);
			return;
		}

		//+++ Story mode only
		currentLevel++;
		GameObject.Find("Canvas").GetComponent<PanelController>().ShowMessage("Sector Clear");
		//TODO open warp
		panFadeIn.GetComponent<HypFader>().ResetTimer(true, 2.5f);
		if (currentLevel < finalLevel) {  //final level = 26?
			//NextLevel();  //TODO instead, go to hyperspace scene
			prefs.SetGameStats(PrefsControl.stats.Level, currentLevel);
			prefs.SetGameStats(PrefsControl.stats.Score, score);
			prefs.SetGameStats(PrefsControl.stats.Ships, shipsRemaining);
		} else {
			lm.LoadScene("Finish");
		}
	}

	public void GotoNextScene() {
		if (bArcadeMode) {
			lm.LoadScene("Main");
		} else {
			lm.LoadScene("Hyperspace");
		}
	}

	public void LoseShip() {
		shipsRemaining--;

	}

	public void ShowGameOver() {
		bGameOver = true;
		gameOverMenu.LaunchGameOver();
		//canvasGameOver.SetActive(true);
	}

	public void SpawnMeteor (mine type, int size, int num) {
		SpawnMeteor(type, size, num, new Vector3 (0f, 0f, 1f));
	}

	public void SpawnMeteor (mine type, int size, int num, Vector3 loc) {
		GameObject s_Meteor = pre_Meteor[0]; //set to default
		GameObject go;
		int i;
		bool child = (loc.z != 1f);

		if (type == mine.Meteor) {   //Meteor
			if ((size <= 3) && (size >= 1)) 
				{ s_Meteor = PickMeteor(type, size); }
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == mine.Test) {   //Test blocks
			if (size == 3) 
				{ s_Meteor = pre_Test[0]; }
			else if (size == 2)
				{ s_Meteor = pre_Test[1]; }
			else if (size == 1)
				{ s_Meteor = pre_Test[2]; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == mine.Magnet) {   //Magnet
			if (size == 3) 
				{ s_Meteor = pre_Magnet[0]; }
			else if (size == 2)
				{ s_Meteor = pre_Magnet[1]; }
			else if (size == 1)
				{ s_Meteor = pre_Magnet[2]; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == mine.Electric) {   //Electric
			if (size == 3) 
				{ s_Meteor = pre_Electric[0]; }
			else if (size == 2)
				{ s_Meteor = pre_Electric[1]; }
			else if (size == 1)
				{ s_Meteor = pre_Electric[2]; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == mine.ElectroMagnet) {  
			if (size == 3) 
				{ s_Meteor = pre_ElectroMagnet[0]; }
			else if (size == 2)
				{ s_Meteor = pre_ElectroMagnet[1]; }
			else if (size == 1)
				{ s_Meteor = pre_ElectroMagnet[2]; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == mine.Dense) { 
			if ((size <= 3) && (size >= 1)) 
				{ s_Meteor = PickMeteor(type, size); }
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == mine.BlackHole) { 
			if (size == 3) 
				{ s_Meteor = pre_BlackHole[0]; }
			else if (size == 2)
				{ s_Meteor = pre_BlackHole[1]; }
			else if (size == 1)
				{ s_Meteor = pre_BlackHole[2]; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}

		if (child) {   //chance to spawn 3 children rather than 2
			float chance = 0.05f;
			if (bArcadeMode) { chance = chance * 2f; }
			if (type == mine.Meteor || type == mine.Dense || type == mine.Test) 
				{ chance = chance * 2f; }
			if (chance > Random.Range(0f, 1f)) { num++; }
		}
		for (i = 0; i < num; i++) {
			if (type == mine.Meteor || type == mine.Dense) 
				{ s_Meteor = PickMeteor(type, size); }  //refresh for each iteration
			go = Instantiate (s_Meteor, new Vector3(level_width/2, level_height/2, 5f), Quaternion.identity, parMeteor) as GameObject;
			go.GetComponent<EnemyHealth>().SetType(type);
			if (child) {
				if (type == mine.Meteor || type == mine.Dense || type == mine.Test) {
					loc += new Vector3 (Random.Range(0f, spawnRange*2) - spawnRange, Random.Range(0f, spawnRange*2) - spawnRange, 0);
				} else {   //mine spawned from whirlpool
					loc += new Vector3 (Random.Range(0f, spawnRange*8) - spawnRange * 4, Random.Range(0f, spawnRange*8) - spawnRange * 4, 0);
				}
//				if (Vector3.Distance(loc, loc1) < (size / 2)) {
//					Debug.Log("Altering child spawn loc " + loc + " of " + go.name + " bec of " + loc1);
//					loc -= new Vector3(1f, 1f, 0f);
//				}
				while (FreeLocation(loc, (float) size / 2f) == false) {
					Debug.Log(" by " + go.name + " at " + loc);
					loc -= new Vector3(1f, 1f, 0f);
				}
				go.GetComponentInChildren<MeteorControl>().SetLocation(loc);
				//loc1 = loc;
			}
		}
	}

	private GameObject PickMeteor(mine meteorType, int meteorSize) {
		int r = 0;

		if (meteorType == mine.Meteor) {
			if (meteorSize == 3) { r = Random.Range(0, metBig); }
			if (meteorSize == 2) { r = metBig + Random.Range(0, metMed); }
			if (meteorSize == 1) { r = metBig + metMed + Random.Range(0, metSma); }
			//Debug.Log("#" + r + ": " + pre_Meteor[r].name);
			return pre_Meteor[r];
		}

		if (meteorType == mine.Dense) {
			if (meteorSize == 3) { r = Random.Range(0, denBig); }
			if (meteorSize == 2) { r = denBig + Random.Range(0, denMed); }
			if (meteorSize == 1) { r = denBig + denMed + Random.Range(0, denSma); }
			//Debug.Log("#" + r + ": " + pre_Dense[r].name);
			return pre_Dense[r];
		}

		Debug.Log("Error in PickMeteor()");
		return null;
	}

	public bool FreeLocation (Vector3 coords, float fSize) {
		//returns true when location is free
		Collider[] hitColl = Physics.OverlapSphere(coords, fSize, myLayerMask.value);
		if (hitColl.Length > 0) {Debug.Log ("Overlap with " + hitColl[0].gameObject.name);}
		return (hitColl.Length == 0);
	}

	private void SpawnUFO (int ufoType) {
		Transform pShip = GameObject.FindGameObjectWithTag("Player").transform;
		if (pShip == null) {
			Debug.LogError("Missing player ship for UFO spawn");
		}
		Vector3 loc = pShip.position + new Vector3(Random.Range(0, level_width) - (level_width/2), Random.Range(0, level_height) - (level_height/2), 0f);
		while (FreeLocation(loc, 3) == false) {
			Debug.Log("UFO" + ufoType + " moved spawn from " + loc);
			loc -= new Vector3(1f, 1f, 0f);
		}

		GameObject go = Instantiate(pre_UFO[ufoType - 1], loc, Quaternion.identity) as GameObject; 
		go.transform.SetParent(parEnemy);
		go.name = "UFO.0" + ufoType.ToString();
		go.GetComponent<EnemyHealth>().SetHealth(75 * (3 - ufoType));
//		if (ufoType == 1) {
//			spawnTimeUfo1 += go.GetComponent<UFOController>().TimeToWarp();
//		} else {
//			spawnTimeUfo2 += go.GetComponent<UFOController>().TimeToWarp();
//		}
	}

	public void AddScore (mine type, int size) {
		int points = 0;

		//Calculate points +++
		if (type == mine.Meteor) {  //Rock meteor
			points = size * 50;
		} else if (type == mine.Test) {  //Test mine
			points = size * 5;
		} else if (type == mine.Magnet) {  //Magnetic mine
			points = size * 60;
		} else if (type == mine.Electric) {  //Electric mine
			points = size * 75;
		} else if (type == mine.ElectroMagnet) {  //ElectroMag mine
			points = size * 90;
		} else if (type == mine.Dense) {  //Dense mine
			points = size * 120;
		} else if (type == mine.BlackHole) {  //Blackhole mine
			points = size * 100;
		} else if (type == mine.UFO01) {  //Large UFO
			points = 450;
		} else if (type == mine.UFO02) {  //Small UFO
			points = 300;
		}  
		score += points;

		ShowPoints(points);
		txtScore.text = score.ToString(scoreFormat);
	}

//	private void ShowPoints(int pts) {
//		string t = txtScorePlus.text;
//		if (t != "")  { t = " " + t; }
//		txtScorePlus.text = "+" + pts.ToString() + t;
//		CancelInvoke();
//		Invoke (ClearScore(), 2f);
//	}

	private void ShowPoints(int pts) {		
		txtPlus[2].text = txtPlus[1].text;
		txtPlus[1].text = txtPlus[0].text;
		txtPlus[0].text = "+" + pts.ToString(); 
		Invoke ("DestroyText", 2.5f);
	}

	private void DestroyText() {
		if (txtPlus[2].text != "") {
			txtPlus[2].text = "";
		} else if (txtPlus[1].text != "") {
			txtPlus[1].text = "";
		} else {
			txtPlus[0].text = "";
		}
	}

	private IEnumerator CheckForLevelEnd() {
		bool bEnd = false;
		Transform t1 = GameObject.Find("Meteors").transform;
		Transform t2 = GameObject.Find("Enemies").transform;
		while (!bEnd) {
			if (t1.childCount + t2.childCount < 1) { bEnd = true; }
			yield return new WaitForSeconds(0.5f);

			if (bEnd) {    //make sure all spawning child mines have spawned
				yield return new WaitForSeconds(0.15f);
				if (t1.childCount + t2.childCount > 0) { bEnd = false; }
			}
		}
		LevelClear();
	}

	void Update() {
		if (spawnRateUfo1 > 0) {
			spawnTimeUfo1 -= Time.deltaTime; 
			if (spawnTimeUfo1 < 0) {
				SpawnUFO(1);
				spawnTimeUfo1 = Random.Range(spawnRateUfo1, spawnRateUfo1 * 2) + spawnRateUfo1;
			}
		}
		if (spawnRateUfo2 > 0) {
			spawnTimeUfo2 -= Time.deltaTime; 
			if (spawnTimeUfo2 < 0) {
				SpawnUFO(2);
				spawnTimeUfo2 = Random.Range(spawnRateUfo2, spawnRateUfo2 * 2) + spawnRateUfo2;
			}
		}
	}

}
