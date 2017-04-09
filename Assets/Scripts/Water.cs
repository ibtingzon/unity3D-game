using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
	void OnTriggerEnter (Collider col) 
	{
		if (col.gameObject.name == "Player")
		{
			col.gameObject.SendMessage("killPlayer", SendMessageOptions.DontRequireReceiver);
		}
	}
}
