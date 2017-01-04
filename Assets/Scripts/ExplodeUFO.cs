/* Not used anymore? */

using UnityEngine;
using System.Collections;

public class ExplodeUFO : MonoBehaviour {

	void Start() {
		ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem ps1 in ps) {
			ps1.Play();
		}	
	}

}
