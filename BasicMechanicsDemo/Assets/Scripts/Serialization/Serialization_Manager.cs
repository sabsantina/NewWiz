#define TESTING_SAVE_LOAD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class Serialization_Manager : MonoBehaviour {
//	[SerializeField] GameObject m_PlayerPrefab;
	[SerializeField] public Player m_Player;
	[SerializeField] QuestManager m_QuestManager;

	public Serializable_Session m_SerializableSession = new Serializable_Session();

	private readonly string FILEPATH_EXTENSION = "/SavedGame.gd";

	//Executed every time the game scene is loaded.
	void Start()
	{
		//If a saved game file exists...
		if (File.Exists (Application.persistentDataPath + FILEPATH_EXTENSION)) {
			int load_game_intval = UnityEngine.PlayerPrefs.GetInt (MainMenu_UIManager.STRINGKEY_PLAYERPREF_LOADGAME);
			//...and if the user chose to load the game from either the main menu or the game over menu
			if (load_game_intval == 2) {
				Debug.Log ("Serialization_Manager::Loading game; Calling load on start()");
				this.Load ();
				float x = 0.0f, y = 0.0f, z = 0.0f;
				x = this.m_SerializableSession.m_SerializablePlayer.m_PlayerPositionInWorld_X;
				y = this.m_SerializableSession.m_SerializablePlayer.m_PlayerPositionInWorld_Y;
				z = this.m_SerializableSession.m_SerializablePlayer.m_PlayerPositionInWorld_Z;
				this.m_Player.transform.position = new Vector3 (x, y, z);

				//Reset player pref for next interaction with main menu UI
				UnityEngine.PlayerPrefs.SetInt (MainMenu_UIManager.STRINGKEY_PLAYERPREF_LOADGAME, 0);
			}//end if
			//...or if the user chose new game
			else if (load_game_intval == 1) {
			}
			//else if we're just transitioning from one scene to another
			else if (load_game_intval == 0) {
				//then load in the quest information for the quest objects that belong to this region
				this.Load();
			}
			//if we're not starting a new game...
			if (load_game_intval != 1) {
				this.m_Player.setMaxMeter (this.m_Player.healthMeter, this.m_Player.PLAYER_FULL_HEALTH);
				this.m_Player.setMeterValue (this.m_Player.healthMeter, this.m_Player.m_Health);

				//Start off with full mana
				this.m_Player.setMaxMeter (this.m_Player.manaMeter, this.m_Player.PLAYER_FULL_MANA);
				this.m_Player.setMeterValue (this.m_Player.manaMeter, this.m_Player.m_Mana);
			}

		}//end if file exists

	}


	public void Save() {
//		Serializable_Player SP = new Serializable_Player ();
//		this.m_SerializablePlayer = this.m_SerializablePlayer.GenerateSerializableInstance(this.m_Player);
//		this.m_SerializableQuestManager.ParseAllQuestStates(m_QuestManager);
		this.m_SerializableSession.GatherSessionInformation(m_Player.gameObject, m_QuestManager);
		BinaryFormatter bf = new BinaryFormatter();
		Debug.Log ("Saved progress to " + Application.persistentDataPath);
		FileStream file = File.Create (Application.persistentDataPath + FILEPATH_EXTENSION);
//		bf.Serialize(file, m_SavedSessions);
//		bf.Serialize(file, this.m_SerializablePlayer);
//		bf.Serialize(file, this.m_SerializableQuestManager);
		bf.Serialize(file, this.m_SerializableSession);
		file.Close();
	}	

	/**A loading function to be called on click from the main menu.
	*This function assumes the player has saved, gone to the menu, and is loading the game, meaning that their current region is part of the loaded saved information.*/
	public void Load() {
		if(File.Exists(Application.persistentDataPath + FILEPATH_EXTENSION)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + FILEPATH_EXTENSION, FileMode.Open);
//			this.m_SavedSessions = (List<Serializable_Player>)bf.Deserialize(file);
//			this.m_SerializablePlayer = (Serializable_Player)bf.Deserialize(file);
//			this.m_SerializableQuestManager = (Serializable_QuestManager)bf.Deserialize(file);
			this.m_SerializableSession = (Serializable_Session)bf.Deserialize(file);
			file.Close();

			//Set all player information
			this.m_SerializableSession.SetSessionInformation(this.m_Player.gameObject, this.m_QuestManager);
			//Spawn in quest objects
			this.SpawnAllQuestObjects();
		}
	}


	private void SpawnAllQuestObjects()
	{

		//for each quest in the quest list...
		for (int quest_index = 0; quest_index < System.Enum.GetValues (typeof(QuestName)).Length; quest_index++) {
			Quest current_quest = this.m_QuestManager.m_AllQuests[m_QuestManager.m_AllQuests.ElementAt(quest_index).Key];
			int current_scene_index = UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex;
//			Debug.Log ("Current quest region(" + (int)current_quest.m_QuestRegion + ") == current scene index(" + current_scene_index +")? " + ((int)current_quest.m_QuestRegion == current_scene_index));
			//We only want to spawn in the quests that are in the same region as us
			if ((int)current_quest.m_QuestRegion == current_scene_index) {
				//... and we only care about the quest's objects if it's in process
				if ((int)current_quest.m_QuestState == (int)QuestState.IN_PROCESS) {
					//...and if so, then spawn corresponding quest objects
					this.m_QuestManager.SpawnInQuestObjects (current_quest);
				}
			}
		}
	}

	
	// Update is called once per frame
	void Update () {
		#if TESTING_SAVE_LOAD
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			Save ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			Load ();
		}
		#endif
	}
}
