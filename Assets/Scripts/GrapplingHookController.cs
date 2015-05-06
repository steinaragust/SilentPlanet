using UnityEngine;
using System.Collections;

public class GrapplingHookController : MonoBehaviour {

	public GameObject toRotate;
	public float cameraDistance; //The distance between the camera and object
	public bool useController;

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

		if (useController) {
			// ROTATE A GUN OBJECT AROUND THE Z-AXIS
			// BASED ON THE ANGLE OF THE RIGHT ANALOG STICK
			float x = Input.GetAxis ("R_analog_horz");
			float y = Input.GetAxis ("R_analog_vert") * -1;
			float aim_angle = 0.0f;

			// USED TO CHECK OUTPUT
			//Debug.Log(" horz: " + x + "   vert: " + y); 
			
			// CALCULATE ANGLE AND ROTATE
			if (x != 0.0f || y != 0.0f) {
				
				aim_angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
				
				// ANGLE GUN
				transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
			}

		} 
		else {
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
}
