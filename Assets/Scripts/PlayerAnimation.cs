using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	// Variables
	public Animator anim; // Refrerence to the animator
	//private float fallSpeed; // The speed the character falls
	//private float verticalMovement; // The amount of vertical movement
	//private bool onGround; // Flag to check whether the character is on the ground
	private Player_Script player;
	public bool right;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player_Bird").GetComponent<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.move != 0) {
			anim.SetBool ("Running", true);


			//if player is moving left
			//Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self);
			//if(player.move < 0.0f && player.move >= -1.0f){
			//	right = false;
				//if animation is rotated right, flip, else do nothing
			//	if(!(transform.rotation.y > 179.0f)) 
					//transform.Rotate(0.0f, 180.0f, 0.0f);
			//}
			//else{
			//	if(!(transform.rotation.y < 0.3f)) 
			//		transform.Rotate(0.0f, 180.0f, 0.0f);
			//	right = true;
				//transform.Rotate(0.0f, -180.0f, 0.0f);
		}
		 else {
			anim.SetBool ("Running", false);
		}

		//transform.Rotate (0f, player.move * 180, 0f);
		//Debug.Log (transform.rotation.y);
		anim.SetBool ("Jumping", (!player.grounded && !player.isGrappled));




	}
}
