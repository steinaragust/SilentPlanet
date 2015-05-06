using UnityEngine;
using System.Collections;

public class Player_Script : MonoBehaviour {
	
	public Transform shooter; // Transform coordinates of the Player
	public LayerMask grappleLayer; // The Layer that you can grapple too
	private bool isGrappled = false;
	public Material mat; // Material of the grappling rope
	
	public float maxRopeLength = 5; // Maximum length of the rope
	public float moveSpeed = 3; // the movespeed of the player
	public float ropeSwingSpeed = 2.35f; // speed of player movement when swinging from the rope
	public float ropeReelSpeed = 0.05f; // the speed of which the rope reels in or slackens in length
	public float jumpHeight = 6;
	public float afterBeingGrappledAirMovement = 0.35f;
	
	public float maxMoveSpeed = 2;

	//BÆTT INN, AÐFERÐ 2 FYRIR GROUNDCHECK
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;
	
	private bool wasGrappled = false;
	
//	public Transform startPos; // for casting rays to see if we are grounded
//	public Transform endPos;   // -------------------""---------------------
//	public LayerMask groundLayer; // The ground layer
	
	private Vector3 mousePos;
	
	private LineRenderer rope; // The LineRender object that will draw the rope
	private SpringJoint2D grapple; // The SpringJoint that is between the player and the grappled point
	
	private Vector3 hitPosition;
	private GameObject hitObject; // The object that is grappled to
	
	private Rigidbody2D playerRigidBody;

	//BÆTT INN FYRIR KNOCKBACK/WALLJUMP !!!!
	public float knockBack;
	public float knockBackLength;
	public float knockBackCount;

	//BÆTT INN FYRIR WALLJUMP!!!
	public Transform wallCheck;
	public float wallCheckRadius;
	public LayerMask WhatIsWall;

	//private float move; // The horizontal movement input by the player's controls
	
	// Use this for initialization
	void Start () {
		playerRigidBody = this.GetComponent<Rigidbody2D>(); // Get RigidBody component of the player
		
		grapple = GetComponent<SpringJoint2D> (); // Get component of SpringJoint2D 
		grapple.enabled = false;                  // Disable the SpringJoint between the player and the hooable object
		rope = GetComponent<LineRenderer> ();     // Access the linerenderer for the rope
		hitObject = GameObject.FindGameObjectWithTag("GrappleHit");  // Get the object that points to the grapple point
		
		// The code for the rendering of the line
		rope.SetWidth(0.05f, 0.05f);         // Width of the rope
		rope.SetVertexCount(2);              // Number of rope elements
		rope.material.color = Color.black;   // Rope is black
		rope.enabled = false;                // Make rope not render by default
	}


