using UnityEngine;
using System.Collections;

public class Shockwave : MonoBehaviour {

	private float dur, elapsed;
	private Transform parEff;
	[SerializeField] private GameObject pre_Explosion;

	private int waveDamage = 40;
	private float upShoDam = 1.0f;

	void Start () {
		SoundManager aud = GameObject.Find("SoundManager").GetComponent<SoundManager>();
		ParticleSystem ps = GetComponent<ParticleSystem>();
		dur = ps.main.startLifetime.constant;
		elapsed = 0f;
		aud.PlaySoundImmediate("shockwave");
		parEff = GameObject.Find("Effects").transform;
	}

	void Update() {
		elapsed += Time.deltaTime;
	}

	public void SetDamageMult(float dmgMult) {
		upShoDam = dmgMult;
	}

	void OnParticleCollision (GameObject obj) {
		int damage = (int)(waveDamage * Mathf.Pow(1 - (elapsed/dur), 2) * upShoDam);
		if (damage < 10) { damage = 10; }
		Collider coll = obj.GetComponentInChildren<Collider>();
		if (coll == null) { 
			//Debug.LogWarning("Shockwave Error: " + obj.name); 
			return; 
		}
		obj = coll.gameObject;
		if (obj.tag == "Meteor" || obj.tag == "Enemy") {
			//Vector3 expPos = coll.ClosestPointOnBounds(gameObject.transform.parent.position);
			Vector3 expPos = obj.gameObject.transform.position;
			Vector3 v3 = (obj.transform.position - transform.position).normalized;
			obj.GetComponentInParent<Rigidbody>().AddForceAtPosition(v3 * (damage/10), obj.transform.position + v3, ForceMode.VelocityChange);
			obj.GetComponentInParent<EnemyHealth>().DamageHealth(damage);
			GameObject go = Instantiate(pre_Explosion, expPos, Quaternion.identity, parEff) as GameObject;
			ParticleSystem ps1 = go.GetComponent<ParticleSystem>();
			//ps1.startSize = 2;
			var ps01 = ps1.main.startLifetime.constant;
			ps01 = 2f;
			var ps02 = ps1.main.startSpeed.constant; 
			ps02 = ps1.main.startSpeed.constant * 2f;
			Destroy(go, 2f);
		}
	}
}