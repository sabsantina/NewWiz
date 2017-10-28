/*
* A class to define the objective behavior if the quest objective is to kill a group of targets
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillEverything : Objective {
	/**A list of quest targets; once they're all dead, the player's completed the quest.*/
	public List<Enemy> m_Targets = new List<Enemy>();
	/**A spawner to spawn loot by the enemy, when they die.*/
	public Spawner m_EnemyLootSpawner;

	/**A function to check whether every target has been killed.*/
	public override bool CheckForObjectiveIsMet()
	{
		//if the target count is greater than or equal to 1
		if (this.m_Targets.Count >= 1) {
			//foreach enemy in the target list...
			foreach (Enemy enemy in this.m_Targets) {
				//...if any one enemy is alive...
				if (enemy != null) {
					//return false
					return false;
				}//end if
			}//end foreach
			//else if no enemies are alive, return true
			this.UpdateObjectiveIsMet();
			return true;
		}//end if
		return false;
	}//end f'n bool CheckForObjectiveIsMet()

	/**A function to spawn the given enemy [prefab] at the given position; if there are more */
	public void SpawnEnemiesAtPosition (Vector3 position, GameObject prefab, int number_of_enemies)
	{
		//for however many enemies are needed...
		for (int index = 0; index < number_of_enemies; index++) {
			//Spawn the enemy
			GameObject enemy_obj = GameObject.Instantiate(prefab);
			//Position the enemy
			enemy_obj.transform.position = position;
			//Adjust position with respect to number of enemies


			//Then extract the Enemy component and store it in the target list, to be checked later
			Enemy enemy = enemy_obj.GetComponent<Enemy>();
			//if the enemy component isn't in the object, it's in one of the children
			if (enemy == null) {
				enemy = enemy_obj.GetComponentInChildren<Enemy> ();
				enemy.m_Spawner = this.m_EnemyLootSpawner;
			}

			this.m_Targets.Add (enemy);
		}//end for

	}//end f'n void SpawnEnemiesAtPosition(Vector3, GameObject, int)

	private void UpdateObjectiveIsMet()
	{
		this.m_QuestObjectiveIsMet = true;
	}
}//end class KillEverything
