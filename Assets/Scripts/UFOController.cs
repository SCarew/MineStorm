using UnityEngine;
using System.Collections;

public class UFOController : MonoBehaviour {

	private Rigidbody rb;
	//private Vector3 destination;
	private GameManager gm;
	private Transform pShip;
	private Transform parEff;
	private Vector3 originalScale;
	private Transform renderChild;
	private EnemyHealth eh;
	private MeshCollider[] mc;
	private float timeToDest, timeSpent = 0f;
	private float timeToWarp = 30f;
	private float moveSpeed = 5.0f;   //min (max=+50%)
	private float rotateSpeed = 30f;  //abs max
	private float maxPursueDistance = 24f;   //for UFO_01 purple ship
	private float minPursueDistance = 8f;    //for UFO_01 purple ship
	private bool bPursueMovement = false;

	[SerializeField] private GameObject pre_WarpEnter;
	private bool adjustScaleIn, adjustScaleOut = false;
	private float timeScaleIn = 1.3f;   //time to fully warp into hyperspace
	private float timeScaleOut = 1f;   //time to warp into regular space

	[SerializeField] private GameObject pre_laser;
	[SerializeField] private GameObject pre_torpedo;

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		pShip = GameObject.Find("PlayerShip").transform;
		parEff = GameObject.Find("Effects").transform;
		mc = GetComponentsInChildren<MeshCollider>(true);
		renderChild = GetComponentInChildren<MeshRenderer>().gameObject.transform;
		eh = GetComponent<EnemyHealth>();
		originalScale = renderChild.localScale;

		rotateSpeed = Random.Range(-rotateSpeed, rotateSpeed);
		moveSpeed = Random.Range(0f, moveSpeed) + (moveSpeed / 2f);
		rb = GetComponent<Rigidbody>();
		rb.AddTorque(transform.forward * rotateSpeed, ForceMode.Force);

		adjustScaleOut = true;
		renderChild.localScale = Vector3.zero;

		Invoke("SetMovement", timeScaleOut);
		timeToWarp += Random.Range(0f, timeToWarp/2f);

