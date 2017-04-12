﻿using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	private int score;
	private PrefsControl prefs;

	void Start () {
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		score = prefs.GetGameStats(PrefsControl.stats.Score);
	}

	public void AddScore(int amount) {
		score += amount;
	}

	public int GetScore() {
		return score;
	}

	void OnDestroy() {
		prefs.SetGameStats(PrefsControl.stats.Score, score);
	}

}