/*
* A class to define the objective behavior if the quest objective is to kill a group of targets
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillEverything : Objective {
	/**A list of quest targets; once they're all dead, the player's completed the quest.*/
//	public List<Enemy> m_Targets = new List<Enemy>();
	public List<DefaultEnemy> m_Targets = new List<DefaultEnemy>();
	/**A spawner to spawn loot by the enemy, when they die.*/
	public Spawner m_EnemyLootSpawner;
	/**An empty gameobject to contain the enemies more easily in the scene gameobject list*/
	public GameObject m_EnemyContainer;


	/**A function to check whether every target has been killed.*/
	public override bool CheckForObjectiveIsMet()
	{
		//if the target count is greater than or equal to 1
		if (this.m_Targets.Count >= 1) {
			//foreach enemy in the target list...
			foreach (DefaultEnemy enemy in this.m_Targets) {
				//...if any one enemy is alive...
				if (enemy != null || enemy.m_Health > 0.0f) {
					//return false
					return false;
				}//end if
			}//end foreach
			//else if no enemies are alive, return true
			this.UpdateObjectiveIsMet();
			GameObject.Destroy (this.m_EnemyContainer);
			return true;
		}//end if
		return false;
	}//end f'n bool CheckForObjectiveIsMet()

	/**A function to spawn the given enemy [prefab] at the given position; if there are more */
	public void SpawnEnemiesAtPosition (Vector3 position, GameObject prefab, int number_of_enemies)
	{
		if (this.m_EnemyContainer == null) {
			this.m_EnemyContainer = new GameObject ();
//			this.m_EnemyContainer.name = "KE_EnemyContainer";
		}
		//For consistency, though the empty's position really doesn't matter.
		this.m_EnemyContainer.transform.position = position;
		//for however many enemies are needed...
		for (int index = 0; index < number_of_enemies; index++) {
			//Spawn the enemy
			GameObject enemy_obj = GameObject.Instantiate(prefab, this.m_EnemyContainer.transform);
			//Position the enemy
			enemy_obj.transform.position = position;
//			this.m_EnemyContainer.transform.enemy_obj
			//Adjust position with respect to number of enemies
			//?

			//Then extract the Enemy component and store it in the target list, to be checked later
			DefaultEnemy enemy = enemy_obj.GetComponent<DefaultEnemy>();
			//if the enemy component isn't in the object, it's in one of the children
			if (enemy == null) {
				enemy = enemy_obj.GetComponentInChildren<DefaultEnemy> ();
				enemy.m_Spawner = this.m_EnemyLootSpawner;
			}

			this.m_Targets.Add (enemy);
		}//end for

	}//end f'n void SpawnEnemiesAtPosition(Vector3, GameObject, int)

	/**A function to spawn the given enemy [prefab] at the given position; if there are more */
	public void SpawnEnemiesAtPosition (Spawner enemy_spawner, Vector3 position, EnemyName enemy_name, int number_of_enemies)
	{
		if (this.m_EnemyContainer == null) {
			this.m_EnemyContainer = new GameObject ();
			//			this.m_EnemyContainer.name = "KE_EnemyContainer";
		}
		//For consistency, though the empty's position really doesn't matter.
		this.m_EnemyContainer.transform.position = position;
		//for however many enemies are needed...
		for (int index = 0; index < number_of_enemies; index++) {
			//Spawn the enemy
//			GameObject enemy_obj = GameObject.Instantiate(prefab, this.m_EnemyContainer.transform);
			GameObject enemy_obj = enemy_spawner.SpawnEnemy(enemy_name);
			//Position the enemy
			enemy_obj.transform.position = position;
			//			this.m_EnemyContainer.transform.enemy_obj
			//Adjust position with respect to number of enemies
			//?

			//Then extract the Enemy component and store it in the target list, to be checked later
			DefaultEnemy enemy = enemy_obj.GetComponent<DefaultEnemy>();
			//if the enemy component isn't in the object, it's in one of the children
			if (enemy == null) {
				enemy = enemy_obj.GetComponentInChildren<DefaultEnemy> ();
				enemy.m_Spawner = this.m_EnemyLootSpawner;
			}

			this.m_Targets.Add (enemy);
		}//end for

	}//end f'n void SpawnEnemiesAtPosition(Spawner, Vector3, EnemyName, int)

	private void UpdateObjectiveIsMet()
	{
		this.m_QuestObjectiveIsMet = true;
	}
}//end class KillEverything
