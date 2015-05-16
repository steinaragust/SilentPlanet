using UnityEngine;
using System.Collections;

public class HiddenPlayer : MonoBehaviour {

	public Player_Script player;
	public bool hidden;
	private float objectHeight;
	public float objectLeft;
	public float objectRight;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		hidden = false;
		objectHeight = transform.position.y + (transform.GetComponent<Renderer> ().bounds.size.y / 2);
		objectLeft = transform.position.x - (transform.GetComponent<Renderer> ().bounds.size.x / 2);
		objectRight = transform.position.x + (transform.GetComponent<Renderer> ().bounds.size.x / 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.y + (player.transform.GetComponent<Renderer> ().bounds.size.y / 2) < objectHeight &&
			player.transform.position.x - (player.transform.GetComponent<Renderer> ().bounds.size.x / 2) > objectLeft &&
		    player.transform.position.x + (player.transform.GetComponent<Renderer> ().bounds.size.x / 2) < objectRight) {
			hidden = true;
			player.gameObject.layer = 23;
		} 
		else {
			hidden = false;
			player.gameObject.layer = 8;
		}
	}
}
