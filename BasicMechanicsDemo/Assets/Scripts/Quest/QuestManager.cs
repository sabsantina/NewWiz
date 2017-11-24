using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    /**A reference to the spawner; we need to still be able to spawn items from quest enemy corpses.*/
    [SerializeField] public Spawner m_Spawner;

    /**A reference to the player. We need to check the player's inventory for quest items.*/
    [SerializeField] public Player m_Player;

    /*		For quest Rooster Bane: 	*/

    /**The NPC responsible for giving the quest to the player*/
    [SerializeField] public QuestGiver m_QuestGiver_RoosterBane;

    /**The default rooster prefab for the enemies we'll spawn once the quest is given to the player.*/
    [SerializeField] public GameObject m_DefautRoosterPrefab;

    /**The number of quest enemies to kill*/
    public int NUMBER_ENEMIES_ROOSTERBANE = 5;


    /*		For quest Potion Master		*/

    /**The NPC responsible for giving the quest to the player*/
    [SerializeField] public QuestGiver m_QuestGiver_PotionMaster;

    /**The default potion prefab for the potions we'll spawn in once the quest is given to the player.*/
    [SerializeField] public GameObject m_DefaultPotionQuestItemPrefab;

    /**The number of quest items to fetch*/
    public int NUMBER_ITEMS_POTIONMASTER = 2;

    /*		For quest Hot Chicks		*/
    [SerializeField] public QuestGiver m_QuestGiver_HotChicks;

    /**The default rooster prefab for the enemies we'll spawn once the quest is given to the player.*/
    [SerializeField] public GameObject m_FireChicken;

    /**The number of quest enemies to kill*/
    public int NUMBER_ENEMIES_HOTCHICKS = 3;


    /**/

    /**A list of all the quests in the game.*/
    public List<Quest> m_AllQuests = new List<Quest>();

    /**A list of all the quest-givers in the game. We need this for serialization.*/
    public List<QuestGiver> m_AllQuestGivers = new List<QuestGiver>();

    // Use this for initialization
    void Awake()
    {
        this.InitializeAllQuests();
        this.AssignAllQuests();
    }

    void Start()
    {
    }

    void Update()
    {
        this.UpdateActiveQuestObjectives();
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
        rooster_bane.m_RequisitePrefabs.Add(this.m_DefautRoosterPrefab);
        rooster_bane.m_NumberOfEnemiesToKill = NUMBER_ENEMIES_ROOSTERBANE;
        rooster_bane.SetQuestGoalLocation(new Vector3(15.74f, 0.55f, -0.43f));
        rooster_bane.m_KillEverything.m_EnemyLootSpawner = this.m_Spawner;

        this.m_QuestGiver_RoosterBane.m_PlayerInventory = this.m_Player.GetComponent<PlayerInventory>();

        //Set reward item (as an example)
        ItemClass health_potion = new ItemClass();
        health_potion.GenerateInstance(ItemName.Health_Potion);
        this.m_QuestGiver_RoosterBane.m_RewardItem = health_potion;

        //Set reward spell (as an example)
        SpellClass iceball = new SpellClass();
        iceball = iceball.GenerateInstance(SpellName.Iceball);
        this.m_QuestGiver_RoosterBane.m_RewardSpell = iceball;

        //Potion master
        Quest potion_master = new Quest();
        potion_master.m_QuestName = QuestName.POTION_MASTER;
        potion_master.m_QuestNameString = "Potion Master!";
        potion_master.m_QuestState = QuestState.NOT_YET_GIVEN;
        potion_master.m_QuestType = QuestType.FETCH;

        potion_master.InitializeObjectiveType();
        potion_master.m_RequisitePrefabs.Add(this.m_DefaultPotionQuestItemPrefab);
        QuestItem potion = new QuestItem();
        potion.GenerateQuestItem(QuestItemName.POTION_OF_WISDOM);
        potion_master.m_ItemInformation = potion;
        potion_master.m_Fetch.m_PlayerInventory = this.m_Player.GetComponent<PlayerInventory>();
        potion_master.m_NumberOfItemsToFind = NUMBER_ITEMS_POTIONMASTER;
        potion_master.SetQuestGoalLocation(new Vector3(15.74f, 0.55f, -0.43f));
//		potion_master.m_KillEverything.m_EnemyLootSpawner = this.m_Spawner;

        this.m_QuestGiver_PotionMaster.m_PlayerInventory = this.m_Player.GetComponent<PlayerInventory>();


        //Set reward item (as an example)
//		ItemClass health_potion = new ItemClass();
        health_potion.GenerateInstance(ItemName.Health_Potion);
        this.m_QuestGiver_PotionMaster.m_RewardItem = health_potion;

        //Set reward spell (as an example)
//		SpellClass iceball = new SpellClass();
        iceball = iceball.GenerateInstance(SpellName.Iceball);
        this.m_QuestGiver_PotionMaster.m_RewardSpell = iceball;


        //Hot Chicks
        Quest hot_chicks = new Quest();
        hot_chicks.m_QuestName = QuestName.HOT_CHICKS;
        hot_chicks.m_QuestNameString = "Hot Chicks!";
        hot_chicks.m_QuestState = QuestState.NOT_YET_GIVEN;
        hot_chicks.m_QuestType = QuestType.KILL_EVERYTHING;

        hot_chicks.InitializeObjectiveType();
        hot_chicks.m_RequisitePrefabs.Add(m_FireChicken);
        hot_chicks.m_NumberOfEnemiesToKill = NUMBER_ENEMIES_HOTCHICKS;
        hot_chicks.SetQuestGoalLocation(new Vector3(5.74f, 0.55f, 2f));
        hot_chicks.m_KillEverything.m_EnemyLootSpawner = m_Spawner;

		this.m_QuestGiver_HotChicks.m_PlayerInventory = this.m_Player.GetComponent<PlayerInventory>();

        //Set reward item
        ItemClass mana_potion = new ItemClass();
        mana_potion.GenerateInstance(ItemName.Mana_Potion);
        m_QuestGiver_HotChicks.m_RewardItem = mana_potion;

		//Add all quests
        
        this.m_AllQuests.Add(rooster_bane);
        this.m_AllQuests.Add(potion_master);
        this.m_AllQuests.Add(hot_chicks);
    } //end f'n void InitializeAllQuests()

    /**A function to provide each quest giver with their respective quest.*/
    private void AssignAllQuests()
    {
        //for every quest...
        foreach (Quest quest in this.m_AllQuests)
        {
            switch ((int) quest.m_QuestName)
            {
                case (int) QuestName.ROOSTER_BANE:
                {
                    //Assign rooster bane quest to quest giver
                    this.m_QuestGiver_RoosterBane.m_QuestToGive = quest;

                    break;
                } //end case Rooster Bane
                case (int) QuestName.POTION_MASTER:
                {
                    //Assign rooster bane quest to quest giver
                    this.m_QuestGiver_PotionMaster.m_QuestToGive = quest;

                    break;
                } //end case Rooster Bane
                case(int) QuestName.HOT_CHICKS:
                {
                    this.m_QuestGiver_HotChicks.m_QuestToGive = quest;
                    break;
                }
                //default:
                //	{
                //Impossible
                //		break;
                //	}//end case default
            } //end switch
        } //end foreach
    } //end f'n void AssignAllQuests()

    /**A function to keep track of whether or not quest objectives are completed; this will only apply to quests whose states are IN_PROCESS*/
    private void UpdateActiveQuestObjectives()
    {
        //for every quest...
        foreach (Quest quest in this.m_AllQuests)
        {
            //...if the quest is in process...
            if (quest.m_QuestState == QuestState.IN_PROCESS)
            {
                //...then check to see if it's fulfilled the conditions for completion
                if (quest.CheckQuestObjectiveCompleted())
                {
                    //...if so, then update the quest status
                    this.UpdateQuestState(quest.m_QuestName, QuestState.COMPLETED);
                } //end if
            } //end if
        } //end foreach
    } //end f'n void UpdateActiveQuestObjectives()

    /**Update quest [quest_name] to have state [quest_state].*/
    public void UpdateQuestState(QuestName quest_name, QuestState quest_state)
    {
        this.m_AllQuests[(int) quest_name].m_QuestState = quest_state;
    }
}