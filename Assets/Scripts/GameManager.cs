using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int currentLevel = 0;
	public float level_width, level_height;
	public GameObject pre_Meteor;
		// add additional mines prefabs
	private Transform parMeteor;

	void Awake () {
		level_width = 100f;
		level_height = 100f;
	}

	void Start() {
		parMeteor = GameObject.Find("Meteors").gameObject.transform;
		NextLevel();
	}

	void NextLevel() {
		int numMeteors = 0, numLaserMines = 0, numMagMines = 0, numLaserMagMines = 0;
		int i;

		currentLevel++;
		if (currentLevel==1) {
			numMeteors = 8;
			numLaserMines = 0;
			numMagMines = 0;
			numLaserMagMines = 0;
		}

		//add other levels

		if (numMeteors > 0) {
			for (i = 0; i < numMeteors; i++) {
				Instantiate(pre_Meteor, parMeteor);
			}
		}

		//add other Mines instantiation
	}
}
