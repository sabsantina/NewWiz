﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : DefaultEnemy {

	/**The spell the ranged enemy will be casting.
	*To be set in children classes.*/
	public SpellClass m_SpellToCast;

	public RangedAttackPattern m_AttackPattern;

	protected GameObject m_GeneratedSpellCubeInstance;

	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.
	* Note that in the MovementPattern class, we have a function executing movement executing in Update, and what it does depends on the MovementPatternState. So all we need do to affect movement is change the value of the movement pattern state.*/
	public override void Move ()
	{
		//		Debug.Log (this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ());
		//If the player is detected in the patrol region but not in the attack region...
		if (this.m_MovementPattern.IsPlayerDetectedInPatrolRegion () && !this.m_AttackPattern.PlayerIsDetectedInAttackDetectionRegion ()) {
			//...then a ranged enemy should edge closer to the player
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.MOVE_TOWARDS_PLAYER;
		} 
		//else if the player is detected in the attack region...
		else if (this.m_AttackPattern.PlayerIsDetectedInAttackDetectionRegion ()) {
			//...then a ranged enemy should stay still to attack the player from afar
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.STAY_STILL;
		}
		//else if the player isn't detected...
		else {
			//...then an enemy should just roam
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.ROAM;
		}
	}//end f'n void Move()

	/**A function to more easily manage our attack pattern.
	*To be expanded upon in child classes.*/
	protected void ManageAttack()
	{
		if (this.IsPlayerInRangeOfAttack ()) {
			this.Attack ();
		} else {
			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
		}
	}

	/**A function to establish whether or not the player is in range of the enemy, for the specific enemy. 
	 * - Sets this.mPlayerIsInRange as well as returns a bool*/
	protected override bool IsPlayerInRangeOfAttack()
	{
		this.m_PlayerIsInRange = this.m_AttackPattern.PlayerIsDetectedInAttackDetectionRegion ();
		Debug.Log ("Player detected for attack region? " + this.m_PlayerIsInRange);
		return this.m_PlayerIsInRange;
	}

	/**A function to have the enemy apply their given attack on the player.*/
	public override void Attack()
	{
		this.m_AttackPattern.m_AttackPatternState = AttackPatternState.RANGED;
	}

	public virtual void SetSpellToCast(SpellName spell)
	{
		//To be overridden in children classes
	}

	protected virtual void SetGeneratedSpellInstance()
	{
		//To be overridden in children classes
	}
}
