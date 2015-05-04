using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;
	private Player_Script player;

	public GameObject deathParticle;
	public GameObject respawnParticle;

	public float respawnDelay;
	private Camera_Script camera;

	public HealthManager healthManager;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
		camera = FindObjectOfType<Camera_Script> ();
		healthManager = FindObjectOfType<HealthManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RespawnPlayer(){
		StartCoroutine ("RespawnPlayerCo");
	}

	public IEnumerator RespawnPlayerCo(){
		player.letGo ();
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.enabled = false;
		player.GetComponent<Renderer> ().enabled = false;
		camera.isFollowing = false;
		Debug.Log ("Player respawn here!");
		yield return new WaitForSeconds (respawnDelay);
		player.transform.position = currentCheckpoint.transform.position;	
		player.enabled = true;
		player.GetComponent<Renderer> ().enabled = true;
		healthManager.FullHealth ();
		healthManager.isDead = false;
		camera.isFollowing = true;
		Instantiate (respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
	}
}
