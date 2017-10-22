/*
* A script to manage the player's status.
*/

#define TESTING_ZERO_HEALTH

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	/**A variable to keep track of the player's health.*/
	public float m_Health;
	/**A variable to help us know what the player's full health it; public for accessibility in the Item classes (think health potions).*/
	public readonly float PLAYER_FULL_HEALTH = 100.0f;

	void Start()
	{
		//Start off with full health
		this.m_Health = PLAYER_FULL_HEALTH;
	}

	void Update()
	{
		if (this.m_Health == 0)
		{
			#if TESTING_ZERO_HEALTH
			Debug.Log("Zero health; player dead\tResurrection time!");
			this.m_Health = PLAYER_FULL_HEALTH;
			#endif
			//Game Over
			//Should we respawn the player at a checkpoint? How do we want to do this?
		}//end if
	}

	/**A function to add [effect] to the player's health.*/
	public void AffectHealth(float effect)
	{
		this.m_Health += effect;
	}//end f'n void AffectHealth(float)
}
