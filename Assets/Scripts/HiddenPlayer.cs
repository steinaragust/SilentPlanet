using UnityEngine;
using System.Collections;

public class HiddenPlayer : MonoBehaviour {

	public Player_Script player;
	public bool hidden;
	private float objectHeight;
	public float objectLeft;
	public float objectRight;

	public bool first;
	public bool second;
	public bool third;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		hidden = false;
		objectHeight = transform.localPosition.y + (transform.GetComponent<Renderer> ().bounds.size.y / 2);
		objectLeft = transform.localPosition.x - (transform.GetComponent<Renderer> ().bounds.size.x / 2);
		objectRight = transform.localPosition.x + (transform.GetComponent<Renderer> ().bounds.size.x / 2);
//		Debug.Log ("object Height: " + transform.localPosition.y + " + " + transform.GetComponent<Renderer> ().bounds.size.y / 2);
//		Debug.Log ("object Left: " + transform.localPosition.x + " - " + transform.GetComponent<Renderer> ().bounds.size.x / 2);
//		Debug.Log ("object Right: " + transform.localPosition.x + " - " + (transform.GetComponent<Renderer> ().bounds.size.x / 2));
	}
	
	// Update is called once per frame
	void Update () {
		first = (player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y) + (player.transform.GetComponent<Renderer> ().bounds.size.y / 2) < objectHeight;
		second = (player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) - (player.transform.GetComponent<Renderer> ().bounds.size.x / 2) > objectLeft;
		third = (player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) + (player.transform.GetComponent<Renderer> ().bounds.size.x / 2) < objectRight;
		if ((player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y) < objectHeight &&
		    (player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) - (player.transform.GetComponent<Renderer> ().bounds.size.x * 0.3) > objectLeft &&
		    (player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) + (player.transform.GetComponent<Renderer> ().bounds.size.x * 0.6) < objectRight) {
//			Debug.Log ("says true");
			hidden = true;
			player.gameObject.layer = 23;
//			Debug.Log ("says false: " + first + " " + second + " " + third + "with: " + (player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y + (player.transform.GetComponent<Renderer> ().bounds.size.y / 2)) + "<" + objectHeight + " and " + ((player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) - (player.transform.GetComponent<Renderer> ().bounds.size.x / 2)) + ">" + objectLeft + " and " + ((player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) + (player.transform.GetComponent<Renderer> ().bounds.size.x / 2)) + "<" + objectRight);
		}
		else {
//			Debug.Log ("says false: " + first + " " + second + " " + third + "with: " + (player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y + (player.transform.GetComponent<Renderer> ().bounds.size.y / 2)) + "<" + objectHeight + " and " + ((player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) - (player.transform.GetComponent<Renderer> ().bounds.size.x / 2)) + ">" + objectLeft + " and " + ((player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) + (player.transform.GetComponent<Renderer> ().bounds.size.x / 2)) + "<" + objectRight);
			hidden = false;
			player.gameObject.layer = 8;
		}
//		Debug.Log ("says false: " + first + " " + second + " " + third + "with: " + (player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y + (player.transform.GetComponent<Renderer> ().bounds.size.y / 2)) + "<" + objectHeight + " and " + ((player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) - (player.transform.GetComponent<Renderer> ().bounds.size.x / 2)) + ">" + objectLeft + " and " + ((player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) + (player.transform.GetComponent<Renderer> ().bounds.size.x / 2)) + "<" + objectRight);
//		Debug.Log ("player Height: " + (player.transform.localPosition.y + player.transform.parent.gameObject.transform.localPosition.y);
//		Debug.Log ("player Left: " + (player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) + " - " + player.transform.GetComponent<Renderer> ().bounds.size.x * 0.3);
//		Debug.Log ("player Right: " + (player.transform.localPosition.x + player.transform.parent.gameObject.transform.localPosition.x) + "+ " + player.transform.GetComponent<Renderer> ().bounds.size.x * 0.6);
	}
}
