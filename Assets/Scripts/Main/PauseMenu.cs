using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
	[SerializeField] private GameObject pauseMenuCanvas;
	[SerializeField] private Text txtInfo;
	[SerializeField] private GameObject panNext, panPrev;

	public bool isPaused = false;
	private bool wasPaused = false;  //true on first frame after paused
	private GameManager gm;
	private ShipController sc;
	private InfoControl info;
	private bool bSettledArcadeMode = false;
	private int infoValue = 0;
	private string[] st;

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		sc = GameObject.Find("PlayerShip").GetComponent<ShipController>();
		info = GameObject.Find("LevelManager").GetComponent<InfoControl>();
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
				if (!bSettledArcadeMode && gm.bArcadeMode) {
					bSettledArcadeMode = true;
					GameObject.Find("imgInfo").SetActive(false);
					Transform tPause = GameObject.Find("txtPaused").transform;
					tPause.GetComponent<RectTransform>().localPosition = new Vector3(-200f, 0f, 0f);
				}
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
				KillSounds(true);
				DisplayInfo();
			}
			wasPaused = false;
		} else {   //not paused
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
				KillSounds(false);
			}
			wasPaused = false;
		}
		if (Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.P)) {
			isPaused = !isPaused;
			wasPaused = true;
		}
		if (isPaused) {
			if (Input.GetButtonDown("Secondary")) {
				infoValue++;
				if (infoValue >= st.Length) { infoValue = st.Length - 1; }
				ShowInfoMessage(st[infoValue]);
			}
			if (Input.GetButtonDown("Primary")) {
				infoValue--;
				if (infoValue < 0) { infoValue = 0; }
				ShowInfoMessage(st[infoValue]);
			}
		}
	}

	void ShowInfoMessage(string s) {
		if 		(s.Length < 250) { txtInfo.fontSize = 40; }
		else if (s.Length < 325) { txtInfo.fontSize = 38; }
		else if (s.Length < 400) { txtInfo.fontSize = 36; } 
		else 					 { txtInfo.fontSize = 34; }

		if (infoValue <= 0) 			{ panNext.SetActive(false); }
		else 							{ panNext.SetActive(true); }
		if (infoValue >= st.Length - 1) { panPrev.SetActive(false); }
		else 							{ panPrev.SetActive(true); }

		txtInfo.text = s;
	}

	void DisplayInfo() {
		if (gm.bArcadeMode) { return; }

		Transform tMeteors = GameObject.Find("Meteors").transform;
		int big = 0;
		foreach ( Transform t in tMeteors ) {
			if (t.name.Contains(".B."))  { big++; }
		}
		if (big > 0) {
			st = info.GetInfoArray(gm.currentLevel, false);
			//st = info.GetInfoArray(gm.currentLevel);
		} else {
			st = info.GetInfoArray(gm.currentLevel, true);
			//st = info.GetInfoArray(gm.currentLevel, true);
		}
		infoValue = 0;
		ShowInfoMessage(st[infoValue]);
	}

	void KillSounds(bool bPause) {
		AudioSource[] aud = GameObject.FindObjectsOfType<AudioSource>();
		foreach (AudioSource a in aud) {
			if (bPause) {
				if (!a.name.StartsWith("Theme") && !a.name.StartsWith("Insert")) {
					a.Pause();
				}
			} else {
				if (!a.name.StartsWith("Theme") && !a.name.StartsWith("Insert")) {
					a.UnPause();
				}
			}
		}
	}
}
