using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

	public float moveSpeed;
	public bool moveRight;

	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask WhatIsWall;
	private bool hittingWall;

	private bool notAtEdge;
	public Transform edgeCheck;

	public bool stunned;

	public float howLongStunned;
	public float stunnedFor;

	// Use this for initialization
	void Start () {
		stunned = false;
	}
	// Update is called once per frame

	//
	void Update () {
		if (stunnedFor > 0) {
			stunnedFor -= Time.deltaTime;
			return;
		} 
//		else {
//			stunned = false;
//		}
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, WhatIsWall);
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, wallCheckRadius, WhatIsWall);

		if (hittingWall || !notAtEdge) {
			moveRight = !moveRight;
		}

		if (moveRight) {
			transform.localScale = new Vector3(-1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
			transform.localScale = new Vector3(1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		//knockBackCount -= Time.deltaTime;
		if (other.tag == "TrapsForEnemys") {
//			stunned = true;
			stunnedFor = howLongStunned;
		}
	}
}
