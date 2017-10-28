using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Interactable {
	[SerializeField] QuestManager m_QuestManager;

	public Quest m_QuestToGive;

	/**A function to return the quest dialog, with respect to quest type and quest state*/
	public override string ReturnDialog ()
	{
		//If the quest hasn't yet been given to the player...
		if (this.m_QuestToGive.m_QuestState == QuestState.NOT_YET_GIVEN) {
			//...then we know that it's been given now, so update the status
			this.m_QuestManager.UpdateQuestState (this.m_QuestToGive.m_QuestName, QuestState.IN_PROCESS);
			//and return input based on quest type

			string message = "";

			if (this.m_QuestToGive.m_QuestType == QuestType.KILL_EVERYTHING) {
				//Kill ...
				message += "Kill ";
				//...x number ...
				message += this.m_QuestToGive.m_NumberOfEnemiesToKill + " ";
				//...of [ENEMY NAME]
				for (int index = 0; index < this.m_QuestToGive.m_RequisitePrefabs.Count; index++) {
					//if we're at the last of the enemy types to kill...
					if (index == this.m_QuestToGive.m_RequisitePrefabs.Count - 1) {
						//...then just output the name and an "s" to make it plural, with no "and"
						message += m_QuestToGive.m_RequisitePrefabs [index].name + "s";
					}//end if
					//else if we're not at the last of the enemy types to kill...
					else {
						//...then output the enemy name with an "s" and an "and," for the sentence to make sense.
						message += m_QuestToGive.m_RequisitePrefabs [index].name + "s and ";
					}//end else
				}//end for
			}//end if

			return message;
		}//end if

		return "";
	}//end f'n string ReturnDialog()


}
