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
		playerShip = GameObject.FindGameObjectWithTag("Player").transform;

		StartCoroutine(FindDistance());
	}

	IEnumerator FindDistance() {
		float x0, y0, x1, y1, xd, yd;
		bool bChange;

		while (1 > 0) {
			bChange = false;
			x0 = transform.position.x;
			y0 = transform.position.y;
			x1 = playerShip.position.x;
			y1 = playerShip.position.y;
			xd = x0 - x1;
			yd = y0 - y1;

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
