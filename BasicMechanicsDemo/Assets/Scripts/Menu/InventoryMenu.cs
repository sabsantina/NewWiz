using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : Menu {
	/**A reference to the player's inventory*/
	public PlayerInventory m_PlayerInventory;
	public Dictionary<ItemClass, int> itemDic;
	private ItemIcon m_ItemIcon;
	public ItemSlot[] itemSlots;
	private bool notActive;
	void Start()
	{
		m_PlayerInventory = GetComponentInParent<PlayerInventory> ();
		itemDic = m_PlayerInventory.m_ItemDictionary;
		m_ItemIcon = GetComponentInParent<ItemIcon> ();
		itemSlots = GetComponentsInChildren<ItemSlot> ();
		notActive = true;
	}

	void Update()
	{
		if (gameObject.activeSelf) {
			if (notActive) {
				updateInventoryMenu ();
				notActive = false;
			}
		}
		else
		{
			notActive = true;
		}
		
	}

	public void updateInventoryMenu()
	{
		Debug.Log ("Update inventory");
		if (itemDic != null) 
		{
			foreach (KeyValuePair<ItemClass, int> entry in itemDic) 
			{
				ItemSlot findInInventory = findItem (entry.Key);
				if (findInInventory != null) 
				{
					findInInventory.setNumber (entry.Value);
				} 
				else 
				{
					ItemSlot empty = findEmptySlot ();
					if (empty == null)
						Debug.Log ("There are no empty slots");
					empty.setNumber (entry.Value);
					empty.item = entry.Key;
					Sprite itemIcon = m_ItemIcon.getItemSpire (entry.Key.m_ItemName);
					empty.setItemSprite (itemIcon);
				}
			}
		}
	}


	ItemSlot findEmptySlot()
	{
		foreach (ItemSlot slot in itemSlots) 
		{
			if (slot.getText ().text == "")
				return slot;
		}

		return null;
	}

	ItemSlot findItem(ItemClass current)
	{
		foreach (ItemSlot slot in itemSlots) 
		{
			if (slot.getText ().text != "") 
			{
				if (slot.item.m_ItemName == current.m_ItemName)
					return slot;
			}
		}

		return null;
	}
}
