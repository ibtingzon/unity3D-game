using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

	public GravityAttractor attractor;
	private Transform myTransform;
	private float moveSpeed = 30;
	private float distance;
	private Vector3 amountToMove, moveDirection;

	void Start () {
		rigidbody.useGravity = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		myTransform = transform;
	}

	void FixedUpdate () {
		if (attractor)
			attractor.Attract(myTransform);
		if(transform.gameObject.name == "Player"){
			moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")).normalized;
			rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
		
		}
		else if(transform.gameObject.name == "Spider"){

		}
	}
}
