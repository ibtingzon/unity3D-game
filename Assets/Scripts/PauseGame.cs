using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

	public bool paused = false;

	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.P) && paused == false) {
			Debug.Log("Paused");
			Time.timeScale = 0.0f;
			paused = true; 
		}
		else if (Input.GetKeyDown(KeyCode.P) && paused == true){
			Debug.Log("Unpaused");
			Time.timeScale = 1.0f;
			paused = false; 
		}
	}
}
