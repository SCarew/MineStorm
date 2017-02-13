//used on Start screen only 
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BGScrollControl : MonoBehaviour {

	public  float parallax = 16.0f; //(higher = slower)
	private Material mat;

	void Start () {
		//mat = GetComponent<Image>().material;
		mat = GetComponent<MeshRenderer>().material;
	}
	
	void Update () {
		Vector2 off0 = mat.mainTextureOffset;
		off0.x += Time.deltaTime / parallax;
		mat.mainTextureOffset = off0;
	}
}
