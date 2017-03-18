using System.Collections;
using UnityEngine;

public class HypLaser : MonoBehaviour {

	private float fireSpeed = 15.0f;
	private int damage = 100;

	public float lifetime = 3.0f;
	[SerializeField] private GameObject pre_Explosion;
	[SerializeField] private GameObject pre_LaserTimedExplosion;
	private Rigidbody rb;
	private float lifeSpent = 0f;
	private bool bMissile = false;
	private Quaternion rot;
	private float missVel;
	static private Transform parEff;  //for empty parent container
//	static private GameManager gm;
	static private SoundManager aud;
	static private Camera cam;
	static private Transform quad;
	static private HypCrosshair cross;

	void Start () {
		Destroy(gameObject, lifetime);
		Rigidbody shipRb = GameObject.Find("Hyp_PlayerShip").GetComponent<Rigidbody>();
		rb = GetComponent<Rigidbody>();
//		if (gm == null) 
//			{ gm = GameObject.Find("GameManager").GetComponent<GameManager>(); }
		if (parEff == null) 
			{ parEff = GameObject.Find("Effects").transform; }
		if (!aud) 
			{ aud = GameObject.Find("SoundManager").GetComponent<SoundManager>(); }
		if (cam == null) 
			{ cam = GameObject.Find("Main Camera").GetComponent<Camera>(); }
		if (quad == null)
			{ quad = GameObject.Find("Background").transform; }
		if (cross == null)
			{ cross = GameObject.Find("Crosshair").GetComponent<HypCrosshair>(); }
		
		//rb.MoveRotation(shipRb.rotation);
		//Vector3 v3 = new Vector3(0f, -1f, 0f);
		//transform.rotation = Quaternion.FromToRotation(transform.forward, transform.forward + v3);
		//rb.MoveRotation(Quaternion.Euler(new Vector3(0f, 45f, 0f)));
		//transform.localRotation.eulerAngles = new Vector3(0f, 45f, 0f);
		//rb.rotation = Quaternion.AngleAxis(10f, transform.forward);
		GetTarget();
		//transform.LookAt(v3);
		//transform.RotateAround(transform.position, transform.right, -1.25f);
		Vector3 f = fireSpeed * transform.forward;
		rb.AddForce(f, ForceMode.VelocityChange);
		//Debug.Log(rb.transform.localRotation.eulerAngles + " + " + f);

		aud.PlaySoundImmediate(gameObject.name);   

	}
	
	void Update () {
		lifeSpent += Time.deltaTime;
	}

	public int GetDamage() {
		return damage;
	}

	private void GetTarget() {
		Vector3 target = GameObject.Find("Crosshair").GetComponent<RectTransform>().position;
		//Vector3 pos = new Vector3(target.x, target.y, quad.position.z * 1.5f);
		Vector3 pos = new Vector3(target.x, target.y, cross.GetZDistance() - cam.transform.position.z);
	    pos = cam.ScreenToWorldPoint(pos);
	    //pos.z = cross.GetZDistance();
    	Vector3 aimingDirection = pos - transform.position;
     	transform.rotation = Quaternion.LookRotation(aimingDirection);
     	Debug.Log("pos = " + pos);
	}

	void OnDestroy() {
//		if (gm.bGameOver) { return; }
		Vector3 pos = transform.position;
		pos -= 0.4f * (rb.velocity.normalized);   //for correcting explosion location
		//Debug.Log(transform.position + " -> " + pos);
		GameObject go;
		//Debug.Log(lifetime + " vs " + lifeSpent + " t=" + (lifetime - lifeSpent));
		if ((lifetime - 0.015f) <= lifeSpent) {
			go = Instantiate(pre_LaserTimedExplosion, pos, Quaternion.identity) as GameObject;
		} else {  //for everything but timed laser exp
			go = Instantiate(pre_Explosion, pos, Quaternion.identity) as GameObject;
			aud.PlaySoundVisible("expLaser", gameObject.transform); 
		}
		go.transform.SetParent(parEff);
//		if ((gameObject.name == "Laser") || (gameObject.name == "UFOLaser"))
//			{ go.transform.rotation = MakeInverse(transform.rotation); }
		Destroy (go, 2.0f);

	}

}
