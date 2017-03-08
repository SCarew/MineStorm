using System.Collections;
using UnityEngine;

public class HyperSwirl : MonoBehaviour {

	public Vector3 euler = new Vector3(0f, 0f, 1f);

	void Start () {
		
	}
	
	void Update () {
		transform.Rotate(euler);
	}
}
