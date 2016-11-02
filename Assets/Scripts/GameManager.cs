using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private float spawnRange = 1.5f;
	public int currentLevel = 0;
	public float level_width, level_height;
	public GameObject pre_B_Meteor;
	public GameObject pre_M_Meteor;
	public GameObject pre_S_Meteor;
	public GameObject pre_B_Test;
	public GameObject pre_M_Test;
	public GameObject pre_S_Test;
	public GameObject pre_B_Magnet;
	public GameObject pre_M_Magnet;
	public GameObject pre_S_Magnet;
	public GameObject pre_B_Electric;
	public GameObject pre_M_Electric;
	public GameObject pre_S_Electric;
		// add additional mines prefabs (array?)
	private Transform parMeteor;
	private int score = 0;
	private Text txtScore, txtScorePlus;
	private string scoreFormat; //sets leading zeroes, set in Start()

	void Awake () {
		level_width = 100f;
		level_height = 100f;
	}

	void Start() {
		parMeteor = GameObject.Find("Meteors").gameObject.transform;
		txtScore = GameObject.Find("txtScore").GetComponent<Text>();
		txtScorePlus = GameObject.Find("txtScorePlus").GetComponent<Text>();
		scoreFormat = txtScore.text;
		NextLevel();
	}

	void NextLevel() {
		int numMeteors = 0, numLaserMines = 0, numMagMines = 0, numLaserMagMines = 0;
		int i;

		currentLevel++;
		if (currentLevel==1) {
			numMeteors = 8;
			numMagMines = 0;
			numLaserMines = 0;
			numLaserMagMines = 0;
		}

		//add other levels

		if (numMeteors > 0) {
			SpawnMeteor(1, 3, numMeteors);
		}
		if (numMagMines > 0) {
			SpawnMeteor(2, 3, numMagMines);
		}
		if (numLaserMines > 0) {
			SpawnMeteor(3, 3, numLaserMines);
		}
		if (numLaserMagMines > 0) {
			SpawnMeteor(4, 3, numLaserMagMines);
		}

		//add other Mines instantiation
	}

	public void SpawnMeteor (int type, int size, int num) {
		SpawnMeteor(type, size, num, new Vector3 (0f, 0f, 1f));
	}

	public void SpawnMeteor (int type, int size, int num, Vector3 loc) {
		GameObject s_Meteor = pre_B_Meteor; //set to default
		GameObject go;
		int i;
		bool child = (loc.z != 1f);

		if (type == 1) {   //Meteor
			if (size == 3) 
				{ s_Meteor = pre_B_Meteor; }
			else if (size == 2)
				{ s_Meteor = pre_M_Meteor; }
			else if (size == 1)
				{ s_Meteor = pre_S_Meteor; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == 0) {   //Test blocks
			if (size == 3) 
				{ s_Meteor = pre_B_Test; }
			else if (size == 2)
				{ s_Meteor = pre_M_Test; }
			else if (size == 1)
				{ s_Meteor = pre_S_Test; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == 2) {   //Magnet
			if (size == 3) 
				{ s_Meteor = pre_B_Magnet; }
			else if (size == 2)
				{ s_Meteor = pre_M_Magnet; }
			else if (size == 1)
				{ s_Meteor = pre_S_Magnet; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}
		if (type == 3) {   //Electric
			if (size == 3) 
				{ s_Meteor = pre_B_Electric; }
			else if (size == 2)
				{ s_Meteor = pre_M_Electric; }
			else if (size == 1)
				{ s_Meteor = pre_S_Electric; } 
			else 
				{ Debug.LogError("Spawn Unknown Meteor"); }
		}

		for (i = 0; i < num; i++) {
			go = Instantiate (s_Meteor, new Vector3(level_width/2, level_height/2, 5f), Quaternion.identity, parMeteor) as GameObject;
			go.GetComponent<EnemyHealth>().SetType(type);
			if (child) {
				loc += new Vector3 (Random.Range(0f, spawnRange*2) - spawnRange, Random.Range(0f, spawnRange*2) - spawnRange, 0);
				if (go.GetComponentInChildren<MeteorControl>() == null) //TODO simplify this
					{ go.GetComponentInChildren<MeteorControl2>().SetLocation(loc); }
				else
					{ go.GetComponentInChildren<MeteorControl>().SetLocation(loc); }
			}
		}
	}

	public void AddScore (int type, int size) {
		int points = 0;

		//Calculate points +++
		if (type == 1) {  //Rock meteor
			points = size * 50;
		} else if (type == 2) {  //Magnetic mine
			points = size * 60;
		} else if (type == 3) {  //Electric mine
			points = size * 75;
		}  //TODO add more scoring to this section
		score += points;

		ShowPoints(points);
		txtScore.text = score.ToString(scoreFormat);
	}

	private void ShowPoints(int pts) {
		//TODO instantiate a text component showing points
		string t = txtScorePlus.text;
		if (t != "")  { t = " " + t; }
		txtScorePlus.text = "+" + pts.ToString() + t;
		StartCoroutine(ClearScore());
	}

	IEnumerator ClearScore() {
		yield return new WaitForSeconds(2f);
		txtScorePlus.text = "";
	}
}
