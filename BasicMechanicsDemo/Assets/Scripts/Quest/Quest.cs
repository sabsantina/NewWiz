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

//	public Fetch m_Fetch;

	/**A function to be overloaded, to initialize an objective type (ideally of type KillEverything)*/
	public void InitializeObjectiveType()
	{
		if (this.m_QuestType == QuestType.KILL_EVERYTHING) {
			this.m_KillEverything = new KillEverything ();
//			this.m_Fetch = null;
		}//end if
	}

	public void KillEverything_SetTargets(List<GameObject> all_targets)
	{
		foreach (GameObject obj in all_targets) {
			//if the object itself doesn't have the enemy component...
			if (obj.GetComponent<Enemy>() == null) {
				//...then find it among the children
				this.m_KillEverything.m_Targets.Add (obj.GetComponentInChildren<Enemy> ());
			}//end if
			//else if the object has the enemy component...
			else {
				//...then grab it
				this.m_KillEverything.m_Targets.Add (obj.GetComponent<Enemy> ());
			}//end else
		}//end foreach
	}

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
