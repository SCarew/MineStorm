using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
	public GameObject pauseMenuCanvas;
	public bool isPaused = false;
	private bool wasPaused = false;  //true on first frame after paused
	private GameManager gm;
	private ShipController sc;

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		sc = GameObject.Find("PlayerShip").GetComponent<ShipController>();
		pauseMenuCanvas.SetActive(false);
	}
	
	void Update () {
		if (gm.bGameOver) {
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
					if (ps1.gameObject.transform.parent.tag == "Pauseable") {
						ps1.GetComponentInParent<Swirl>().PauseSwirl(true);
					}
				}
				sc.FreezeRotation(true);
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
					if (ps1.gameObject.transform.parent.tag == "Pauseable") {
						ps1.GetComponentInParent<Swirl>().PauseSwirl(false);
					}
				}
				sc.FreezeRotation(false);
			}
			wasPaused = false;
		}
		if (Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.P)) {
			isPaused = !isPaused;
			wasPaused = true;
		}
	}
}
