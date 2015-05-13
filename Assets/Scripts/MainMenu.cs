using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public string startLevel;
//	public PauseMenu paused;

//	public void Start(){
//		paused = FindObjectOfType<PauseMenu>();
//	}

	public void NewGame(){
//		if (paused.isPaused) {
//			paused.isPaused = !paused.isPaused;
//		}
		Application.LoadLevel (startLevel);
	}

	public void QuitGame(){
		Application.Quit();
	}

}