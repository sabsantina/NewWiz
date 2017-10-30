using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIcon : MonoBehaviour {
	[SerializeField] Sprite healthPotionIcon;
	[SerializeField] Sprite manaPotionIcon;
	[SerializeField] Sprite none;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Sprite getItemSpire(ItemName item)
	{
		switch (item) 
		{
		case ItemName.Health_Potion:
			return healthPotionIcon;
		case ItemName.Mana_Potion:
			return manaPotionIcon;
		}
		return none;
	}
}
