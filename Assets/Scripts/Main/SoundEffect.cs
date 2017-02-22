using System.Collections;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

	private AudioSource audio;

	void Start () {
		audio = GetComponent<AudioSource>();
		Invoke("SelfDestruct", 0.1f);
	}

	void SelfDestruct() {
		float t = 1f;
		if (audio.clip != null) {
			t = audio.clip.length;
			if ((t <= 0f) || (t > 10f)) 
				{ t = 1f; }
		} 
		Destroy (gameObject, t);
	}
}
