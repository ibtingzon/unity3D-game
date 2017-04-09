using UnityEngine;
using System.Collections;

public class LevelComplete : MonoBehaviour {

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.name == "Player"){
			Application.LoadLevel("LevelComplete");
		}
	}
}
