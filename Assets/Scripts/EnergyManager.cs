using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour {
	public static int energy;

	Text text;

	void Start(){
		text = GetComponent<Text> ();

		energy = 0;
	}

	void Update(){
		if (energy < 0) {
			energy = 0;
		}
		text.text = "" + energy;
	}

	public static void AddEnergy (int energyToAdd){
		energy += energyToAdd;
	}

	public static void Reset(){
		energy = 0;
	}
}