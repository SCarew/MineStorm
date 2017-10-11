using System.Collections;
using UnityEngine;

public class FinishClear : MonoBehaviour {

	private PrefsControl prefs;

	void Start () {
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		Invoke("ClearSavedData", 2f);
	}

	void ClearSavedData() {
		prefs.SetGameStats(PrefsControl.stats.Level, 0);
		prefs.SetGameStats(PrefsControl.stats.Score, 0);
		prefs.SetGameStats(PrefsControl.stats.Ships, 0);
		if (prefs.GetGameType() != "Arcade")
			{ prefs.ReplaceUpgrade(""); }
	}
}
