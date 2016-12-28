using UnityEngine;
using System.Collections;

public class CameraControllerOld : MonoBehaviour {

	private Transform ship;
	private Vector3[] coords;
	private int steps = 6;
	private float interval = 0.04f;
	private float smoothing = 2.5f;
	[SerializeField] private bool staticCam = false;  //true = camera doesn't drift

	void Start () {
		ship = GameObject.Find("PlayerShip").transform;	
		coords = new Vector3[steps];
		if (!staticCam) 
			{ StartCoroutine(CameraMove()); }
	}

	IEnumerator CameraMove() {
		bool bLoop = true;
		int i;
		for (i=0; i<steps; i++) {
			coords[i] = new Vector3(ship.position.x, ship.position.y, transform.position.z);
		}
		yield return new WaitForSeconds(interval * steps);

		while (bLoop) {
			for (i=0; i<(steps-1); i++) {
				coords[i] = coords[i+1];
			}
			coords[steps - 1] = new Vector3(ship.position.x, ship.position.y, transform.position.z);
			yield return new WaitForSeconds(interval);
		}
	}

//	void FixedUpdate() {
//		if (!staticCam) {
//			transform.position = Vector3.Lerp(transform.position, coords[0], Time.fixedDeltaTime * smoothing);
//		}
//	}

	void Update() {
		if (staticCam) {
			transform.position = new Vector3(ship.position.x, ship.position.y, transform.position.z);
		} else {
			transform.position = Vector3.Lerp(transform.position, coords[0], Time.deltaTime * smoothing);
		}
	}
}
