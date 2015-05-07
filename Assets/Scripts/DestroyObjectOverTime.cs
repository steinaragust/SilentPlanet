using UnityEngine;
using System.Collections;

public class DestroyObjectOverTime : MonoBehaviour {

//	public float lifetime;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
//		lifetime -= Time.deltaTime;
//
//		if (lifetime < 0) {
//			Destroy(gameObject);
//		}
	
	}

	void OnCollisionEnter2D(Collision2D the_Collider){
		if (the_Collider.gameObject.layer == LayerMask.NameToLayer ("Enemy")) {
			Destroy (gameObject);
		}
	}
}
