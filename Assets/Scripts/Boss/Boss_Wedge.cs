using System.Collections;
using UnityEngine;

public class Boss_Wedge : MonoBehaviour {

	private Transform[] wedges;
	private float[] x0, y0, x1, y1;

	private float driftSpeed = 0.003f;
	private float driftRange = 0.5f;

	void Start () {
		wedges = new Transform[101];
		MeshRenderer[] wedgesMesh = gameObject.GetComponentsInChildren<MeshRenderer>();
		int j=0;
		for (int i=0; i < wedgesMesh.Length; i++) {
			if (wedgesMesh[i].tag == "Untagged") { 
				wedges[j] = wedgesMesh[i].gameObject.transform; 
				Debug.Log(i + " == " + wedges[j].name + " (" + wedgesMesh[i].name + ")");
				j++;
			}
		}
		wedgesMesh = null;
		x0 = new float[wedges.Length];
		y0 = new float[wedges.Length];
		x1 = new float[wedges.Length];
		y1 = new float[wedges.Length];
		for (int i=0; i < wedges.Length; i++) {
			x0[i] = wedges[i].localPosition.x;
			y0[i] = wedges[i].localPosition.y;

			x1[i] = 1.1f * x0[i] + Random.Range(-driftRange, driftRange);
			y1[i] = 1.1f * y0[i] + Random.Range(-driftRange, driftRange);
		}

	}
	
	void Update () {
		for (int i=0; i < wedges.Length; i++) {
			wedges[i].localPosition = Vector3.MoveTowards(wedges[i].localPosition, new Vector3(x1[i], y1[i], wedges[i].localPosition.z), driftSpeed);
			if (wedges[i].localPosition.x == x1[i] && wedges[i].localPosition.y == y1[i]) {
				float tmp1 = x1[i];
				float tmp2 = y1[i];
				x1[i] = x0[i];
				y1[i] = y0[i];
				x0[i] = tmp1;
				y0[i] = tmp2;
			}
		}
	}
}
