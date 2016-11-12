﻿using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	private int health;
	private GameManager gm;
	public GameManager.mine myType = GameManager.mine.Meteor;
	public GameObject ps_Pieces;

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();	
	}

	public void SetType(GameManager.mine type) {
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

		gm.AddScore(myType, size);
		if (size == 3) {
			gm.SpawnMeteor(myType, 2, 2, gameObject.transform.position);
		} else if (size == 2) {
			gm.SpawnMeteor(myType, 1, 2, gameObject.transform.position);
		}
		ExplodeIntoPieces();
		Destroy(gameObject, 0.1f);
	}

	private void ExplodeIntoPieces() {
		ParticleSystem ps;
		GameObject go;
		go = Instantiate(ps_Pieces, gameObject.transform.position, Quaternion.identity) as GameObject;
		ps = go.GetComponent<ParticleSystem>();
		ps.Play();
		Destroy(go, ps.duration);
	}

}
