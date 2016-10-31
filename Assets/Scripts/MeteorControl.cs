using UnityEngine;
using System.Collections;

public class MeteorControl : MonoBehaviour {

	private GameManager gm;
	//private Rigidbody rb;
	private float moveSpeed;
	private float rotTime = 1f / 3f;  // denom = num of secs
	private float x,y,v,h,w;
	private float zDepth = 0f;
	private int iSize = 3;  //default 3=big 2=medium 1=small

	private Transform parObj;
	private EnemyHealth eh;
	private Vector3 location = new Vector3(0f, 0f, 1f);

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		//rb = GetComponentInChildren<Rigidbody>();
		eh = gameObject.GetComponentInParent<EnemyHealth>();
		parObj = transform.parent.transform;

		SetSize ();

		x = Random.Range(1f, gm.level_width) - (gm.level_width/2);
		y = Random.Range(1f, gm.level_height) - (gm.level_height/2);

		if (location.z == 1) {
			parObj.position = new Vector3(x, y, zDepth);
		} else {
			//Debug.Log(gameObject.name + " spawned at " + location);
			parObj.position = location;
		}

		h = Random.Range(-1f, 1f);
		v = Random.Range(-1f, 1f);
		w = Random.Range(-1f, 1f);
		moveSpeed = Random.Range(0.1f, 4 + gm.currentLevel) + (3-iSize);

	}

	public void SetLocation(Vector3 loc) {
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

	void Update () {
		float h0 = h * 360f * Time.deltaTime * rotTime;
		float v0 = v * 360f * Time.deltaTime * rotTime;
		float w0 = w * 360f * Time.deltaTime * rotTime;
		float h1 = h * Time.deltaTime * moveSpeed;
		float v1 = v * Time.deltaTime * moveSpeed;

		parObj.Rotate(h0, v0, w0, Space.Self);
		parObj.Translate(v1, h1, 0f, Space.World);
	}

	void OnCollisionEnter(Collision coll) {
		int damage = 100;  //temp test
		Debug.Log(coll.gameObject.name + " hit for " + damage);
	
		if (coll.gameObject.tag == "Laser") {
			damage = coll.gameObject.GetComponent<TorpedoController>().GetDamage();
			eh.DamageHealth(damage);
			Debug.Log(gameObject.name + " hit for " + damage + " with " + coll.relativeVelocity.magnitude + " vel");
			Destroy(coll.gameObject);
		}
		if (coll.gameObject.tag == "Player") {
			eh.DamageHealth(damage);
			coll.gameObject.GetComponentInParent<ShipHealth>().DamageHealth(100);
		}
	}

}
