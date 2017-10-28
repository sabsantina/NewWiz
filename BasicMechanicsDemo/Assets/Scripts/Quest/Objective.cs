using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective {

	public string m_QuestObjectiveDialog;
	protected bool m_QuestObjectiveIsMet = false;

	/**A function to check to see whether the objective has been met by the player.*/
	public virtual bool CheckForObjectiveIsMet()
	{
		/*To be overridden in children.*/
		return false;
	}//end f'n bool CheckForObjectiveIsMet()


}
