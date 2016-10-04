using UnityEngine;
using System.Collections;

public class TorpedoController : MonoBehaviour {

	private float fireSpeed = 10.0f;

	void Start () {
		Destroy(gameObject, 2.0f);

	}
	
	void Update () {
		transform.position += transform.up * Time.deltaTime * fireSpeed;
	}
}
