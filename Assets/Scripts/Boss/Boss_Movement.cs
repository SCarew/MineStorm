using System.Collections;
using UnityEngine;

public class Boss_Movement : MonoBehaviour {

	private GameManager gm;
	private Rigidbody rb;
	private enum BossType {Wedge, Spider, Mothership, Other};
	[SerializeField] private BossType myType = BossType.Wedge;
	[SerializeField] private float moveSpeed = 500f;
	private float changeDirection = 0f;
	private Vector3 targetVector;
	private bool bSpeedUp = false;
	private float slowdownTime = 2.0f;

	void Start () {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = GetComponent<Rigidbody>();

		if (myType == BossType.Wedge) {
			//moveSpeed = 1f;
			targetVector = SetTarget();

		}

		//rb.AddForce((targetVector - transform.position) * moveSpeed, ForceMode.Force);

	}

	Vector3 SetTarget() {
		Vector3 wayPoint = Vector3.zero;

		if (myType == BossType.Wedge) {
			float h = gm.level_height;
			float w = gm.level_width;
			wayPoint = new Vector3(Random.Range(-w/2, w/2), Random.Range(-h/2, h/2), 0f);
		}
		//rb.velocity = Vector3.zero;
		changeDirection = slowdownTime;
		bSpeedUp = false;
		return wayPoint;

	}

	void Update () {
		if (transform.position.z != 0f) {
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, 0f), 0.1f);
		}
		if (((int)(transform.position.x*10) == (int)(targetVector.x*10)) || ((int)(transform.position.y*10) == (int)(targetVector.y*10))) {
			targetVector = SetTarget();
			//rb.AddForce((targetVector - transform.position) * moveSpeed, ForceMode.Force);
			//Debug.Log("Vel:" + rb.velocity);
		}
		if (changeDirection > 0f) {
			changeDirection -= Time.deltaTime;
			if (changeDirection < 0f) { changeDirection = 0f; }
			if (!bSpeedUp) {
				rb.velocity = rb.velocity * (changeDirection/slowdownTime); //decrease velocity;
			} else {
				rb.velocity = Vector3.zero;
				rb.AddForce(targetVector * moveSpeed, ForceMode.Force);
				rb.velocity = rb.velocity * ((slowdownTime - changeDirection)/slowdownTime); //increase velocity
			}
			if (changeDirection == 0f && !bSpeedUp) {
				bSpeedUp = true;
				changeDirection = slowdownTime;
			}
		}
		//transform.position = Vector3.MoveTowards(transform.position, targetVector, moveSpeed * Time.deltaTime);

		//Debug.Log("Pos:" + transform.position + "  Tar:" + targetVector);
	}
}
