using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private static bool isNotFirstInstance = false;
	private string sceneName01 = "", sceneName02 = "";

	void Start () {
		if (isNotFirstInstance) {    //make singleton
			Destroy(gameObject);
		} else {
			isNotFirstInstance = true;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadNextScene() {
		Debug.Log("Scenes queued: " + sceneName01 + ", " + sceneName02);
		if (sceneName01 == "") {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			return;
		}
		string sceneName0 = sceneName01;
		sceneName01 = sceneName02;
		sceneName02 = "";
		
		SceneManager.LoadScene(sceneName0);
	}

	public void LoadScene(string sceneName0, string sceneName1 = "", string sceneName2 = "") {
		sceneName01 = sceneName1;
		sceneName02 = sceneName2;
		SceneManager.LoadScene(sceneName0);
		Debug.Log("Scenes queuing: " + sceneName01 + ", " + sceneName02);
	}

	public string SectorName(int round) {
		// Compare with SectorDisplay.Start(), GameManager.NextLevel()
		if (round > 26 || round < 0) { return ""; }

		int alpha = 6, beta = 14, delta = 21, omega = 27;
			//sectors 

		string s = "";
		if (round < alpha) {
		 	s = "Alpha";
		} 
		else if (round < beta) {
		 	s = "Beta";  
			round = round+1 - alpha;
		} 
		else if (round < delta) {
		 	s = "Delta"; 
			round = round+1 - beta;
		} 
		else if (round < omega) {
		 	s = "Omega"; 
			round = round+1 - delta;
		} 
		if (round < 1)        { s = s + " Null"; }
		else if (round == 1)  { s = s + " I"; }
		else if (round == 2)  { s = s + " II"; }
		else if (round == 3)  { s = s + " III"; }
		else if (round == 4)  { s = s + " IV"; }
		else if (round == 5)  { s = s + " V"; }
		else if (round == 6)  { s = s + " VI"; }
		else if (round == 7)  { s = s + " VII"; }
		else if (round == 8)  { s = s + " VIII"; }
		else if (round == 9)  { s = s + " IX"; }
		else if (round >= 10) { s = s + " X"; }

		//s = s + " - " + round.ToString();
		return s;
	}
}
