using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipController : MonoBehaviour {

	private PrefsControl pc;
	private GameManager gm;
	private MeshCollider mc;
	private int conLayout;   //controller layout set somewhere else
	private int primaryWeapon, secondaryWeapon;   //set somewhere else
	private Rigidbody rb;
	private float thrustVelocity = 12f;
	private float currentThrust = 0f;
	private float maxThrust = 15f;
	//private float inertia = 0.05f;
	private Vector3 thrustDirection;
	private float rotationSpeed = 4.0f;
	private float deadZone = 0.25f;
	private Vector3 velVector = new Vector3(0, 0, 0);
	private bool bUseThrust = false;
	private Text txtVelocity;  //for testing

	private float timeScaleIn = 1f;   //time for ship to warp in
	private float timeScaleOut = 1f;  //time for ship to warp out 
	private float timeSpent = 0f;    //counter to timeScale
	private bool adjustScaleIn = false;
	private bool adjustScaleOut = false;
	[SerializeField] private GameObject pre_WarpEnter, pre_WarpExit;  //warp effects

	public GameObject pre_torpedo;
	private Transform launcher;
	private ParticleSystem[] ps;

	void Start () {
		pc = GameObject.Find("GameManager").GetComponent<PrefsControl>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		mc = GetComponentInChildren<MeshCollider>(true);
		launcher = GameObject.Find("Launcher").transform;
		rb = GetComponent<Rigidbody>();
		txtVelocity = GameObject.Find("txtVelocity").GetComponent<Text>();
		ps = GetComponentsInChildren<ParticleSystem>();

		conLayout = 0;       //for testing
		primaryWeapon = 0;   //for testing - 0=torp 1=laser 2=missiles
		secondaryWeapon = 0; //for testing - 0=hyper 1=force 2=shockwave

		thrustDirection = new Vector3(0, 0, 0);
	}
	
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		bool bP = Input.GetButtonDown("Primary");
		bool bS = Input.GetButtonDown("Secondary");
		bool bT = Input.GetButton("Thrust");

		//if (h !=0 || v != 0 || bT != false) {Debug.Log ("h=" + h + " v=" + v + " b=" + bT); }

		if (adjustScaleIn) {   //entering hyperjump
			timeSpent += Time.deltaTime;
			if (timeSpent > timeScaleIn) {
				adjustScaleIn = false;
				transform.localScale = new Vector3(0f, 0f, 0f);
				Invoke("HyperJumpOut", (timeScaleIn + timeScaleOut)/2);
			} else {
				transform.localScale = new Vector3(1f, 1f, 1f) * (1 - (timeSpent / timeScaleIn));
			}
			return;
		}

		if (adjustScaleOut) {   //exiting hyperjump
			timeSpent += Time.deltaTime;
			if (timeSpent > timeScaleOut) {
				adjustScaleOut = false;
				transform.localScale = new Vector3(1f, 1f, 1f);
				mc.enabled = true;  //turn on mesh collider again
			} else {
				transform.localScale = new Vector3(1f, 1f, 1f) * (timeSpent / timeScaleOut);
			}
			return;
		}

		if (bP) {
			if (primaryWeapon == 0)
				{ FireTorpedo(); }
		}

		if (conLayout == 0) {
			if (v > deadZone ) {
				Thrust(v);
				bUseThrust = true;
			} else {
				//Thrust(0);
				bUseThrust = false;
			}

			if (h > deadZone) {
				TurnClockwise();
			}
			if (h < -deadZone) {
				TurnCounterClockwise();
			}

			if (bS) {
				if (secondaryWeapon == 0)
					{ HyperJump(); }
			}
		}

		if (conLayout == 1) {    //this doesn't fully work yet
			if (v > deadZone || v < -deadZone || h > deadZone || h < -deadZone) { 
				Vector3 dir = new Vector3(-h, 0f, v);
				Vector3 old = transform.rotation.eulerAngles;
				Quaternion rot = Quaternion.LookRotation(dir, Vector3.forward);
				old.z = rot.eulerAngles.z;
				//transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 100f * rotationSpeed * Time.deltaTime);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(old), 100f * rotationSpeed * Time.deltaTime);
			}
		}

		if (conLayout == 2) {
			Vector3 vec = transform.rotation.eulerAngles;
			float z = vec.z;

			if (v > deadZone || v < -deadZone || h > deadZone || h < -deadZone) { 
				vec.z = TurnAct(-h, v, z); 
				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(vec), 100f * rotationSpeed * Time.deltaTime);
			}

			if (bT) {
				Thrust(0.75f);
				bUseThrust = true;
			} else {
				bUseThrust = false;
			}

			if (bS) {
				if (secondaryWeapon == 0)
					{ HyperJump(); }
			}
		}

	}

	void Thrust(float power) {
		//rb.AddForce(transform.forward * power * 10f);
		//rb.velocity = new Vector3(rb.velocity.x, thrustVelocity * power, 0f);

		/*float mass = 1.0f;
		Vector3 accel = new Vector3(power * 10f, 0, 0) / mass;
		velVector += accel * Time.deltaTime; 
		if (accel.magnitude < 0.1f) { velVector *= 0.9f; } */

//		Vector3 vecTarget = transform.position + (transform.up * 100f);
//		transform.position = Vector3.MoveTowards(transform.position, vecTarget, thrustVelocity * power * Time.deltaTime);

		//****Testing****
		//velVector += thrustVelocity * power * transform.up;
		//return;
		//****End Testing****
		rb.AddForce(transform.up * power * thrustVelocity, ForceMode.Force);
		if (rb.velocity.sqrMagnitude > (maxThrust*maxThrust)) {
			rb.AddForce(transform.up * power * -thrustVelocity, ForceMode.Force);
		}
//		foreach (ParticleSystem ps1 in ps) {
//			ps1.Play();
//		}
		return;
		//****End Testing****

//		currentThrust = Mathf.Clamp(currentThrust + (thrustVelocity * power), 0f, maxThrust);
//		thrustDirection = transform.up * 100f;
	}

	void FixedUpdate() {
		if (conLayout == 0 || conLayout == 2) {
			float mag = rb.velocity.magnitude;   //TODO: change to sqrMag or remove
			txtVelocity.text = mag.ToString();
			if (mag < 0.4f && mag != 0f) 
				{ rb.drag = 4f; }
			else 
				{ rb.drag = 0.3f; }

			//Debug.Log ("Thrust=" + bUseThrust + "  Playing=" + gameObject.GetComponentInChildren<ParticleSystem>().isPlaying);
			if (!bUseThrust) {
				foreach (ParticleSystem ps1 in ps) {
					ps1.Stop();
				}
			} else {
				foreach (ParticleSystem ps1 in ps) {
					if (ps1.isStopped)
						{ ps1.Play(); }
				}
			}

//			if (mag > 0f) {
//				foreach (ParticleSystem ps1 in ps) {
//					ps1.emission.rate = 10 + mag;
//					ps1.Play();
//				}
//			} else {
//				foreach (ParticleSystem ps1 in ps) {
//					ps1.Stop();
//				}
//			}
		}
		return;

//		if (conLayout == 0 || conLayout == 2) {
//			Vector3 dragVector = -0.1f * velVector;
//			velVector += dragVector;
//			if (velVector.sqrMagnitude < 0.01f) { velVector = Vector3.zero; }
//			txtVelocity.text = velVector.ToString();
//			transform.position = Vector3.MoveTowards(transform.position, transform.position + velVector, maxThrust * Time.fixedDeltaTime);
//		}
//		return;

//		if (conLayout == 0 || conLayout == 2) {
//			Vector3 vecTarget = transform.position + (thrustDirection);
//			transform.position = Vector3.MoveTowards(transform.position, vecTarget, currentThrust * Time.deltaTime);
//			currentThrust = Mathf.Clamp(currentThrust - inertia, 0f, currentThrust);
//			txtVelocity.text = currentThrust.ToString();
//		}
//		return;

//		if (conLayout == 0 || conLayout == 2) {
//			Vector3 dragVector = (0.1f * Mathf.Pow(velVector.magnitude, 1f)) * -velVector.normalized;
//			velVector += dragVector;
//			if (velVector.magnitude < 0.01) {velVector = Vector3.zero;}
//			txtVelocity.text = velVector.ToString();
//			transform.position = Vector3.MoveTowards(transform.position, transform.position + velVector, maxThrust * Time.deltaTime);
//		}

		//transform.position += velVector * Time.fixedDeltaTime;
	}

	void Turn(float rot) {
		transform.Rotate(new Vector3(0,0,1) * rot);

		//Vector3 v = transform.rotation.eulerAngles;
		//Debug.Log("Angle = " + v.z); 
	}

	void TurnClockwise() {
		Turn(-rotationSpeed);
	}

	void TurnCounterClockwise() {
		Turn(rotationSpeed);
	}

	void TurnDirection(Vector3 v) {
		Vector3 vm = Vector3.RotateTowards(transform.position, v, 2f * rotationSpeed * Time.deltaTime, rotationSpeed);
		Debug.Log("Turn vector: " + vm);
		transform.rotation = Quaternion.LookRotation(vm, -Vector3.right);
	}

	float TurnAct(float h, float v, float z) {
		float zAngle = 0;
		if (h > 0) {zAngle = 90;}
		if (h < 0) {zAngle = 270;}
		if (v > 0) {
			if (zAngle == 90) { zAngle = 45; }
			else if (zAngle == 270) { zAngle = 315;}
			else { zAngle = 0; }
		} 
		if (v < 0) {
			if (zAngle == 90) { zAngle = 135; }
			else if (zAngle == 270) { zAngle = 225;}
			else { zAngle = 180; }
		}
		return zAngle;
	}

	void FireTorpedo() {
		GameObject go = Instantiate(pre_torpedo, launcher.position, Quaternion.identity) as GameObject;
		go.transform.rotation = transform.rotation;
	}

	void FreezeMovement() {
		//TODO freeze velocity and rotation?
		rb.velocity = Vector3.zero;
	}

	void HyperJump() {
		if (pre_WarpEnter == null) { return; }

		mc.enabled = false;  //make invulnerable
		GameObject go = Instantiate(pre_WarpEnter, transform.position, Quaternion.identity) as GameObject;
		adjustScaleIn = true;
		timeSpent = 0f;
		FreezeMovement();
	}

	void HyperJumpOut() {
		float x = transform.position.x + (gm.level_width/2) - Random.Range(0f, gm.level_width);
		float y = transform.position.y + (gm.level_height/2) - Random.Range(0f, gm.level_height);
		transform.position = new Vector3(x, y, transform.position.z);
		Turn(Random.Range(0f, 360f));
		FreezeMovement();
		//TODO drain power from weapons or engines?
		timeSpent = 0f;
		GameObject go = Instantiate(pre_WarpExit, transform.position, Quaternion.identity) as GameObject;
		go.GetComponent<Swirl>().countdown = 1.2f;
		adjustScaleOut = true;
	}
}
