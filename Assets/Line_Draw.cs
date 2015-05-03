using UnityEngine;
using System.Collections;

public class Line_Draw : MonoBehaviour {


	public Transform sightStart;
	public Transform sightEnd;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		other ();
		//Debug.DrawLine (sightStart.position, sightEnd.position, Color.green);
	}
	void other()
	{
		Debug.DrawLine (sightStart.position, sightEnd.position, Color.green);
	}
}
