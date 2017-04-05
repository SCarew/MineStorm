using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HypShipHealth : MonoBehaviour {

	private int shields = 3;
	public Texture[] ShieldImages;
	[SerializeField] private GameObject redFlash;
	private RawImage imgShield;

	private float timeReset = 0.20f;
	private float timeElapsed = 0f;
	private bool bTakeHit = false;
	private float invulnerableTime = 0f;

	void Start () {
		imgShield = GameObject.Find("imgShieldStrength").GetComponent<RawImage>();
		UpdateShields();
		redFlash.SetActive(false);
	}
	
	void UpdateShields () {
		imgShield.texture = ShieldImages[shields];

	}

	void Explode() {
		//TODO blow up ship
	}

	public void TakeDamage() {
		if (invulnerableTime > 0) { return; }
		shields -= 1;
		Debug.Log("Shields = " + shields);
		if (shields < 0) {
			Explode();
			return;
		}

		invulnerableTime = 0.5f;
		UpdateShields();
		bTakeHit = true;
	}

	void Update() {
		if (bTakeHit) {
			timeElapsed += Time.deltaTime;
			if (timeElapsed > timeReset) {
				bTakeHit = false;
				redFlash.SetActive(false);
				timeElapsed = 0f;
			} else {
				redFlash.SetActive(true);
			}
		}
		if (invulnerableTime > 0f) { invulnerableTime -= Time.deltaTime; }
	}
}
