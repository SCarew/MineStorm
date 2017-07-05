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
	[SerializeField] private AudioClip S1_bg_theme;
	[SerializeField] private AudioClip[] S1_insert;
	[SerializeField] private AudioClip S2_bg_theme;
	[SerializeField] private AudioClip[] S2_insert;
	[SerializeField] private AudioClip S3_bg_theme;
	[SerializeField] private AudioClip[] S3_insert;
	[SerializeField] private AudioClip S4_bg_theme;
	[SerializeField] private AudioClip[] S4_insert;
	[SerializeField] private AudioClip SH_bg_theme;
	[SerializeField] private AudioClip[] SH_insert;

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

	public void PlayMusic(int sceneNum) {
		GameObject go;
		AudioSource audio;
		AudioClip ac;

		if (!parAudio)  { FindAudioParent(); }  //will be null on scene change
		go = Instantiate(pre_MusicEffect, parAudio) as GameObject;
		audio = go.GetComponent<AudioSource>();

		ac = mainTheme;
		if (sceneNum == 0) { ac = mainTheme; }
		else if (sceneNum == 1) { ac = S1_bg_theme; }
		else if (sceneNum == 2) { ac = S2_bg_theme; }
		else if (sceneNum == 3) { ac = S3_bg_theme; }
		else if (sceneNum == 4) { ac = S4_bg_theme; }
		else if (sceneNum == 5) { ac = SH_bg_theme; }   //hyperspace
		currentSceneNum = sceneNum;

		go.name = "Theme " + sceneNum.ToString();
		audio.loop = true;
		audio.clip = ac;
		audio.volume = musicVolume;
		audio.Play();
	}

	void PlayInsert() {
		if (currentSceneNum == 0) { return; }  //no inserts for title theme

		GameObject go;
		AudioSource audio;
		AudioClip ac = null;

		if (!parAudio)  { FindAudioParent(); }  //will be null on scene change
		go = Instantiate(pre_MusicEffect, parAudio) as GameObject;
		audio = go.GetComponent<AudioSource>();

		int insertNum = 0;
		if (currentSceneNum == 1) {
			do { 
				insertNum = Random.Range(0, S1_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = S1_insert[insertNum];
		}
		if (currentSceneNum == 2) {
			do { 
				insertNum = Random.Range(0, S2_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = S2_insert[insertNum];
		}
		if (currentSceneNum == 3) {
			do { 
				insertNum = Random.Range(0, S3_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = S3_insert[insertNum];
		}
		if (currentSceneNum == 4) {
			do { 
				insertNum = Random.Range(0, S4_insert.Length);
			} while (lastInsertNum == insertNum);
			ac = S4_insert[insertNum];
		}
		if (currentSceneNum == 5) {
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
