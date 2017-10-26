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
	/**A variable to keep track of the player's mana.*/
	public float m_Mana;
	/**A variable to help us know what the player's full health is; public for accessibility in the Item classes (think health potions).*/
	public readonly float PLAYER_FULL_HEALTH = 100.0f;
	/**A variable to help us know what the player's full mana count is.*/
	public readonly float PLAYER_FULL_MANA = 100.0f;

	public bool m_IsShielded = false;

	public bool m_CanCastSpells = true;

	void Start()
	{
		//Start off with full health
		this.m_Health = PLAYER_FULL_HEALTH;
		//Start off with full mana
		this.m_Mana = PLAYER_FULL_MANA;
	}

	void Update()
	{
		if (this.m_Health <= 0)
		{
			#if TESTING_ZERO_HEALTH
			Debug.Log("Zero health; player dead\tResurrection time!");
			this.m_Health = PLAYER_FULL_HEALTH;
			#endif
			//Game Over
			//Should we respawn the player at a checkpoint? How do we want to do this?
		}//end if
		//if the player runs out of mana, they can't cast spells anymore
		if (this.m_Mana <= 0
			|| this.GetComponent<PlayerInventory>().m_ActiveSpellClass.m_ManaCost > this.m_Mana) {
			this.m_CanCastSpells = false;
		}//end if
	}

	/**A function to add [effect] to the player's health.*/
	public void AffectHealth(float effect)
	{
		this.m_Health += effect;
	}//end f'n void AffectHealth(float)

	public void AffectMana(float effect)
	{
		this.m_Mana += effect;
	}
}
