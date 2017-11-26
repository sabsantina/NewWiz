/*
* A script to be attached to an enemy's patrol region.
* All this script does is tell the MovementPattern class whether or not the player is inside the given region.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionRegion : MonoBehaviour {

	/**A bool telling us whether or not the player's in the given patrol region*/
	public bool m_PlayerInRegion = false;
	/**A reference to the player entering the detection region*/
	public Player m_Player;

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Player> () != null) {
			this.m_PlayerInRegion = true;
			this.m_Player = other.GetComponent<Player>();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Player> () != null) {
			this.m_PlayerInRegion = false;
		}
	}


}
