using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : ItemClass {
	/**A variable to let us know what the quest item's name is*/
	public QuestItemName m_QuestItemName;
	/**A bool to tell us whether or not the item has been collected*/
	public bool m_IsCollected = false;

	public void GenerateQuestItem (QuestItemName name)
	{
		this.m_QuestItemName = name;
	}
}
