using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float jumpHeight;
	Rigidbody2D player; 

	// Use this for initialization
	void Start () {
		player = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			player.velocity = new Vector2(player.velocity.x, jumpHeight);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			player.velocity = new Vector2(moveSpeed, player.velocity.y);
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			player.velocity = new Vector2(-moveSpeed, player.velocity.y);
		}
	
	}
}
