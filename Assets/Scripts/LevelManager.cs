using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;
	private Player_Script player;

	public GameObject deathParticle;
	public GameObject respawnParticle;

	public float respawnDelay;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<Player_Script> ();
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
		Debug.Log ("Player respawn here!");
		yield return new WaitForSeconds (respawnDelay);
		player.enabled = true;
		player.GetComponent<Renderer> ().enabled = true;
		player.transform.position = currentCheckpoint.transform.position;
		Instantiate (respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);
	}
}
