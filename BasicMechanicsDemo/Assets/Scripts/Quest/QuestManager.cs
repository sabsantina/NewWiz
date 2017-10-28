using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	/*		For quest Rooster Bane: 	*/

	/**The default rooster prefab for the enemies we'll spawn once the quest is given to the player.*/
	[SerializeField] public GameObject m_DefautRoosterPrefab;
	public int NUMBER_ENEMIES_ROOSTERBANE = 5;



	public List<Quest> m_AllQuests = new List<Quest>();

	// Use this for initialization
	void Awake () {
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
		rooster_bane.m_QuestType = QuestType.KILL_EVERYTHING;

		rooster_bane.InitializeObjectiveType();
		rooster_bane.m_RequisitePrefabs.Add (this.m_DefautRoosterPrefab);
		rooster_bane.SetQuestGoalLocation (new Vector3 (15.74f, 0.55f, -0.43f));


		this.m_AllQuests.Add (rooster_bane);
//		rooster_bane.m_QuestObjective
	}//end f'n void InitializeAllQuests()

	/**A function to keep track of whether or not quest objectives are completed; this will only apply to quests whose states are IN_PROCESS*/
	private void UpdateActiveQuestObjectives()
	{
		foreach (Quest quest in this.m_AllQuests) {
			if (quest.m_QuestState == QuestState.IN_PROCESS) {
//				quest.m_QuestObjective.CheckForObjectiveIsMet ();
			}
		}
	}

	/**Update quest [quest_name] to have state [quest_state].*/
	public void UpdateQuestState(QuestName quest_name, QuestState quest_state)
	{
		this.m_AllQuests [(int)quest_name].m_QuestState = quest_state;
	}
}
