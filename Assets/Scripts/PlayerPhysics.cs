using UnityEngine;
using System.Collections;


[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
	
	public LayerMask collisionMask;
	public LayerMask movingPlatformMask;
	public LayerMask waterMask;
	public LayerMask checkPointMask;
	private BoxCollider collider;
	private Vector3 size, center, originalSize, originalCentre;
	private float colliderScale, space = .005f;
	private CharacterController characterController;
	
	[HideInInspector]
	public bool onGround;
	public bool onMovingPlatform;
	public bool onCheckPoint;

	Ray ray;
	RaycastHit hit;
	
	void Start() {
		characterController = GetComponent<CharacterController> ();
		collider = GetComponent<BoxCollider>();
		colliderScale = transform.localScale.x;
		
		originalSize = collider.size;
		originalCentre = collider.center;
		SetCollider(originalSize,originalCentre);
	}

	public void SetCollider(Vector3 sizeScale, Vector3 centerScale) {
		collider.size = sizeScale;
		collider.center = centerScale;
		size = sizeScale * colliderScale;
		center = centerScale * colliderScale;
	}
	
	public void Move(Vector3 moveAmt) {
		onGround = false;
		onMovingPlatform = false;
		Vector3 pos = transform.position;

		for (int i = 0; i < 6; i ++) {
			float direction = Mathf.Sign(moveAmt.y);
			float x = (pos.x + center.x - size.x/2) + size.x/(5) * i; 
			float z = (pos.z + center.z - size.z/2) + size.z/(5) * i;
			float y = pos.y + center.y + size.y/2 * direction;
			
			ray = new Ray(new Vector3(x, y, z), new Vector3(0, direction, 0));
			Debug.DrawRay(ray.origin,ray.direction);

			if (Physics.Raycast(ray, out hit, Mathf.Abs(moveAmt.y) + space, movingPlatformMask)) {
				Debug.Log("On Plaform");
				onMovingPlatform = true;
				break;
			}
			else if (Physics.Raycast(ray, out hit, Mathf.Abs(moveAmt.y) + space, collisionMask)) {
				float displacement = Vector3.Distance (ray.origin, hit.point);
				onGround = true;
				break;
			}
			else if (Physics.Raycast(ray, out hit, Mathf.Abs(moveAmt.y) + space, checkPointMask)) {
				onCheckPoint = true;
			}
		}
		//Vector3 finalTrans = new Vector3(0, moveAmt.y, 0);	
		//characterController.Move (finalTrans);
		//transform.Translate(finalTrans);
	}
}
