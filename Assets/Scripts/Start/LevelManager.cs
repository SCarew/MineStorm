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

	void Update () {
		
	}
}
