using UnityEngine;
using System.Collections;

public class Player_Script : MonoBehaviour {


	public bool grounded;
	public bool falling;
	public float velocityX;
	public float velocityY;
	public float move;

	public Transform shooter; // Transform coordinates of the Player
	public LayerMask layersDetectedByHook; // The Layer that you can grapple too
	private bool isGrappled = false;
	public Material mat; // Material of the grappling rope
	public float grapplingRopeWidth;
	
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

	// BÆTT AF ATLA
	private Stack ropeCollisionPoints; // Stack for keeping the transform coordinates of the collision points
	private ArrayList ropeCollPoes;
	
	private float oldRopeTotalLength;
	private Stack oldRopeSegmentLengths;
	
	public LayerMask collidableLayersForRope; //nonCollidableForRope = collidableLayersForRope
	
	private Transform distractionTransforms;
	private bool hasDistraction = false;
	private float distractionDelay;
	// ----------------------------------------------------------


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
		rope.SetWidth(grapplingRopeWidth, grapplingRopeWidth);         // Width of the rope
		rope.SetVertexCount(2);              // Number of rope elements
		rope.material.color = Color.black;   // Rope is black
		rope.enabled = false;                // Make rope not render by default

		ropeCollisionPoints = new Stack();
		ropeCollPoes = new ArrayList();
		
