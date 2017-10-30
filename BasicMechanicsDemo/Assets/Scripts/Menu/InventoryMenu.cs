using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : Menu {
	[SerializeField] GameObject hotKey1;
	[SerializeField] GameObject hotKey2;
	[SerializeField] GameObject hotKey3;

	/**A reference to the player's inventory*/
	public PlayerInventory m_PlayerInventory;
	public Dictionary<ItemClass, int> itemDic;
	private ItemIcon m_ItemIcon;
	public ItemSlot[] itemSlots;
	private bool notActive;

	public ItemSlot selectedSlot;

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
			if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				if(checkTwoOtherHotkeys(hotKey2, hotKey3))
					setHotKeys (hotKey1);
			}
			if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				if(checkTwoOtherHotkeys(hotKey1, hotKey3))
					setHotKeys (hotKey2);
			}
			if(Input.GetKeyDown(KeyCode.Alpha3))
			{
				if(checkTwoOtherHotkeys(hotKey1, hotKey2))
					setHotKeys (hotKey3);
			}
		}
		else
		{
			notActive = true;
		}
		
	}

	public bool checkTwoOtherHotkeys(GameObject hk1, GameObject hk2)
	{
		return hk1.GetComponentInChildren<HotKeys>().item != selectedSlot.item && hk2.GetComponentInChildren<HotKeys>().item != selectedSlot.item;
	}


	public void setHotKeys(GameObject hotkey)
	{
		if (selectedSlot.item != null) {
			ItemSlot currentSlot = selectedSlot.GetComponent<ItemSlot> ();
			hotkey.GetComponentInChildren<HotKeys> ().item = currentSlot.item;
			GameObject imageUI = hotkey.GetComponentInChildren<HotKeys> ().gameObject;
			imageUI.GetComponent<Image> ().sprite = currentSlot.getItemSprite ();
			hotkey.GetComponentInChildren<Text> ().text = currentSlot.getText ().text;
		} 
		else 
		{
			Debug.Log ("This slot is empty");
		}
	}

	public void setSelectedSlot(ItemSlot slot)
	{
		selectedSlot = slot;
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
