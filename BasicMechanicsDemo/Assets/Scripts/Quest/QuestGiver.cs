using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Interactable {
	[SerializeField] QuestManager m_QuestManager;
	/**The actual quest the NPC gives the player.*/
	public Quest m_QuestToGive;
	/**The item reward, if any*/
	public ItemClass m_RewardItem;
	/**The spell reward, if any*/
	public SpellClass m_RewardSpell;
	/**A reference to the player inventory*/
	public PlayerInventory m_PlayerInventory;
	/**A bool to let us know whether the player's been rewarded.*/
	public bool m_RewardHasBeenGiven = false;
	/**A bool to let us know whether the player's quest items has been reclaimed.*/
	public bool m_QuestItemsReclaimedFromPlayer = false;

    /**A function to return the quest dialog, with respect to quest type and quest state*/
    public override string ReturnDialog ()
	{
		//If the quest hasn't yet been given to the player...
		if (this.m_QuestToGive.m_QuestState == QuestState.NOT_YET_GIVEN) {

			//return input based on quest type

			string message = "";

			//if kill everything...

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

			//else if fetch...
			else if (this.m_QuestToGive.m_QuestType == QuestType.FETCH) {
				//Fetch ...
				message += "Fetch ";
				//...x number ...
				message += this.m_QuestToGive.m_NumberOfItemsToFind + " ";
				//... of [ITEM NAME]
				for (int index = 0; index < this.m_QuestToGive.m_RequisitePrefabs.Count; index++) {
					//if we're at the last of the item types to find...
					if (index == this.m_QuestToGive.m_RequisitePrefabs.Count - 1) {
						//...then just output the name and an "s" to make it plural, with no "and"
						message += m_QuestToGive.m_RequisitePrefabs [index].name + "s";
					}//end if
					//else if we're not at the last of the item types to find...
					else {
						//...then output the enemy name with an "s" and an "and," for the sentence to make sense.
						message += m_QuestToGive.m_RequisitePrefabs [index].name + "s and ";
					}//end else
				}//end for
			}//end else if

			//After the quest has been given, spawn in the quest objects
//			this.m_QuestManager.m_AllQuests[(int)this.m_QuestToGive.m_QuestName].SpawnInQuestObjects();
			this.m_QuestManager.SpawnInQuestObjects(this.m_QuestToGive);

			//Then we know that the quest's been given now, so update the status to check for completion
			this.m_QuestManager.UpdateQuestState (this.m_QuestToGive.m_QuestName, QuestState.IN_PROCESS);

			return message;
		}//end if quest state is not yet given

		//else if the quest has already been given and is IN PROCESS...
		else if (this.m_QuestToGive.m_QuestState == QuestState.IN_PROCESS) {

			//if kill everything...
			string message = "";
			if (this.m_QuestToGive.m_QuestType == QuestType.KILL_EVERYTHING) {
				//...then just tell the player to go kill the critters
				message += "Go on! Go get those critters!";
			}//end if

			//else if fetch...
			else if (this.m_QuestToGive.m_QuestType == QuestType.FETCH) {
				//...then just tell the player to go fetch the stuff
				message += "You got the goods?! No?! Go, then!";
			}//end else if

			return message;
		}//end if quest state is in process
		//else if the quest has already been given and is IN PROCESS...
		else if (this.m_QuestToGive.m_QuestState == QuestState.COMPLETED) {

			//if kill everything...
			string message = "";
			if (this.m_QuestToGive.m_QuestType == QuestType.KILL_EVERYTHING) {
				//Kill ...
				message += "And so, the kingdom will finally be rid of those ";
				//...x number ...
				message += this.m_QuestToGive.m_NumberOfEnemiesToKill + " ";
				//...of [ENEMY NAME]
				for (int index = 0; index < this.m_QuestToGive.m_RequisitePrefabs.Count; index++) {
					//if we're at the last of the enemy types to kill...
					if (index == this.m_QuestToGive.m_RequisitePrefabs.Count - 1) {
						//...then just output the name and an "s" to make it plural, with no "and"
						message += " pestering " + m_QuestToGive.m_RequisitePrefabs [index].name + "s";
					}//end if
					//else if we're not at the last of the enemy types to kill...
					else {
						//...then output the enemy name with an "s" and an "and," for the sentence to make sense.
						message += m_QuestToGive.m_RequisitePrefabs [index].name + "s and ";
					}//end else
				}//end for
			}//end if

			//else if fetch...
			else if (this.m_QuestToGive.m_QuestType == QuestType.FETCH) {
				//Fetch ...
				message += "Finally, my ";
				//...x number ...
				message += this.m_QuestToGive.m_NumberOfItemsToFind + " ";
				//...of [ITEM NAME]
				for (int index = 0; index < this.m_QuestToGive.m_RequisitePrefabs.Count; index++) {
					//if we're at the last of the enemy types to kill...
					if (index == this.m_QuestToGive.m_RequisitePrefabs.Count - 1) {
						//...then just output the name and an "s" to make it plural, with no "and"
						message += " precious " + m_QuestToGive.m_RequisitePrefabs [index].name + "s!";
					}//end if
					//else if we're not at the last of the enemy types to kill...
					else {
						//...then output the enemy name with an "s" and an "and," for the sentence to make sense.
						message += m_QuestToGive.m_RequisitePrefabs [index].name + "s and ";
					}//end else
				}//end for
			}//end else if

			//if the reward's not yet been given...
			if (!this.m_RewardHasBeenGiven) {
				//...then reward the player
				this.RewardPlayer ();
			}//end if

			//if this is a fetch quest and the player hasn't had the quest items removed from their inventory yet...
			if (this.m_QuestToGive.m_QuestType == QuestType.FETCH
			    && !this.m_QuestItemsReclaimedFromPlayer) {

				message += "\n" + "I'll just take those...";

				for (int index = 0; index < this.m_QuestToGive.m_NumberOfItemsToFind; index++) {
					this.m_PlayerInventory.m_QuestItems.Remove (this.m_QuestToGive.m_ItemInformation);
				}//end for

				//Update var
				this.m_QuestItemsReclaimedFromPlayer = true;
			}//end if

			return message;
		}//end if quest state is in process

		//Impossible for us to reach this point
		return "";
	}//end f'n string ReturnDialog()

	public void RewardPlayer()
	{
		if (this.m_RewardItem != null) {
			this.m_PlayerInventory.AddItem (this.m_RewardItem);
		}
		if (this.m_RewardSpell != null) {
			this.m_PlayerInventory.AddSpell (this.m_RewardSpell);
		}
		this.m_RewardHasBeenGiven = true;
	}

}
