using System.Collections;
using UnityEngine;

public class FinShipController : MonoBehaviour {

	private Transform parEff;   //for empty parent container
	[SerializeField] private GameObject pre_WarpExit;
	[SerializeField] private GameObject[] pre_Fireworks;
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
		MusicManager music = GameObject.Find("MusicManager").GetComponent<MusicManager>();
		anim = GetComponent<Animator>();
		transform.localScale = new Vector3(0f, 0f, 0f);
		music.PlayMusic(9);
		HyperSpaceJump();	
		StartCoroutine(PlayFireworks());	
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

	IEnumerator PlayFireworks() {
		GameObject go;
		float fDelay = 0.3f;
		float rangeX = 10f, rangeY = 6f;
		Vector3 v3;
		for (int i=0; i<15; i++) {
			v3 = new Vector3(transform.position.x + Random.Range(-rangeX, rangeX), transform.position.y + Random.Range(-rangeY, rangeY), 15f);
			go = Instantiate(pre_Fireworks[Random.Range(0, pre_Fireworks.Length)], v3, Quaternion.identity, parEff) as GameObject;
			go.transform.localScale *= Random.Range(0.75f, 1.5f);
			aud.PlaySoundVisible("fireworks", go.transform, 2);
			Destroy(go, 5.0f);
			yield return new WaitForSeconds(fDelay);
			fDelay = Random.Range(0.2f, 1f);
		}
	}

	void AnimateShip() {
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
			}
			return;
		}
		if (bWarpedIn) {
			bWarpedIn = false;
			AnimateShip();
		}	
	}
}
