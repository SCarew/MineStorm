using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	private int score;
	private PrefsControl prefs;

	void Start () {
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		score = prefs.GetGameStats(PrefsControl.stats.Score);

		//play music 
		MusicManager music = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		music.PlayMusic(6);  //hyperspace music
	}

	public void AddScore(int amount) {
		score += amount;
	}

	public int GetScore() {
		return score;
	}

	void OnDestroy() {
		//if (GameObject.Find("Hyp_PlayerShip").GetComponent<HypShipHealth>().isDead == false) {
		if (!prefs.isHypDead) {   //only save score if player not dead
			prefs.SetGameStats(PrefsControl.stats.Score, score);
		}
	}

}
