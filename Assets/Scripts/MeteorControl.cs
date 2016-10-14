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

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		//rb = GetComponentInChildren<Rigidbody>();
		parObj = transform.parent.transform;

		SetSize ();

		x = Random.Range(1f, gm.level_width) - (gm.level_width/2);
		y = Random.Range(1f, gm.level_height) - (gm.level_height/2);

		parObj.position = new Vector3(x, y, zDepth);

		h = Random.Range(-1f, 1f);
		v = Random.Range(-1f, 1f);
		w = Random.Range(-1f, 1f);
		moveSpeed = Random.Range(0.1f, 5f) + (3-iSize);

	}

	void SetSize ()	{
		string s = parObj.name.ToUpper ();
		if (s.Contains (".M."))
			{ iSize = 2; }
		else if (s.Contains (".S.")) 
			{ iSize = 1; }
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
			gameObject.GetComponentInParent<EnemyHealth>().DamageHealth(damage);
			Debug.Log(gameObject.name + " hit for " + damage + " with " + coll.relativeVelocity.magnitude + " vel");
		}
		if (coll.gameObject.tag == "Player") {
			gameObject.GetComponentInParent<EnemyHealth>().DamageHealth(damage);
			Debug.Log("Enemy " + coll.gameObject.name + " hit for " + damage);
		}
		Destroy(coll.gameObject);
	}

}
