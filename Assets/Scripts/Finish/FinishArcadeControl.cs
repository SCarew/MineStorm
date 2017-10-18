using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FinishArcadeControl : MonoBehaviour {

	public GameObject panFadein;
	[SerializeField] private Text txtScores;
	[SerializeField] private Text txtFinalScore;
	[SerializeField] private Text[] txtLetter; 
	//[SerializeField] private GameObject[] pre_Fireworks;
	private PrefsControl prefs;
	private LevelManager lm;
	private MusicManager music;
	private Color defaultColor, changeColor;

	private bool bGetName = false;
	private string alphaNum;
	private int currentLetter = 1;
	private int currentAlpha = 0;
	private int score;
	private string sCRLF = System.Environment.NewLine;   // \r\n

	private float countdown = 2f;
	private float deadZone = 0.25f;


	void Start () {
		panFadein.SetActive(true);
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		lm = prefs.GetComponent<LevelManager>();
		music = GameObject.Find("MusicManager").GetComponent<MusicManager>();

		//PlayerPrefs.SetInt("ArcadeScore", 2000); 
		//prefs.SetGameType("Arcade");
		alphaNum = "ABCDEFGHIJKLMNOPQRSTUVWXYZ_*1234567890 ";
		defaultColor = txtLetter[0].color;
		changeColor = new Color(255f/255f, 156f/255f, 91f/255f, 255f/255f);
		GetMyScore();
		FillList();
		music.PlayMusic(0);
	}

	void GetMyScore() {
		score = prefs.GetGameStats(PrefsControl.stats.Score);
		txtFinalScore.text = score.ToString("0000000");
	}

	void FillList() {
		int n = prefs.GetTopScore(-1);
		string s = "";
		string sn, ss;

		for (int i=1; i<=n; i++) {
			ss = prefs.GetTopScore(i).ToString("0000000");
			sn = prefs.GetTopScoreName(i);
			if (i<10)
				{ s = s + i.ToString() + "  " + sn + "  " + ss + sCRLF; }
			else
				{ s = s + i.ToString() + " " + sn + "  " + ss + sCRLF; }
		}
		txtScores.text = s.Substring(0, s.Length - 1);
		countdown = 2f;

		if (score >= prefs.GetTopScore(n)) { 
			bGetName = true; 
			txtLetter[0].color = changeColor;
			//StartCoroutine(PlayFireworks());
		} else {
			bGetName = false;
			txtLetter[0].enabled = false;
			txtLetter[1].enabled = false;
			txtLetter[2].enabled = false;
		}
	}

//	IEnumerator PlayFireworks() {
//		GameObject go;
//		SoundManager aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
//		Transform cam = GameObject.Find("Main Camera").transform;
//		Transform parEff = GameObject.Find("Effects").transform;
//		float fDelay = 0.5f;
//		float rangeX = 10f, rangeY = 6f;
//		Vector3 v3;
//		while(bGetName) {
//			v3 = new Vector3(cam.position.x + Random.Range(-rangeX, rangeX), cam.position.y + Random.Range(-rangeY, rangeY), 5f);
//			go = Instantiate(pre_Fireworks[Random.Range(0, pre_Fireworks.Length)], v3, Quaternion.identity, parEff) as GameObject;
//			go.transform.localScale *= Random.Range(0.75f, 1.5f);
//			aud.PlaySoundVisible("fireworks", go.transform, 1);
//			Destroy(go, 5.0f);
//			yield return new WaitForSeconds(fDelay);
//			fDelay = Random.Range(1.3f, 3.25f);
//		}
//	}

	void changeLetter(int a) {
		countdown = 0.5f; 
		currentLetter += a;
		if (currentLetter < 1) { currentLetter = 1; }
		if (currentLetter > 3) { currentLetter = 3; }
		currentAlpha = alphaNum.IndexOf(txtLetter[currentLetter - 1].text);
	}

	void changeAlpha(int a) {
		countdown = 0.2f;
		currentAlpha += a;
		if (currentAlpha < 0) { currentAlpha = alphaNum.Length - 1; }
		if (currentAlpha >= alphaNum.Length) { currentAlpha = 0; }
	}

	void Update () {
		countdown -= Time.deltaTime;
		if (countdown <= 0f) { countdown = 0f; }
		else                 { return; }

		bool bP = Input.GetButtonDown("Primary"); 
		bool bS = Input.GetButtonDown("Secondary");
		if (bGetName) {
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			if (v > deadZone)  { changeAlpha(1); }
			if (v < -deadZone) { changeAlpha(-1); }
			if (h > deadZone)  { changeLetter(1); }
			if (h < -deadZone || bS) { changeLetter(-1); }
			if (bP) {
				if (currentLetter >= 3) {
					string initials = "";
					for (int i=0; i<3; i++) {
						initials = initials + txtLetter[i].text.Substring(0, 1);
					}
					prefs.SetTopScore(score, initials);
					score = 0;
					FillList();
					return;
				} else {
					changeLetter(1); 
				}
			}

			for (int i=0; i<3; i++) {
				txtLetter[i].color = defaultColor;
			}
			txtLetter[currentLetter - 1].color = changeColor;
			txtLetter[currentLetter - 1].text = alphaNum.Substring(currentAlpha, 1);
		} else {   // !bGetName
			if ((bP || bS) && (countdown <= 0f)) { 
				lm.LoadScene("Title");
				return;
			}
		}

	}

}
