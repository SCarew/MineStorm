using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	[SerializeField] private GameObject pre_SoundEffect;

	private Transform parAudio;   //for empty parent container
	private float mainVolume = 0.5f;

	[SerializeField] private AudioClip laser1;
	[SerializeField] private AudioClip torp1;
	[SerializeField] private AudioClip missile1;
	[SerializeField] private AudioClip engine1;
	[SerializeField] private AudioClip forcefieldOn1;
	[SerializeField] private AudioClip forcefieldOff1;
	[SerializeField] private AudioClip shockwave1;
	[SerializeField] private AudioClip explosionMet1;
	[SerializeField] private AudioClip explosionMag1;
	[SerializeField] private AudioClip explosionElec1;
	[SerializeField] private AudioClip explosionDen1;
	[SerializeField] private AudioClip explosionBHole1;
	[SerializeField] private AudioClip expLaser1;
	[SerializeField] private AudioClip expTorpedo1;
	[SerializeField] private AudioClip expMissile1;
	[SerializeField] private AudioClip UFO11;
	[SerializeField] private AudioClip UFO12;
	[SerializeField] private AudioClip expUFOTorpedo1;
	[SerializeField] private AudioClip expUFOLaser1;
	[SerializeField] private AudioClip UFOTorpedo1;
	[SerializeField] private AudioClip UFOLaser1;
	[SerializeField] private AudioClip explosionUFO11;
	[SerializeField] private AudioClip explosionUFO21;
	[SerializeField] private AudioClip explosionShip1;
	[SerializeField] private AudioClip hypExplosionMeteor1;
	[SerializeField] private AudioClip meteorCollide1;
	[SerializeField] private AudioClip swirlLarge1;
	[SerializeField] private AudioClip swirlSmall1;
	[SerializeField] private AudioClip[] choiceButtons;
	[SerializeField] private AudioClip[] startButtons;

	void Start () {
		FindAudioParent();
	}

	private void FindAudioParent() {
		parAudio = GameObject.Find("Audio").transform;
		mainVolume = GetComponentInParent<PrefsControl>().GetMainVolume();
	}

	private void PlaySound(AudioClip ac, float volume = 1f, Transform parAttach = null, bool bLoop = false, int num = 1) {
		GameObject go;
		AudioSource audio;
		bool bConstant = false;
		if (!parAudio)  { FindAudioParent(); }  //will be null on scene change
		if (!parAttach) { 
			parAttach = parAudio; 
		} else {
			bConstant = true;
		}
		go = Instantiate(pre_SoundEffect, parAttach) as GameObject;

		if (num > 1) {     //for PlaySoundLimited() only
			int n = 0;
			foreach (Transform t in parAudio.transform) {
				if (t.name == ac.name) { n++; }
			}
			if (n < num) {
				go.name = ac.name;
			} else {
				Destroy(go);
				return;
			}
		}

		audio = go.GetComponent<AudioSource>();
		audio.clip = ac;
		if (bLoop) {
			audio.loop = true;
		}
		if (bConstant) { go.AddComponent<SoundEffectConstant>(); }
		audio.volume = mainVolume * volume;
		audio.Play();
	}

	/// <summary>
	/// Plays sound immediately.  Intended for sounds emanating from ship.
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	public void PlaySoundImmediate(string soundName) {   //directly from player ship
		AudioClip ac = null;
		soundName = soundName.ToLower();

		if (soundName == "torpedo" || soundName == "torp") 
			{ ac = torp1; }
		else if (soundName == "laser") 
			{ ac = laser1; }
		else if (soundName == "missile") 
			{ ac = missile1; }
		else if (soundName == "hyplaser") 
			{ ac = laser1; }
		else if (soundName == "forcefield") 
			{ ac = forcefieldOn1; }
		else if (soundName == "forcefieldoff") 
			{ ac = forcefieldOff1; }
		else if (soundName == "shockwave") 
			{ ac = shockwave1; }
		else if (soundName == "explosionship") 
			{ ac = explosionShip1; }
		else if (soundName == "explosionhypmeteor") 
			{ ac = hypExplosionMeteor1; }
		/* ----------------------------------- */
		else if (soundName == "choicemove") 
			{ ac = choiceButtons[0]; }
		else if (soundName == "choiceselect") 
			{ ac = choiceButtons[1]; }
		else if (soundName == "startmove") 
			{ ac = startButtons[0]; }
		else if (soundName == "startselect") 
			{ ac = startButtons[1]; }

		PlaySound(ac);
	}

	/// <summary>
	/// Starts playing a sound clip limited to n instances.  Intended for engines.
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	/// <param name="number">Max number of instances.</param>
	public void PlaySoundLimited(string soundName, int number) {
		AudioClip ac = null;
		soundName = soundName.ToLower();

		if (soundName == "engine") 
			{ ac = engine1; }

		PlaySound(ac, 1f, null, false, number);
	}

	/// <summary>
	/// Plays a constant sound while object is alive and visible.  Intended for UFOs and bosses.
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	/// <param name="parObj">Object to attach sound to.</param>
	public void PlaySoundConstant(string soundName, Transform parObj) {   //constant/looping (if visible)
		AudioClip ac = null;
		float vol = 1f;
		soundName = soundName.ToLower();

		if (soundName == "swirllarge") 
			{ ac = swirlLarge1; }
		else if (soundName == "swirlsmall") //actually played from PlaySoundVisible()
			{ ac = swirlSmall1; }
			//vol = parObj.GetComponentInChildren<ParticleSystem>().transform.localScale.x; }
		else if (soundName == "ufo1")  
			{ ac = UFO11; }
		else if (soundName == "ufo2") 
			{ ac = UFO12; }

		PlaySound(ac, vol, parObj, true);
	}

	/// <summary>
	/// Plays the sound if object is visible.  Intended for meteors and explosions.
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	/// <param name="obj">Object.</param>
	/// <param name="size">Size of meteor.</param>
	public void PlaySoundVisible(string soundName, Transform obj, int size = 3) {   //play only if visible
		AudioClip ac = null;
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		soundName = soundName.ToLower();
		float vol = size / 3f;
		Vector3 visibility = cam.WorldToViewportPoint(obj.position);
		if (visibility.x > 1f || visibility.x < 0f || visibility.y > 1f || visibility.y < 0f) {
			return;    //object not visible to camera, so don't play sound
		}

		if (soundName == "explosionmeteor") 
			{ ac = explosionMet1; }
		else if (soundName == "explosionmagnet") 
			{ ac = explosionMag1; }
		else if (soundName == "explosionelectric") 
			{ ac = explosionElec1; }
		else if (soundName == "explosiondense") 
			{ ac = explosionDen1; }
		else if (soundName == "explosionbhole") 
			{ ac = explosionBHole1; }
		else if (soundName == "expmissile") 
			{ ac = expMissile1; }
		else if (soundName == "explaser") 
			{ ac = expLaser1; }
		else if (soundName == "exptorpedo") 
			{ ac = expTorpedo1; }
		else if (soundName == "expufolaser") 
			{ ac = expUFOLaser1; }
		else if (soundName == "expufotorp") 
			{ ac = expUFOTorpedo1; }
		else if (soundName == "ufolaser") 
			{ ac = UFOLaser1; }
		else if (soundName == "ufotorp") 
			{ ac = UFOTorpedo1; }
		else if (soundName == "explosionufo1") 
			{ ac = explosionUFO11; }
		else if (soundName == "explosionufo2") 
			{ ac = explosionUFO21; }
		else if (soundName == "meteorhit") 
			{ ac = meteorCollide1; vol = vol * 0.25f; }
		else if (soundName == "swirlsmall") 
			{ ac = swirlSmall1; }

		PlaySound(ac, vol);
	}

	/// <summary>
	/// Plays all sounds generically.  Reference function - should not be used.
	/// </summary>
	/// <param name="soundName">Sound name.</param>
	public void PlaySoundGeneric(string soundName) {
		AudioClip ac = null;
		float vol = 1f;
		soundName = soundName.ToLower();

		//this section includes all sound names for reference
		if (soundName == "torpedo" || soundName == "torp") 
			{ ac = torp1; }
		else if (soundName == "laser") 
			{ ac = laser1; }
		else if (soundName == "missile") 
			{ ac = missile1; }
		else if (soundName == "hyplaser") 
			{ ac = laser1; }
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
		else if (soundName == "explosionhypmeteor") 
			{ ac = hypExplosionMeteor1; }
		else if (soundName == "expmissile") 
			{ ac = expMissile1; }
		else if (soundName == "explaser") 
			{ ac = expLaser1; }
		else if (soundName == "exptorpedo") 
			{ ac = expTorpedo1; }
		else if (soundName == "ufolaser") 
			{ ac = UFOLaser1; }
		else if (soundName == "ufotorp") 
			{ ac = UFOTorpedo1; }
		else if (soundName == "expufolaser") 
			{ ac = expUFOLaser1; }
		else if (soundName == "expufotorp") 
			{ ac = expUFOTorpedo1; }
		else if (soundName == "explosionship") 
			{ ac = explosionShip1; }
		else if (soundName == "explosionufo1") 
			{ ac = explosionUFO11; }
		else if (soundName == "explosionufo2") 
			{ ac = explosionUFO21; }
		else if (soundName == "meteorhit") 
			{ ac = meteorCollide1; }
		else if (soundName == "swirllarge") //***
			{ ac = swirlLarge1; }
		else if (soundName == "swirlsmall") 
			{ ac = swirlSmall1; }
			//vol = parObj.GetComponentInChildren<ParticleSystem>().transform.localScale.x; }
		else if (soundName == "engine") 
			{ ac = engine1; }
		else if (soundName == "ufo1")  //***
			{ ac = UFO11; }
		else if (soundName == "ufo2")  //***
			{ ac = UFO12; }

		PlaySound(ac);
	}
}
