using UnityEngine;
using System.Collections;

public class Camera_Script : MonoBehaviour {

	public Transform the_player;

	// Use this for initialization
	void Start () {
	
	}

	void Awake (){
		//Cursor.visible = false; // turn off cursor visibility
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(the_player.position.x, the_player.position.y + 1.3f, -20); 
	}
}
