using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	/**An enum variable for the item name. See ItemName.cs for all item names.*/
	public ItemName m_ItemName { set; get; }
	/**An enum variable for the item effect. See ItemEffect.cs for all item effects.*/
	public ItemEffect m_ItemEffect { set; get;}
	/**An int to tell us the quantity of the item in the player's inventory.*/
	public int m_Quantity;
}
