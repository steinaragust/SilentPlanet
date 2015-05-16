using UnityEngine;
using System.Collections;

namespace BriteSpriteExamples
{
	public class BSExample_LightRotator : MonoBehaviour 
	{
		public Vector3 rotationPoint;
		public Vector3 rotationAxis;
		public float speed;

		// Update is called once per frame
		void Update () 
		{
			this.transform.RotateAround(rotationPoint, rotationAxis, Time.deltaTime * speed);
		}
	}
}