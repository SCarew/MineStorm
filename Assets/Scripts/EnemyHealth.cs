using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	private int health;

	void Start () {
	
	}

	public void SetHealth(int hp) {
		health = hp;
	}

	public void DamageHealth(int hp) {
		health -= hp;
	}

}
