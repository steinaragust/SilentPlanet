using UnityEngine;
using System.Collections;

public class GrapplingHookController : MonoBehaviour {

	public GameObject toRotate;
	public float cameraDistance; //The distance between the camera and object

	float damping; //TODO: add damping to grappling hook movement
	
	Vector3 mousePos;
	Transform target; //Assign to the object you want to rotate
	Vector3 objectPos;
	float angle;
	
	void Start(){
		target = GameObject.Find (toRotate.name).transform; 
	}
	
	// Update is called once per frame
	void Update () {
		mousePos = Input.mousePosition;
		mousePos.z = cameraDistance; 
		
		//Normalize mouseposition
		objectPos = Camera.main.WorldToScreenPoint(target.position);
		
		//fix mouse position for object
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		
		//calculate the correct angle
		angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		Vector3 euler = new Vector3 (0, 0, angle);
		
		//apply the transformation
		transform.rotation = Quaternion.Euler(euler);
		
	}
}
