using UnityEngine;
using System.Collections;

public class BSExample_LightMover : MonoBehaviour {

	public float minX;
	public float maxX;
	
	// Update is called once per frame
	void Update () 
	{
	
		float t = Mathf.PingPong(Time.timeSinceLevelLoad / 2.0f, 1.0f);

		t = Mathf.SmoothStep(0, 1, t);

		this.transform.position = new Vector3( Mathf.Lerp(minX, maxX, t), this.transform.position.y, this.transform.position.z);

	}
}
