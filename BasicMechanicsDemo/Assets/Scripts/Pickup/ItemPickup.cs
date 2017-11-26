/*
* A class for item pickups.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the object has a collider so we can pick it up
[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(AudioSource))]
public class ItemPickup : MonoBehaviour {
	public ItemClass m_Item = new ItemClass();
	public string m_ItemName;

//	[SerializeField] private AudioClip m_Clip;
//	/**The place the sound comes from.*/
//	private AudioSource m_AudioSource;

	void Awake()
	{
		this.gameObject.GetComponent<Collider> ().isTrigger = true;
//		this.m_AudioSource = this.GetComponent<AudioSource>();
	}

	/**A function to set the Item instance to this ItemPickup.*/
	public void SetItem(ItemClass item_spawned)
	{
		this.m_Item.m_ItemName = item_spawned.m_ItemName;
		this.m_Item.m_ItemEffect = item_spawned.m_ItemEffect;
		this.m_ItemName = item_spawned.m_ItemName.ToString ();

//		Debug.Log ("Set item: " + this.m_Item.ReturnItemInstanceInfo ());


	}//end f'n void SetItem(Item)

	//A function to trigger when the player walks into an item pickup
	void OnTriggerEnter(Collider other)
	{
		//if the object colliding with the item pickup is the player...
		if (other.gameObject.GetComponent<PlayerInventory> ()) {
//			Debug.Log ("Item picked up: " + this.m_Item.ReturnItemInstanceInfo ());

			other.gameObject.GetComponent<PlayerInventory> ().AddItem (this.m_Item);
			Debug.Log ("Play pickup sound!");
			other.gameObject.GetComponent<Player> ().playSound(other.gameObject.GetComponent<PlayerAudio>().itemPickUpSound());

			GameObject.Destroy (this.gameObject);
		}
	}//end f'n void OnTriggerEnter(Collider)

}//end class ItemPickup
