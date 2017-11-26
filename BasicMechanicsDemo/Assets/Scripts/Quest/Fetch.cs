using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : Objective {

	public List<QuestItem> m_QuestItems = new List<QuestItem>();
	public PlayerInventory m_PlayerInventory;
	private int m_NumberOfItems = 0;
	/**An empty gameobject to contain the enemies more easily in the scene gameobject list*/
	public GameObject m_ItemContainer;


	/**A function to spawn the quest items to be collected at the given position*/
	public void SpawnQuestItemsAtPosition(Spawner item_spawner, Vector3 position, QuestItemName quest_item_name, int number_of_items)
	{
		if (this.m_ItemContainer == null) {
			this.m_ItemContainer = new GameObject ();
			this.m_ItemContainer.name = quest_item_name.ToString() + "Container";
		}
		this.m_NumberOfItems = number_of_items;
		//for however many quest items are needed...
		for (int index = 0; index < number_of_items; index++) {
			//Spawn the quest item
			GameObject item_obj = item_spawner.SpawnQuestItem(quest_item_name, this.m_ItemContainer.transform);
			//Position the item
			item_obj.transform.position = position;
			//Adjust position with respect to number of items
			Vector3 translation = item_obj.transform.position + (2.0f * index * Vector3.forward);
			item_obj.transform.position = translation;
			//Set QuestItem value
//			item_obj.GetComponent<QuestItemPickup> ().m_QuestItem = item;
			this.m_QuestItems.Add (item_obj.GetComponent<QuestItemPickup> ().m_QuestItem);
		}//end for
	}//end f'n SpawnQuestItemsAtPosition(Vector3, GameObject, int)

//	/**A function to spawn the quest items to be collected at the given position*/
//	public void SpawnQuestItemsAtPosition(Vector3 position, GameObject prefab, QuestItem item, int number_of_items)
//	{
//		if (this.m_ItemContainer == null) {
//			this.m_ItemContainer = new GameObject ();
//		}
//		this.m_NumberOfItems = number_of_items;
//		//for however many quest items are needed...
//		for (int index = 0; index < number_of_items; index++) {
//			//Spawn the quest item
//			GameObject item_obj = GameObject.Instantiate(prefab);
//			//Position the item
//			item_obj.transform.position = position;
//			//Adjust position with respect to number of items
//			Vector3 translation = item_obj.transform.position + (2.0f * index * Vector3.forward);
//			item_obj.transform.position = translation;
//			//Set QuestItem value
//			item_obj.GetComponent<QuestItemPickup> ().m_QuestItem = item;
//			this.m_QuestItems.Add (item_obj.GetComponent<QuestItemPickup> ().m_QuestItem);
//		}//end for
//	}//end f'n SpawnQuestItemsAtPosition(Vector3, GameObject, int)

	/**A function to check whether every item has been collected.*/
	public override bool CheckForObjectiveIsMet()
	{
		int number_of_items_found = 0;
		//the player inventory should have at least as many quest items as would be required for this quest alone
		if (this.m_PlayerInventory.m_QuestItems.Count >= this.m_QuestItems.Count) {
			//a temp list to help with the inventory searching
			List<QuestItem> temp_holder = new List<QuestItem> ();
			//for each item in the player inventory...
			foreach (QuestItem inventory_item in this.m_PlayerInventory.m_QuestItems) {
				//...and for each item in the list of items spawned...
				foreach (QuestItem item in this.m_QuestItems) {
					//...if the item in the player inventory shares a name with one of the items spawned...
					if (inventory_item.m_QuestItemName == item.m_QuestItemName) {
						//...increment the number of items found
						number_of_items_found++;
						///...Save the item in a temp list
						temp_holder.Add (item);
						//...and remove the value from the quest items, to ensure we don't take more values than we need
						this.m_QuestItems.Remove (item);
//						Debug.Log (number_of_items_found);
						break;
					}//end if
				}//end foreach
			}//end foreach

			//refill the quest items
			foreach (QuestItem item in temp_holder) {
				this.m_QuestItems.Add (item);
			}//end foreach
		}//end if

		if (number_of_items_found == this.m_NumberOfItems) {
			GameObject.Destroy (this.m_ItemContainer);
		}
		return (number_of_items_found == this.m_NumberOfItems);
	}//end f'n bool CheckForObjectiveIsMet()

	private void UpdateObjectiveIsMet()
	{
		this.m_QuestObjectiveIsMet = true;
	}
}
