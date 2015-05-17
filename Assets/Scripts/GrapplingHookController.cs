using UnityEngine;
using System.Collections;

public class GrapplingHookController : MonoBehaviour {
	
	//public GameObject toRotate;
	public float cameraDistance; //The distance between the camera and object
	public bool useGamepad;
	public Player_Script pScript;
	public float crosshairFadeTime;
	private Color crosshairColor;
	private Color transparent;
	private Color grappledCrosshairColor;

	[HideInInspector]  public float rAnalogX; 
	[HideInInspector]  public float rAnalogY; 
	
	//float angle;
	PlayerAnimation pAnim;
	GameObject crosshair;

	SpriteRenderer crosshairRenderer;
	SpriteRenderer gamepadCrosshairRenderer;
	
	
	void Start(){

		//target = GameObject.Find (toRotate.name).transform;
		pScript = GameObject.Find ("Player_Bird").GetComponent<Player_Script>();
		pAnim = GameObject.Find ("PlayerAnimation").GetComponent<PlayerAnimation> ();
		crosshair = GameObject.Find ("Crosshair");

		crosshairColor = crosshair.GetComponent<FollowMouse> ().crosshairColor;
		crosshairRenderer = crosshair.GetComponent<SpriteRenderer> ();
		gamepadCrosshairRenderer = GameObject.Find ("GamepadCrosshair").GetComponent<SpriteRenderer> ();
		crosshairFadeTime = 0.2f;
		transparent = crosshairColor;
		transparent.a = 0f;
		grappledCrosshairColor = crosshair.GetComponent<FollowMouse> ().grappledCrosshairColor;

		cameraDistance = 4f;

		//We start out with an invisible crosshair
		crosshairRenderer.color = transparent;
		gamepadCrosshairRenderer.color = transparent;
	}
	
	// Update is called once per frame
	void Update () {

		//player grapple rotation
		/*if (pScript.isGrappled) {
			//pScript.playerRigidBody.fixedAngle = false;
			float tiltAroundZ = Input.GetAxis ("Horizontal") * tiltAngle;
			Quaternion target2 = Quaternion.Euler (0, 0, tiltAroundZ);
			pScript.transform.rotation = Quaternion.Slerp (pScript.transform.rotation, target2, Time.deltaTime * swingingTiltSmooth);
		}
		else {
			Quaternion target = Quaternion.Euler(0, 0, 0);
			pScript.transform.rotation = Quaternion.Slerp(pScript.transform.rotation, target, Time.deltaTime * afterGrappleSmooth);
		}*/

		rAnalogX = Input.GetAxis ("R_analog_horz");
		rAnalogY = Input.GetAxis ("R_analog_vert") * -1;

		if (useGamepad) {
			// ROTATE A GUN OBJECT AROUND THE Z-AXIS
			// BASED ON THE ANGLE OF THE RIGHT ANALOG STICK

			float aim_angle = 0.0f;
			
			// CALCULATE ANGLE AND ROTATE
			if (rAnalogX != 0.0f || rAnalogY != 0.0f) {

				if(pScript.hasGrapplingHook){
					if(pScript.isGrappled){
						Color lerpedColor = gamepadCrosshairRenderer.color; 
						gamepadCrosshairRenderer.color = Color.Lerp(lerpedColor, grappledCrosshairColor, crosshairFadeTime);
					}
					else{
						Color lerpedColor = gamepadCrosshairRenderer.color; 
						gamepadCrosshairRenderer.color = Color.Lerp(lerpedColor, crosshairColor, crosshairFadeTime);
						//gamepadCrosshairRenderer.color = crosshairColor;
					}

				}
				
				aim_angle = Mathf.Atan2(rAnalogY, rAnalogX) * Mathf.Rad2Deg - 90f;
				
				// ANGLE GUN
				transform.rotation = Quaternion.AngleAxis(aim_angle, Vector3.forward);
			}
			else{
				Color lerpedColor = gamepadCrosshairRenderer.color; 
				gamepadCrosshairRenderer.color = Color.Lerp(lerpedColor, transparent, crosshairFadeTime);
				//gamepadCrosshairRenderer.color = transparent;
			}
			
		} 
		else {
			if(pScript.hasGrapplingHook){
				if(pScript.isGrappled){
					Color lerpedColor = crosshairRenderer.color; 
					crosshairRenderer.color = Color.Lerp(lerpedColor, grappledCrosshairColor, crosshairFadeTime);
				}
				else{
					Color lerpedColor = crosshairRenderer.color; 
					crosshairRenderer.color = Color.Lerp(lerpedColor, crosshairColor, crosshairFadeTime);
					//gamepadCrosshairRenderer.color = crosshairColor;
				}
			}
		}
	}
}

