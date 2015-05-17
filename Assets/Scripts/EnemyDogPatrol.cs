using UnityEngine;
using System.Collections;

public class EnemyDogPatrol : MonoBehaviour {
	private Animator animator;

	public float enemyScaling = 1;

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
	public Rigidbody2D enemyRigidbody;

	//Ground check
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	public bool grounded;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
		enemyRigidbody = this.GetComponent<Rigidbody2D> ();
		stunned = false;
		player = FindObjectOfType<Player_Script> ();
	}
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		if (grounded) {
			animator.SetBool("isJumping", false);
		}
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
		if (hittingWall && grounded ||
		    !notAtEdge && grounded ||
		    (!hittingWall && notAtEdge &&  GetComponent<Rigidbody2D>().velocity.x == 0 && grounded) ||
		    transform.localScale.x > 0 && player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange && grounded ||
		    transform.localScale.x < 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange && grounded) {
			Debug.Log("Swapping");
			moveRight = !moveRight;
		}
		if (player.transform.position.x < transform.position.x + playerRange && player.transform.position.x > transform.position.x - playerRange) {
			animator.SetBool("isWalking", false);
			moveSpeed = moveSpeedIncrease;
		} 
		else {
			animator.SetBool("isWalking", true);
			moveSpeed = normalMoveSpeed;
		}
		if (moveRight) {
			//			Debug.Log ("moving right");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			//animator.SetBool("isWalking", false);
			transform.localScale = new Vector3(-enemyScaling, enemyScaling, enemyScaling);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
			//			Debug.Log ("moving left");
			//			Debug.Log ("speed of: " + gameObject.name + ": " + GetComponent<Rigidbody2D>().velocity.x);
			transform.localScale = new Vector3(enemyScaling, enemyScaling, enemyScaling);
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
//		Debug.Log ("bounds size: " + player.transform.GetComponent<Renderer> ().bounds.size.y);
		if ((transform.position.y + (transform.GetComponent<Renderer> ().bounds.size.y / 2)) < (player.transform.position.y - (player.transform.GetComponent<Renderer> ().bounds.size.y / 2)) &&
		    (player.transform.position.x + player.transform.GetComponent<Renderer> ().bounds.size.x) > (transform.position.x - transform.GetComponent<Renderer> ().bounds.size.x) &&
		    (player.transform.position.x - player.transform.GetComponent<Renderer> ().bounds.size.x) < (transform.position.x + transform.GetComponent<Renderer> ().bounds.size.x) &&
		    grounded) {
			Debug.Log ("DOG DOES JUMP");
//			enemyRigidbody.AddForce(new Vector2(0, 0.2f));
			animator.SetBool("isJumping", true);
			enemyRigidbody.velocity = new Vector2 (enemyRigidbody.velocity.x, 6.5f);
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "TrapsForEnemys") {
			stunned = true;
			stunnedFor = howLongStunned;
		}
	}
}