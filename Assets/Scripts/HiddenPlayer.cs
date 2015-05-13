using UnityEngine;
using System.Collections;

public class HiddenPlayer : MonoBehaviour {

	public Player_Script player;
	public bool hidden;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		hidden = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.y + (player.transform.GetComponent<Renderer> ().bounds.size.y / 2) < transform.position.y + (transform.GetComponent<Renderer> ().bounds.size.y / 2) &&
			player.transform.position.x - (player.transform.GetComponent<Renderer> ().bounds.size.x / 2) > transform.position.x - (transform.GetComponent<Renderer> ().bounds.size.x / 2) &&
			player.transform.position.x + (player.transform.GetComponent<Renderer> ().bounds.size.x / 2) < transform.position.x + (transform.GetComponent<Renderer> ().bounds.size.x / 2)) {
			hidden = true;
			player.gameObject.layer = 23;
		} 
		else {
			hidden = false;
			player.gameObject.layer = 8;
		}
	}
}
