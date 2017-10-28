using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	[SerializeField] public Spawner m_Spawner;

	/*		For quest Rooster Bane: 	*/

	/**The default rooster prefab for the enemies we'll spawn once the quest is given to the player.*/
	[SerializeField] public QuestGiver m_QuestGiver_RoosterBane;

	[SerializeField] public GameObject m_DefautRoosterPrefab;
	public int NUMBER_ENEMIES_ROOSTERBANE = 5;


	/**A list of all the quests in the game.*/
	public List<Quest> m_AllQuests = new List<Quest>();
	/**A list of all the quest-givers in the game.*/
	public List<QuestGiver> m_AllQuestGivers = new List<QuestGiver> ();

	// Use this for initialization
	void Awake () {
		this.InitializeAllQuests ();
		this.AssignAllQuests ();
	}

	void Start()
	{
		
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
		rooster_bane.m_QuestName = QuestName.ROOSTER_BANE;
		rooster_bane.m_QuestNameString = "Rooster Bane!";
		rooster_bane.m_QuestState = QuestState.NOT_YET_GIVEN;
		rooster_bane.m_QuestType = QuestType.KILL_EVERYTHING;

		rooster_bane.InitializeObjectiveType();
		rooster_bane.m_RequisitePrefabs.Add (this.m_DefautRoosterPrefab);
		rooster_bane.m_NumberOfEnemiesToKill = 5;
		rooster_bane.SetQuestGoalLocation (new Vector3 (15.74f, 0.55f, -0.43f));
		rooster_bane.m_KillEverything.m_EnemyLootSpawner = this.m_Spawner;

		this.m_AllQuests.Add (rooster_bane);


	}//end f'n void InitializeAllQuests()

	/**A function to provide each quest giver with their respective quest.*/
	private void AssignAllQuests()
	{
		//for every quest...
		foreach (Quest quest in this.m_AllQuests) {
			
			switch ((int)quest.m_QuestName) {
			case (int)QuestName.ROOSTER_BANE:
				{
					//Assign rooster bane quest to quest giver
					this.m_QuestGiver_RoosterBane.m_QuestToGive = quest;
					break;
				}//end case Rooster Bane
			default:
				{
					//Impossible
					break;
				}//end case default
			}//end switch
		}//end foreach
	}//end f'n void AssignAllQuests()

	/**A function to keep track of whether or not quest objectives are completed; this will only apply to quests whose states are IN_PROCESS*/
	private void UpdateActiveQuestObjectives()
	{
		//for every quest...
		foreach (Quest quest in this.m_AllQuests) {
			//...if the quest is in process...
			if (quest.m_QuestState == QuestState.IN_PROCESS) {
				//...then check to see if it's fulfilled the conditions for completion
				if (quest.CheckQuestObjectiveCompleted ()) {
					//...if so, then update the quest status
					this.UpdateQuestState (quest.m_QuestName, QuestState.COMPLETED);
				}//end if
			}//end if
		}//end foreach
	}//end f'n void UpdateActiveQuestObjectives()

	/**Update quest [quest_name] to have state [quest_state].*/
	public void UpdateQuestState(QuestName quest_name, QuestState quest_state)
	{
		this.m_AllQuests [(int)quest_name].m_QuestState = quest_state;
	}
}
