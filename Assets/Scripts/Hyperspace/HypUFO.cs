using System.Collections;
using UnityEngine;

public class HypUFO : MonoBehaviour {

	public  enum UFOType {Pink, Gray};
	public  UFOType myType = UFOType.Gray;
	[SerializeField] private GameObject ps_Pieces;
	[SerializeField] private GameObject ps_SmokeExplosion;
	[SerializeField] private GameObject pre_SmallMeteor;
	[SerializeField] private GameObject pre_MediumMeteor;
	[SerializeField] private GameObject pre_BigMeteor;
	[SerializeField] private GameObject pre_PS_Launch;   //for meteor weapon launch
	private Transform parEff, parMet;
	private SoundManager aud;
	private ScoreManager sm;
	private HypShipHealth pShipHealth;
	private Transform pShip;
	private Rigidbody rb;
	//private Vector3 myTarget;
	private Vector3 myGravity;
	private bool bInCorridor = false;
	private bool bNewlySpawned = false;
	private bool bOutOfBounds = false;
	private int health = 3;
	private int pinkScore = 225, grayScore = 150;
	private float weaponTime = 0.8f;
	private float maxDistance = 30f;   //for remaining in play
	private float rotateSpeed = 5f;

	private Vector3 velocity, gravity;
	public  int startingSpawner = 1;   //set from HypSpawner.cs

	void Start () {
		if (myType == UFOType.Gray) { 
			health = 2; 
			weaponTime = 1.6f;
		}
				
		parEff = GameObject.Find("Effects").transform;
		parMet = GameObject.Find("Meteors").transform;
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		pShip = GameObject.Find("Hyp_PlayerShip").transform;
		pShipHealth = pShip.GetComponent<HypShipHealth>();
		rb = GetComponent<Rigidbody>();
		bNewlySpawned = true;

		StartCoroutine(FireUpdate());

		velocity = InitialVelocity(startingSpawner);
		rb.velocity = velocity;
		//Debug.Log("init vel = " + velocity + " [" + startingSpawner + "]");
		rotateSpeed = Random.Range(1.5f, 2.5f);

		InitialRotation(startingSpawner);
		aud.PlaySoundConstant("UFOHum", gameObject.transform);
	}

	void TakeHit() {
		ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
		ps.Play();
	}

	void Explode () {
		GameObject go = Instantiate(ps_Pieces, transform.position, Quaternion.identity, parEff) as GameObject;
		if (myType == UFOType.Pink) {
			aud.PlaySoundVisible("explosionUFO1", transform);
			sm.AddScore(pinkScore);
		} else {
			aud.PlaySoundVisible("explosionUFO2", transform);
			sm.AddScore(grayScore);
		}
		Destroy(go, 1f);
		Destroy(gameObject);
	}

	void MakeSmoke() {    //triggered when UFO leaves bounds of corridor
		if (bOutOfBounds) { return; }
		bOutOfBounds = true;
		GameObject go;
		//go = Instantiate(ps_SmokeExplosion, transform.position + new Vector3(0f, 0f, -0.5f), Quaternion.identity, parEff) as GameObject;
		go = Instantiate(ps_SmokeExplosion, transform.position + new Vector3(0f, 0f, -0.5f), Quaternion.identity, gameObject.transform) as GameObject;
		Destroy(go, 1f);
		Destroy(gameObject, 1f);  //was 0.4f
	}

	public void NowInCorridor(bool bCorr) {
		bInCorridor = bCorr;
	}

