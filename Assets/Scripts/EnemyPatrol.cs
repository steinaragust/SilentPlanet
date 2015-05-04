using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

	public float moveSpeed;
	public bool moveRight;

	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame

	//
	void Update () {


		if (moveRight) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		} 
		else {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (-moveSpeed, GetComponent<Rigidbody2D> ().velocity.y);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Wall") {
			moveRight = !moveRight;
		}
	}
}
