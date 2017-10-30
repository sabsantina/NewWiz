using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour 
{
	public ItemClass item;
	public GameObject itemImage;
	private Text itemNumber;
	// Use this for initialization
	void Start () {
		foreach (Transform child in gameObject.transform) {
			if (child.tag == "SlotImage") 
			{
				itemImage = child.gameObject;
				break;
			}
		}
		itemNumber = GetComponentInChildren<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setNumber(int value)
	{
		itemNumber.text = "" + value;
	}

	public void setItemSprite(Sprite s)
	{
		itemImage.GetComponent<Image> ().sprite = s;
	}

	public Sprite getItemSprite()
	{
		return itemImage.GetComponent<Image> ().sprite;
	}

	public Text getText()
	{
		return itemNumber;
	}
}
