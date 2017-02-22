using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImgScrollControl : MonoBehaviour {

	private Material mat;
	public  float parallax = 16.0f;  // (higher = slower)

	void Start () {
		mat = GetComponent<RawImage>().material;
	}
	
	void Update () {
		Vector2 off0 = mat.mainTextureOffset;
		off0.x += Time.deltaTime / parallax;
		mat.mainTextureOffset = off0;
	}
}
