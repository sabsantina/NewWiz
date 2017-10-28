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
//
//			//Then extract the Enemy component and store it in the target list, to be checked later
//			Enemy enemy = item_obj.GetComponent<Enemy>();
//			//if the enemy component isn't in the object, it's in one of the children
//			if (enemy == null) {
//				enemy = enemy_obj.GetComponentInChildren<Enemy> ();
//				enemy.m_Spawner = this.m_EnemyLootSpawner;
//			}
//
//			this.m_Targets.Add (enemy);
//		}//end for
	}
}
