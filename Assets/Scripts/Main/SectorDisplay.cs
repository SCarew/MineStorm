using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SectorDisplay : MonoBehaviour {

	private Text txtSector, txtStaticSector;
	private LevelManager lm;
	private Color col;

	private float timeToFade = 3f;
	private float timer;

	void Start () {
		txtSector = GetComponent<Text>();
		txtStaticSector = GameObject.Find("txtStaticSector").GetComponent<Text>();
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		col = txtSector.color;

		int round = GameObject.Find("GameManager").GetComponent<GameManager>().currentLevel;
		txtSector.text = lm.SectorName(round);
		Debug.Log("Round = " + round);

		timer = timeToFade;
	}

	void Update() {
		if (timer > 0f) { 
			float t = Time.deltaTime;
			timer -= t; 
			col.a -= t / timeToFade;
			txtSector.color = col;
			txtStaticSector.color = col;
		} else {
			gameObject.transform.parent.gameObject.SetActive(false);
		}
	}
}
