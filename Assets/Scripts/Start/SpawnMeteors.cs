using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMeteors : MonoBehaviour {

	public  GameObject[] meteors;
	private int numMeteors;
	private float nextMeteorTime;
	private Transform spawner;
	private Transform parObj;

	void Start () {
		numMeteors = meteors.Length;
		nextMeteorTime = Random.Range(0.1f, 1f);	
		spawner = GameObject.Find("Spawner").transform;
		parObj = GameObject.Find("Meteors").transform;
	}
	
	void Update () {
		nextMeteorTime -= Time.deltaTime;
		if (nextMeteorTime < 0f) {
			nextMeteorTime = Random.Range(2f, 5f);
			AddMeteor();
		}
		foreach (Transform child in parObj.transform) {
			if ((child.position.x < -15f) || (child.position.x > 15f)) {
				Destroy(child.gameObject);
			}
		}
	}

	void AddMeteor() {
		float h, v, w, moveSpeed, scale;
		float rotTime = 1f / 6f;
		int i = Random.Range(0, numMeteors);
		float y_s = spawner.localScale.y;
		float y = Random.Range(0f, y_s);
		Vector3 v3pos = new Vector3(spawner.position.x, spawner.position.y - (y_s/2) + y, Random.Range(-9f, 9f));
		Debug.Log("Pos: " + v3pos);
		GameObject go = Instantiate(meteors[i], v3pos, Quaternion.identity, parObj) as GameObject;

		scale = Random.Range(0.4f, 0.6f);
		go.transform.localScale = new Vector3(scale, scale, scale);
		go.GetComponent<MeteorControl>().enabled = false;
		go.GetComponent<EnemyHealth>().enabled = false;
		go.GetComponent<Wrapper>().enabled = false;
		go.GetComponentInChildren<MeshRenderer>().enabled = false;
		Rigidbody rb = go.GetComponent<Rigidbody>();
		h = Random.Range(-1f, -0.4f);
		v = Random.Range(-0.6f, 0.6f);
		w = Random.Range(-1f, 1f);
		moveSpeed = Random.Range(0.8f, 4f);
		rb.AddForce(new Vector3(h * moveSpeed, v * moveSpeed, 0f), ForceMode.VelocityChange);
		rb.AddTorque(new Vector3(h * 360 * rotTime, v * 360 * rotTime, w * 360 * rotTime), ForceMode.Force);

	}
}
