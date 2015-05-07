﻿using UnityEngine;
using System.Collections;

public class GrapplingHookPickup : MonoBehaviour { 

	public GameObject pickedUpBy;

	void Start(){
		pickedUpBy = GameObject.FindGameObjectWithTag ("Player");
	}
	

	//when the player collides with the grappling hook pickup, its assigned to the player and is destroyed
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == pickedUpBy.tag){
			pickedUpBy.GetComponent<Player_Script>().hasGrapplingHook = true;
			GameObject hookPickup = GameObject.FindGameObjectWithTag("GrapplingHookPickup");
			Object.Destroy(hookPickup);
		}
	}
}
