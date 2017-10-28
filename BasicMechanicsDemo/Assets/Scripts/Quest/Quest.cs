using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {
	public QuestName m_QuestName;
	public string m_QuestNameString;
	public QuestState m_QuestState;
	public QuestType m_QuestType;

	/**The prefabs to be spawned for that quest.*/
	public List<GameObject> m_RequisitePrefabs = new List<GameObject>();
	/**The position where we want to spawn stuff for the quest.*/
	public Vector3 m_QuestObjectivePosition;

	/*
	* Initially, I'd been trying to use an Objective superclass to manage all the types of quests it could be. But the problem
	* with that is that if you assign a child-class objective type as a parent-class objective type, slicing will occur.
	* So for now, this was the best way I could think to do this:
	* Each quest gets its objective type initialized through the QuestType variable.
	*/

	/*If the quest is a Kill Everything*/

	public KillEverything m_KillEverything;
	public int m_NumberOfEnemiesToKill;

	/*If the quest is a Fetch*/

	public Fetch m_Fetch;
	public int m_NumberOfItemsToFind;

	/**A function to be overloaded, to initialize an objective type*/
	public void InitializeObjectiveType()
	{
		if (this.m_QuestType == QuestType.KILL_EVERYTHING) {
			this.m_KillEverything = new KillEverything ();
			this.m_Fetch = null;
			return;
		}//end if
		if (this.m_QuestType == QuestType.FETCH) {
			this.m_Fetch = new Fetch ();
			this.m_KillEverything = null;
		}
	}

	/**A function to spawn in whatever is needed for the quest.*/
	public void SpawnInQuestObjects()
	{
		//if the quest type is KILL EVERYTHING...
		if (this.m_QuestType == QuestType.KILL_EVERYTHING) {
			
//			int random_int = Random.Range (1, this.m_NumberOfEnemiesToKill - 1);

			foreach (GameObject obj in this.m_RequisitePrefabs) {
				if (this.m_RequisitePrefabs.Count == 1) {
					this.m_KillEverything.SpawnEnemiesAtPosition (this.m_QuestObjectivePosition, obj, this.m_NumberOfEnemiesToKill);
				}//end if

				//Dunno how to do this if there's more than one enemy type at a time. Will come back to it later
			
			}//end foreach
		}//end if
		//else if the quest type is FETCH...
		else if (this.m_QuestType == QuestType.FETCH) {
			
			foreach (GameObject obj in this.m_RequisitePrefabs) {
				
				if (this.m_RequisitePrefabs.Count == 1) {
					this.m_Fetch.SpawnQuestItemsAtPosition (this.m_QuestObjectivePosition, obj, this.m_NumberOfItemsToFind);
				}//end if

				//Dunno how to do this if there's more than one item type at a time. Will come back to it later

			}//end foreach
		}
	}//end f'n void SpawnInQuestObjects()

	/**A function to set the location of the quest objective, so we know where to spawn things.*/
	public void SetQuestGoalLocation(Vector3 location)
	{
		this.m_QuestObjectivePosition = location;
	}

	/**A function to return whether or not the quest objective was completed.*/
	public bool CheckQuestObjectiveCompleted()
	{
		//if the quest is a Kill Everything...
		if (this.m_QuestType == QuestType.KILL_EVERYTHING) {
			//...check and return that objective condition
			return this.m_KillEverything.CheckForObjectiveIsMet ();
		}//end if

		return false;
	}//end f'n bool CheckQuestObjectiveCompleted()


}
