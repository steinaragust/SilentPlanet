using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	// Variables
	public Animator anim; // Refrerence to the animator
	//private float fallSpeed; // The speed the character falls
	//private float verticalMovement; // The amount of vertical movement
	//private bool onGround; // Flag to check whether the character is on the ground
	private Player_Script player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player_Bird").GetComponent<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.move != 0) {
			anim.SetBool ("Running", true);
		} else {
			anim.SetBool ("Running", false);
		}

	}
}
