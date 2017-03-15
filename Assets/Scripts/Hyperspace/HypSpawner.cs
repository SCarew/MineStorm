using System.Collections;
using UnityEngine;

public class HypSpawner : MonoBehaviour {

	private Transform spawner, ender;
	private Transform parMet;
	private Vector3 max0, min0;   //bounds of spawner object
	private Vector3 max1, min1;   //bounds of target object (ender)
	[SerializeField] private GameObject[] mines;
	private int iMeteor = 0;

	void Start () {
		ender = GameObject.Find("Ender").transform;
		spawner = gameObject.transform;
		parMet = GameObject.Find("Meteors").transform;

		max0 = spawner.GetComponent<MeshRenderer>().bounds.max;
		min0 = spawner.GetComponent<MeshRenderer>().bounds.min;
		max1 = ender.GetComponent<MeshRenderer>().bounds.max;
		min1 = ender.GetComponent<MeshRenderer>().bounds.min;
		//Debug.Log("spawner max: " + max0);
		//Debug.Log("spawner min: " + min0);
		//Debug.Log("  ender max: " + max1);
		//Debug.Log("  ender min: " + min1);

		SpawnMeteor();
	}

	void SpawnMeteor() {
		Vector3 rnd0 = new Vector3(Random.Range(min0.x, max0.x), Random.Range(min0.y, max0.y), Random.Range(min0.z, max0.z));
		Vector3 rnd1 = new Vector3(Random.Range(min1.x, max1.x), Random.Range(min1.y, max1.y), Random.Range(min1.z, max1.z));
		int num = Random.Range(0, mines.Length);
//		Debug.Log("rnd0: " + rnd0);
//		Debug.Log("rnd1: " + rnd1);
		Debug.DrawLine(rnd0, rnd1);
		iMeteor++;
		GameObject go = Instantiate(mines[num], rnd0, Quaternion.identity, parMet) as GameObject;
		go.name = "Meteor" + iMeteor.ToString("000");
		go.GetComponent<HypMeteor>().SetTarget(rnd1);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			SpawnMeteor();
		}
	}
}
