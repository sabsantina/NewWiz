using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeys : MonoBehaviour {
	[SerializeField] GameObject m_InventoryMenu;

	/**The ItemClass variable in the hotkey slot.*/
	public ItemClass item;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**A function to consume the item in the current hotkey slot*/
	public void useItem()
	{
		if (item != null) {
			GetComponentInParent<PlayerInventory> ().itemEffectUsage (item);
			int numberOfItem = GetComponentInParent<PlayerInventory> ().getNumberItem (item);
			gameObject.transform.parent.GetComponentInChildren<Text> ().text = "" + numberOfItem;
		} else {
			Debug.Log ("No item in hotkey");
		}
	}

	public void GetItemInformationFromInventoryMenu()
	{
		Canvas IM_canvas = this.m_InventoryMenu.GetComponent<Canvas> ();
//		IM_canvas.planeDistance = 
		float initial_canvas_plane_distance = IM_canvas.planeDistance;
		IM_canvas.planeDistance = 0.0f;
		this.m_InventoryMenu.SetActive (true);
//		this.m_InventoryMenu.GetComponent<InventoryMenu>().
	}

//	/**A function to call to update the number of a given item in the slot.
//	*This is intended to be called from the player inventory on item pickup, in the event that a hotkeyed item be picked up.
//	*Assumes that [this.item] is not null.*/
//	public void UpdateSlotItemCount()
//	{
//		int numberOfItem = GetComponentInParent<PlayerInventory> ().getNumberItem (item);
//		gameObject.transform.parent.GetComponentInChildren<Text> ().text = "" + numberOfItem;
//	}//end f'n UpdateSlotItemCount()

	/**A function to check whether [item] is equal to [this.item]*/
	public bool ContainItem(ItemClass item)
	{
		return this.item.isEqual(item);
	}//end f'n ContainItem(ItemClass)

	/**A function to set the current hotkey's sprite to the corresponding to item [itemToSet], and to set the hotkey's text component to reflect the number of items in the player inventory*/
	public void setHotKeys(ItemClass itemToSet, int numberOfItems)
	{
		item = itemToSet;
		ItemIcon itemIcon= GetComponentInParent<ItemIcon> ();
		gameObject.GetComponent<Image> ().sprite = itemIcon.getItemSpire (itemToSet.m_ItemName);
		string new_text = "" + numberOfItems;
		gameObject.transform.parent.gameObject.GetComponentInChildren<Text> ().text = new_text;

//		Debug.Log(numberOfItems + " of " + itemToSet.m_ItemName.ToString() + " to set to hotkey, with sprite " + gameObject.GetComponent<Image> ().sprite.name);
	}
}
