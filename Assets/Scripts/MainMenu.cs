using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public int playerLives;

	public string startLevel;
//	public PauseMenu paused;

//	public void Start(){
//		paused = FindObjectOfType<PauseMenu>();
//	}

	public void NewGame(){
//		if (paused.isPaused) {
//			paused.isPaused = !paused.isPaused;
//		}
		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);
		Application.LoadLevel (startLevel);
	}

	public void QuitGame(){
		Application.Quit();
	}

}