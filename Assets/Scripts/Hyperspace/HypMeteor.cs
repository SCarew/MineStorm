using System.Collections;
using UnityEngine;

public class HypMeteor : MonoBehaviour {

	private Vector3 target = new Vector3(0f, 0f, 0f);
	private Rigidbody rb;
	private float moveSpeed = 1f;
	private float rotTime = 1f / 6f;

	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.AddTorque(new Vector3(Random.Range(-1f, 1f) * 360 * rotTime, Random.Range(-1f, 1f) * 360 * rotTime, Random.Range(-1f, 1f) * 360 * rotTime), ForceMode.Force);
	}

	public void SetTarget(Vector3 location, float speed = 0f) {
		target = location;
		if (speed == 0f) 
			{ moveSpeed += Random.Range(0f, moveSpeed); }
		else
			{ moveSpeed = speed; }
	}

	void Update () {
		//transform.Translate(target * moveSpeed * Time.deltaTime);
		transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
	}
}
