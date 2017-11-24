using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BioButton : MonoBehaviour {
	[SerializeField] Sprite bioSprite;
	[SerializeField] Sprite inventorySprite;
	[SerializeField] GameObject bioCanvas;

	Sprite buttonImage;

	// Use this for initialization
	void Start () 
	{
		buttonImage = GetComponent<Image> ().sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void buttonClick()
	{
		if (buttonImage == bioSprite) 
		{
			bioCanvas.SetActive (true);
			buttonImage = inventorySprite;
		} 
		else 
		{
			bioCanvas.SetActive (false);
			buttonImage = bioSprite;
		}
	}
}
