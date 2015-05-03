using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;
	private Player_Script player;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RespawnPlayer(){
		Debug.Log ("Player respawn here!");
		player.transform.position = currentCheckpoint.transform.position;
	}
}
