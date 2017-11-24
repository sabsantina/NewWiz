using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Serializable_Session{
	public Serializable_Player m_SerializablePlayer = new Serializable_Player();
	public Serializable_QuestManager m_SerializableQuestManager = new Serializable_QuestManager();

	/**A function to gather all session information, for the serialization.*/
	public void GatherSessionInformation(GameObject player, QuestManager quest_manager)
	{
		this.m_SerializablePlayer = this.m_SerializablePlayer.GenerateSerializableInstance(player);
		this.m_SerializableQuestManager.ParseAllQuestStates (quest_manager);
	}//end f'n void GatherSessionInformation(GameObject, GameObject)

	/**A function to set all session information, for the deserialization.*/
	public void SetSessionInformation(GameObject player, QuestManager quest_manager)
	{
		this.m_SerializablePlayer.SetAllPlayerInformation (player);
		this.m_SerializableQuestManager.SetAllQuestStates (quest_manager);
//		this.m_SerializableQuestManager.SpawnInQuestObjects (quest_manager);
	}//end f'n void SetSessionInformation(GameObject, GameObject)
}
