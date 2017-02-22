using UnityEngine;
using System.Collections;

public class ExplodeShip : MonoBehaviour {

	public void StartExplosion () {
		ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps1 in ps) {
			ps1.Play();
		}	
	}
	
}
