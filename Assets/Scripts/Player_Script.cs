using UnityEngine;
using System.Collections;

public class Player_Script : MonoBehaviour {
	
	public Transform shooter;
	public LayerMask grappleLayer;
	public bool isGrappled = false;
	public Material mat;
	
	public float maxRopeLength = 5;
	public float normalMoveSpeed = 6;
	
	public float maxMoveSpeed = 2;
	public bool grounded = false;
	
	public bool wasGrappled = false;
	
	public Transform startPos;
	public Transform endPos;
	public LayerMask groundLayer;
	
	public bool canJump = false;
	
	private Vector3 mousePos;
	
	public LineRenderer rope;
	public SpringJoint2D grapple;
	
	public Vector3 hitPosition;
	public GameObject hitObject;
	
	public Rigidbody2D playerRigidBody;
	
	// Use this for initialization
	void Start () {
		playerRigidBody = this.GetComponent<Rigidbody2D>();
		
		grapple = GetComponent<SpringJoint2D> ();
		grapple.enabled = false;
		rope = GetComponent<LineRenderer> ();
		hitObject = GameObject.FindGameObjectWithTag("GrappleHit");
		rope.SetWidth(0.05f, 0.05f);
		rope.SetVertexCount(2);
		rope.material.color = Color.black;
		rope.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//endVertex = Input.mousePosition;
		
		if (Input.GetMouseButtonDown (0))
		{
			Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			v.Normalize();
			RaycastHit2D hit = Physics2D.Raycast(shooter.position, v, maxRopeLength, grappleLayer.value);
			if(hit.collider != null)
			{
				Debug.Log("IT HIT!!!");
				
				hitPosition = hit.point; // vistar staðsetningunna (punktinn) þar sem við hittum yfirborðið
				hitObject.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
				
				grapple.distance = Vector2.Distance(hitPosition, transform.position);
				grapple.distance = grapple.distance - (grapple.distance / 10);
				
				grapple.enabled = true; // Activate spring joint for grappling hanging
				
				
				
				isGrappled = true;
				rope.enabled = true; // lætur reipið sem er teiknað verða synilegt
				rope.SetPosition(0, shooter.position);
				rope.SetPosition(1, hitPosition);
			}
			else
			{
				Debug.Log ("NO, it didn't");
			}
		}
		
		else if(Input.GetMouseButton(0) && (isGrappled == true))
		{
			//Debug.Log("HELD");
			//float move = Input.GetAxisRaw("Horizontal");
			
			// Swing right on rope
			if (Input.GetKey (KeyCode.D)){
				//Debug.Log("SPACE is pressed");
				playerRigidBody.AddForce(new Vector2(2.35f,0));
			}
			// Swing left on rope
			if (Input.GetKey (KeyCode.A)){
				//Debug.Log("SPACE is pressed");
				playerRigidBody.AddForce(new Vector2(-2.35f,0));
			}
			
			if(Input.GetKey(KeyCode.W))
			{
				grapple.distance = grapple.distance - 0.05f;
			}
			
			if(Input.GetKey(KeyCode.S))
			{
				if(grapple.distance <= maxRopeLength)
				{
					grapple.distance = grapple.distance + 0.05f;
				}
			}
			
			Debug.Log(playerRigidBody.velocity);
			rope.SetPosition(0, shooter.position);
			rope.SetPosition(1, hitPosition);
		}
		
		// Til að sleppa takinu a grappling hook'inum
		else if (Input.GetMouseButtonUp (0)) {
			letGo();
		}
		
		else
		{
			Debug.Log("STANDARD MOVE is RUN");
			RaycastHit2D hitInfo = Physics2D.Linecast(startPos.position, endPos.position);
			if (hitInfo.collider != null)
			{
				Debug.Log("Grounded");
				canJump = true;
				grounded = true;
			}
			else
			{
				grounded = false;
				canJump = false;
			}
			
			float move = Input.GetAxisRaw ("Horizontal");
			if (grounded == true) {
				playerRigidBody.velocity = new Vector2 (move * 3, playerRigidBody.velocity.y);
			} 
			// in air movement
			if(wasGrappled == true) {
				if ((Input.GetKey (KeyCode.D) || (Input.GetKey (KeyCode.A))) && (playerRigidBody.velocity.magnitude <= maxMoveSpeed))
				{
					Debug.Log("D is pressed");
					playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x + (move * 0.35f), playerRigidBody.velocity.y);
				}
				wasGrappled = false;
				// Swing left on rope
				/*if (Input.GetKey (KeyCode.A)&& (playerRigidBody.velocity <= maxMoveSpeed)){
					Debug.Log("A is pressed");
					//playerRigidBody.velocity = new Vector2(move*2.5f, playerRigidBody.velocity.y);
					playerRigidBody.velocity = new Vector2(playerRigidBody.velocity + move * 1.5f , playerRigidBody.velocity.y);
				}*/
			}
			
			else
			{
				if ((Input.GetKey (KeyCode.D)) || (Input.GetKey (KeyCode.A)))
				{
					Debug.Log("D is pressed");
					playerRigidBody.velocity = new Vector2(move * 3, playerRigidBody.velocity.y);
				}
			}
			
			//float move = Input.GetAxisRaw("Horizontal");
			//playerRigidBody.velocity = new Vector2(move * 2.5f, playerRigidBody.velocity.y);
			
			if (Input.GetKeyDown (KeyCode.Space) && canJump == true) // && camJump == true
			{
				
				playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 6);
				grounded = false;
				canJump = false;
			}
			
		}
	}

	public void letGo(){
		Debug.Log ("Released");
		isGrappled = false;
		rope.enabled = false;
		grapple.enabled = false;
		wasGrappled = true;
	}
}
