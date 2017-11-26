using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioButton : MonoBehaviour {
	[SerializeField] Sprite bioSprite;
	[SerializeField] Sprite inventorySprite;
	[SerializeField] GameObject bioCanvas;

	Button biobtn;
	public Sprite buttonImage;

	// Use this for initialization
	void Start () 
	{
		biobtn = GetComponent<Button> ();
		//Without this line, the bio button won't open on first click.
		buttonImage = biobtn.image.sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void buttonClick()
	{
//		Debug.Log ("Button image " + buttonImage.name + " == bio sprite " + bioSprite.name + "? " + (buttonImage == bioSprite));
		if (buttonImage == bioSprite) 
		{
			bioCanvas.SetActive (true);
			biobtn.image.overrideSprite = inventorySprite;
			biobtn.image.sprite = inventorySprite;
			buttonImage = inventorySprite;

		} 
		else 
		{
			bioCanvas.SetActive (false);
			biobtn.image.overrideSprite = bioSprite;
			biobtn.image.sprite = bioSprite;
			buttonImage = bioSprite;
		}
	}
}
