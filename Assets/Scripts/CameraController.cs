using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Transform ship;

	void Start () {
		ship = GameObject.Find("PlayerShip").transform;	
	}
	
	void Update () {
		transform.position = new Vector3(ship.position.x, ship.position.y, transform.position.z);
	}
}
