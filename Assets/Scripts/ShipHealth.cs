using UnityEngine;
using System.Collections;

public class ShipHealth : MonoBehaviour {

	private int health = 100;

	public int GetHealth() {
		return health;
	}

	public void DamageHealth (int hp) {
		health -= hp;
		if (health <= 0) {
			KillShip();
		}
	}

	void KillShip() {
		Debug.Log("Ship destroyed");

	}
}
