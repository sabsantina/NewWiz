using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour {
	/**The item's sprite*/
	public Sprite m_QuestItemSprite;
	/**A variable to let us know what the quest item's name is*/
	public QuestItemName m_QuestItemName;
	/**A bool to tell us whether or not the item has been collected*/
	public bool m_IsCollected = false;

	public void SetQuestItemName(QuestItemName quest_item_name)
	{
		this.m_QuestItemName = quest_item_name;
	}

	public void GenerateQuestItem(QuestItemName name)
	{
		this.m_QuestItemName = name;
	}
}
