/*
* A class for QUEST item pickups. Literally the same as the Item pickup, only with quest items.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the object has a collider so we can pick it up
[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(AudioSource))]
public class QuestItemPickup : MonoBehaviour {

	public QuestItem m_QuestItem;
	public string m_ItemName;

	void Awake()
	{
		this.gameObject.GetComponent<Collider> ().isTrigger = true;
	}


	//A function to trigger when the player walks into an item pickup
	void OnTriggerEnter(Collider other)
	{
		//if the object colliding with the item pickup is the player...
		if (other.gameObject.GetComponent<PlayerInventory> ()) {
			//			Debug.Log ("Item picked up: " + this.m_Item.ReturnItemInstanceInfo ());

			other.gameObject.GetComponent<PlayerInventory> ().m_QuestItems.Add (this.m_QuestItem);

			GameObject.Destroy (this.gameObject);
		}
	}//end f'n void OnTriggerEnter(Collider)

}//end class ItemPickup