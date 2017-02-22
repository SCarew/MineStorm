using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField] private GameObject pre_SoundEffect;
	private Transform parAudio;   //for empty parent container

	[SerializeField] private AudioClip laser1;
	[SerializeField] private AudioClip torp1;
	[SerializeField] private AudioClip missile1;
	[SerializeField] private AudioClip forcefieldOn1;
	[SerializeField] private AudioClip forcefieldOff1;
	[SerializeField] private AudioClip shockwave1;
	[SerializeField] private AudioClip explosionMet1;
	[SerializeField] private AudioClip explosionMag1;
	[SerializeField] private AudioClip explosionElec1;
	[SerializeField] private AudioClip explosionDen1;
	[SerializeField] private AudioClip explosionBHole1;
	[SerializeField] private AudioClip expSmall1;

	void Start () {
		parAudio = GameObject.Find("Audio").transform;
	}

	public void PlaySound(string soundName) {
		GameObject go;
		AudioClip ac = null;
		AudioSource audio;
		soundName = soundName.ToLower();

		if (soundName == "torpedo" || soundName == "torp") 
			{ ac = torp1; }
		else if (soundName == "laser") 
			{ ac = laser1; }
		else if (soundName == "missile") 
			{ ac = missile1; }
		else if (soundName == "forcefield") 
			{ ac = forcefieldOn1; }
		else if (soundName == "forcefieldoff") 
			{ ac = forcefieldOff1; }
		else if (soundName == "shockwave") 
			{ ac = shockwave1; }
		else if (soundName == "explosionmeteor") 
			{ ac = explosionMet1; }
		else if (soundName == "explosionmagnet") 
			{ ac = explosionMag1; }
		else if (soundName == "explosionelectric") 
			{ ac = explosionElec1; }
		else if (soundName == "explosiondense") 
			{ ac = explosionDen1; }
		else if (soundName == "explosionbhole") 
			{ ac = explosionBHole1; }
		else if (soundName == "expsmall") 
			{ ac = expSmall1; }

		go = Instantiate(pre_SoundEffect, parAudio) as GameObject;
		audio = go.GetComponent<AudioSource>();
		audio.clip = ac;
		audio.Play();
	}

}
