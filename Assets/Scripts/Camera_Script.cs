using UnityEngine;
using System.Collections;

public class Camera_Script : MonoBehaviour {

	public Transform the_player;

	public bool isFollowing;

	public float xOffset;
	public float yOffset;

	// Use this for initialization
	void Start () {
//		the_player = FindObjectOfType<Player_Script> ();
		isFollowing = true;
	}

	void Awake (){
		//Cursor.visible = false; // turn off cursor visibility
	}
	
	// Update is called once per frame
	void Update () {
		if (isFollowing) {
			transform.position = new Vector3(the_player.transform.position.x + xOffset, the_player.transform.position.y + yOffset, transform.position.z);
		}
//		transform.position = new Vector3(the_player.position.x, the_player.position.y + 1.3f, -20); 
	}
}
