using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Serializable_QuestManager {
	/*
	* Honestly, for the most part all we need from this is the list of quest states.
	* Although when it comes to Kill Everything quests, do we want to record stuff like how many enemies are left?
	* 
	*/

	/**An int list to contain the quest state of the quest at the same position as any given int.
	*So for the quest state of the quest ROOSTER_BANE, for instance, this would be the first value in the list.*/
	public List<int> m_AllQuestStates = new List<int>();
	/**An int list to contain the number of enemies remaining for every Kill Everything quest at the time of the same.
	*The way we'll do this is we'll just get the data on every Kill Everything quest in order.*/
//	List<int> m_KillEverything_EnemiesRemaining = new List<int>();

	//The enemies remaining thing is kind of a drag

	/*
	* Basically what's happening now is if we stop the game and load, the quest enemies and items aren't spawning in.
	*/

	/**A function to record all quest states and record them to [this.m_AllQuestStates]*/
	public void ParseAllQuestStates(QuestManager manager)
	{
		this.m_AllQuestStates.Clear ();
		//for each quest...
		foreach (Quest quest in manager.m_AllQuests) {
			//...Add the current state
			this.m_AllQuestStates.Add ((int)quest.m_QuestState);
		}//end foreach
	}//end f'n void ParseAllQuestStates(QuestManager)

	/**Set all the quest states from the saved information*/
	public void SetAllQuestStates(QuestManager manager)
	{

		//for each quest...
		for (int index = 0; index < manager.m_AllQuests.Count; index++) {
			switch (this.m_AllQuestStates [index]) {
			case (int)QuestState.COMPLETED:
				{
					manager.m_AllQuests [index].m_QuestState = QuestState.COMPLETED;
					break;
				}//end case completed
			case (int)QuestState.IN_PROCESS:
				{
					manager.m_AllQuests [index].m_QuestState = QuestState.IN_PROCESS;
					break;
				}//end case in process
			case (int)QuestState.NOT_YET_GIVEN:
				{
					manager.m_AllQuests [index].m_QuestState = QuestState.NOT_YET_GIVEN;
					break;
				}//end case not yet given
			default:
				{
					//Impossible
					break;
				}
			}//end switch
		}//end for
	}//end f'n void SetAllQuestStates(QuestManager)

//	public void SpawnInQuestObjects(QuestManager manager)
//	{
//		foreach (Quest quest in manager.m_AllQuests) {
//			if (quest.m_QuestState == QuestState.IN_PROCESS) {
//				quest.SpawnInQuestObjects ();
//			}//end if
//		}//end foreach
//	}


//	public void SetAllEnemiesRemaining(QuestManager manager)
//	{
//		/*
//		* So if we're caring about enemies remaining, it's that the quest is in process and a Kill Everything.
//		* This necessarily means that when the player loads, at least all targets are spawned in.
//		* We just need to remove elements from the m_Targets list
//		*/
//		int last_quest_index = 0;
//		for (int index = 0; index < this.m_KillEverything_EnemiesRemaining.Count; index++) {
//			for (last_quest_index; last_quest_index < manager.m_AllQuests.Count; last_quest_index++) {
//				if (manager.m_AllQuests [last_quest_index].m_QuestState == QuestState.IN_PROCESS) {
//					if (manager.m_AllQuests [last_quest_index].m_QuestType == QuestType.KILL_EVERYTHING) {
//						int targets_to_remove = manager.m_AllQuests [last_quest_index].m_NumberOfEnemiesToKill - this.m_KillEverything_EnemiesRemaining [index];
//
//						for (int target = 0; target < targets_to_remove; target++) {
//							KillEverything kill_everything_member = manager.m_AllQuests [last_quest_index].m_KillEverything;
//							kill_everything_member.m_Targets.Remove (kill_everything_member.m_Targets [0]);
//						}
//					}
//				}
//
//			}
//		}
//	}

//	/**A function to get all enemies remaining for all KillEverything quests.*/
//	public void ParseAllEnemiesRemaining(QuestManager manager)
//	{
//		//for each quest...
//		foreach (Quest quest in manager.m_AllQuests) {
//			//only if the quest is in process...
//			if (quest.m_QuestState == QuestState.IN_PROCESS) {
//				//if the quest is a Kill Everything...
//				if (quest.m_KillEverything != null) {
//					int enemies_killed = 0;
//					//for every enemy in the target list...
//					foreach (Enemy enemy in quest.m_KillEverything.m_Targets) {
//						//...if the enemy is null
//						if (enemy == null) {
//							//...then we already killed it
//							enemies_killed++;
//						}//end if
//					}//end foreach
//					int enemies_remaining = quest.m_NumberOfEnemiesToKill - enemies_killed;
//					this.m_KillEverything_EnemiesRemaining.Add (enemies_remaining);
//				}//end if
//			}//end if
//		}//end foreach
//	}//end 
}//end class Serializable_QuestManager
