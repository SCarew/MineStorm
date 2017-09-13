using System.Collections;
using UnityEngine;

public class HypPauseMenu : MonoBehaviour {
	public GameObject pauseMenuCanvas;
	public bool isPaused = false;
	private bool wasPaused = false;  //true on first frame after paused
	private PrefsControl prefs;

	void Start () {
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		pauseMenuCanvas.SetActive(false);
	}
	
	void Update () {
		if (prefs.bHypGameOver) {
			isPaused = false;
			pauseMenuCanvas.SetActive(false);
			return;
		}
		if (isPaused) {
			pauseMenuCanvas.SetActive(true);
			Time.timeScale = 0f;
			if (wasPaused) {
				ParticleSystem[] ps = GameObject.FindObjectsOfType<ParticleSystem>();
				foreach (ParticleSystem ps1 in ps) {
					if (ps1.gameObject.name == "PS_EngineFireR" || ps1.gameObject.name == "PS_EngineFireL") {
						ps1.Stop();
					} else {
						ps1.Pause();
					}
				}
				HyperSwirl[] hs = GameObject.FindObjectsOfType<HyperSwirl>();
				foreach (HyperSwirl hs1 in hs) {
					hs1.pauseSwirl(true);
				}
			}
			wasPaused = false;
		} else {
			pauseMenuCanvas.SetActive(false);
			Time.timeScale = 1f;
			if (wasPaused) {
				ParticleSystem[] ps = GameObject.FindObjectsOfType<ParticleSystem>();
				foreach (ParticleSystem ps1 in ps) {
					if (ps1.gameObject.name != "PS_EngineFireR" && ps1.gameObject.name != "PS_EngineFireL") {
						ps1.Play();
					}
				}
				HyperSwirl[] hs = GameObject.FindObjectsOfType<HyperSwirl>();
				foreach (HyperSwirl hs1 in hs) {
					hs1.pauseSwirl(false);
				}
			}
			wasPaused = false;
		}
		if (Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.P)) {
			isPaused = !isPaused;
			wasPaused = true;
		}
	}
}
