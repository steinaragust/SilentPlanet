using UnityEngine;
using System.Collections;

public class HurtPlayerOnContact : MonoBehaviour {

	public int damageToGive;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Player_Bird" && HealthManager.playerHealth > 0) {
			HealthManager.HurtPlayer(damageToGive);
			other.GetComponent<AudioSource>().Play();
			var player = other.GetComponent<Player_Script>();
			player.knockBackCount = player.knockBackLength;
			if(other.transform.position.x < transform.position.x){
				player.knockPlayer(false);
			}
			else{
				player.knockPlayer(true);
			}
		}
	}
}
