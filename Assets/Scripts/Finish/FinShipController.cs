using System.Collections;
using UnityEngine;

public class FinShipController : MonoBehaviour {

	private Transform parEff;   //for empty parent container
	[SerializeField] private GameObject pre_WarpExit;
	private float timeScaleOut = 2f;   //time for ship to warp in
	private float timeSpent = 0f;     //counter to timeScale
	private bool adjustScaleOut = false;
	private bool bWarpedIn = false;
	private bool bUseThrust = false;
	private ParticleSystem[] ps;
	private Animator anim;
	private SoundManager aud;
	private Camera cam;

	void Start () {
		parEff = GameObject.Find("Effects").transform;	
		ps = GetComponentsInChildren<ParticleSystem>();
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		anim = GetComponent<Animator>();
		transform.localScale = new Vector3(0f, 0f, 0f);
		HyperSpaceJump();		
	}

	private void HyperSpaceJump() {
		GameObject go;
		go = Instantiate(pre_WarpExit, transform.position, Quaternion.identity) as GameObject;
		go.GetComponent<Swirl>().countdown = 1.5f;
		adjustScaleOut = true;
		float size = 0.75f;
		go.transform.Find("Whirl 1").localScale = new Vector3(size, size, size);
		go.transform.Find("Whirl 2").localScale = new Vector3(size, size, size);
		go.transform.Find("Whirl 3").gameObject.SetActive(true);
		go.transform.Find("Whirl 3").localScale = new Vector3(size, size, size);

		go.transform.SetParent(parEff);
		timeSpent = 0f;

	}

	void AnimateShip() {
		Debug.Log("Animation begun");
		anim.SetTrigger("trgFlyShip");
	}

	public void Thrust() {
		bUseThrust = !bUseThrust;
		if (bUseThrust)
			{ aud.PlaySoundLimited("engine", 2); }
		else
			{ aud.PlaySoundLimited("engine", 0); }
	}

	public void KillShip() {
		Debug.Log("Killing ship animation");
		//aud.PlaySoundLimited("engine", 0);
		bUseThrust = false;
		anim.enabled = false;
		//Destroy(gameObject);
		CameraScroll();
	}

	void CameraScroll() {
		cam.GetComponent<FinCameraController>().ScrollCamera();
	}

	void FixedUpdate() {
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
	}

	void Update () {
		if (adjustScaleOut) {   //exiting hyperjump
			timeSpent += Time.deltaTime;
			if (timeSpent > timeScaleOut) {
				adjustScaleOut = false;
				transform.localScale = new Vector3(1f, 1f, 1f);
				bWarpedIn = true;
			} else {
				transform.localScale = new Vector3(1f, 1f, 1f) * (timeSpent / timeScaleOut);
				//Debug.Log("t = " + (timeSpent / timeScaleOut));
			}
			return;
		}
		if (bWarpedIn) {
			bWarpedIn = false;
			AnimateShip();
		}	
	}
}
