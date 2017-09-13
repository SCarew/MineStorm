using System.Collections;
using UnityEngine;

public class HyperSwirl : MonoBehaviour {

	public Vector3 euler = new Vector3(0f, 0f, 1f);
	private bool isPaused = false;

	public void pauseSwirl(bool bPaused) {
		isPaused = bPaused;
	}

	void Update () {
		if (!isPaused) {
			transform.Rotate(euler);
		}
	}
}
