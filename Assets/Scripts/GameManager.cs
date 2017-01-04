using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private float spawnRange = 1.5f;  //of child mines
	public int enemyFireL = 50;  //from enemy UFOs
	public int enemyFireT = 100;  //from enemy UFOs
	public int mineFire = 100;  //from electro mines
	public int mineHit = 100;  //from hitting a mine/meteor

	public int currentLevel = 0;
	public float level_width, level_height;
	public GameObject[] pre_Meteor;   // [0]=big  [1]=med  [2]=sma
	public GameObject[] pre_Test;
	public GameObject[] pre_Magnet;
	public GameObject[] pre_Electric;
	public GameObject[] pre_ElectroMagnet;
	public GameObject[] pre_Dense;
	public GameObject[] pre_BlackHole;
	public GameObject[] pre_UFO;
		// add additional mines prefabs (array?)
	private Transform parMeteor, parTextScores, parEnemy;
	private int score = 0;
	private Text txtScore; 
	//private Text txtScorePlus;
	private string scoreFormat; //sets leading zeroes, set in Start()
	public GameObject pre_ScorePlus;
	private LayerMask myLayerMask;
	private float spawnRateUfo1 = 0f, spawnRateUfo2 = 0f;
	private float spawnTimeUfo1 = 0f, spawnTimeUfo2 = 0f;

	public enum mine {Test, Meteor, Magnet, Electric, ElectroMagnet, Dense, BlackHole, UFO01, UFO02};

	void Awake () {
		level_width = 100f;
		level_height = 100f;
	}

	void Start() {
		parMeteor = GameObject.Find("Meteors").gameObject.transform;
		parTextScores = GameObject.Find("TextScores").gameObject.transform;
		parEnemy = GameObject.Find("Enemies").gameObject.transform;
		txtScore = GameObject.Find("txtScore").GetComponent<Text>();
		//txtScorePlus = GameObject.Find("txtScorePlus").GetComponent<Text>();
		scoreFormat = txtScore.text;
		myLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Meteor")) | (1 << LayerMask.NameToLayer("Enemy"));
		//Debug.Log(myLayerMask + "=" + myLayerMask.value);
		//mine mineType;  
		NextLevel();
	}

	void NextLevel() {
		int numMeteors = 0, numElecMines = 0, numMagMines = 0;
		int numElecMagMines = 0, numDenseMines = 0, numBHMines = 0;
		spawnRateUfo1 = 0f;    //purple mothership 
		spawnRateUfo2 = 0f;    //gray ship
		int i = 0;

		currentLevel++;
		if (currentLevel==1) {
			numMeteors = 8;
			spawnRateUfo2 = 60f;
		}

		//add other levels

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
			if (size == 3) 
				{ s_Meteor = pre_Meteor[0]; }
			else if (size == 2)
				{ s_Meteor = pre_Meteor[1]; }
			else if (size == 1)
				{ s_Meteor = pre_Meteor[2]; } 
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
			if (size == 3) 
				{ s_Meteor = pre_Dense[0]; }
			else if (size == 2)
				{ s_Meteor = pre_Dense[1]; }
			else if (size == 1)
				{ s_Meteor = pre_Dense[2]; } 
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

		//Vector3 loc1 = new Vector3(0, 0, 0);
		for (i = 0; i < num; i++) {
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
		}  //TODO add more scoring to this section
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
		int num = 2 * parTextScores.childCount + 1;
		float base_x = txtScore.rectTransform.position.x;
		float base_y = txtScore.rectTransform.position.y;
		float base_z = txtScore.rectTransform.position.z;

		GameObject go;
		go = Instantiate (pre_ScorePlus, new Vector3(0f, 0f, 0f), Quaternion.identity, parTextScores) as GameObject;
		go.GetComponent<RectTransform>().position = new Vector3(base_x + 75f - (35f * num), base_y - 30f, base_z);
		go.GetComponent<Text>().text = "+" + pts.ToString();
		StartCoroutine (DestroyText(go));
	}

	private IEnumerator DestroyText(GameObject obj) {
		yield return new WaitForSeconds(2.5f);
		Destroy(obj);
		if (parTextScores.childCount > 0) {
			foreach (Transform t in parTextScores) {
				t.transform.position = new Vector3(t.transform.position.x + 70f, t.transform.position.y, t.transform.position.z);
			}
		}
	}

//	void ClearScore() {
//		txtScorePlus.text = "";
//	}

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
