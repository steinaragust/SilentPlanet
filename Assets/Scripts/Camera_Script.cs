﻿using UnityEngine;
using System.Collections;

public class Camera_Script : MonoBehaviour {

	public Transform the_player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(the_player.position.x, the_player.position.y + 0.35f, -20); 
	}
}