using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public List<Quest> m_AllQuests;

	[SerializeField] List<GameObject> enemy_list;

	// Use this for initialization
	void Start () {
		this.InitializeAllQuests ();
	}

	void Update()
	{
		this.UpdateActiveQuestObjectives ();
	}

	/**A function to set all quests to their NOT_YET_GIVEN states and all that entails (quest objectives, and the like).*/
	private void InitializeAllQuests()
	{
		//Rooster bane
		Quest rooster_bane = new Quest();
		rooster_bane.m_QuestName = "Rooster Bane!";
		rooster_bane.m_QuestState = QuestState.NOT_YET_GIVEN;
		KillEverything kill_everything_objective = new KillEverything ();
//		kill_everything_objective.m_Targets = new List
		rooster_bane.m_QuestObjective = new KillEverything ();
//		rooster_bane.m_QuestObjective.
	}//end f'n void InitializeAllQuests()

	/**A function to keep track of whether or not quest objectives are completed; this will only apply to quests whose states are IN_PROCESS*/
	private void UpdateActiveQuestObjectives()
	{
		foreach (Quest quest in this.m_AllQuests) {
			if (quest.m_QuestState == QuestState.IN_PROCESS) {
				quest.m_QuestObjective.CheckForObjectiveIsMet ();
			}
		}
	}
}
