using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {
	
	public float rotationAmount = 5.0f;
	public Transform effect;

	void Update () 
	{ 
		PauseGame pauseGame = transform.parent.gameObject.GetComponent<PauseGame>();
		if(!pauseGame.paused)
		{
			transform.Rotate(new Vector3(0,0,rotationAmount));
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player")
		{
			Instantiate(effect, transform.position, transform.rotation);
			Destroy(gameObject);
			col.gameObject.SendMessage("addHearts", SendMessageOptions.DontRequireReceiver);
		}
	}
}