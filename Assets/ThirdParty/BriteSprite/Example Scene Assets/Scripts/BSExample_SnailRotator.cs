using UnityEngine;
using System.Collections;

namespace BriteSpriteExamples
{
	public class BSExample_SnailRotator : MonoBehaviour 
	{
		public float speed;
		public float stepSize;

		private float accum = 0;

		// Update is called once per frame
		void Update () 
		{
			accum += Time.deltaTime * speed;

			this.transform.localRotation = Quaternion.Euler(0, Mathf.Floor(accum/stepSize) * stepSize, 0);
		}
	}
}