	void FixedUpdate() {
		if (bOutOfBounds) {  //already out of bounds
			gameObject.transform.localScale -= new Vector3(1f/30f, 1f/30f, 1f/30f);
		}
		if (OutOfBounds()) {  //enters out of bounds
			MakeSmoke();
			return;
		}
		if (myType == UFOType.Gray && bNewlySpawned) {
			SetGrayTarget();
		}
//		float t = Time.deltaTime;
//		Vector3 v = transform.position - myTarget;
//		v = v * t + 0.5f * t * t * myGravity;
//		transform.position = v;

//		float speed = 2f;
//		Vector3 v = (transform.position - myTarget).normalized * speed;
//		rb.velocity += v * Time.deltaTime;
		velocity = velocity - gravity * Time.deltaTime;
		//Debug.Log("vel=" + velocity);
		if (gameObject.transform.position.z >= 37) {  //keep in front of background
			velocity = new Vector3(velocity.x, velocity.y, -velocity.z);
		}
		if (gameObject.transform.position.z <= 2) {   //keep in front of playership
			velocity = new Vector3(velocity.x, velocity.y, -velocity.z);
		}
		rb.velocity = velocity;

		float alterGrav = 0.01f;
		float x0 = Random.Range(-alterGrav, alterGrav);
		float y0 = Random.Range(-alterGrav, alterGrav);
		float z0 = Random.Range(-alterGrav, alterGrav);
		gravity = gravity - new Vector3(x0, y0, z0);  

		//transform.position = Vector3.MoveTowards(transform.position, myTarget, Time.deltaTime);

		if (startingSpawner < 2) {
			//rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.Euler(0f ,0f ,0f), Mathf.Abs(rb.velocity.x) * rotateSpeed * Time.deltaTime);
			rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.Euler(0f ,0f ,0f), rotateSpeed * 15 * Time.deltaTime);
		} else {
			//rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.Euler(0f ,0f ,0f), Mathf.Abs(rb.velocity.y) * rotateSpeed * Time.deltaTime);
			rb.rotation = Quaternion.RotateTowards(rb.rotation, Quaternion.Euler(0f ,0f ,0f), rotateSpeed * 15 * Time.deltaTime);
		}

	}

	void InitialRotation(int spawner = 1) {
		//rb.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		if (spawner == 0) {
			rb.rotation = Quaternion.LookRotation(new Vector3(90f, 0f, 0f));
		}
		else if (spawner == 1) {
			rb.rotation = Quaternion.LookRotation(new Vector3(-90f, 0f, 0f));
		}
		else if (spawner == 2) {
			rb.rotation = Quaternion.LookRotation(new Vector3(0f, 90f, 0f));
		}
		else {  // (spawner == 3) 
			rb.rotation = Quaternion.LookRotation(new Vector3(0f, -90f, 0f));
		}
	}

	Vector3 InitialVelocity(int spawner = 1) {
		Vector3 v0;
		v0 = transform.position;
		Vector3 v1 = v0;
		v1.x += Random.Range(5, 20);
		v1.y += Random.Range(1, 5);
		v1.z += Random.Range(1, 5);
		Vector3 dir = v1 - v0;
		//float range = dir.magnitude;
		dir = dir.normalized;
		Vector3 grav = dir * Random.Range(0.3f, 2.0f);
		Vector3 newVel = new Vector3(Mathf.Sqrt(2f * grav.x * (v1.x - v0.x)), Mathf.Sqrt(2f * grav.y * (v1.y - v0.y)), Mathf.Sqrt(2f * grav.z * (v1.z - v0.z)));
		newVel = new Vector3(newVel.x * 2, newVel.y * 2, newVel.z * 2);

		float offset = 2f, multiplier = 1.5f;
		float ranXY = Random.Range(-1.5f, 1.5f);
		float ranZ  = Random.Range(-0.5f, 0.5f);
		if (myType == UFOType.Pink) {
			offset = 1f;
			multiplier = 1f;
		}

		if (spawner == 0) {   		  //right
			newVel = new Vector3(-newVel.x, newVel.y, newVel.z);
			grav = new Vector3(-grav.x*multiplier - offset, ranXY, ranZ);
		} else if (spawner == 1) {    //left
			newVel = new Vector3(newVel.x, newVel.y, newVel.z);
			grav = new Vector3(grav.x*multiplier + offset, ranXY, ranZ);
		} else if (spawner == 2) {    //top
			newVel = new Vector3(newVel.y/4f, -newVel.x, newVel.z);
			grav = new Vector3(ranXY, -grav.y*multiplier - offset, ranZ);
		} else { //(spawner == 3)     //bottom
			newVel = new Vector3(newVel.y/4f, newVel.x, newVel.z);
			grav = new Vector3(ranXY, grav.y*multiplier + offset, ranZ);
		}

		gravity = grav;
		return newVel;
	}

	bool OutOfBounds() {
		if (Mathf.Abs(transform.position.x) > (maxDistance + 1) || Mathf.Abs(transform.position.y) > (maxDistance + 1)) {
			return true;
		} 
		return false;
	}

	void SetGrayTarget() {
		bNewlySpawned = false;
		Vector3 v3 = transform.position;
		float x, y, z;
		/*   //seems this section is now unused
		if (v3.x <= 0f && v3.y <= 0f) {
			x = Random.Range(v3.x, maxDistance);
			y = Random.Range(v3.y, maxDistance);
		}
		else if (v3.x > 0f && v3.y <= 0f) {
			x = Random.Range(-maxDistance, v3.x);
			y = Random.Range(v3.y, maxDistance);
		}
		else if (v3.x <= 0f && v3.y > 0f) {
			x = Random.Range(v3.x, maxDistance);
			y = Random.Range(-maxDistance, v3.y);
		}
		else {   //x>0 && y>0
			x = Random.Range(-maxDistance, v3.x);
			y = Random.Range(-maxDistance, v3.y);
		}
		z = Random.Range(1f, 37f); 
		myTarget = new Vector3(x, y, z); 
		*/
		float maxGrav = 5f;
		x = Random.Range(-maxGrav, maxGrav);
		y = Random.Range(-maxGrav, maxGrav);
		z = Random.Range(-maxGrav, maxGrav);
		if (v3.x > 0f && x > 0) { x = x/2; }
		if (v3.x < 0f && x < 0) { x = x/2; }
		if (v3.y > 0f && y > 0) { y = y/2; }
		if (v3.y < 0f && y < 0) { y = y/2; }
		myGravity = new Vector3(x, y, z);
	}

	private IEnumerator FireUpdate() {
		bool bLoop = true;
		GameObject go0, go1, go2, go3, go4;
		Vector3 v3 = new Vector3(0f, 0f, 0f);
		Vector3 vl = new Vector3(0f, 0f, 0f); 
		Transform launch = transform.Find("MLauncher").transform;
		int rnd;
		float r = 10f;   //range of random offset from ship for target
		while (bLoop) {
			if (bInCorridor) {
				if (rb.rotation == Quaternion.Euler(0f, 0f, 0f)) {
					v3 = pShip.position;
					vl = launch.position;
					go0 = Instantiate(pre_PS_Launch, vl, Quaternion.identity, parEff) as GameObject;
					Destroy(go0, go0.GetComponent<ParticleSystem>().main.duration);
					aud.PlaySoundImmediate("UfoMeteor");
					if (myType == UFOType.Pink) {
						rnd = Random.Range(0, 2);   //currently, small not used
						if (rnd == 0) { 
							go1 = Instantiate(pre_BigMeteor, vl, transform.rotation, parMet) as GameObject;
							go1.name = "BigPinkMetA" + (int)(weaponTime * 100);
						}
						else if (rnd == 1) { 
							go1 = Instantiate(pre_MediumMeteor, vl, transform.rotation, parMet) as GameObject; 
							go1.name = "MedPinkMetA" + (int)(weaponTime * 100);
						}
						else { 
							go1 = Instantiate(pre_SmallMeteor, vl, transform.rotation, parMet) as GameObject; 
							go1.name = "SmaPinkMetA" + (int)(weaponTime * 100);
						}
						//go2 = Instantiate(pre_SmallMeteor, vl, transform.rotation, parMet) as GameObject;
						//go3 = Instantiate(pre_SmallMeteor, vl, transform.rotation, parMet) as GameObject;
						//go4 = Instantiate(pre_SmallMeteor, vl, transform.rotation, parMet) as GameObject;
						go1.GetComponent<HypMeteor>().SetTarget(new Vector3 (v3.x + Random.Range(-r, r), v3.y + Random.Range(-r, r), -5f));
						//go2.GetComponent<HypMeteor>().SetTarget(new Vector3 (v3.x + Random.Range(-r, r), v3.y + Random.Range(-r, r), -5f));
						//go3.GetComponent<HypMeteor>().SetTarget(new Vector3 (v3.x + Random.Range(-r, r), v3.y + Random.Range(-r, r), -5f));
						//go4.GetComponent<HypMeteor>().SetTarget(new Vector3 (v3.x + Random.Range(-r, r), v3.y + Random.Range(-r, r), -5f));
						//go1.name = "SmaPinkMetA" + (int)(weaponTime * 100);
						//go2.name = "SmaPinkMetB" + (int)(weaponTime * 100);
						//go3.name = "SmaPinkMetC" + (int)(weaponTime * 100);
						//go4.name = "SmaPinkMetD" + (int)(weaponTime * 100);
					} else {   // UFOType.Gray
						go1 = Instantiate(pre_SmallMeteor, vl, transform.rotation, parMet) as GameObject;
						go1.GetComponent<HypMeteor>().SetTarget(new Vector3 (v3.x + Random.Range(-r, r), v3.y + Random.Range(-r, r), -5f));
						go1.name = "SmaGrayMet" + (int)(weaponTime * 100);
					}
					weaponTime += Random.Range(-0.25f, 0.25f);
				}
			}
			Debug.Log(bInCorridor + "/" + rb.rotation.eulerAngles + "/" + v3);
			if (weaponTime < 0.25f) { weaponTime = 0.25f; }
			yield return new WaitForSeconds(weaponTime);
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.name == "HypLaser") {
			Destroy(coll.gameObject);
			health -= 1;
			if (health <= 0) {
				Explode();
			} else {
				TakeHit();
			}
		}
		if (coll.gameObject.name == "Hyp_PlayerShip") {
			pShipHealth.TakeDamage();
			Explode();
		}
	}

}
