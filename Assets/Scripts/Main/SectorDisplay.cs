using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SectorDisplay : MonoBehaviour {

	private Text txtSector, txtStaticSector;
	private LevelManager lm;
	private Color col;

	[SerializeField] private Material[] starfields_bg;
	[SerializeField] private Material[] starfields_fg;
	[SerializeField] private GameObject[] lights;  //0-3 Alpha-Omega  4 Headlight  5 Spotlight

	private float timeToFade = 3f;
	private float timer;
	private bool  bHideSector = true;  //to fix 'Level Clear' bug

	void Start () {
		txtSector = GetComponent<Text>();
		txtStaticSector = GameObject.Find("txtStaticSector").GetComponent<Text>();
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		col = txtSector.color;

		int round = gm.currentLevel;
		string sec; 
		if (gm.bArcadeMode) { 
			sec = lm.SectorName(round % (int)(gm.finalLevel/2));
			txtSector.text = "Level " + round; 
			txtStaticSector.text = "";
		} else {   //story mode
			sec = lm.SectorName(round);
			txtSector.text = sec;
		}

		//---------------------------------------------------------------
		// Start music   
		// Compare sectors with LevelManager.SectorName()
		int n = 0;
		MusicManager music = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		if (sec.StartsWith("Alph"))  	  { n = 1; }  //sector alpha
		else if (sec.StartsWith("Beta"))  { n = 2; }  //sector beta
		else if (sec.StartsWith("Delt"))  { n = 3; }  //sector delta
		else if (sec.StartsWith("Gamm"))  { n = 4; }  //sector gamma
		else if (sec.StartsWith("Omeg"))  { n = 5; }  //sector omega
		else { n = 1; }   //just in case
//		if (currentLevel >=  1 && currentLevel <=  5)  { n = 1; }  //sector alpha
//		if (currentLevel >=  6 && currentLevel <= 13)  { n = 2; }  //sector beta
//		if (currentLevel >= 14 && currentLevel <= 20)  { n = 3; }  //sector delta
//		if (currentLevel >= 21 && currentLevel <= 26)  { n = 4; }  //sector omega
		music.PlayMusic(n);
		//---------------------------------------------------------------

		//---------------------------------------------------------------
		// Starfield & Light select
		MeshRenderer mr   = GameObject.Find("Starfield").GetComponent<MeshRenderer>();
		MeshRenderer mr_2 = GameObject.Find("Starfield FG").GetComponent<MeshRenderer>();
		for (int i=0; i<lights.Length; i++) 
			{ lights[i].SetActive(false); }

		if (sec.StartsWith("Alph")) {
			mr.material       = starfields_bg[0];
			mr_2.material     = starfields_fg[0];
			lights[0].SetActive(true);
		} else if (sec.StartsWith("Beta")) {
			mr.material       = starfields_bg[1];
			mr_2.material     = starfields_fg[1];
			lights[1].SetActive(true);
		} else if (sec.StartsWith("Delt")) {
			mr.material       = starfields_bg[2];
			mr_2.material     = starfields_fg[2];
			lights[2].SetActive(true);
		} else if (sec.StartsWith("Gamm")) {
			mr.material       = starfields_bg[3];
			mr_2.material     = starfields_fg[3];
			lights[3].SetActive(true);
			lights[5].SetActive(true);
			lights[6].SetActive(true);
		} else if (sec.StartsWith("Omeg")) {
			mr.material       = starfields_bg[4];
			mr_2.material     = starfields_fg[4];
			lights[4].SetActive(true);
			lights[6].SetActive(true);
		} else {
			mr.material       = starfields_bg[0];
			mr_2.material     = starfields_fg[0];
			lights[0].SetActive(true);
			lights[6].SetActive(true);
		}
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
		} else if (bHideSector) {
			gameObject.transform.parent.gameObject.SetActive(false);
			bHideSector = false;
		}
	}
}
