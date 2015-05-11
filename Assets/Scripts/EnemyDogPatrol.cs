using UnityEngine;
using System.Collections;

public class EnemyDogPatrol : MonoBehaviour {
	
	public float moveSpeed;
	public bool moveRight;
	
	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask WhatIsWall;
	public bool hittingWall;
	
	public bool notAtEdge;
	public Transform edgeCheck;
	
	public bool stunned;
	
	public float howLongStunned;
	private float stunnedFor;

	public Player_Script player;
	public float playerRange;
	public float moveSpeedIncrease;
	public float normalMoveSpeed;
	
	// Use this for initialization
	void Start () {
		stunned = false;
		player = FindObjectOfType<Player_Script> ();
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
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, wallCheckRadius, WhatIsWall);
		Debug.DrawLine (new Vector3 (transform.position.x - playerRange, transform.position.y, transform.position.z), new Vector3 (transform.position.x + playerRange, transform.position.y, transform.position.z));
		
		if (hittingWall || !notAtEdge || (!hittingWall && notAtEdge &&  GetComponent<Rigidbody2D>().velocity.x == 0) || transform.localScale.x > 0 && player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange || transform.localScale.x < 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange) {
			moveRight = !moveRight;
		}
		if (player.transform.position.x < transform.position.x + playerRange && player.transform.position.x > transform.position.x - playerRange) {
			moveSpeed = moveSpeedIncrease;
		} 
		else {
			moveSpeed = normalMoveSpeed;
		}
		if (moveRight) {
			//			Debug.Log ("moving right");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(-1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
			//			Debug.Log ("moving left");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(1f, 1f, 1f);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "TrapsForEnemys") {
			stunned = true;
			stunnedFor = howLongStunned;
		}
	}
}