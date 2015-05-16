using UnityEngine;
using System.Collections;

namespace BriteSpriteExamples
{
	public class BSExample_CameraRotator : MonoBehaviour {

		// Use this for initialization
		void Start () 
		{
			StartCoroutine(RotateCamera());
		}

		IEnumerator RotateCamera()
		{
			while (true)
			{
				yield return new WaitForSeconds(6.0f);

				float rotationAccumulated = 0;
				while (rotationAccumulated > -80)
				{
					this.transform.localRotation = Quaternion.Euler(0,rotationAccumulated,0);

					rotationAccumulated -= Time.deltaTime * 30;

					yield return null;
				}

				yield return new WaitForSeconds(1.0f);

				rotationAccumulated = 0;

				while (rotationAccumulated < 80)
				{
					this.transform.localRotation = Quaternion.Euler(0,-80 + rotationAccumulated,0);
					
					rotationAccumulated += Time.deltaTime * 30;
					
					yield return null;
				}

				this.transform.localRotation = Quaternion.identity;
			}
		}

	}
}