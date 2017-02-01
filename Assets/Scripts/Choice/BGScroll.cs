using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGScroll : MonoBehaviour {

	private RawImage bg;
	private float scrollRate = 30f;

	void Start () {
		bg = GameObject.Find("BackgroundImage").GetComponent<RawImage>();
	}
	
	void Update () {
		float offset = bg.uvRect.x;
		offset += Time.deltaTime / scrollRate;
		if (offset > 1) 
			{ offset -= 1f; }
		Rect rec = bg.uvRect;
		rec.x = offset;
		bg.uvRect = rec; 
	}
}
