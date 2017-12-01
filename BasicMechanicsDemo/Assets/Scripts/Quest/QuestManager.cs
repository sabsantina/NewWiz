using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    /**A reference to the spawner; we need to still be able to spawn items from quest enemy corpses.*/
    [SerializeField] public Spawner m_Spawner;

    /**A reference to the player. We need to check the player's inventory for quest items.*/
    [SerializeField] public Player m_Player;

    /*		For quest Rooster Bane: 	*/

    /**The NPC responsible for giving the quest to the player*/

    /**The default rooster prefab for the enemies we'll spawn once the quest is given to the player.*/
    [SerializeField] public GameObject m_DefautRoosterPrefab;

    /**The number of quest enemies to kill*/
    public int NUMBER_ENEMIES_ROOSTERBANE = 5;

    /*		For quest Potion Master		*/

    /**The default potion prefab for the potions we'll spawn in once the quest is given to the player.*/
    [SerializeField] public GameObject m_DefaultPotionQuestItemPrefab;

    /**The number of quest items to fetch*/
    public int NUMBER_ITEMS_POTIONMASTER = 2;

    /*		For quest Hot Chicks		*/

    /**The default rooster prefab for the enemies we'll spawn once the quest is given to the player.*/
    [SerializeField] public GameObject m_FireChicken;

    /**The number of quest enemies to kill*/
    public int NUMBER_ENEMIES_HOTCHICKS = 3;

    /*	For quest Double Trouble	*/

    [SerializeField] public GameObject m_ArmoredSoldierPrefab;

    public int NUMBER_ENEMIES_DOUBLETROUBLE = 10;

    [SerializeField] public GameObject m_QuestGiver_Generic;

    [SerializeField] public GameObject m_Leoghaire;

    [SerializeField] public Image DarkScreen;

    //A list of cutscene boolean markers. each index is the number of the cutscene in MainGame.txt
    [SerializeField] public List<bool> CutscenesDone = new List<bool>();

    /**A list of all the quests in the game.*/
    [SerializeField] public Dictionary<QuestName, Quest> m_AllQuests = new Dictionary<QuestName, Quest>();

    /**A list of all the quest-givers in the game. We need this for serialization.*/
    [SerializeField] public Dictionary<QuestName, QuestGiver> m_AllQuestGivers = new Dictionary<QuestName, QuestGiver>();

    // Use this for initialization
    private void Awake()
    {
        //		Debug.Log ("QuestManager::Awake::Called every scene");
        //		if (UnityEngine.PlayerPrefs.GetInt (MainMenu_UIManager.STRINGKEY_PLAYERPREF_LOADGAME) != 0) {
        //			Debug.Log ("QuestManager::Awake::Called only once");
        //			this.InitializeAllQuests();
        //			this.AssignAllQuests();
        //		}

        //Called every time we load a scene
        this.InitializeAllQuests();
        this.AssignAllQuests();
        this.ManageQuestGiversInScene();
    }

    private void Start()
    {
        int active_scene_index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        Scenes current_scene = Player.ReturnSceneAtIndex(active_scene_index);
        switch (current_scene)
        {
            case Scenes.FOREST:
                {
                    if (!CutscenesDone[0])
                    {
                        m_Player.transform.position = new Vector3(-28.7f, 0.55f, 2.51f);
                        DarkScreen.color = new Color(0, 0, 0, 1);
                    }
                    if (CutscenesDone[9])
                        Destroy(m_Leoghaire.gameObject);
                    break;
                }
            case Scenes.SOIRHBEACH:
                {
                    if (CutscenesDone[14])
                        Destroy(m_Leoghaire.gameObject);
                    break;
                }
        }
    }

    private void Update()
    {
        this.UpdateActiveQuestObjectives();
    }

    private void ManageQuestGiversInScene()
    {
        int active_scene_index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        Scenes current_scene = Player.ReturnSceneAtIndex(active_scene_index);

        foreach (KeyValuePair<QuestName, QuestGiver> kvp in this.m_AllQuestGivers)
        {
            QuestGiver quest_giver = kvp.Value;
            if (quest_giver.m_QuestToGive.m_QuestRegion != current_scene)
            {
                quest_giver.gameObject.SetActive(false);
            }
            else
            {
                quest_giver.gameObject.SetActive(true);
            }
        }
    }

    /**A function to set all quests to their NOT_YET_GIVEN states and all that entails (quest objectives, and the like).*/

    private void InitializeAllQuests()
    {
        //Rooster bane
        Quest roosterBane = GenerateKillEverything(QuestName.ROOSTER_BANE, "Rooster Bane!", NUMBER_ENEMIES_ROOSTERBANE, Scenes.OVERWORLD,
            new Vector3(15.74f, 0.55f, -0.43f), m_DefautRoosterPrefab);
        QuestGiver qgRoosterBane = GenerateQuestGiver(new Vector3(-13.39f, 0.55f, -0.43f), null, new[] { SpellName.Shield });
        Add(ref roosterBane, ref qgRoosterBane);

        //Potion master
        Quest potionMaster = GenerateFetch(QuestName.POTION_MASTER, "Potion Master!", NUMBER_ITEMS_POTIONMASTER, QuestItemName.POTION_OF_WISDOM,
            Scenes.OVERWORLD, new Vector3(15.74f, 0.55f, -0.43f), m_DefaultPotionQuestItemPrefab);
        QuestGiver qgPotionMaster =
            GenerateQuestGiver(new Vector3(-6.718053f, 0.55f, -11.17016f), new[] { ItemName.Health_Potion }, new[] { SpellName.Iceball });
        Add(ref potionMaster, ref qgPotionMaster);

        //Hot Chicks
        Quest hotChicks = GenerateKillEverything(QuestName.HOT_CHICKS, "Hot Chicks!", NUMBER_ENEMIES_HOTCHICKS,
            Scenes.OVERWORLD, new Vector3(5.74f, 0.55f, 2f), m_FireChicken);
        QuestGiver qgHotChicks = GenerateQuestGiver(new Vector3(-15.96f, 0.55f, -14.08f), new[] { ItemName.Mana_Potion }, new[] { SpellName.Thunderball });
        Add(ref hotChicks, ref qgHotChicks);

        Quest doubleTrouble = GenerateKillEverything(QuestName.DOUBLE_TROUBLE, "Double trouble!", NUMBER_ENEMIES_DOUBLETROUBLE, Scenes.OVERWORLD,
                                  new Vector3(5.74f * 2.0f, 0.55f, 2.0f * 2.0f), this.m_ArmoredSoldierPrefab);
        QuestGiver qgDoubleTrouble = GenerateQuestGiver(new Vector3(-15.96f * 2.0f, 0.55f, -14.08f * 2.0f), new[] { ItemName.Health_Potion }, new[] { SpellName.Tornado });
        Add(ref doubleTrouble, ref qgDoubleTrouble);

        //		this.ManageQuestGiversInScene ();
    } //end f'n void InitializeAllQuests()

    /**A function to provide each quest giver with their respective quest.*/

    private void Add(ref Quest q, ref QuestGiver qg)
    {
        m_AllQuests.Add(q.m_QuestName, q);
        m_AllQuestGivers.Add(q.m_QuestName, qg);

        //Name the generic quest givers with respect to their assigned quest to more easily tell them apart in testing
        string default_name = qg.gameObject.name;
        qg.gameObject.name = default_name + q.m_QuestName.ToString();
    }

    private Quest GenerateKillEverything(QuestName questName, String nameString, int nbEnemies, Scenes region, Vector3 goalLocation,
        params GameObject[] requisitePrefabs)
    {
        Quest quest = GenerateQuest(questName, nameString, QuestType.KILL_EVERYTHING, requisitePrefabs.ToList(), region, goalLocation);
        quest.m_NumberOfEnemiesToKill = nbEnemies;
        quest.m_KillEverything.m_EnemyLootSpawner = this.m_Spawner;
        return quest;
    }

    private Quest GenerateFetch(QuestName questName, String nameString, int nbItems, QuestItemName questItemName, Scenes region, Vector3 goalLocation,
        params GameObject[] requisitePrefabs)
    {
        Quest quest = GenerateQuest(questName, nameString, QuestType.FETCH, requisitePrefabs.ToList(), region, goalLocation);
        QuestItem potion = new QuestItem();
        potion.GenerateQuestItem(questItemName);
        quest.m_ItemInformation = potion;
        quest.m_Fetch.m_PlayerInventory = this.m_Player.GetComponent<PlayerInventory>();
        quest.m_NumberOfItemsToFind = nbItems;

        return quest;
    }

    private Quest GenerateQuest(QuestName questName, String nameString, QuestType questType, List<GameObject> requisitePrefabs, Scenes region,
        Vector3 goalLocation)
    {
        Quest quest = new Quest();
        quest.m_QuestName = questName;
        quest.m_QuestNameString = nameString;
        quest.m_QuestState = QuestState.NOT_YET_GIVEN;
        quest.m_QuestType = questType;
        quest.InitializeObjectiveType();
        quest.m_RequisitePrefabs = requisitePrefabs;
        quest.m_QuestRegion = region;
        quest.SetQuestGoalLocation(goalLocation);

        return quest;
    }

    private QuestGiver GenerateQuestGiver(Vector3 position, ItemName[] itemRewards, SpellName[] spellRewards)
    {
        QuestGiver qg = Instantiate(m_QuestGiver_Generic).GetComponent<QuestGiver>();
        qg.m_QuestManager = this;
        qg.m_PlayerInventory = this.m_Player.GetComponent<PlayerInventory>();
        qg.transform.position = position;
        if (itemRewards != null)
            foreach (ItemName reward in itemRewards)
            {
                ItemClass thisReward = new ItemClass();
                thisReward.GenerateInstance(reward);
                qg.m_RewardItem = thisReward;
            }
        if (spellRewards != null)
            foreach (var reward in spellRewards)
            {
                SpellClass thisReward = new SpellClass();
                thisReward = thisReward.GenerateInstance(reward);
                print(reward);
                print(thisReward.ReturnSpellInstanceInfo());
                qg.m_RewardSpell = thisReward;
            }
        return qg;
    }

    private void AssignAllQuests()
    {
        //for every quest...
        foreach (KeyValuePair<QuestName, Quest> kvp in m_AllQuests)
        {
            print(kvp);
            this.m_AllQuestGivers[kvp.Key].m_QuestToGive = m_AllQuests[kvp.Key];
        } //end foreach
    } //end f'n void AssignAllQuests()

    /**A function to keep track of whether or not quest objectives are completed; this will only apply to quests whose states are IN_PROCESS*/

    private void UpdateActiveQuestObjectives()
    {
        //for every quest...
        foreach (KeyValuePair<QuestName, Quest> kvp in m_AllQuests)
        {
            //...if the quest is in process and we're in the region for that quest...
            if (kvp.Value.m_QuestState == QuestState.IN_PROCESS && Player.m_CurrentRegion == kvp.Value.m_QuestRegion)
            {
                //...then check to see if it's fulfilled the conditions for completion
                if (kvp.Value.CheckQuestObjectiveCompleted())
                {
                    //...if so, then update the quest status
                    this.UpdateQuestState(m_AllQuests[kvp.Key].m_QuestName, QuestState.COMPLETED);
                } //end if
            } //end if
        } //end foreach
    } //end f'n void UpdateActiveQuestObjectives()

    /**Update quest [quest_name] to have state [quest_state].*/

    public void UpdateQuestState(QuestName quest_name, QuestState quest_state)
    {
        this.m_AllQuests[quest_name].m_QuestState = quest_state;
    }

    /**A function to spawn in whatever is needed for the quest; to be called from the quest giver on giving the quest.*/

    public void SpawnInQuestObjects(Quest quest)
    {
        switch ((int)quest.m_QuestType)
        {
            case (int)QuestType.KILL_EVERYTHING:
                {
                    //for each enemy type (though I'm not really sure this would work for more than one enemy type)
                    for (int enemy_type = 0; enemy_type < quest.m_RequisitePrefabs.Count; enemy_type++)
                    {
                        //get the default enemy component and get
                        DefaultEnemy enemy_component = quest.m_RequisitePrefabs[enemy_type].GetComponentInChildren<DefaultEnemy>();
                        Debug.Log("Enemy component? " + (enemy_component != null) + " Name: " + enemy_component.m_EnemyName);

                        quest.m_KillEverything.SpawnEnemiesAtPosition(this.m_Spawner, quest.m_QuestObjectivePosition, enemy_component.m_EnemyName,
                            quest.m_NumberOfEnemiesToKill);
                    } //end for
                    break;
                } //end case kill everything
            case (int)QuestType.FETCH:
                {
                    //for each item type (though I'm not really sure this would work for more than one item type)
                    for (int item_type = 0; item_type < quest.m_RequisitePrefabs.Count; item_type++)
                    {
                        //get the default enemy component and get
                        quest.m_RequisitePrefabs[item_type].GetComponent<QuestItemPickup>().m_QuestItem = new QuestItem();
                        QuestItem quest_item = quest.m_RequisitePrefabs[item_type].GetComponent<QuestItemPickup>().m_QuestItem;
                        //					Debug.Log ("Quest item exists? " + (quest_item != null));
                        quest.m_Fetch.SpawnQuestItemsAtPosition(this.m_Spawner, quest.m_QuestObjectivePosition, quest_item.m_QuestItemName,
                            quest.m_NumberOfItemsToFind);
                    } //end for
                    break;
                } //end case fetch
        } //end switch
    } //end f'n void SpawnInQuestObjects(Quest)

    public void FinishCutscene()
    {
        StartCoroutine("NextCutscene");
    }

    private float oldEnemyDamageValue;

    public IEnumerator NextCutscene()
    {
        RPGTalkArea rpgTalkArea = m_Leoghaire.GetComponentInChildren<RPGTalkArea>();
        LeoghaireBehaviour leoghaireBehaviour = m_Leoghaire.GetComponentInChildren<LeoghaireBehaviour>();
        for (int i = 0; i < CutscenesDone.Count; ++i)
        {
            if (!CutscenesDone[i])
            {
                CutscenesDone[i] = true;
                switch (i)
                {
                    case 0:
                        {
                            DarkScreen.color = new Color(0, 0, 0, 0);
                            rpgTalkArea.rpgtalkTarget.NewTalk("1s", "1e");
                            break;
                        }
                    case 1:
                        {
                            leoghaireBehaviour.GoToHalfOfCoille();
                            break;
                        }
                    case 2:
                        {
                            leoghaireBehaviour.GotToEndOfCoille();
                            break;
                        }
                    case 3:
                        {
                            leoghaireBehaviour.enemy.gameObject.SetActive(true);
                            leoghaireBehaviour.enemy.Pause();
                            rpgTalkArea.rpgtalkTarget.NewTalk("4s", "4e");
                            break;
                        }
                    case 4:
                        {
                            leoghaireBehaviour.enemy.Resume();
                            oldEnemyDamageValue = leoghaireBehaviour.enemy.m_AttackDamageValue;
                            float currentHealth = m_Player.m_Health;
                            while (m_Player.m_Health == currentHealth)
                            {
                                currentHealth = m_Player.m_Health;
                                yield return new WaitForSeconds(1);
                            }
                            leoghaireBehaviour.enemy.Pause();
                            leoghaireBehaviour.enemy.m_AttackDamageValue = 0;
                            rpgTalkArea.shouldInteractWithButton = true;
                            rpgTalkArea.interactionKey = KeyCode.E;
                            break;
                        }
                    case 5:
                        {
                            break;
                        }
                    case 6:
                        {
                            leoghaireBehaviour.enemy.Resume();
                            leoghaireBehaviour.enemy.m_AttackDamageValue = oldEnemyDamageValue;
                            float currentMana = m_Player.m_Mana;
                            while (m_Player.m_Mana == currentMana)
                            {
                                currentMana = m_Player.m_Mana;
                                yield return new WaitForSeconds(1);
                            }
                            leoghaireBehaviour.enemy.Pause();
                            leoghaireBehaviour.enemy.m_AttackDamageValue = 0;
                            break;
                        }
                    case 7:
                        {
                            leoghaireBehaviour.enemy.Resume();
                            leoghaireBehaviour.enemy.m_AttackDamageValue = oldEnemyDamageValue;
                            Vector3 location = leoghaireBehaviour.enemy.transform.position;
                            while (leoghaireBehaviour.enemy)
                                yield return new WaitForSeconds(1);
                            m_Spawner.Spawn_Item(ItemName.Mana_Potion, location);
                            rpgTalkArea.shouldInteractWithButton = false;
                            rpgTalkArea.triggerEnter = true;
                            break;
                        }
                    case 8:
                        {
                            break;
                        }
                    case 9:
                        {
                            leoghaireBehaviour.LeaveCoille();
                            break;
                        }
                    case 10:
                        {
                            leoghaireBehaviour.GoToApothecary();
                            yield return new WaitForSeconds(4);
                            break;
                        }
                    case 11:
                        {
                            leoghaireBehaviour.GoToPlayer();
                            break;
                        }
                    case 12:
                        {
                            DarkScreen.color = new Color(0, 0, 0, 1);
                            break;
                        }
                    case 13:
                        {
                            DarkScreen.color = new Color(0, 0, 0, 0);
                            leoghaireBehaviour.GoToEndOfSoirbheach();
                            break;
                        }
                    case 14:
                        {
                            leoghaireBehaviour.LeaveSoirbheach();
                            break;
                        }
                }
                print(i + 1);
                rpgTalkArea.NewCutscene(i + 1);
                if (i + 1 >= 5 && i + 1 <= 8 || i + 1 >= 11 && i + 1 <= 13)
                    rpgTalkArea.StartNext();
                break;
            }
        }
    }
}

public static class RPGTalkAreaHelper
{
    public static void NewCutscene(this RPGTalkArea rpgTalkArea, int cutscene)
    {
        rpgTalkArea.lineToStart = cutscene.ToString() + "s";
        rpgTalkArea.lineToBreak = cutscene.ToString() + "e";
        rpgTalkArea.alreadyHappened = false;
    }
}