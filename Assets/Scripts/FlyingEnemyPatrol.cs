using UnityEngine;
using System.Collections;

public class FlyingEnemyPatrol : MonoBehaviour {

	public float moveSpeed;
	public bool moveRight;

	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask WhatIsWall;
	public bool hittingWall;
	
	public bool stunned;

	public float howLongStunned;
	private float stunnedFor;

	public float turnAroundTime;
	public float countdown;

	public float howMuchScale;


	// Use this for initialization
	void Start () {
		stunned = false;
		countdown = turnAroundTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (stunnedFor > 0) {
			stunnedFor -= Time.deltaTime;
			return;
		} 
		else {
			stunned = false;
		}
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, WhatIsWall);
		if (countdown <= 0 || hittingWall) {
			moveRight = !moveRight;
			countdown = turnAroundTime;
		}
		if (moveRight) {
			//			Debug.Log ("moving right");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(-howMuchScale, howMuchScale, howMuchScale);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
			//			Debug.Log ("moving left");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(howMuchScale, howMuchScale, howMuchScale);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
		countdown -= Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "TrapsForEnemys") {
			stunned = true;
			stunnedFor = howLongStunned;
		}
	}
}
