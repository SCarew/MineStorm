/* Currently not used in anything */

using UnityEngine;
using System.Collections;

public class ShockwaveOld : MonoBehaviour {

	private float dur, elapsed;
	private Transform parEff;
	[SerializeField] private GameObject pre_Explosion;

	private int waveDamage = 40;

	void Start () {
		ParticleSystem ps = GetComponent<ParticleSystem>();
		dur = ps.startLifetime;
		elapsed = 0f;
		parEff = GameObject.Find("Effects").transform;
	}

	void Update() {
		elapsed += Time.deltaTime;
	}

	void OnParticleCollision (GameObject obj) {
		int damage = (int)(waveDamage * Mathf.Pow(1 - (elapsed/dur), 2));
		//Debug.Log ("Shockwave particle-collided with " + obj.name + " for " + damage); 
		if (damage < 10) { damage = 10; }
		obj = obj.GetComponentInChildren<Collider>().gameObject;
		if (obj.tag == "Meteor" || obj.tag == "Enemy") {
			//Vector3 expPos = coll.ClosestPointOnBounds(gameObject.transform.parent.position);
			Vector3 expPos = obj.gameObject.transform.position;
			//Debug.Log ("Shockwave triggered " + obj.name + " for " + damage);
			Vector3 v3 = (obj.transform.position - transform.position).normalized;
			obj.GetComponentInParent<Rigidbody>().AddForceAtPosition(v3 * (damage/10), obj.transform.position + v3, ForceMode.VelocityChange);
			obj.GetComponentInParent<EnemyHealth>().DamageHealth(damage);
			GameObject go = Instantiate(pre_Explosion, expPos, Quaternion.identity, parEff) as GameObject;
			ParticleSystem ps1 = go.GetComponent<ParticleSystem>();
			//ps1.startSize = 2;
			ps1.startLifetime = 2;
			ps1.startSpeed = ps1.startSpeed * 2;
			Destroy(go, 2f);
		}
	}
}