using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class HypCrosshair : MonoBehaviour {

	[SerializeField] private Texture crosshairG, crosshairR;
	[SerializeField] private GameObject target;
	private float zDistance = 50f;

	//testing
	public Text txtX, txtY;

	public float GetZDistance () {
		return zDistance;
	}
	
	void Update () {
		txtX.text = target.transform.position.x.ToString();
		txtY.text = target.transform.position.y.ToString();
		Ray ray = Camera.main.ScreenPointToRay(target.transform.position);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 50f) == true) {
			target.GetComponent<RawImage>().texture = crosshairR;
			zDistance = hit.transform.position.z;
		} else {
			target.GetComponent<RawImage>().texture = crosshairG;
			zDistance = 50f;
		}
		txtY.text = zDistance.ToString();
	}
}
