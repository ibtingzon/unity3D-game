using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PauseGame))]
public class PlanetPlayerController : MonoBehaviour {


	public PlayerHealth playerHealth;
	public PauseGame pauseGame;
	public Texture2D pauseImage;


	private float distance, rotation = 0;
	public bool isAlive;

	//Coin GUI
	public int coinCount = 0;
	public GUIText guiText;
	public Texture2D coinImage;
	private GUIStyle font;
	private Vector3 moveDirection;

	void Start(){
		playerHealth = GetComponent<PlayerHealth>();
		pauseGame = GetComponent<PauseGame> ();

		font = new GUIStyle();
		font.fontSize = 32;
	}

	void Update() {
		if(!pauseGame.paused){
			rotation = 0;
			if (Input.GetAxisRaw ("Vertical") != 0 || Input.GetAxisRaw ("Horizontal") != 0)
				animation.CrossFade("run");
			else{
				animation.CrossFade("idle");
			}

			if (Input.GetAxis("Mouse X") != 0){
				rotation += 7*Input.GetAxis("Mouse X");
				transform.Rotate (0,rotation,0);
			}

			if (Input.GetKeyDown ("space")) 
			{
				moveDirection = new Vector3(0,0,0);
				animation.Play("jump");
				moveDirection.y += 20;
				moveDirection.y -= -20 * Time.deltaTime;
				rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveDirection) * 10 * Time.deltaTime);
			}

			RaycastHit hit;
			if(Input.GetMouseButtonDown(0)){
				animation.Play("attack");
				if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)){
					distance = hit.distance;
					hit.transform.SendMessage("ApplyDamage", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	public void killPlayer(){
		Application.LoadLevel("GameOver");
	}
	
	void OnGUI () 
	{
		GUI.BeginGroup (new Rect (10, 58, 150 , 50));
		GUI.Label(new Rect (0,0, 100, 50), coinImage);
		font.normal.textColor = Color.white;
		GUI.Box(new Rect (60, 10, 50, 35), coinCount.ToString(), font);
		GUI.EndGroup ();
		
		if(pauseGame.paused)
		{
			GUI.BeginGroup (new Rect (Screen.width/4 + 50, Screen.height/4, 750 , 750));
			GUI.Label(new Rect (0, 0, 750, 750), pauseImage);
			GUI.EndGroup ();
		}
	}
	
	public void collectCoins()
	{
		coinCount += 1;
		Debug.Log (coinCount);
	}
	
	public void ApplyDamage()
	{
		playerHealth.reduceHealth(2);
	}
	
	public void addHearts(){ playerHealth.AddHearts(1);}

}
