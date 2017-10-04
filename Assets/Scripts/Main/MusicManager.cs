using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	[SerializeField] private GameObject pre_MusicEffect;
	private Transform parAudio;   //for empty parent container
	private float musicVolume = 0.5f;
	private int currentSceneNum = 0;
	private float insertTimer = 0f;
	private float insertTimerRange = 20f;    //time between inserts, +/-50%
	private int lastInsertNum = -1;    //to keep from duplicating inserts consecutively

	[SerializeField] private AudioClip mainTheme;
	[SerializeField] private AudioClip Sa_bg_theme;
	[SerializeField] private AudioClip[] Sa_insert;
	[SerializeField] private AudioClip Sb_bg_theme;
	[SerializeField] private AudioClip[] Sb_insert;
	[SerializeField] private AudioClip Sd_bg_theme;
	[SerializeField] private AudioClip[] Sd_insert;
	[SerializeField] private AudioClip Sg_bg_theme;
	[SerializeField] private AudioClip[] Sg_insert;
	[SerializeField] private AudioClip So_bg_theme;
	[SerializeField] private AudioClip[] So_insert;
	[SerializeField] private AudioClip SH_bg_theme;
	[SerializeField] private AudioClip[] SH_insert;
	[SerializeField] private AudioClip SF_bg_theme;

	void Start () {
		FindAudioParent();
	}

	private void FindAudioParent() {
		parAudio = GameObject.Find("Audio").transform;
		musicVolume = GetComponentInParent<PrefsControl>().GetMusicVolume();
		ResetInsertTimer();
	}

	private void ResetInsertTimer() {
		insertTimer = (insertTimerRange / 2) + Random.Range(0, insertTimerRange);
	}

	public void PlayMusic(int clusterNum) {
		GameObject go;
		AudioSource audio;
		AudioClip ac;

		if (!parAudio)  { FindAudioParent(); }  //will be null on scene change
		go = Instantiate(pre_MusicEffect, parAudio) as GameObject;
		audio = go.GetComponent<AudioSource>();

		ac = mainTheme;
		if (clusterNum == 0) 	  { ac = mainTheme; }
		else if (clusterNum == 1) { ac = Sa_bg_theme; }
		else if (clusterNum == 2) { ac = Sb_bg_theme; }
		else if (clusterNum == 3) { ac = Sd_bg_theme; }  
		else if (clusterNum == 4) { ac = Sg_bg_theme; }
		else if (clusterNum == 5) { ac = So_bg_theme; }
		else if (clusterNum == 6) { ac = SH_bg_theme; }   //hyperspace
		else if (clusterNum == 9) { ac = SF_bg_theme; }   //finish
		currentSceneNum = clusterNum;

		go.name = "Theme " + clusterNum.ToString();
		audio.loop = true;
		if (currentSceneNum == 9) { audio.loop = false; }  //no loop for finish
		audio.clip = ac;
		audio.volume = musicVolume;
		audio.Play();
	}

	void PlayInsert() {
		if (currentSceneNum == 0) { return; }  //no inserts for title theme
		if (currentSceneNum == 9) { return; }  //no inserts for finish theme

		GameObject go;
		AudioSource audio;
		AudioClip ac = null;

		if (!parAudio)  { FindAudioParent(); }  //will be null on scene change
		go = Instantiate(pre_MusicEffect, parAudio) as GameObject;
		audio = go.GetComponent<AudioSource>();

		int insertNum = 0;
		if (currentSceneNum == 1) {
			do { 
				insertNum = Random.Range(0, Sa_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = Sa_insert[insertNum];
		}
		if (currentSceneNum == 2) {
			do { 
				insertNum = Random.Range(0, Sb_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = Sb_insert[insertNum];
		}
		if (currentSceneNum == 3) {
			do { 
				insertNum = Random.Range(0, Sd_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = Sd_insert[insertNum];
		}
		if (currentSceneNum == 4) {
			do { 
				insertNum = Random.Range(0, Sg_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = Sg_insert[insertNum];
		}
		if (currentSceneNum == 5) {
			do { 
				insertNum = Random.Range(0, So_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = So_insert[insertNum];
		}
		if (currentSceneNum == 6) {
			do { 
				insertNum = Random.Range(0, SH_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = SH_insert[insertNum];
		}

		lastInsertNum = insertNum;
		go.name = "Insert " + currentSceneNum.ToString() + insertNum.ToString();
		audio.clip = ac;
		audio.volume = musicVolume;
		audio.Play();
		Destroy(go, audio.clip.length + 0.2f);
	}

	public void UpdateVolume() {
		musicVolume = GetComponentInParent<PrefsControl>().GetMusicVolume();
		foreach (Transform child in parAudio) {
			if (child.name.StartsWith("Insert") || child.name.StartsWith("Theme")) {
				child.GetComponent<AudioSource>().volume = musicVolume;
			}
		}
	}

	void Update () {
		if (insertTimer > 0) {
			insertTimer -= Time.deltaTime;
		} 
		else if (insertTimer < 0) {
			PlayInsert();
			ResetInsertTimer();
		} 
	}
}
