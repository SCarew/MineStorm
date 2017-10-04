using UnityEngine;
using System.Collections;

public class FinMeteorControl : MonoBehaviour {

	private Rigidbody rb;
	private Transform parObj;
	private Transform parWarp;
	private SoundManager aud;

	[SerializeField] private GameObject pre_Warp;   //whirlpool effect
	private float timeScale = 1f;  //time for child meteor to warp in 
	private float timeSpent = 0f;    //counter to timeScale
	private bool adjustScale = true;  //is meteor warping in?

	private float moveSpeed;
	private float rotTime = 1f / 6f;  // denom = num of secs
	private float x,y,v,h,w;
	private float zDepth = 0f;
	private int iSize = 3;  //default 3=big 2=medium 1=small
	private Vector3 location = new Vector3(0f, 0f, 1f);
	private float spawnDist = 5f;  //distance from ship meteors can spawn
	private float spawnChildDist = 1.5f;  //distance from ship child meteors can spawn
	private float magDistance = 12f;  //distance from ship Magnets are attracted
	private float bholeDistance = 12f;  //distance from ship BHole mines attract

	void Start () {
		rb = transform.GetComponentInParent<Rigidbody>();
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		parObj = gameObject.transform; //transform.parent.transform;
		parWarp = GameObject.Find("Effects").transform;

		iSize = 3;

		if (location.z == 1) {  //spawn new meteor
			parObj.position = new Vector3(x, y, zDepth);
		} else {  				//spawning child meteor from meteor
			parObj.position = location;
		}

		SpawnSwirl(); 

		h = Random.Range(-1f, 1f);
		v = Random.Range(-1f, 1f);
		w = Random.Range(-1f, 1f);
		moveSpeed = Random.Range(0.4f, 2f);

		StartMovement();
	}

	void StartMovement() {
		rb.AddForce(new Vector3(h * moveSpeed, v * moveSpeed, w/2 - 2f), ForceMode.VelocityChange);
		rb.AddTorque(new Vector3(h * 360 * rotTime, v * 360 * rotTime, w * 360 * rotTime), ForceMode.Force);
		StartCoroutine(CheckVelocity());
	}

	public void SetLocation(Vector3 loc) {
		//parObj.position = loc;
		location = loc;
	}

	IEnumerator CheckVelocity() {  //to prevent fast overlapping meteor bug
		yield return new WaitForSeconds(0.05f);
		float vel = rb.velocity.sqrMagnitude;
		if (vel > (moveSpeed * moveSpeed)) {
			rb.velocity = rb.velocity.normalized * moveSpeed; 
			Debug.Log(gameObject.name + " vel reduced from " + Mathf.Sqrt(vel) + " to " + rb.velocity.magnitude + " with max of " + moveSpeed);
		}
		//yield return new WaitForSeconds(1.6f);
		//Debug.Log(gameObject.name + "+2 secs vel = " + rb.velocity.magnitude);
	}

	void Update() {
		if (adjustScale) {  //for warping in
			timeSpent += Time.deltaTime;
			if (timeSpent > timeScale) {
				adjustScale = false;
				parObj.localScale = new Vector3(1f, 1f, 1f);
			} else {
				parObj.localScale = new Vector3(1f, 1f, 1f) * (timeSpent / timeScale);
			}
		}
	}

	void SpawnSwirl() {
		if (pre_Warp == null) { return; }

		float mult = 1f;
		GameObject go = Instantiate(pre_Warp, parObj.position, Quaternion.identity, parWarp) as GameObject;
		if (iSize == 2) 
			{ mult = 0.45f; }
		else if (iSize == 1) 
			{ mult = 0.25f; }
		foreach (Transform t in go.transform) {
			t.localScale = t.localScale * mult;
		}
		//aud.PlaySoundConstant("swirlSmall", go.transform);
		adjustScale = true;
	}

	void OnCollisionEnter(Collision coll) {
		int damage = 100;  //temp test
		//Debug.Log(coll.gameObject.name + " hit for " + damage);
	
		if (coll.gameObject.tag == "MeteorParent") {
			//Debug.Log("!!! Meteor Collision detected for " + coll.gameObject.tag);
			aud.PlaySoundVisible("meteorHit", gameObject.transform, 1);
		}
	}

}
