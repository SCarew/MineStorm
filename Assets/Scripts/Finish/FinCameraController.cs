using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinCameraController : MonoBehaviour {

	private Transform ship;
	private Camera cam;
	[SerializeField] private GameObject[] pre_Dense;
	private Transform parMeteors;
	private float initZ;
	private Vector3[] coords;
	private int steps = 5;
	//private float smoothing = 2.5f;
	public  bool fixedCam = false;  //true = camera is fixed on ship
	private float smoothTime = 0.5f;
	private Vector3 velocity = Vector3.zero;
	private bool bCameraScroll = false;   //true = move camera away from ship
	private bool bDense = true;    //one-time flag for spawn dense coroutine
	private bool bFloat = false;   //true to float camera back
	private bool bShipDestroyed = false;   //true when ship is destroyed()
	private bool bExit = false;  //true when scene can be exited
	private float timeScroll = 2f;
	private float maxTimeScroll = 10f;
	private LayerMask myLayerMask;
	private Text txtQ, txtE;

	void Start () {
		ship = GameObject.Find("Fin_PlayerShip").transform;	
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		parMeteors = GameObject.Find("Meteors").transform;
		initZ = transform.position.z;
		coords = new Vector3[steps];
		for (int i=0; i<steps; i++) {
			coords[i] = new Vector3(ship.position.x, ship.position.y, initZ);
		}
		myLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Meteor")) | (1 << LayerMask.NameToLayer("Enemy"));
		myLayerMask = myLayerMask | (1 << LayerMask.NameToLayer("MapPlayer")) | (1 << LayerMask.NameToLayer("MapMeteor")) | (1 << LayerMask.NameToLayer("MapEnemy"));
		txtQ = GameObject.Find("txtQuestion").GetComponent<Text>();
		txtE = GameObject.Find("txtExclamation").GetComponent<Text>();
	}

	void SpawnDense() {
		//x=-20 y=32 <-Center of cam
		if (bDense) {
			bDense = false;
			StartCoroutine(Spawn());
		}
	}

	IEnumerator Spawn() {
		float max = 10f;
		float xCam = cam.transform.position.x;
		float yCam = cam.transform.position.y;
		float zCam = 20f;
		GameObject go;
		Vector3 loc;
		//Vector3[] v3s = new Vector3[15];
		FloatCamera();
		for (int i=0; i<25; i++) {
			loc = new Vector3(Random.Range(-max*2f, max*2f) + xCam, Random.Range(-max, max) + yCam, Random.Range(-max, max) + zCam);
			go = Instantiate(pre_Dense[Random.Range(0, pre_Dense.Length)], new Vector3(0f, 0f, 0f), Quaternion.identity, parMeteors) as GameObject;
			while (FreeLocation(loc, 1.5f) == false) {
				Debug.Log(" by " + go.name + " at " + loc);
				loc -= new Vector3(1.5f, 1.5f, 0f);
			}
			go.GetComponentInChildren<FinMeteorControl>().SetLocation(loc);
			yield return new WaitForSeconds(0.25f);
		}
	}

	private void FloatCamera() {
		Destroy(ship.gameObject);
		bShipDestroyed = true;
		bFloat = true;
		timeScroll = 10f;
		maxTimeScroll = timeScroll;
		//GameObject.Find("txtQuestion").GetComponent<Text>().text = "?";
	}

	private bool FreeLocation (Vector3 coords, float fSize) {
		//returns true when location is free
		Collider[] hitColl = Physics.OverlapSphere(coords, fSize, myLayerMask.value);
		if (hitColl.Length > 0) {Debug.Log ("Overlap with " + hitColl[0].gameObject.name);}
		return (hitColl.Length == 0);
	}

	void LateUpdate() {
		if (!fixedCam && !bShipDestroyed) {
			transform.position = Vector3.SmoothDamp(transform.position, coords[0], ref velocity, smoothTime);
		}
	}

	void Update() {
		if (bCameraScroll) {
			float t = Time.deltaTime;
			timeScroll -= t;
			if (timeScroll > 0f) {
				float xRate = -5f * t;
				float yRate = 20f * t;
				transform.position = new Vector3(transform.position.x + xRate, transform.position.y + yRate, initZ);
			} else {
				timeScroll = 0f;
				bCameraScroll = false;
				SpawnDense();
			}
			return;
		}
		if (!bShipDestroyed) {
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

		if (bFloat) {
			Vector3 dist = new Vector3(0f, -1f, 0f);
			cam.orthographicSize = 5f + ((maxTimeScroll - timeScroll) * 9f / maxTimeScroll);
			timeScroll -= Time.deltaTime;
			if (timeScroll < 0) { 
				bFloat = false; 
				timeScroll = 0; 
				bExit = true;
			}
			Color c = txtE.color;
			c.a = timeScroll/maxTimeScroll;
			txtE.color = c;
			c.a = (maxTimeScroll - timeScroll)/maxTimeScroll;
			txtQ.color = c;
		}

		if (bExit || Input.GetKeyDown(KeyCode.Escape)) {
			if (Input.GetButtonDown("Primary") || Input.GetButtonDown("Secondary") || Input.GetKeyDown(KeyCode.Escape)) {
				GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadScene("Title");
				return;
			}
		}
	}

	public void ScrollCamera() {
		bCameraScroll = true;
		fixedCam = true;
	}
}
