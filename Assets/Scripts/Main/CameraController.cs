using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Transform ship;
	private float initZ;
	private Vector3[] coords;
	private int steps = 5;
	//private float smoothing = 2.5f;
	public  bool fixedCam = false;  //true = camera is fixed on ship
	private float smoothTime = 0.5f;
	private Vector3 velocity = Vector3.zero;

	void Start () {
		ship = GameObject.Find("PlayerShip").transform;	
		initZ = transform.position.z;
		coords = new Vector3[steps];
		for (int i=0; i<steps; i++) {
			coords[i] = new Vector3(ship.position.x, ship.position.y, initZ);
		}
	}

	void LateUpdate() {
		if (!fixedCam) {
			transform.position = Vector3.SmoothDamp(transform.position, coords[0], ref velocity, smoothTime);
		}
	}

	void Update() {
		if (fixedCam) {
			transform.position = new Vector3(ship.position.x, ship.position.y, initZ);
		} else {
			//transform.position = Vector3.Lerp(transform.position, coords[0], Time.deltaTime * smoothing);
			for (int i=0; i<(steps-1); i++) {
				coords[i] = coords[i+1];
			}
			coords[steps - 1] = new Vector3(ship.position.x, ship.position.y, initZ);
		}
	}

}
