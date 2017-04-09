﻿using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {
	
	public float distance;
	public Transform target;
	public float lookAtDistance = 80;
	public float chaseRange = 20;
	public float attackRange = 15;
	public float speed = 10;
	public float damping = 6;
	public float damage = 8;
	
	private PlayerPhysics playerPhysics;
	public CharacterController controller;
	public float gravity = 20;
	
	private Vector3 moveDirection = new Vector3 (0, 0, 0);
	private float attackTimeRepeat = 1;
	private float attackTime;
	public float health;
	public bool kill, isAlive = true;
	private Camera myCamera;
	private GUIStyle currentStyle;
	private float maxHealth = 20;
	
	void Start () 
	{
		EnemyCamera newCamera = transform.parent.gameObject.GetComponent<EnemyCamera> ();
		myCamera = newCamera.myCamera;
		controller = GetComponent<CharacterController> ();
		attackTime = Time.time;
		isAlive = true;
		kill = false;
		health = 20;
	}
	
	void Update () 
	{
		if (health == 0) 
		{
			animation.Play("death1");
			if(kill == false)
			{
				Destroy(gameObject, 5);
				kill = true;
			}
			isAlive = false;
		}
		else
		{
			distance = Vector3.Distance (target.position, transform.position);
			if (distance < lookAtDistance)
			{
				Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
				animation.CrossFade("idle");
			}
			if (distance > lookAtDistance) 
				animation.CrossFade("idle");
			if (distance < attackRange) 
				attack ();
			//[NOTE] No collision detection yet.
			else if (distance < chaseRange) 
			{
				animation.CrossFade("run");
				Vector3 direction = transform.TransformDirection (Vector3.forward);
				moveDirection = Vector3.forward;
				moveDirection.y = 0;
				transform.Translate (moveDirection * speed * Time.deltaTime);
			}
		}
	}

	void OnGUI(){
		if (health == 0 || distance > chaseRange)
			return;
		InitStyles ();
		float healthBarLength = 10.0f;
		Vector3 position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
		Vector3 screenPosition =  myCamera.WorldToScreenPoint(position);
		screenPosition.y = Screen.height - screenPosition.y -1; 
		GUI.Box (new Rect(screenPosition.x, screenPosition.y, 300, healthBarLength), "Enemy");
		GUI.Box (new Rect(screenPosition.x, screenPosition.y, (health/maxHealth)*300, healthBarLength), "Enemy", currentStyle);
	}
	
	private void InitStyles(){
		currentStyle = new GUIStyle( GUI.skin.box );
		if(health > 2)
			currentStyle.normal.background = MakeTex( 2, 2, new Color( 0f, 1f, 0f, 0.5f ) );
		else
			currentStyle.normal.background = MakeTex( 2, 2, new Color( 1f, 0f, 0f, 0.5f ) );
	}
	
	private Texture2D MakeTex( int width, int height, Color col ){
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i ){
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}

	
	public void attack()
	{
		if (Time.time > attackTime) 
		{
			animation.Play("attack1");
			attackTime = Time.time + attackTimeRepeat;
			target.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			Debug.Log ("Attack");
		}
	}
	
	public void ApplyDamage()
	{
		chaseRange += 30;
		speed += 2;
		lookAtDistance += 50;
		if (isAlive) 
		{
			animation.Play("hit1");
			health -= 1;
			Debug.Log ("Hit!");
		}
	}
}


