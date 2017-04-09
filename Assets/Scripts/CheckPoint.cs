using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.name == "Player"){
			Debug.Log("CheckPoint!");
			col.gameObject.SendMessage("checkPointFnc", SendMessageOptions.DontRequireReceiver);
		}
	}
}
