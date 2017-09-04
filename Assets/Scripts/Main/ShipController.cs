using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipController : MonoBehaviour {

	private PrefsControl prefs;
	private GameManager gm;
	private SoundManager aud;
	private MeshCollider mc;
	private MeshRenderer[] mr;
	private int conLayout;   //controller layout set somewhere else
	private int primaryWeapon, secondaryWeapon;   //set somewhere else
	private Rigidbody rb;
	//private float currentThrust = 0f;
	//private float inertia = 0.05f;
	//private Vector3 thrustDirection;
	private float deadZone = 0.25f;
	private int dev = 0;
	//private Vector3 velVector = new Vector3(0, 0, 0);
	private bool bUseThrust = false;
	private Text txtVelocity;  //for testing
	private bool bEscape = false;    //true when ship destroyed & escape pod launched
	private bool bGameOver = false;  //true when in escape pod and no ships left
	private bool bPaused = false;    //true when game is paused
	private int currentLvl = -1;	 //for determining end of level

	private float timeScaleIn = 1f;   //time for ship to warp in
	private float timeScaleOut = 1.5f;  //time for ship to warp out 
	private float timeSpent = 0f;    //counter to timeScale
	private bool adjustScaleIn = false;
	private bool adjustScaleOut = false;
	[SerializeField] private GameObject pre_WarpEnter, pre_WarpExit;  //warp effects
	[SerializeField] private GameObject pre_Forcefield;
	[SerializeField] private GameObject pre_Shockwave;
	[SerializeField] private GameObject pre_ShipExplosion;

	[SerializeField] private GameObject pre_torpedo, pre_laser, pre_missile;
	private Transform launcher;
	private Transform parEff;   //for empty parent container
	private ParticleSystem[] ps;

	//======= Ship Stats =======
	private float thrustVelocity = 12f;
	private float maxThrust = 15f;
	private float rotationSpeed = 4.0f;
	private int   missileNumber = 8;
	public  float priRechargeRate = 2f;   //for weapons; set in Start()
	public  float priCurrentCharge = 0f;  
	private float chargeTorp = 1.75f, chargeLaser = 0.25f, chargeMissile = 1.5f;
	public  float secRechargeRate = 4f;   //for secondary device
	public  float secCurrentCharge = 0f;
	private float chargeHyperjump = 6f, chargeForcefield = 8f, chargeShockwave = 60f;
	public  float engRechargeRate = 2.5f;
	public  float engCurrentCharge = 0f;
	public  float lifeRechargeRate = 6f;   //for escape pod life support
	public  float lifeCurrentCharge = 0f;
	//==========================

	//======== Upgrades ========
	public  float upTorpDam = 1f; 
	public  float upTorpVel = 1f;   //torpedo velocity
	public  float upLasDam  = 1f;
	public  float upLasRan  = 1f;   //laser range
	public  float upMissDam = 1f;
	public  float upMissVel = 1f;   //missile velocity
	public  float upHypCon  = 1f;   //hyperwarp power consumption
	public  float upForCon  = 1f;   //forcefield power consumption
	public  float upForDur  = 1f;   //forcefield duration
	public  float upShoDam  = 1f;   //shockwave damage +
	public  float upHullFor = 1f;   //hull fortification
	//==========================

	void Start () {
		prefs = GameObject.Find("LevelManager").GetComponent<PrefsControl>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		mc = GetComponentInChildren<MeshCollider>(true);
		launcher = GameObject.Find("Launcher").transform;
		rb = GetComponent<Rigidbody>();
		parEff = GameObject.Find("Effects").transform;
		txtVelocity = GameObject.Find("txtVelocity").GetComponent<Text>();
		ps = GetComponentsInChildren<ParticleSystem>();
		mr = GetComponentsInChildren<MeshRenderer>(true);

		if (prefs.GetGameType() == "Arcade") {
			primaryWeapon = prefs.GetPrimaryWeapon(true);
			secondaryWeapon = prefs.GetSecondaryWeapon(true);
		} else {
			primaryWeapon = prefs.GetPrimaryWeapon();
			secondaryWeapon = prefs.GetSecondaryWeapon();
		}
		Debug.Log("Primary=" + primaryWeapon + "  Secondary=" + secondaryWeapon + "  GameType=" + prefs.GetGameType());

		conLayout = prefs.GetControlLayout();
		//conLayout = 0;       //for testing
		//primaryWeapon = 0;   //for testing - 0=torp 1=laser 2=missiles
		//secondaryWeapon = 2; //for testing - 0=hyper 1=force 2=shockwave

		if (primaryWeapon == 0)         //torp
			{ priRechargeRate = chargeTorp; }
		else if (primaryWeapon == 1)    //laser
			{ priRechargeRate = chargeLaser; }
		else if (primaryWeapon == 2)    //missiles
			{ priRechargeRate = chargeMissile; }
		priCurrentCharge = priRechargeRate;
		if (secondaryWeapon == 0)         //hyperjump
			{ secRechargeRate = chargeHyperjump; }
		else if (secondaryWeapon == 1)    //forcefield
			{ secRechargeRate = chargeForcefield; }
		else if (secondaryWeapon == 2)    //shockwave
			{ secRechargeRate = chargeShockwave; }
		secCurrentCharge = secRechargeRate;
		engCurrentCharge = engRechargeRate;
		lifeCurrentCharge = lifeRechargeRate;

		currentLvl = gm.currentLevel;
		CheckUpgrades();
		HyperSpaceJump();   //should only happen at start of scene
	}
	
	void Update () {
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		bool bS = Input.GetButtonDown("Secondary");
		bool bT = Input.GetButton("Thrust");
		bool bP;
		if (primaryWeapon == 1)   //lasers
			{ bP = Input.GetButton("Primary"); }
		else     // torps or missiles
			{ bP = Input.GetButtonDown("Primary"); }

		if (bPaused) { return; }
		//if (h !=0 || v != 0 || bT != false) {Debug.Log ("h=" + h + " v=" + v + " b=" + bT); }

		priCurrentCharge += Time.deltaTime;
		if (priCurrentCharge > priRechargeRate)  { priCurrentCharge = priRechargeRate; }
		secCurrentCharge += Time.deltaTime;
		if (secCurrentCharge > secRechargeRate)  { secCurrentCharge = secRechargeRate; }
		engCurrentCharge += Time.deltaTime;
		if (engCurrentCharge > engRechargeRate)  { engCurrentCharge = engRechargeRate; }
		if (bEscape) { lifeCurrentCharge -= Time.deltaTime; }

		// **** Testing Start ****    //TODO remove this section
		if (prefs.GetDevMode()) {
			if (Input.GetKeyDown(KeyCode.X)) {   
				if (bEscape == false) { BlowUpShip(); }
				else { bEscape = false; lifeCurrentCharge = lifeRechargeRate; }
			}
			if (Input.GetKeyDown(KeyCode.Alpha1)) { 
				primaryWeapon = 0; priRechargeRate = chargeTorp;
				Debug.Log("* Primary weapon: Torpedo");
			} 
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				primaryWeapon = 1; priRechargeRate = chargeLaser;
				Debug.Log("* Primary weapon: Laser");
			} 
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				primaryWeapon = 2; priRechargeRate = chargeMissile;
				Debug.Log("* Primary weapon: Missile");
			} 
			if (Input.GetKeyDown(KeyCode.Alpha4)) { 
				secondaryWeapon = 0; secRechargeRate = chargeHyperjump; secCurrentCharge = secRechargeRate;
				Debug.Log("* Secondary weapon: Hyperjump");
			} 
			if (Input.GetKeyDown(KeyCode.Alpha5)) { 
				secondaryWeapon = 1; secRechargeRate = chargeForcefield; secCurrentCharge = secRechargeRate;
				Debug.Log("* Secondary weapon: Forcefield");
			} 
			if (Input.GetKeyDown(KeyCode.Alpha6)) { 
				secondaryWeapon = 2; secRechargeRate = chargeShockwave; secCurrentCharge = secRechargeRate;
				Debug.Log("* Secondary weapon: Shockwave");
			} 
		} else {
			if (Input.GetKey("left ctrl") || Input.GetKey("right ctrl")) {
				if (Input.GetKeyDown("d"))
					{ dev = 1; }
				if (Input.GetKeyDown("e") && dev == 1)
					{ dev = 2; }
				if (Input.GetKeyDown("v") && dev == 2)
					{ prefs.SetDevMode(true); dev = 3; }
			}
		}
		// **** Testing End ****

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

		if (bEscape) {   //escape pod in use
			ConstantThrust();
			if (lifeCurrentCharge <= 0f) {
				//Debug.Log("Game Over");
				if (gm.shipsRemaining > 0) {
					bS = true;   //make sense to summon ship automatically when time expired?
				} else {
					//Debug.Log("Game Over");
					if (!bGameOver) {
						gm.ShowGameOver();
					}
					bGameOver = true;
					// TODO complete this section
					return;
				}
			}
			if (bS && (gm.shipsRemaining > 0)) {   //summon another ship
				transform.localScale = new Vector3(0f, 0f, 0f);
				mr[1].enabled = true;
				FreezeMovement();
				//TODO drain power from weapons or engines?
				timeSpent = 0f;
				GameObject go = Instantiate(pre_WarpExit, transform.position, Quaternion.identity) as GameObject;
				go.GetComponent<Swirl>().countdown = 1.2f;
				adjustScaleOut = true;
				bEscape = false;
				lifeCurrentCharge = lifeRechargeRate;
				gm.LoseShip();
				GetComponent<ShipHealth>().ResetHealth();
			}
			bP = false;   //can't use these while in escape pod
			bT = false;
			bS = false;
			v = 0f;       //can only move H
		}

		if (bP && (priCurrentCharge >= priRechargeRate)) {
			priCurrentCharge = 0f;
			if (primaryWeapon == 0)
				{ FirePrimaryWeapon(pre_torpedo, "Torpedo"); }
			else if (primaryWeapon == 1)
				{ FirePrimaryWeapon(pre_laser, "Laser"); }
			else if (primaryWeapon == 2)
				{ FireMissiles(); }
		}

		if (conLayout == 0) {
			if ((v > deadZone) && (engCurrentCharge > 0)) {
				engCurrentCharge -= Time.deltaTime * 1.5f;
				Thrust(v);
				aud.PlaySoundLimited("engine", 2); 
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

			if (bS && (secCurrentCharge >= secRechargeRate)) {
				if (secondaryWeapon == 0)
					{ HyperJump(); }
				else if (secondaryWeapon == 1)
					{ RaiseForcefield(); }
				else if (secondaryWeapon == 2)
					{ LaunchShockwave(); }
			}
		}

		if (conLayout == 1) {    //TODO this doesn't fully work yet
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

			if (bT && (engCurrentCharge > 0)) {
				engCurrentCharge -= Time.deltaTime * 1.5f;
				aud.PlaySoundLimited("engine", 2); 
				Thrust(0.75f);
				bUseThrust = true;
			} else {
				bUseThrust = false;
			}

			if (bS && (secCurrentCharge >= secRechargeRate)) {
				if (secondaryWeapon == 0)
					{ HyperJump(); }
				else if (secondaryWeapon == 1) 
					{ RaiseForcefield(); }
				else if (secondaryWeapon == 2)
					{ LaunchShockwave(); }
			}
		}

		if (currentLvl != gm.currentLevel) {
			currentLvl = gm.currentLevel;
			HyperSpaceJump(true);			//for end of level jump
		}
	}

	void Thrust(float power) {
		if (rb.velocity.sqrMagnitude <= (maxThrust*maxThrust)) {
			rb.AddForce(transform.up * power * thrustVelocity * (rb.mass/2), ForceMode.Force);
		}
		return;

//		Vector3 vecTarget = transform.position + (transform.up * 100f);
//		transform.position = Vector3.MoveTowards(transform.position, vecTarget, thrustVelocity * power * Time.deltaTime);

		//****Testing****
		//velVector += thrustVelocity * power * transform.up;
		//return;
		//****End Testing****
		//rb.AddForce(transform.up * power * thrustVelocity, ForceMode.Force);
		//if (rb.velocity.sqrMagnitude > (maxThrust*maxThrust)) {
		//	rb.AddForce(transform.up * power * -thrustVelocity, ForceMode.Force);
		//}
//		foreach (ParticleSystem ps1 in ps) {
//			ps1.Play();
//		}

//		currentThrust = Mathf.Clamp(currentThrust + (thrustVelocity * power), 0f, maxThrust);
//		thrustDirection = transform.up * 100f;
	}

	public void BlowUpShip() {
		GameObject go = Instantiate(pre_ShipExplosion, transform.position, transform.rotation, parEff) as GameObject;
		go.GetComponentInChildren<ExplodeShip>().StartExplosion();
		mr[1].enabled = bEscape;
		mc.enabled = bEscape;
		Invoke("LaunchEscapePod", 0.3f);

		aud.PlaySoundImmediate("explosionShip");
		//gm.LoseShip();
	}

	void LaunchEscapePod() {
		bEscape = true;
	}

	public bool isEscaping() {
		return bEscape;
	}

	void FixedUpdate() {
		if (conLayout == 0 || conLayout == 2) {
			float mag = rb.velocity.sqrMagnitude;   //TODO: change to sqrMag or remove
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
		if (bEscape) 
			{ rot = rot * 0.5f; }    //slow turning for escape pod
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

	public void FreezeRotation(bool isPaused) {   //for game pause
		//transform.Rotate(new Vector3(0,0,1) );
		//transform.rotation = transform.up;
		//transform.Rotate(0,0,-transform.rotation.eulerAngles.z);
		bPaused = isPaused;
		Debug.Log ("Freezing ship at " + transform.rotation.eulerAngles);
	}

	void FireMissiles() {
		aud.PlaySoundImmediate("Missile");
		for (int i=0; i<missileNumber; i++) {
			GameObject go = Instantiate(pre_missile, launcher.position, Quaternion.identity) as GameObject;
			go.transform.SetParent(parEff);
			go.transform.rotation = transform.rotation;
			go.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up + Vector3.up * Random.Range(-0.2f, 0.2f));
			go.name = "Missile";
			TorpedoController tc = go.GetComponent<TorpedoController>();
			tc.damage = (int) (tc.damage * upMissDam);
			tc.fireSpeed *= upMissVel;
		}
	}

	void FirePrimaryWeapon(GameObject pre_primary, string primaryName) {
		GameObject go = Instantiate(pre_primary, launcher.position, Quaternion.identity) as GameObject;
		go.transform.SetParent(parEff);
		go.transform.rotation = transform.rotation;
		go.name = primaryName;
		TorpedoController tc = go.GetComponent<TorpedoController>();
		if (primaryName == "Laser") {
			tc.lifetime *= upLasRan;
			tc.damage = (int) (tc.damage * upLasDam);
		} else if (primaryName == "Torpedo") {
			tc.damage = (int) (tc.damage * upTorpDam);
			tc.fireSpeed *= upTorpVel;
		}
	}

	void LaunchShockwave() {
		secCurrentCharge = 0f;
		engCurrentCharge = priCurrentCharge = -2f;

		GameObject go = Instantiate(pre_Shockwave, transform.position, transform.rotation) as GameObject;
		go.transform.SetParent(parEff);
		go.name = "Shockwave";
		go.GetComponent<Shockwave>().SetDamageMult(upShoDam);
		Destroy(go, go.GetComponent<ParticleSystem>().main.duration);
	}

	void RaiseForcefield() {
		priCurrentCharge = -2.5f;   //also done in ForceField.cs when shield ends
		//priCurrentCharge = priRechargeRate - ((priRechargeRate + 2.5f) * upForCon);
					//also done in ForceField.cs when shield ends

		GameObject go = Instantiate(pre_Forcefield, transform.position, Quaternion.identity) as GameObject;
		go.transform.SetParent(parEff);
		go.transform.rotation = transform.rotation;
		go.name = "Forcefield";
	}

	void ConstantThrust() {   //while in escape pod
		float speed = 7f;
		Vector3 velVector = speed * transform.up;
		rb.velocity = speed * velVector.normalized;
	}

	void FreezeMovement() {
		//TODO freeze velocity and rotation?
		rb.velocity = Vector3.zero;
	}

	void HyperJump() {
		if (pre_WarpEnter == null) { return; }
		secCurrentCharge = 0f;
		engCurrentCharge = -3f * upHypCon;

		mc.enabled = false;  //make invulnerable
		GameObject go = Instantiate(pre_WarpEnter, transform.position, Quaternion.identity) as GameObject;
		go.transform.SetParent(parEff);
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
		go.GetComponent<Swirl>().countdown = 1.4f;
		adjustScaleOut = true;
	}

	/// <summary>
	/// Start large hyperspace whirlpool (level start/end).
	/// </summary>
	/// <param name="jumpOut">If set to <c>true</c> jump out.</param>
	public void HyperSpaceJump(bool jumpOut = false) {
		GameObject go;
		if (!jumpOut) {
			go = Instantiate(pre_WarpExit, transform.position, Quaternion.identity) as GameObject;
			go.GetComponent<Swirl>().countdown = 1.5f;
			adjustScaleOut = true;
		} else {
			go = Instantiate(pre_WarpEnter, transform.position, Quaternion.identity) as GameObject;
			adjustScaleIn = true;
		}
		go.transform.Find("Whirl 1").localScale = new Vector3(1.5f, 1.5f, 1.5f);
		go.transform.Find("Whirl 2").localScale = new Vector3(1.5f, 1.5f, 1.5f);
		//go.transform.FindChild("Whirl 3").localScale = new Vector3(3f, 3f, 1f);
		FreezeMovement();
		go.transform.SetParent(parEff);
		timeSpent = 0f;

	}

	void CheckUpgrades() {    //compare with CreateListUpgrade() in PrefsControl

		string s = prefs.GetUpgrades();
		//*********Testing - Remove**********
		for (int i=0; i<(s.Length/3); i++) {
			Debug.Log(s.Substring(i*3, 3) + " => " + prefs.UpgradeText(s.Substring(i*3, 3), true));
		}
		//***********************************

		if (s.Contains("***")) {   //additional ship
			s = s.Replace("***", "");
			prefs.ReplaceUpgrade(s);
			gm.shipsRemaining += 1;
		}
		if (s.Contains("T+1")) { upTorpDam = 1.25f; }
		if (s.Contains("T+2")) { upTorpDam = 1.5f; }
		if (s.Contains("T+A")) { chargeTorp *= 0.75f; priRechargeRate = chargeTorp; }
		if (s.Contains("T+B")) { chargeTorp *= 0.80f; priRechargeRate = chargeTorp; }
		if (s.Contains("T+S")) { upTorpVel = 1.2f; }

		if (s.Contains("L+1")) { upLasDam = 1.36f; }
		if (s.Contains("L+2")) { upLasDam = 1.68f; }
		if (s.Contains("L+3")) { upLasDam = 2.0f; }
		if (s.Contains("L+A")) { chargeLaser *= 0.80f; priRechargeRate = chargeLaser; }
		if (s.Contains("L+B")) { chargeLaser *= 0.80f; priRechargeRate = chargeLaser; }
		if (s.Contains("L+R")) { upLasRan = 1.2f; }

		if (s.Contains("M+1")) { upMissDam = 1.25f; }
		if (s.Contains("M+2")) { upMissDam = 1.56f; }
		if (s.Contains("M+A")) { missileNumber += 2; }
		if (s.Contains("M+B")) { missileNumber += 2; }
		if (s.Contains("M+S")) { upMissVel = 1.25f; }
		if (s.Contains("M+G")) { chargeMissile *= 0.75f; priRechargeRate = chargeMissile; }

		if (s.Contains("H+1")) { upHypCon = 0.70f; }
		if (s.Contains("H+2")) { upHypCon = 0.40f; }
		if (s.Contains("H+A")) { chargeHyperjump *= 0.80f; secRechargeRate = chargeHyperjump; }
		if (s.Contains("H+B")) { chargeHyperjump *= 0.75f; secRechargeRate = chargeHyperjump; }

		if (s.Contains("F+1")) { upForCon = 0.70f; }
		if (s.Contains("F+2")) { upForCon = 0.40f; }
		if (s.Contains("F+A")) { upForDur = 1.25f; }
		if (s.Contains("F+B")) { upForDur = 1.50f; }

		if (s.Contains("S+1")) { upShoDam = 1.25f; }
		if (s.Contains("S+2")) { upShoDam = 1.50f; }
		if (s.Contains("S+A")) { chargeShockwave *= 0.80f; secRechargeRate = chargeShockwave; }
		if (s.Contains("S+B")) { chargeShockwave *= 0.75f; secRechargeRate = chargeShockwave; }

		if (s.Contains("U+1")) { upHullFor = 1.20f; }
		if (s.Contains("U+2")) { upHullFor = 1.50f; }
		if (s.Contains("U+3")) { upHullFor = 1.80f; }

	}

}
