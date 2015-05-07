using UnityEngine;
using System.Collections;

public class EnemyProjectileController : MonoBehaviour {

	public float speed;
	public Player_Script player;
//	public float rotationSpeed;
	public int damageToGive;
	private Rigidbody2D myRigidBody2D;
	public HealthBarSwapper healthBarSwapper;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		myRigidBody2D = GetComponent<Rigidbody2D> ();
		healthBarSwapper = FindObjectOfType<HealthBarSwapper> ();
		if (player.transform.position.x < transform.position.x) {
			speed = -speed;
//			rotationSpeed = -rotationSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () {
		myRigidBody2D.velocity = new Vector2 (speed, myRigidBody2D.velocity.y);
//		Debug.Log ("speed: " + speed);
//		myRigidBody2D.angularVelocity = rotationSpeed;
	}

	void OnTriggerEnter2D(Collider2D other){	
		if (other.name == "Player_Bird") {
//			HealthManager.HurtPlayer(damageToGive);
			healthBarSwapper.HurtPlayer(damageToGive);
		}
//		Destroy (gameObject); //HMM ??
	}

	void OnCollisionEnter2D(){
		Destroy (gameObject);
	}
}
