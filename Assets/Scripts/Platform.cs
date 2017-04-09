using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	private float speed = 3f;
	private float timeToMove = 10;
	private float moveTime = 0;
	private bool move = false;
	private Collider collider;

	void Update(){
		if (move) {
			PauseGame pauseGame = transform.parent.gameObject.GetComponent<PauseGame>();
			if(!pauseGame.paused){
				if( Time.time < moveTime){
					Vector3 direction = -1*Vector3.up;
					transform.Translate (direction * speed * Time.deltaTime);
				}
				else{
					collider.gameObject.SendMessage("stopPlayerOnPlatform", SendMessageOptions.DontRequireReceiver);
					Destroy (gameObject);
				}
			}
		}
	}

	void OnTriggerEnter (Collider col) {
		Debug.Log ("Platform");
		if (col.gameObject.name == "Player"){
			collider = col;
			renderer.material.color = Color.yellow;
			Debug.Log ("Collide! Platform");
			moveTime = Time.time+ timeToMove;
			collider.gameObject.SendMessage("movePlayerOnPlatform", SendMessageOptions.DontRequireReceiver);
			move = true;
		}
	}
}
