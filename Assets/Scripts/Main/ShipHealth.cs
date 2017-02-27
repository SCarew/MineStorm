using UnityEngine;
using System.Collections;

public class ShipHealth : MonoBehaviour {

	private int health = 100;
	public int maxHealth = 100;
	private ShipController sc;

	void Start() {
		health = maxHealth;
		sc = GetComponent<ShipController>();
	}

	public int GetHealth() {
		return health;
	}

	public void ResetHealth() {
		health = maxHealth;
	}

	public void DamageHealth (int hp) {
		health -= hp;
		if (health <= 0) {
			KillShip();
		}
	}

	void KillShip() {
		//Debug.Log("Ship destroyed");
		sc.BlowUpShip();
	}

}
