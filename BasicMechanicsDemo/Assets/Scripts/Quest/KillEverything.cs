/*
* A class to define the objective behavior if the quest objective is to kill a group of targets
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KillEverything : Objective {

	public List<Enemy> m_Targets = new List<Enemy>();

	/**A function to check whether every target has been killed.*/
	public override bool CheckForObjectiveIsMet()
	{
		//foreach enemy in the target list...
		foreach (Enemy enemy in this.m_Targets) {
			//...if any one enemy is alive...
			if (enemy.m_IsAlive) {
				//return false
				return false;
			}//end if
		}//end foreach
		//else if no enemies are alive, return true
		this.UpdateObjectiveIsMet();
		return true;
	}//end f'n bool CheckForObjectiveIsMet()

	private void UpdateObjectiveIsMet()
	{
		this.m_QuestObjectiveIsMet = true;
	}
}//end class KillEverything
