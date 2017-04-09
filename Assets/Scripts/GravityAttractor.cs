using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {

	public float gravity = -20;
	private float speed = 50f;

	public void Attract(Transform body) {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.up;

		body.rigidbody.AddForce(gravityUp * gravity);

		Quaternion targetRotation = Quaternion.FromToRotation(localUp,gravityUp) * body.rotation;
		body.rotation = Quaternion.Slerp(body.rotation,targetRotation, speed * Time.deltaTime );
	}   
}
