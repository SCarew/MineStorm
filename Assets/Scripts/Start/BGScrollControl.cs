//used on Start screen & Finish[Arcade] only 
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BGScrollControl : MonoBehaviour {

	public  float parallaxX = 16.0f; //(higher = slower)
	public  float parallaxY = 0.0f;
	private Material mat;

	void Start () {
		//mat = GetComponent<Image>().material;
		mat = GetComponent<MeshRenderer>().material;
	}
	
	void Update () {
		Vector2 off0 = mat.mainTextureOffset;
		if (parallaxX > 0f)
			{ off0.x += Time.deltaTime / parallaxX; }
		if (parallaxY > 0f)
			{ off0.y += Time.deltaTime / parallaxY; }
		mat.mainTextureOffset = off0;
	}
}
