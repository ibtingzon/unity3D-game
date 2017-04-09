using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {


	public int startHealth = 30;
	public int healthPerHeart = 10;
	public int currHealth;
	public int maxHealth;
	public GUITexture heartGUI;

	public Texture[] heartImages;
	private int heartIndex = -1;
	public float maxHeartsOnRow = 5;

	public ArrayList hearts = new ArrayList();
	private Vector2 spacing;

	void Start () {
		spacing = new Vector2(heartGUI.pixelInset.width, heartGUI.pixelInset.height);
		AddHearts(startHealth/healthPerHeart);
	}

	void Update () {
		updateHealth (0);
		UpdateHearts();
	}

	public void updateHealth(int value){
		currHealth += value;
		if (currHealth < 0)
			currHealth = 0;
		if(currHealth > maxHealth)
			currHealth = maxHealth;
	}

	public void reduceHealth(int value){
		currHealth -= value;
	}

	public void AddHearts(int noOfHeart) {
		for (int i = 0; i < noOfHeart; i ++) { 
			Transform newHeart = ((GameObject)Instantiate(heartGUI.gameObject)).transform; 
			newHeart.parent = this.transform.parent;

			int y = (int)(Mathf.FloorToInt(hearts.Count / maxHeartsOnRow));
			int x = (int)(hearts.Count - y * maxHeartsOnRow);

			newHeart.GetComponent<GUITexture>().pixelInset = new Rect(x * spacing.x, 0, 50, 50);
			newHeart.GetComponent<GUITexture>().texture = heartImages[0];
			hearts.Add(newHeart);
		}
		maxHealth += noOfHeart * healthPerHeart;
		currHealth = maxHealth;
		UpdateHearts();
	}

	public void removeHeart(){

	}


	void UpdateHearts() {
		int i = 0;
		int heartToDestroy = -1;
		foreach (Transform heart in hearts) {
			i += 1; 
			if (currHealth >= i * healthPerHeart) {
				heart.guiTexture.texture = heartImages[heartImages.Length-1]; 
			}
			else {
				int currentHeartHealth = (int)(healthPerHeart - (healthPerHeart * i - currHealth));
				int healthPerImage = healthPerHeart / heartImages.Length; 
				int imageIndex = currentHeartHealth / healthPerImage;

				if (imageIndex <= 0 ) {
					heart.guiTexture.texture = heartImages[0];
					heartToDestroy = hearts.Count;
				}
				else{
					heart.guiTexture.texture = heartImages[imageIndex];
				}
				break;
			}
		}

		if(heartToDestroy != -1){
			Debug.Log("Remove heart!");
			hearts.RemoveAt (hearts.Count - 1);
		}
	}	
}
