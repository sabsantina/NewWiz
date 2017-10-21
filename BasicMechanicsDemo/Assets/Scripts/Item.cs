/**
* A script to manage the Items both in and out of the player inventory
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Item : MonoBehaviour {
	/**An enum variable for the item name. See ItemName.cs for all item names.*/
	public ItemName m_ItemName { set; get; }
	/**An enum variable for the item effect. See ItemEffect.cs for all item effects.*/
	public ItemEffect m_ItemEffect { set; get;}

	void Awake()
	{
		//ensure the box collider's isTrigger is set to true
		this.GetComponent<BoxCollider> ().isTrigger = true;
	}//end f'n void Awake()
}
