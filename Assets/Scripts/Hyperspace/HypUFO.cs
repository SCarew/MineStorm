using System.Collections;
using UnityEngine;

public class HypUFO : MonoBehaviour {

	public  enum UFOType {Pink, Gray};
	public  UFOType myType = UFOType.Gray;
	[SerializeField] private GameObject ps_Pieces;
	[SerializeField] private GameObject pre_SmallMeteor;
	private Transform parEff, parMet;
	private SoundManager aud;
	private ScoreManager sm;
	private HypShipHealth pShipHealth;
	private bool bInCorridor = false;

	private int health = 4;
	private int pinkScore = 225, grayScore = 150;
	private float weaponTime = 0.25f;

	void Start () {
		if (myType == UFOType.Gray) { 
			health = 2; 
			weaponTime = 0.5f;
		}
				
		parEff = GameObject.Find("Effects").transform;
		parMet = GameObject.Find("Meteors").transform;
		aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		pShipHealth = GameObject.Find("Hyp_PlayerShip").GetComponent<HypShipHealth>();

		StartCoroutine(FireUpdate());
	}

	void TakeHit() {
		ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
		ps.Play();
	}

	void Explode () {
		Instantiate(ps_Pieces, transform.position, Quaternion.identity, parEff);
		if (myType == UFOType.Pink) {
			aud.PlaySoundVisible("explosionUFO1", transform);
			sm.AddScore(pinkScore);
		} else {
			aud.PlaySoundVisible("explosionUFO2", transform);
			sm.AddScore(grayScore);
		}
		Destroy(gameObject);
	}

	public void NowInCorridor(bool bCorr) {
		bInCorridor = bCorr;
	}

	private IEnumerator FireUpdate() {
		bool bLoop = true;
		while (bLoop) {
			if (bInCorridor) {
				Instantiate(pre_SmallMeteor, transform.position, transform.rotation, parMet);
			}
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
