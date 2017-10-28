using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : Objective {

	public List<QuestItem> m_QuestItems = new List<QuestItem>();

	public void SpawnQuestItemsAtPosition(Vector3 position, GameObject prefab, int number_of_items)
	{
//		//for however many quest items are needed...
//		for (int index = 0; index < number_of_items; index++) {
//			//Spawn the quest item
//			GameObject item_obj = GameObject.Instantiate(prefab);
//			//Position the enemy
//			item_obj.transform.position = position;
//			//Adjust position with respect to number of enemies
//
////			item_obj.GetComponent<QuestItemPickup>().m_QuestItem
//		}//end for
	}
}
