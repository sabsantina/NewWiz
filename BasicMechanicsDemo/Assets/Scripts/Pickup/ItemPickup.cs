/*
* A class for item pickups.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the object has a collider so we can pick it up
[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour {
	
	public Item m_Item;
	public string m_ItemName;

	void Awake()
	{
		this.gameObject.GetComponent<Collider> ().isTrigger = true;
	}

	/**A function to set the Item instance to this ItemPickup.*/
	public void SetItem(Item item_spawned)
	{
		this.m_Item = item_spawned;
		this.m_ItemName = this.m_Item.m_ItemName.ToString();
	}//end f'n void SetItem(Item)

	//A function to trigger when the player walks into an item pickup
	void OnTriggerEnter(Collider other)
	{
		//if the object colliding with the item pickup is the player...
		if (other.gameObject.GetComponent<PlayerInventory> ()) {
			other.gameObject.GetComponent<PlayerInventory> ().AddItem (this.m_Item);

			GameObject.Destroy (this.gameObject);
		}
	}//end f'n void OnTriggerEnter(Collider)

}//end class ItemPickup
