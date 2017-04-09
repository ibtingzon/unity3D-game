using UnityEngine;
using System.Collections;

public class EnemyAI2 : MonoBehaviour {
	
	public float distance;
	public Transform target;
	public float lookAtDistance = 20;
	public float chaseRange = 20;
	public float attackRange = 5;
	public float speed = 13;
	public float damping = 6;
	public float damage = 8;
	
	private PlayerPhysics playerPhysics;
	public CharacterController controller;
	public float gravity = 20;
	
	private Vector3 moveDirection = new Vector3 (0, 0, 0);
	private float attackTimeRepeat = 2;
	private float attackTime;
	public float health;
	public bool kill, isAlive = true;
	
	
	void Start () {
		controller = GetComponent<CharacterController> ();
		attackTime = Time.time;
		isAlive = true;
		kill = false;
		health = 10;
	}

	void onGUI(){
		Vector3 screenPosition =  Camera.current.WorldToScreenPoint(transform.position);
		screenPosition.y = Screen.height - (screenPosition.y + 1);
		GUI.Box ( new Rect(screenPosition.x - 50, screenPosition.y - 50, 50, 50), "Enemy");
	}

	void Update () {
		PauseGame pauseGame = transform.parent.gameObject.GetComponent<PauseGame>();
		if(!pauseGame.paused){
			if (health == 0) {
				if(kill == false){
					animation.Play("death");
					Destroy(gameObject, 5);
					kill = true;
				}
				isAlive = false;
			}
			else{
				distance = Vector3.Distance (target.position, transform.position);
				if (distance < lookAtDistance) {
					animation.CrossFade("iddle");
					if (distance < attackRange) {
						attack ();
					}
					//[NOTE] No collision detection yet.
					else if (distance < chaseRange) {
						Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
						transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
						animation.CrossFade("walk");
						Vector3 direction = transform.TransformDirection (Vector3.forward);
						moveDirection = Vector3.forward;
						moveDirection.y = 0;
						transform.Translate (moveDirection * speed * Time.deltaTime);
					}
				}
				if (distance > lookAtDistance) {
					animation.CrossFade("iddle");
				}
			}
		}
	}
	
	public void attack(){
		if (Time.time > attackTime) {
			animation.Play("attack_Melee");
			attackTime = Time.time + attackTimeRepeat;
			target.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			Debug.Log ("Attack");
		}
	}
	
	public void ApplyDamage(){
		chaseRange += 30;
		speed += 2;
		lookAtDistance += 50;
		if (isAlive) {
			animation.Play("damage");
			health -= 1;
		}
	}

}


