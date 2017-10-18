using System.Collections;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

	private AudioSource aud;

	void Start () {
		aud = GetComponent<AudioSource>();
		Invoke("SelfDestruct", 0.1f);
	}

	void SelfDestruct() {
		if (aud.loop) { return; }
		float t = 1f;
		if (aud.clip != null) {
			t = aud.clip.length;
			if ((t <= 0f) || (t > 10f)) 
				{ t = 1f; }
		} 
		Destroy (gameObject, t);
	}
}
