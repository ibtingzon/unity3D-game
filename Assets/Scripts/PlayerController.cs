using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerPhysics))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PauseGame))]

public class PlayerController : MonoBehaviour {

	//Set Variables
	public float gravity = 20;
	public float walkSpeed = 10;
	public float runSpeed = 10;
	public float acceleration = 20;
	public float jumpHeight = 4;
	public float damping = 6;
	public bool isAlive;

	//Sounds
	public AudioSource jumpSound;

	//Coin GUI
	public int coinCount = 0;
	public GUIText guiText;
	public Texture2D coinImage;
	private GUIStyle font;

	//Movement and Speed
	public PlayerPhysics playerPhysics;
	public PlayerHealth playerHealth;
	public PauseGame pauseGame;
	public Texture2D pauseImage;
	private CharacterController characterController;
	private bool movePlayer = false;
	public bool checkPoint = false;
	private float rotation;
	private Vector3 currentSpeed;
	private Vector3 targetSpeed;
	private Vector3 amountToMove;

	//Attack 
	public float damage = 50;
	public float distance;
	
	void Start () {
		font = new GUIStyle();
		font.fontSize = 32;
		characterController = GetComponent<CharacterController> ();
		playerPhysics = GetComponent<PlayerPhysics>();
		playerHealth = GetComponent<PlayerHealth>();
		pauseGame = GetComponent<PauseGame> ();
		isAlive = true;
		renderer.receiveShadows = true;
		renderer.castShadows = true;
	}
	
	void Update () {
		if(!pauseGame.paused){
			//Speed
			float speed = (Input.GetButton("Run"))?runSpeed:walkSpeed;
			targetSpeed = new Vector3(Input.GetAxisRaw("Vertical") * speed, 0, Input.GetAxisRaw("Horizontal") * speed);
			currentSpeed = new Vector3(IncrementTowards(currentSpeed.x, targetSpeed.x, acceleration), 0, IncrementTowards(currentSpeed.z, targetSpeed.z, acceleration));

			//Attack
			RaycastHit hit;
			if(Input.GetMouseButtonDown(0)){
				animation.Play("attack");
				if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)){
					distance = hit.distance;
					hit.transform.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
				}
			}

			//Movement in x and z direction
			playerPhysics.Move (amountToMove * Time.deltaTime);

			Vector3 moveDirection = Vector3.zero;
			Vector3 vertical = transform.TransformDirection (Vector3.forward);
			Vector3 horizontal = transform.TransformDirection (Vector3.right);
			
			characterController.Move (vertical*currentSpeed.x*Time.deltaTime);
			characterController.Move (horizontal*currentSpeed.z*Time.deltaTime);

			//Animation
			if ((playerPhysics.onGround || playerPhysics.onMovingPlatform) && Input.GetAxisRaw ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0) 
				animation.CrossFade("run");
			else
				animation.CrossFade("idle");

			//Rotation
			rotation += 5*Input.GetAxis("Mouse X");
			transform.rotation = Quaternion.Euler (0, rotation, 0);

			//Jump
			if (Input.GetKeyDown ("space")) {
				if (playerPhysics.onGround || (movePlayer == true && playerPhysics.onMovingPlatform)) {
					jumpSound.Play();
					animation.Play("jump");
					amountToMove.y = jumpHeight;
					movePlayer = false;
				}
			}

			//CheckPoint checker
			if(playerPhysics.onCheckPoint){
				Debug.Log("CheckPoint!");
				checkPointFnc();
			}

			//Check Health
			if (playerHealth.hearts.Count == 0)
				killPlayer();

			//Movement in y direction
			amountToMove.y -= gravity * Time.deltaTime;
			characterController.Move (amountToMove * Time.deltaTime);
		}
	}

	private float IncrementTowards(float currentSpeed, float targetSpeed, float alpha) 
	{
		if (currentSpeed == targetSpeed) 
			return currentSpeed;
		else {
			float direction = Mathf.Sign(targetSpeed - currentSpeed); 
			currentSpeed += alpha * Time.deltaTime * direction;
			return (direction == Mathf.Sign(targetSpeed - currentSpeed))? currentSpeed: targetSpeed; 
		}
	}

	public void killPlayer(){
		if(!checkPoint){
			Debug.Log ("Player died.");
			Application.LoadLevel("GameOver");
		}
		else{
			Application.LoadLevel("GameOver");
		}
	}

	void OnGUI () {
		GUI.BeginGroup (new Rect (10, 58, 150 , 50));
		GUI.Label(new Rect (0,0, 100, 50), coinImage);
		font.normal.textColor = Color.white;
		GUI.Box(new Rect (60, 10, 50, 35), coinCount.ToString(), font);
		GUI.EndGroup ();
		
		if(pauseGame.paused){
			GUI.BeginGroup (new Rect (Screen.width/4 + 50, Screen.height/4, 750 , 750));
			GUI.Label(new Rect (0, 0, 750, 750), pauseImage);
			GUI.EndGroup ();
		}
	}

	public void collectCoins(){
		coinCount += 1;
		Debug.Log (coinCount);
	}

	public void ApplyDamage(){
		if (isAlive) {
			int playerDamage = 2;
			playerHealth.reduceHealth(playerDamage);
		}
	}

	public void addHearts(){ playerHealth.AddHearts(1);}
	
	public void movePlayerOnPlatform(){ movePlayer = true; }

	public void stopPlayerOnPlatform(){ movePlayer = false; }

	public void checkPointFnc(){ checkPoint = true; }
}
