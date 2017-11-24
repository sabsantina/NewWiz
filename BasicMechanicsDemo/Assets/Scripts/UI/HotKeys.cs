using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeys : MonoBehaviour {
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

	/**A function to call to update the number of a given item in the slot.
	*This is intended to be called from the player inventory on item pickup, in the event that a hotkeyed item be picked up.
	*Assumes that [this.item] is not null.*/
	public void UpdateSlotItemCount()
	{
		int numberOfItem = GetComponentInParent<PlayerInventory> ().getNumberItem (item);
		gameObject.transform.parent.GetComponentInChildren<Text> ().text = "" + numberOfItem;
	}//end f'n UpdateSlotItemCount()

	/**A function to check whether [item] is equal to [this.item]*/
	public bool ContainItem(ItemClass item)
	{
		return this.item.isEqual(item);
	}//end f'n ContainItem(ItemClass)
}
