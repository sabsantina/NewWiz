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

	/**An int list to contain the quest state of the quest state at the given list index.
	*This includes even quests not yet encountered by the player.*/
	public List<int> m_AllQuestStates = new List<int>();

	public List<bool> m_AllRewardsGiven = new List<bool>();

	/*
	* Note: If the player saves a game after having killed x enemies for a quest (where x is less than the total number of enemies to kill
	* in order for that quest's "condition" or task to be fulfilled), then the enemies will respawn on load and the player's progress towards 
	* the quest will be lost.
	* By the same token, items picked up will be lost unless the player first brings them where they're supposed to go (that might be problematic)
	* Yo, I was wrong! As it happens, I set it up such that once all quest objectives are completed, the quest automatically updates its state.
	* So basically once you kill all the enemies, even without speaking to the quest giver and completing the quest in that way, they'll stay dead.
	* Same deal for items picked up!
	*/

	public void ParseAllQuestRewardsGiven(QuestManager manager)
	{
		this.m_AllRewardsGiven.Clear ();
		//for each quest giver...
		for (int quest_index = 0; quest_index < manager.m_AllQuests.Count; quest_index++) {
			this.m_AllRewardsGiven.Add(manager.m_AllQuestGivers[quest_index].m_RewardHasBeenGiven);
		}
	}

	public void SetAllQuestRewardsGiven(QuestManager manager)
	{
		//for each quest giver...
		for (int quest_index = 0; quest_index < manager.m_AllQuests.Count; quest_index++) {
			manager.m_AllQuestGivers [quest_index].m_RewardHasBeenGiven = this.m_AllRewardsGiven [quest_index];
		}
	}

	/**A function to record all quest states and record them to [this.m_AllQuestStates]*/
	public void ParseAllQuestStates(QuestManager manager)
	{
		this.m_AllQuestStates.Clear ();
		//for each quest...
		foreach (KeyValuePair<QuestName, Quest> kvp in manager.m_AllQuests) {
			//...Add the current state
			this.m_AllQuestStates.Add ((int)kvp.Value.m_QuestState);
		}//end foreach
	}//end f'n void ParseAllQuestStates(QuestManager)

	/**Set all the quest states from the saved information*/
	public void SetAllQuestStates(QuestManager manager)
	{
		int index = 0;
		//for each quest...
		foreach (KeyValuePair<QuestName, Quest> kvp in manager.m_AllQuests) {
			switch (this.m_AllQuestStates [index++]) {
			case (int)QuestState.COMPLETED:
				{
					manager.m_AllQuests [kvp.Key].m_QuestState = QuestState.COMPLETED;
					break;
				}//end case completed
			case (int)QuestState.IN_PROCESS:
				{
					manager.m_AllQuests [kvp.Key].m_QuestState = QuestState.IN_PROCESS;
					break;
				}//end case in process
			case (int)QuestState.NOT_YET_GIVEN:
				{
					manager.m_AllQuests [kvp.Key].m_QuestState = QuestState.NOT_YET_GIVEN;
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