		oldRopeTotalLength = 0;
		oldRopeSegmentLengths = new Stack();
	}


	//BÆTT INN!!!!!
	void FixedUpdate() {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {

		move = Input.GetAxisRaw ("Horizontal");

		velocityX = playerRigidBody.velocity.x;
		velocityY = playerRigidBody.velocity.y;

		//checks if player is falling
		if (playerRigidBody.velocity.y < 0 && !isGrappled) {falling = true;}
		else{falling = false;}


		// -----------------------------------------
		// called if the left-mousebutton is pressed
		if ((Input.GetMouseButtonDown (0) || Input.GetButtonDown("Fire2"))&& !hasDistraction) {
			shootGrapplingHook();
		}
		
		// If you are grappling a surface and you are holding down the left mouse button
		else if(((Input.GetMouseButton(0) || Input.GetButtonDown ("Fire2")) && isGrappled) && !hasDistraction) {
			movementWhileGrappled();
		}
		
		// To let go of the grappling hook
		else if (Input.GetMouseButtonUp (0) && hasDistraction == false) {
			letGo();
		}
		
		//--This is the standard moveset--
		else {
			if(hasDistraction)
			{
				if(distractionDelay > 0 && distractionTransforms != null) {
					distractionDelay -= Time.deltaTime;
					hitObject.transform.position = distractionTransforms.position;
					hitPosition = hitObject.transform.position;
					rope.enabled = true;
					redrawRope();
				}
				else{
					hasDistraction = false;
					rope.enabled = false;
				}
			}
			
			//float move = Input.GetAxisRaw ("Horizontal");
			
			//--Main ground movement--
			if (grounded) {
				normalPlayerMovement();
			}
			
			//--movement after detaching the grapple hook--
			if(wasGrappled) {
				afterBeingGrappledMovement();
			}
			
			//--Main air movement--
			else if(((Input.GetKey (KeyCode.D)) || (Input.GetKey (KeyCode.A))) //if player pushes either directional buttons in air
			        || (!(Input.GetKey (KeyCode.D)) && !(Input.GetKey (KeyCode.A)))){ //if player lets go of both directional buttons 
					
				normalPlayerMovement();
			}
			
			//--Main jump code--
			if ((Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("Fire1")) && grounded) {
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
		RaycastHit2D hit = Physics2D.Raycast(shooter.position, mouseDirection, maxRopeLength, layersDetectedByHook);
		
		if (hit.collider != null) {
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Grapplabe")) { // true when we hit a grapplable surface
				createOriginalRope (hit);
				
				rope.enabled = true; // lets the rendered rope be visible
				redrawRope (); // Actually the first drawing :/
			}
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("TrapsForEnemys")) {
				Rigidbody2D distractionObject = hit.collider.GetComponent<Rigidbody2D>();
				distractionObject.isKinematic = false;
				distractionTransforms = hit.collider.GetComponent<Transform>();
				hasDistraction = true;
				distractionDelay = 0.5f;
			}
		}
	}
	
	public void movementWhileGrappled()
	{
		redrawRope ();
		
		Vector3 oldRopeAngle = hitPosition - transform.position;
		oldRopeAngle.Normalize ();
		Vector3 rayFromRope = new Vector3 (hitPosition.x - (oldRopeAngle.x * 0.05f), hitPosition.y - (oldRopeAngle.y * 0.05f), 0);
		
		// Casts a ray to see if the player shot at a grapplable surface
		RaycastHit2D newRopeHit = Physics2D.Linecast(transform.position, rayFromRope, collidableLayersForRope);
		
		// Checks if the rope collided with something
		if (newRopeHit.collider!= null) { // true when the rope hits a surface
			createNewRopeJoint(newRopeHit); // Creates a new rope joint at the collided surface
		}
		
		//--remove a rope joint if it is no longer needed--
		if (ropeCollisionPoints.Count > 0) {
			removeRopeJoint();
		}
		
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
			if((grapple.distance + oldRopeTotalLength) <= maxRopeLength){
				grapple.distance = grapple.distance + ropeReelSpeed;
			}
		}
		//Debug.Log(grapple.distance + oldRopeTotalLength);
	}

	
	// When you let go of the left mouse button on letgo of the grappling hook
	public void letGo()
	{
		isGrappled = false;
		rope.enabled = false; // hide the rendered rope
		grapple.enabled = false; // deactivate the spring joint
		wasGrappled = true; // bool for the movement right after you detach the grappling hook
		oldRopeTotalLength = 0;
		oldRopeSegmentLengths.Clear ();
		ropeCollisionPoints.Clear();
		ropeCollPoes.Clear ();
	}
	
	public void normalPlayerMovement()
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
	
	public void afterBeingGrappledMovement()
	{
		if ((Input.GetKey (KeyCode.D) || (Input.GetKey (KeyCode.A))) && (playerRigidBody.velocity.magnitude <= maxMoveSpeed)) {
			playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x + (move * afterBeingGrappledAirMovement), playerRigidBody.velocity.y);
		}
		wasGrappled = false;
	}
	
	public void mainJump()
	{
		playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpHeight);
		//grounded = false;
	}


	public void createOriginalRope(RaycastHit2D hit)
	{
		ropeCollisionPoints = new Stack();
		ropeCollPoes = new ArrayList();
		Vector3 normallinn = hit.normal;
		
		hitObject.transform.position = new Vector3(hit.point.x + (normallinn.x * 0.01f), hit.point.y + (normallinn.y * 0.01f), 0);
		hitPosition = hitObject.transform.position;
		
		// Calculates the length of the rope
		grapple.distance = Vector2.Distance(hitPosition, transform.position);
		grapple.distance = grapple.distance - (grapple.distance / 10); // Shortens the rope a little so its pulls you just slightly
		
		grapple.enabled = true; // activate the spring joint for grappling hanging
		isGrappled = true;
	}

	public void redrawRope()
	{
		int counter = ropeCollPoes.Count;
		rope.SetVertexCount (counter + 2);
		for(int i = 0; i < counter; i++) 
		{
			rope.SetPosition(i, (Vector3)ropeCollPoes[i]);
		}
		rope.SetPosition (counter, hitObject.transform.position);
		rope.SetPosition (counter+1, transform.position);
	}

	public void createNewRopeJoint(RaycastHit2D newRopeHit)
	{
		float oldTempDist = Vector3.Distance(hitPosition,transform.position);

		// Length of the previus egment of the rope(the one before the current collision)
		//float oldRopeSegmentLength = Vector3.Distance(newRopeHit.point, hitPosition);
		
		// Keeping tabs on the current length of the parts of the rope that are not active at this time
		//oldRopeTotalLength += oldRopeSegmentLength;
		//oldRopeSegmentLengths.Push(oldRopeSegmentLength);
		
		// Adding the old collision point to the collection of rope collision points
		ropeCollisionPoints.Push(hitObject.transform.position);
		ropeCollPoes.Add(hitObject.transform.position);
		
		// Calculactes the normal of the surface the rope collided with
		Vector3 normallinn = newRopeHit.normal;
		
		// The new collision point is always located a small distance from the normal of the collider surface
		Vector3 newJointLocation = new Vector3(newRopeHit.point.x + (normallinn.x * 0.02f), newRopeHit.point.y + (normallinn.y * 0.02f), 0);

		// Length of the previus egment of the rope(the one before the current collision)
		float oldRopeSegmentLength = Vector3.Distance(newJointLocation, hitPosition);
		// Keeping tabs on the current length of the parts of the rope that are not active at this time
		oldRopeTotalLength += oldRopeSegmentLength;
		oldRopeSegmentLengths.Push(oldRopeSegmentLength);

		hitObject.transform.position = newJointLocation;
		hitPosition = hitObject.transform.position;
		
		// The new distance of the current used rope is updated
		//grapple.distance = Vector2.Distance(hitPosition, transform.position);
		grapple.distance = oldTempDist - oldRopeSegmentLength - 0.2f;
	}

	public void removeRopeJoint()
	{
		RaycastHit2D isOldHit = Physics2D.Linecast((Vector3)ropeCollisionPoints.Peek(), transform.position, collidableLayersForRope);
		if(isOldHit.collider == null)
		{
			hitObject.transform.position = (Vector3)ropeCollisionPoints.Pop (); // Move the rope swing joint position to the old position
			hitPosition = hitObject.transform.position;
			//grapple.distance = Vector2.Distance(hitPosition, transform.position);
			grapple.distance = (grapple.distance + (float)oldRopeSegmentLengths.Peek()); // Making the active rope the correct length 
			oldRopeTotalLength -= (float)oldRopeSegmentLengths.Pop(); // 
			ropeCollPoes.RemoveAt(ropeCollPoes.Count-1);
		}
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
