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
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		col = txtSector.color;

		int round = gm.currentLevel;
		string sec = lm.SectorName(round);
		txtSector.text = sec;
		Debug.Log("Round = " + round);

		//---------------------------------------------------------------
		// Start music   
		// Compare sectors with LevelManager.SectorName()
		int n = 0;
		MusicManager music = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		if (sec.StartsWith("Alph"))  	  { n = 1; }  //sector alpha
		else if (sec.StartsWith("Beta"))  { n = 2; }  //sector beta
		else if (sec.StartsWith("Delt"))  { n = 3; }  //sector delta
		else if (sec.StartsWith("Omeg"))  { n = 4; }  //sector omega
		else { n = 1; }   //just in case
//		if (currentLevel >=  1 && currentLevel <=  5)  { n = 1; }  //sector alpha
//		if (currentLevel >=  6 && currentLevel <= 13)  { n = 2; }  //sector beta
//		if (currentLevel >= 14 && currentLevel <= 20)  { n = 3; }  //sector delta
//		if (currentLevel >= 21 && currentLevel <= 26)  { n = 4; }  //sector omega
		music.PlayMusic(n);
		//---------------------------------------------------------------

		//---------------------------------------------------------------
		// TODO Change starfield
		//---------------------------------------------------------------

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