	//BÆTT INN!!!!!
	void FixedUpdate() {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {
		// -----------------------------------------
		// called if the left-mousebutton is pressed
		if (Input.GetMouseButtonDown (0)) {
			shootGrapplingHook();
		}
		
		// If you are grappling a surface and you are holding down the left mouse button
		else if(Input.GetMouseButton(0) && (isGrappled == true)) {
			movementWhileGrappled();
		}
		
		// To let go of the grappling hook
		else if (Input.GetMouseButtonUp (0)) {
			letGo();
		}
		
		//--This is the standard moveset--
		else {
			//--casts a ray to know if we are grounded--
//			RaycastHit2D hitInfo = Physics2D.Linecast(startPos.position, endPos.position);
//			if (hitInfo.collider != null) {
//				grounded = true;
//				wasGrappled = false;
//			}
			
			float move = Input.GetAxisRaw ("Horizontal");
			
			//--Main ground movement--
			if (grounded == true) {
				normalPlayerMovement(move);
			}
			
			//--movement after detaching the grapple hook--
			if(wasGrappled == true) {
				afterBeingGrappledMovement(move);
			}
			
			//--Main air movement--
			else {
				if ((Input.GetKey (KeyCode.D)) || (Input.GetKey (KeyCode.A))) {
					normalPlayerMovement(move);
				}
			}
			
			//--Main jump code--
			if (Input.GetKeyDown (KeyCode.Space) && (grounded == true)) {
				mainJump();
			}
		}
	}
	
	
	public void shootGrapplingHook()
	{
		// Gets the direction of the mouse relative to the player(the direction that he tried to shoot the grappling hook)
		Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		mouseDirection.Normalize(); // A little Linear Algebra to get the direction vector
		
		// Casts a ray to see if the player shot at a grapplable surface
		RaycastHit2D hit = Physics2D.Raycast(shooter.position, mouseDirection, maxRopeLength, grappleLayer.value);
		
		if(hit.collider != null) // true when we hit a grapplable surface
		{
			Debug.Log("IT HIT!!!");
			hitPosition = hit.point; // Saves the hit position
			
			// Moves the grapplable object to the hit position
			hitObject.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
			
			// Calculates the length of the rope
			grapple.distance = Vector2.Distance(hitPosition, transform.position);
			grapple.distance = grapple.distance - (grapple.distance / 10); // Shortens the rope a little so its pulls you just slightly
			
			grapple.enabled = true; // activate the spring joint for grappling hanging
			isGrappled = true;
			
			// for drawing the rope
			rope.enabled = true; // lets the rendered rope be visible
			rope.SetPosition(0, shooter.position); // sets the starting point of the rendered line 
			rope.SetPosition(1, hitPosition); // sets the end point of the rendered line
		}
	}
	
	
	public void movementWhileGrappled()
	{
		//--Swing right on rope (When D key is pressed)--
		if (Input.GetKey (KeyCode.D)) {
			playerRigidBody.AddForce(new Vector2(ropeSwingSpeed, 0));
		}
		
		//--Swing left on rope (When A key is pressed)--
		if (Input.GetKey (KeyCode.A)) {
			playerRigidBody.AddForce(new Vector2(-ropeSwingSpeed, 0));
		}
		
		//--Reel in the rope--
		if(Input.GetKey(KeyCode.W)) {
			grapple.distance = grapple.distance - ropeReelSpeed;
		}
		
		//--Loosen the rope--
		if(Input.GetKey(KeyCode.S)) {
			// Make shure we don't extend the rope beyond the maximum length of the rope
			if(grapple.distance <= maxRopeLength){
				grapple.distance = grapple.distance + ropeReelSpeed;
			}
		}
		//--redraw the rope--
		rope.SetPosition(0, shooter.position);
		rope.SetPosition(1, hitPosition);
	}
	
	// When you let go of the left mouse button on letgo of the grappling hook
	public void letGo()
	{
		isGrappled = false;
		rope.enabled = false; // hide the rendered rope
		grapple.enabled = false; // deactivate the spring joint
		wasGrappled = true; // bool for the movement right after you detach the grappling hook
	}
	
	public void normalPlayerMovement(float move)
	{
		//BÆTT INN!!!!!!
		if (knockBackCount <= 0) {
			playerRigidBody.velocity = new Vector2 (move * moveSpeed, playerRigidBody.velocity.y);
		}
		//BÆTT INN!!!!
		else {
//			if(knockFromRight){
////				playerRigidBody.AddForce(new Vector2 (-knockBack, knockBack), ForceMode2D.Impulse);
//			}
//			else{
////				playerRigidBody.AddForce(new Vector2 (knockBack, knockBack), ForceMode2D.Impulse);
//			}
			knockBackCount -= Time.deltaTime;
		}
	}
	
	public void afterBeingGrappledMovement(float move)
	{
		if ((Input.GetKey (KeyCode.D) || (Input.GetKey (KeyCode.A))) && (playerRigidBody.velocity.magnitude <= maxMoveSpeed)) {
			playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x + (move * afterBeingGrappledAirMovement), playerRigidBody.velocity.y);
		}
		wasGrappled = false;
	}
	
	public void mainJump()
	{
		playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpHeight);
		grounded = false;
	}


	//BÆTT INN!!!
	public void knockPlayer(bool left){
		if (left) {
			playerRigidBody.velocity = new Vector2 (knockBack, knockBack);
		} 
		else {
			playerRigidBody.velocity = new Vector2 (-knockBack, knockBack);
		}
	}

	//BÆTT INN!!!!
	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Wall" && !grounded && Input.GetKeyDown (KeyCode.Space)) {
			knockPlayer(Physics2D.OverlapCircle (wallCheck.position, wallCheckRadius, WhatIsWall));
			knockBackCount = knockBackLength;
		}
	}
}
