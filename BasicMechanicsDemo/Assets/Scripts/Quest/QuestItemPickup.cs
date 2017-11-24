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

	public QuestItem m_QuestItem = new QuestItem ();
	public string m_ItemName;
	public string m_SpriteName;

	void Awake()
	{
		
		this.gameObject.GetComponent<Collider> ().isTrigger = true;
	}

	void Start()
	{
		//For testing
		this.m_ItemName = this.m_QuestItem.m_QuestItemName.ToString ();
		this.m_SpriteName = this.m_QuestItem.m_QuestItemSprite.name;
	}


	//A function to trigger when the player walks into an item pickup
	void OnTriggerEnter(Collider other)
	{
		//if the object colliding with the item pickup is the player...
		if (other.gameObject.GetComponent<PlayerInventory> () != null) {
			this.m_QuestItem.m_IsCollected = true;

			other.gameObject.GetComponent<PlayerInventory> ().m_QuestItems.Add (this.m_QuestItem);

			GameObject.Destroy (this.gameObject);
		}
	}//end f'n void OnTriggerEnter(Collider)

}//end class ItemPickup