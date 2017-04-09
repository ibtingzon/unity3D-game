using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	
	public float rotationAmount = 5.0f;
	public Transform effect;
	//public AudioSource coinSound;
	
	void Update () { 
		PauseGame pauseGame = transform.parent.gameObject.GetComponent<PauseGame>();
		if(!pauseGame.paused){
			transform.Rotate(new Vector3(0,0,rotationAmount));
		}
	}
	
	void OnTriggerEnter (Collider col) {
		CoinSound coinSound = transform.parent.gameObject.GetComponent<CoinSound>();
		if (col.gameObject.name == "Player"){
			coinSound.coinSound.Play();
			Instantiate(effect, transform.position, transform.rotation);
			Destroy(gameObject);
			col.gameObject.SendMessage("collectCoins", SendMessageOptions.DontRequireReceiver);
		}
	}
}