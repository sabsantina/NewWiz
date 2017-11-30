//#define WITH_TUTORIALS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : Menu {
	[SerializeField] GameObject hotKey1;
	[SerializeField] GameObject hotKey2;
	[SerializeField] GameObject hotKey3;
	/**A reference to the tutorial manager.*/
	[SerializeField] TutorialManager m_TutorialManager;
	/**A boolean telling us whether or not it's the first time the inventory menu was opened.*/
	public bool m_FirstTimeInventoryMenuOpen = true;
	/**A boolean telling us whether or not it's the first time the inventory menu was closed.*/
	public bool m_FirstTimeInventoryMenuClosed = false;

	/**A reference to the player's inventory*/
	public PlayerInventory m_PlayerInventory;
	public Dictionary<ItemClass, int> itemDic;
	public ItemIcon m_ItemIcon;
	public ItemSlot[] itemSlots;

	public Text itemDescription;

	/**A boolean to tell us whether or not the inventory menu is active*/
	private bool notActive = true;

	AudioSource m_audioSource;

	public ItemSlot selectedSlot;

	void Start()
	{
		m_PlayerInventory = GetComponentInParent<PlayerInventory> ();
		itemDic = m_PlayerInventory.m_ItemDictionary;
		m_ItemIcon = GetComponentInParent<ItemIcon> ();
		itemSlots = GetComponentsInChildren<ItemSlot> ();
		itemDescription = GameObject.Find ("Description").GetComponent<Text>();
		m_audioSource = GetComponentInChildren<AudioSource> ();
	}
		
	void Update()
	{
		if (this.notActive) {
			this.updateInventoryMenu ();
			this.notActive = false;
		} 
		if (PlayerCastSpell.m_MenuOpen && Input.GetKey(KeyCode.I)) 
		{
			CloseMenu ();
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
	}//end f'n void Update()
		
	/**A function to be called by the button that closes the menu OnClick.*/
	public void CloseMenu()
	{
		this.notActive = true;

		base.CloseMenu ();
		//if it's the first time we close the menu...
		if (this.m_FirstTimeInventoryMenuClosed) {
			#if WITH_TUTORIALS
			//...then enable the consume hotkeyed item tutorial
			this.m_TutorialManager.Enable (TutorialEnum.CONSUME_HOTKEYED_ITEMS);
			#endif
			//as of now it won't be the first time the menu closes
			this.m_FirstTimeInventoryMenuClosed = false;
		}//end if
	}//end f'n void CloseMenu()

	/**A function to be called by the button that opens the menu OnClick*/
	public void OpenMenu()
	{
		//if it was the first time that the inventory menu was opened...
		if (this.m_FirstTimeInventoryMenuOpen) {
			#if WITH_TUTORIALS
			//...then enable the hotkeys tutorial
			this.m_TutorialManager.Enable (TutorialEnum.HOTKEYS);
			#endif
			//as of now it won't be the first time the menu opens
			this.m_FirstTimeInventoryMenuOpen = false;
			//but if it was the first time the menu opened, then this will be the first time it closes.
			this.m_FirstTimeInventoryMenuClosed = true;
		}//end if
		base.OpenMenu ();
	}//end f'n void OpenMenu()

	public bool checkTwoOtherHotkeys(GameObject hk1, GameObject hk2)
	{
		return hk1.GetComponentInChildren<HotKeys>().item != selectedSlot.item && hk2.GetComponentInChildren<HotKeys>().item != selectedSlot.item;
	}


	public void setHotKeys(GameObject hotkey)
	{
		if (selectedSlot.item != null) {
			Debug.Log ("Set HotKey");
			ItemSlot currentSlot = selectedSlot.GetComponent<ItemSlot> ();
			hotkey.GetComponentInChildren<HotKeys> ().item = currentSlot.item;
			GameObject imageUI = hotkey.GetComponentInChildren<HotKeys> ().gameObject;
			imageUI.GetComponent<Image> ().sprite = currentSlot.getItemSprite ();
			hotkey.GetComponentInChildren<Text> ().text = currentSlot.getText ().text;
		} 
		else 
		{
			Debug.Log ("This slot is empty");
			m_audioSource.PlayOneShot (GetComponent<MenuSounds> ().getErrorSound ());
		}
	}

	public void setSelectedSlot(ItemSlot slot)
	{
		selectedSlot = slot;
		if (slot.item != null) {
			itemDescription.text = slot.item.description;
		} else
			itemDescription.text = "No item in slot.";
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
			if (slot != null && slot.getText ().text != "") 
			{
				if (slot.item.m_ItemName == current.m_ItemName)
					return slot;
			}
		}

		return null;
	}
}
