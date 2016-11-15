using UnityEngine;
using System.Collections;

public class MeteorControl2 : MonoBehaviour {

	private GameManager gm;
	private Rigidbody rb;
	private EnemyHealth eh;
	private Transform parObj;
	private Transform pShip;

	private float moveSpeed;
	private float rotTime = 1f / 6f;  // denom = num of secs
	private float x,y,v,h,w;
	private float zDepth = 0f;
	private int iSize = 3;  //default 3=big 2=medium 1=small
	private Vector3 location = new Vector3(0f, 0f, 1f);
	private float spawnDist = 5f;  //distance from ship meteors can spawn
	private float spawnChildDist = 1f;  //distance from ship child meteors can spawn
	private float magDistance = 12f;  //distance from ship Magnets are attracted

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = transform.GetComponentInParent<Rigidbody>();
		eh = gameObject.GetComponentInParent<EnemyHealth>();
		pShip = GameObject.Find ("PlayerShip").transform;
		parObj = transform; //transform.parent.transform;

		SetSize ();

		if (location.z == 1) {  //spawn new meteor
			SetXY ();
			parObj.position = new Vector3(x, y, zDepth);
		} else {  				//spawning child meteor from meteor
			CheckChildXY();
			parObj.position = location;
		}

		h = Random.Range(-1f, 1f);
		v = Random.Range(-1f, 1f);
		w = Random.Range(-1f, 1f);
		moveSpeed = Random.Range(0.4f, 4 + gm.currentLevel) + (3-iSize);


		rb.AddForce(new Vector3(h * moveSpeed, v * moveSpeed, 0f), ForceMode.VelocityChange);
		rb.AddTorque(new Vector3(h * 360 * rotTime, v * 360 * rotTime, w * 360 * rotTime), ForceMode.Force);
	}

	public void SetLocation(Vector3 loc) {
		//parObj.position = loc;
		location = loc;
	}

	void SetSize ()	{
		string s = parObj.name.ToUpper ();
		if (s.Contains (".M."))
			{ iSize = 2; eh.SetHealth(50); }
		else if (s.Contains (".S.")) 
			{ iSize = 1; eh.SetHealth(25); }
		else //big meteor
			{ eh.SetHealth(100); }
	}

	public int GetSize() {
		return iSize;
	}

	void SetXY ()
	{
		//check that random location for spawn isn't too near ship
		bool bTooClose = true;
		while (bTooClose == true) {
			x = Random.Range (1f, gm.level_width) - (gm.level_width / 2);
			y = Random.Range (1f, gm.level_height) - (gm.level_height / 2);
			if (Vector3.Distance (pShip.position, new Vector3 (x, y, zDepth)) > (spawnDist)) {
				bTooClose = false;
			}
			else {
				Debug.Log (gameObject.name + ":" + x + ", " + y + ", " + zDepth + " = " + Vector3.Distance (pShip.position, new Vector3 (x, y, zDepth)));
			}
		}
	}

	void CheckChildXY() {
		Vector3 v3_dist = location - pShip.position;
		if (Vector3.Distance (pShip.position, location) < spawnChildDist) {
			Debug.Log("Original loc=" + location + " ship=" + pShip.position);
			Vector3 v3_add = new Vector3(0, 0, 0);
			if (v3_dist.x < 0) {v3_add += new Vector3(-1, 0, 0);}
			if (v3_dist.x > 0) {v3_add += new Vector3( 1, 0, 0);}
			if (v3_dist.y < 0) {v3_add += new Vector3( 0, -1, 0);}
			if (v3_dist.y > 0) {v3_add += new Vector3( 0, 1, 0);}
			v3_add = v3_add * spawnChildDist;
			location += v3_add;
			Debug.Log("  New loc=   " + location + " ship=" + pShip.position);
		}
	}

	//void Update () {
		/*
		float h0 = h * 360f * Time.deltaTime * rotTime;
		float v0 = v * 360f * Time.deltaTime * rotTime;
		float w0 = w * 360f * Time.deltaTime * rotTime;
		float h1 = h * Time.deltaTime * moveSpeed;
		float v1 = v * Time.deltaTime * moveSpeed;

		parObj.Rotate(h0, v0, w0, Space.Self);
		parObj.Translate(v1, h1, 0f, Space.World);
		*/
	//}

	void FixedUpdate() {
		if (eh.myType == GameManager.mine.Magnet || eh.myType == GameManager.mine.Test) {
			if (Vector3.Distance(gameObject.transform.position, pShip.position) < magDistance) {
				//finish this
				Vector3 attract = Vector3.Normalize(pShip.position - gameObject.transform.position);
				//Debug.Log(gameObject.name + ":" + attract + " Vel=" + rb.velocity);
				rb.AddForce(attract * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
				rb.AddTorque(attract * Time.deltaTime, ForceMode.Force);
				if (rb.velocity.sqrMagnitude > (moveSpeed * moveSpeed)) {
					//rb.AddForce(-attract * Time.deltaTime, ForceMode.VelocityChange);
					rb.velocity = rb.velocity.normalized * moveSpeed;
				}
			}
		}
	}

	void OnCollisionEnter(Collision coll) {
		int damage = 100;  //temp test
		//Debug.Log(coll.gameObject.name + " hit for " + damage);
	
		if (coll.gameObject.tag == "Laser") {
			damage = coll.gameObject.GetComponent<TorpedoController>().GetDamage();
			eh.DamageHealth(damage);
			Debug.Log(gameObject.name + " hit for " + damage + " with " + coll.relativeVelocity.magnitude + " vel");
			Destroy(coll.gameObject);
		}
		if (coll.gameObject.tag == "Player") {
			eh.DamageHealth(400);
			coll.gameObject.GetComponentInParent<ShipHealth>().DamageHealth(gm.mineHit);
		}
	}

}