		if (gameObject.name == "UFO.01") {
			StartCoroutine(UseTorp());
		} else if (gameObject.name == "UFO.02") {
			StartCoroutine(UseLaser());
		}
	}

	void SetMovement() {
		float x_off = gm.level_width / 2f;
		float y_off = gm.level_height / 2f;

		Vector3 v3; 
		//do {
			v3 = new Vector3(Random.Range(-x_off, x_off), Random.Range(-y_off, y_off), 0f);
		//} while (v3.sqrMagnitude < 1f);
		v3 += transform.position;
		rb.velocity = Vector3.zero;
		rb.AddForce(v3.normalized * moveSpeed, ForceMode.VelocityChange);
		timeToDest = 3f + Random.Range(0f, 12f);

	}

	IEnumerator UseLaser() {
		yield return new WaitForSeconds(timeScaleOut);
		bool bLoop = true;
		while (bLoop) {
			yield return new WaitForSeconds(1f);
			GameObject go = Instantiate(pre_laser, transform.position, Quaternion.identity) as GameObject;
			go.name = "UFOLaser";
			go.tag = "EnemyLaser";
			go.layer = LayerMask.NameToLayer("Enemy fire");
			go.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

			if (adjustScaleIn) { bLoop = false; }
		}
	}

	IEnumerator UseTorp() {
		yield return new WaitForSeconds(timeScaleOut);
		bool bLoop = true;
		float t = 0.2f;
		while (bLoop) {
			yield return new WaitForSeconds(t);
			if (bPursueMovement) {
				t = 1f + Random.Range(0f, 1f);
				GameObject go = Instantiate(pre_torpedo, transform.position, Quaternion.identity) as GameObject;
				go.name = "UFOTorp";
				go.tag = "EnemyLaser";
				go.layer = LayerMask.NameToLayer("Enemy fire");
				//go.transform.rotation = Quaternion.LookRotation((pShip.position - go.transform.position), Vector3.right);
				Vector3 diff = pShip.position - transform.position;
//				string s = "";
//				if (diff.x >= 0f && diff.y >= 0) {s = "+ + ";}
//				if (diff.x < 0f && diff.y >= 0)  {s = "- + ";}
//				if (diff.x >= 0f && diff.y < 0)  {s = "+ - ";}
//				if (diff.x < 0f && diff.y < 0)   {s = "- - ";}
				float deg = Mathf.Atan2(diff.y, diff.x) * 180/Mathf.PI + 270f;
				deg = deg + Random.Range(-30f, 30f);
				//go.transform.rotation = Quaternion.AngleAxis(Vector3.Angle(diff, Vector3.right), Vector3.forward);
				go.transform.rotation = Quaternion.AngleAxis(deg, Vector3.forward);
				//Debug.Log(s + (pShip.position - go.transform.position) + "  " + go.transform.rotation.eulerAngles + "  d=" + deg);
			} else {
				t = 0.2f;
			}
			if (adjustScaleIn) { bLoop = false; }
		}
	}

	void HyperJump() {
		if (pre_WarpEnter == null) { return; }

		GameObject go = Instantiate(pre_WarpEnter, transform.position, Quaternion.identity, parEff) as GameObject;
		if (gameObject.name == "UFO.01") {
			foreach (Transform t in go.transform) {
				t.localScale = t.localScale * 1.5f;
			}
		}
		adjustScaleIn = true;
		rb.velocity = Vector3.zero;
		timeSpent = 0f;
	}

	void Update () {
		timeToWarp -= Time.deltaTime;
		timeSpent += Time.deltaTime;
		if (adjustScaleIn) {    //entering hyperspace
			foreach (MeshCollider mc1 in mc) {
				mc1.enabled = false; }
			if (timeSpent > timeScaleIn) {
				renderChild.localScale = new Vector3(0f, 0f, 0f);
				Destroy(gameObject, 0.1f);
			} else {
				renderChild.localScale = originalScale * (1 - (timeSpent / timeScaleIn));
			}
			return;
		}

		if (adjustScaleOut) {   //exiting hyperspace
			foreach (MeshCollider mc1 in mc) {
				mc1.enabled = false; }
			if (timeSpent > timeScaleOut) {
				adjustScaleOut = false;
				renderChild.localScale = originalScale;
				timeSpent = 0f;
				foreach (MeshCollider mc1 in mc) 
					{ mc1.enabled = true; }
			} else {
				float ts = timeSpent / timeScaleOut;
				if (ts < 0.1f) { ts = 0.1f; }
				renderChild.localScale = originalScale * ts;
			}
			return;
		}

		if (gameObject.name == "UFO.01") {
			float f = Vector3.Distance(transform.position, pShip.position);
			if (f < maxPursueDistance && f > minPursueDistance) {
				bPursueMovement = true;
			} else {
				if (bPursueMovement) {  //if losing Pursue movement (ie too close/far this frame)
					timeSpent = timeToDest + 0.1f;
				}
				bPursueMovement = false;
			}
		}

		if (bPursueMovement) {
			Vector3 v1 = pShip.position - transform.position;
			rb.AddForce(rb.velocity.normalized * -moveSpeed * Time.deltaTime * 0.5f, ForceMode.Impulse);
			rb.AddForce(v1.normalized * moveSpeed * Time.deltaTime, ForceMode.Impulse);
			//Debug.Log("*" + gameObject.name + "=" + rb.velocity.normalized);
			if ((moveSpeed * moveSpeed + 2f) < rb.velocity.sqrMagnitude) {
				rb.velocity = rb.velocity.normalized * moveSpeed;
			}
		}

		if (timeToWarp <= 0f)
			{ HyperJump(); }
		else if (timeSpent > timeToDest) {
			SetMovement();
			timeSpent = 0f;
		}
	}

	void OnCollisionEnter(Collision coll) {
		int damage;
		if (gameObject.name == "UFO.01") { damage = 150; }
		else { damage = 100; }
		//Debug.Log(coll.gameObject.name + " hit for " + damage);

		if (coll.gameObject.tag == "Laser") {
			damage = coll.gameObject.GetComponent<TorpedoController>().GetDamage();
			eh.DamageHealth(damage);
			//Debug.Log(gameObject.name + " hit for " + damage + " with " + coll.relativeVelocity.magnitude + " vel");
			Destroy(coll.gameObject);
		}
		if (coll.gameObject.tag == "Player") {
			eh.DamageHealth(200);
			coll.gameObject.GetComponentInParent<ShipHealth>().DamageHealth(damage);
		}
		if (coll.gameObject.tag == "Meteor") {
			eh.DamageHealth(coll.gameObject.GetComponent<MeteorControl2>().GetSize() * 25);
			coll.gameObject.GetComponentInParent<EnemyHealth>().DamageHealth(damage);
		}
	}
}
