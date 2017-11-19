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
	/**Interior padding to prevent the enemy from leaving the patrol region a little bit earlier*/
	public float m_InteriorPadding = 1.0f;

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Player> () != null) {
			this.m_PlayerInRegion = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Player> () != null) {
			this.m_PlayerInRegion = false;
		}
	}


}
