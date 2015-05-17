using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	// Variables
	public Animator anim; // Refrerence to the animator
	//private float fallSpeed; // The speed the character falls
	//private float verticalMovement; // The amount of vertical movement
	//private bool onGround; // Flag to check whether the character is on the ground
	private Player_Script player;
	public bool turningRight;
	public bool isDead;
	public bool running;
	public bool wallHugging;
	public bool jumping;
	public float rotationTime;
	public bool mouseOnRightSide;
	public bool gamepadRAnalogOnRightSide;
	public bool lookAtMouseGrappled = true;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player_Bird").GetComponent<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {

		//tells if the mouse is on the right of the player
		mouseOnRightSide = (GameObject.Find ("Player_Bird").transform.position.x < Camera.main.ScreenToWorldPoint (Input.mousePosition).x);
		gamepadRAnalogOnRightSide = (player.GPAD_RANALOG_VALUE_X > 0f);

		Debug.Log (gamepadRAnalogOnRightSide? "right side" : "left side");

		
		wallHugging = (player.huggingLeftWall || player.huggingRightWall);
		jumping = (!player.grounded && !player.isGrappled);

		if (player.move != 0) {
			running = true;
			//if player is moving left
			if (player.move < 0.0f && player.move >= -1.0f) {
				turningRight =  false;
			} 
			else {
				turningRight = true;
			}
		} else {running = false;}

		isDead = player.isDead;

		if (turningRight) {
			Debug.Log ("turning right");
	
			if(player.huggingRightWall && !player.grounded){
				//make player look away from wall if he's in the air and hugging wall
				transform.rotation = Quaternion.AngleAxis (180f, Vector3.up);
				turningRight = false;
			}
			else if(player.isGrappled){

				if(lookAtMouseGrappled){
					//makes player look at direction of mouse when grappled
					if(mouseOnRightSide || gamepadRAnalogOnRightSide){ 
						transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
					} else{
						transform.rotation = Quaternion.AngleAxis (180f, Vector3.up); 
						turningRight = false;
					}
				}
			}
			else{
				//turn player normally
				//Quaternion angleaxis = Quaternion.AngleAxis(0f, Vector3.up);
				//transform.rotation = Quaternion.Slerp(transform.rotation, angleaxis, rotationTime);

				transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
			}

		} else {
			Debug.Log ("turning left");
			if(player.huggingLeftWall && !player.grounded){
				//make player look away from wall if he's in the air and hugging wall
				transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
				turningRight = true;
			} else if(player.isGrappled){

				if(lookAtMouseGrappled){
					//makes player look at direction of mouse when grappled
					if(mouseOnRightSide || gamepadRAnalogOnRightSide){ 
						transform.rotation = Quaternion.AngleAxis (0f, Vector3.up);
						turningRight = true;
					} else{
						transform.rotation = Quaternion.AngleAxis (180f, Vector3.up); 
					}
				}
			} 
			else{
				//turn player normally
				//Quaternion angleaxis2 = Quaternion.AngleAxis(180f, Vector3.up);
				//transform.rotation = Quaternion.Slerp(transform.rotation, angleaxis2, rotationTime);

				transform.rotation = Quaternion.AngleAxis (180f, Vector3.up);
			}
				
		}

		//if player is not touching ground and is not grappled, play jump animation
		anim.SetBool ("Jumping", jumping);
		anim.SetBool ("Dead", isDead);
		anim.SetBool ("Running", running);
	}
}
