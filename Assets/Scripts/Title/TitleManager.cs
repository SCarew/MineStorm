using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {

	private LevelManager lm;
	private RawImage tx; 
	private RawImage title;
	public  Texture[] textData;
	private int currentText = 0;

	void Start () {
		lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		tx = GameObject.Find("TextInfo").GetComponent<RawImage>();
		title = GameObject.Find("TitleImage").GetComponent<RawImage>();

		//play music theme
		MusicManager music = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		music.PlayMusic(0);
	}

	void Update () {
		if (Input.GetButtonDown("Primary")) {
			title.enabled = false;
			tx.enabled = true;
			if (textData.Length <= currentText) {
				lm.LoadNextScene();
				//lm.LoadScene("Title");
				return;
			}
			tx.texture = textData[currentText];
			currentText++;
		}
		if (Input.GetButtonDown("Secondary")) {
			lm.LoadNextScene();
		} 
	}
}
