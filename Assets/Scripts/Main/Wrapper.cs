using UnityEngine;
using System.Collections;

public class Wrapper : MonoBehaviour {

	private Transform playerShip;
	private float interval = 0.5f;
	private float xDistance, yDistance;

	void Start () {
		GameManager go = GameObject.Find("GameManager").GetComponent<GameManager>();
		xDistance = go.level_width / 2;
		yDistance = go.level_height / 2;
		playerShip = GameObject.Find("PlayerShip").transform;

		StartCoroutine(FindDistance());
		/*//Testing begins
		if (go == null || playerShip == null)
			{ Debug.LogError("Null object found"); }
		//Testing ends*/
	}

	IEnumerator FindDistance() {
		float x0, y0, xd, yd;
		bool bChange;

		while (1 > 0) {
			bChange = false;
			x0 = transform.position.x;
			y0 = transform.position.y;
			xd = x0 - playerShip.position.x;
			yd = y0 - playerShip.position.y;

			if (xd > xDistance) {
				x0 -= (2 * xDistance);
				bChange = true;
			} else if (xd < -xDistance) {
				x0 += (2 * xDistance);
				bChange = true;
			}
			if (yd > yDistance) {
				y0 -= (2 * yDistance);
				bChange = true;
			} else if (yd < -yDistance) {
				y0 += (2 * yDistance);
				bChange = true;
			}
			if (bChange)
				{ transform.position = new Vector3(x0, y0, transform.position.z); }

			yield return new WaitForSeconds(interval);
		}
	}

}
