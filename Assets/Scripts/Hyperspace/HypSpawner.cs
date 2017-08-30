using System.Collections;
using UnityEngine;

public class HypSpawner : MonoBehaviour {

	private Transform spawner, ender;
	private Transform parMet;
	private PrefsControl prefs;
	private Vector3 max0, min0;   //bounds of meteor spawner object
	private Vector3 max1, min1;   //bounds of target object (ender)
	[SerializeField] private GameObject[] mines;
	[SerializeField] private GameObject[] Ufos;
	[SerializeField] private GameObject[] UFOSpawners;
	private int iMeteor = 0;
	private int level = 0;
	private float meteorSpawnRate = 0f;
	private float graySpawnRate = 0f;
	private float pinkSpawnRate = 0f;
	private float timeToWarp = 100f;
	private float meteorSpawnTime, graySpawnTime, pinkSpawnTime;

	void Start () {
		ender = GameObject.Find("Ender").transform;
		spawner = gameObject.transform;
		parMet = GameObject.Find("Meteors").transform;
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		HypTimer hypTimer = GameObject.Find("txtTime").GetComponent<HypTimer>();

		max0 = spawner.GetComponent<MeshRenderer>().bounds.max;
		min0 = spawner.GetComponent<MeshRenderer>().bounds.min;
		max1 = ender.GetComponent<MeshRenderer>().bounds.max;
		min1 = ender.GetComponent<MeshRenderer>().bounds.min;
		//Debug.Log("spawner max: " + max0);
		//Debug.Log("spawner min: " + min0);
		//Debug.Log("  ender max: " + max1);
		//Debug.Log("  ender min: " + min1);

		level = prefs.GetGameStats(PrefsControl.stats.Level);
		LevelSort();
		hypTimer.SetHypTime(timeToWarp);
		meteorSpawnTime = meteorSpawnRate;
		graySpawnTime = graySpawnRate;
		pinkSpawnTime = pinkSpawnRate;

	}

	void LevelSort() {
		if (level < 2) {
			LevelSortHelper(5.5f, 0f, 0f, 10f); //TODO change back to 30s
		} else if (level == 2 || level == 3) {
			LevelSortHelper(5f, 0f, 0f, 45f);
		} else if (level == 4 || level == 5) {
			LevelSortHelper(5f, 12f, 0f, 70f + Random.Range(0f, 5f));
		} else if (level == 6) {
			LevelSortHelper(2f, 0f, 40f, 51f);
		} else if (level == 7 || level == 8) {
			LevelSortHelper(4f, 10f, 0f, 62f);
		} else if (level == 9) {
			LevelSortHelper(5.5f, 8f, 0f, 25f);
		} else if (level == 10 || level == 11) {
			LevelSortHelper(4.5f, 8f, 30f, 77f + Random.Range(0f, 10f));
		} else if (level == 12 || level == 13) {
			LevelSortHelper(5f, 15f, 20f, 50f + Random.Range(0f, 4f));
		} else if (level == 14 || level == 15) {
			LevelSortHelper(3f, 10f, 20f, 85f);
		} else if (level == 16) {
			LevelSortHelper(1.5f, 0f, 25f, 55f);
		} else if (level == 17 || level == 18) {
			LevelSortHelper(3.5f, 8f, 26f, 95f + Random.Range(0f, 5f));
		} else if (level == 19 || level == 20) {
			LevelSortHelper(4.5f, 4f, 35f, 60f);
		} else if (level == 21 || level == 22) {
			LevelSortHelper(7f, 3f, 15f, 45f + Random.Range(0f, 10f));
		} else if (level == 23 || level == 24) {
			LevelSortHelper(4f, 5f, 10f, 75f + Random.Range(0f, 5f));
		} else if (level == 25) {
			LevelSortHelper(1f, 0f, 13f, 35f);
		} else if (level >= 26) {
			LevelSortHelper(4f, 4f, 10f, 135f);
		}
	}

	void LevelSortHelper(float msRate, float gRate, float pRate, float tWarp) {
		meteorSpawnRate = msRate;
		graySpawnRate = gRate;
		pinkSpawnRate = pRate;
		timeToWarp = tWarp;		
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

	void SpawnUFO(int ufoNum) {      //Ufos 0=pink 1=gray
		int spawnNum = Random.Range(0, 4);   //spawners 0=right 1=left 2=top 3=bottom
		//ufoNum = 0; //Random.Range(0, 2);   
		Vector3 min2, max2;

		//Transform spawn = UFOSpawners[Random.Range(0, UFOSpawners.Length)].transform;
		Transform spawn = UFOSpawners[spawnNum].transform;
		min2 = spawn.GetComponent<BoxCollider>().bounds.min;
		max2 = spawn.GetComponent<BoxCollider>().bounds.max;
		Vector3 loc = new Vector3(Random.Range(min2.x, max2.x), Random.Range(min2.y, max2.y), Random.Range(min2.z, max2.z));
//		GameObject go = Instantiate(Ufos[1], spawn.position, Quaternion.identity, parMet) as GameObject;
		GameObject go = Instantiate(Ufos[ufoNum], loc, Quaternion.identity, parMet) as GameObject;
		go.GetComponent<HypUFO>().startingSpawner = spawnNum;
	}

	void Update () {
		//*** testing ***
		if (Input.GetKeyDown(KeyCode.Space) && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))) {
			SpawnMeteor();
		}
		if (Input.GetKeyDown(KeyCode.U) && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))) {
			SpawnUFO(0);
		}
		//***************

		float t = Time.deltaTime;
		meteorSpawnTime -= t;
		graySpawnTime -= t;
		pinkSpawnTime -= t;
		if (meteorSpawnTime <= 0f && meteorSpawnRate > 0f) {
			meteorSpawnTime = meteorSpawnRate * Random.Range(0.75f, 1.25f);
			SpawnMeteor();
		}
		if (graySpawnTime <= 0f && graySpawnRate > 0f) {
			graySpawnTime = graySpawnRate * Random.Range(0.75f, 1.25f);
			SpawnUFO(1);
		}
		if (pinkSpawnTime <= 0f && pinkSpawnRate > 0f) {
			pinkSpawnTime = pinkSpawnRate * Random.Range(0.75f, 1.25f);
			SpawnUFO(0);
		}

	}
}
