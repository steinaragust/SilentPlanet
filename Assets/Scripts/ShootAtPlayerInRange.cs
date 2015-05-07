using UnityEngine;
using System.Collections;

public class ShootAtPlayerInRange : MonoBehaviour {

	public float playerRange;
	public GameObject enemyProjectile;
	public Player_Script player;
	public Transform launchPoint;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine (new Vector3 (transform.position.x - playerRange, transform.position.y, transform.position.z), new Vector3 (transform.position.x + playerRange, transform.position.y, transform.position.z));
		if (transform.localScale.x < 0 && player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange) {
			Instantiate(enemyProjectile, launchPoint.position, launchPoint.rotation);
		}
		if (transform.localScale.x > 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange) {
			Instantiate(enemyProjectile, launchPoint.position, launchPoint.rotation);
		}
	}
}
