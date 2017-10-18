using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectConstant : MonoBehaviour {

	private AudioSource aud;
	private bool bLoop = true;

	void Start () {
		aud = GetComponent<AudioSource>();
		StartCoroutine(CheckDistance());
	}
	
	IEnumerator CheckDistance() {
		float distance;
		float volume;
		Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		Transform ufo = gameObject.transform.parent.transform;

		while (bLoop) {
			Vector3 visibility = cam.WorldToViewportPoint(ufo.position);
			if (visibility.x > 1f || visibility.x < 0f || visibility.y > 1f || visibility.y < 0f) {
				volume = 0f;
			} else {
				distance = 1f - Mathf.Abs(Vector2.Distance(new Vector2(visibility.x, visibility.y), new Vector2(0.5f, 0.5f)));
				volume = Mathf.Max(distance * distance - 0.2f, 0.05f);
			}
			aud.volume = volume;
			yield return new WaitForSeconds(0.1f);
		} 
	}

	void OnDestroy() {
		bLoop = false;
	}
}
