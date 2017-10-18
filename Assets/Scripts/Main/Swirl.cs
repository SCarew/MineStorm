using UnityEngine;
using System.Collections;

public class Swirl : MonoBehaviour {

	public Vector3 euler = new Vector3(0f, 0f, 1f);
	private ParticleSystem[] ps;
	private bool bFreeze = false;  //true when game is paused

	public float countdown = 2f;  //time until part systems begin cooldown

	void Start () {
		ps = gameObject.GetComponentsInChildren<ParticleSystem>();
		SoundManager aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		if (gameObject.name.StartsWith("Whirl.Exit") || gameObject.name.StartsWith("Whirl.Enter")) {
			if (gameObject.GetComponentInChildren<ParticleSystem>().transform.localScale.x < 1f) {
				aud.PlaySoundVisible("swirlSmall", gameObject.transform, 1);
			} else {
				aud.PlaySoundConstant("swirlLarge", gameObject.transform);
			}
		}
		Invoke("StartVanish", countdown);
	}

	public void StartVanish() {
		if (ps != null) {
			foreach (ParticleSystem ps1 in ps) 
				{ ps1.Stop(); }
		}
		Destroy(gameObject, 3f);
	}

	public void PauseSwirl(bool bStop) {
		bFreeze = bStop;
		/*
		if (bStop == false) {    //pausing may have messed up countdown timer
			StartVanish();
		}
		*/
	}

	void Update () {
		if (bFreeze) { return; }
		transform.Rotate(euler);	
//		if (Input.GetButtonDown("Secondary") == true)   
//			{ startVanish(); }
	}
}
