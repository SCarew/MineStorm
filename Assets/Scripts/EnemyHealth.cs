using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	private int health;
	private GameManager gm;
	private int myType = 1;

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();	
	}

	public void SetType(int type) {
		myType = type;
	}

	public void SetHealth(int hp) {
		health = hp;
	}

	public void DamageHealth(int dmg) {
		health -= dmg;
		if (health <= 0) {
			KillMeteor();
		}
	}

	private void KillMeteor() {
		int size;
		//TODO this if statement must be removed
		if (GetComponentInChildren<MeteorControl2>() == null) {
			size = GetComponentInChildren<MeteorControl>().GetSize();
		}
		else {
			size = GetComponentInChildren<MeteorControl2>().GetSize();
		}

		if (size == 3) {
			gm.SpawnMeteor(myType, 2, 2, gameObject.transform.position);
		} else if (size == 2) {
			gm.SpawnMeteor(myType, 1, 2, gameObject.transform.position);
		}
		Destroy(gameObject, 0.1f);
	}

}
