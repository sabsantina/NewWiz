using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfantry : DefaultEnemy {

	public MeleeAttackPattern m_AttackPattern;

	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.
	* Note that in the MovementPattern class, we have a function executing movement executing in Update, and what it does depends on the MovementPatternState. So all we need do to affect movement is change the value of the movement pattern state.*/
	public override void Move ()
	{
//		Debug.Log ("Player detected for movement region? " + this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ());
		if (this.m_InhibitMovement) {
//			Debug.Log ("Inhibit movement");
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.STAY_STILL;
		}
		//If the player is detected...
		else if (this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ()) {
			//...then an infantry enemy should chase the player
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.CHASE_PLAYER;
		} 
		//else if the player isn't detected...
		else if (!this.m_MovementPattern.IsPlayerDetectedInPatrolRegion()) {
			//...then an infantry enemy should just roam
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.ROAM;
		}
	}//end f'n void Move()



    /**A function to more easily manage our attack pattern.
	*To be expanded upon in child classes.*/
    protected void ManageAttack()
    {
        if (this.IsPlayerInRangeOfAttack())
        {
            this.Attack();
        }
        else
        {
            this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
        }
    }

    /**A function to establish whether or not the player is in range of the enemy, for the specific enemy. 
	 * - Sets this.mPlayerIsInRange as well as returns a bool*/
    protected override bool IsPlayerInRangeOfAttack()
	{
		this.m_PlayerIsInRange = this.m_AttackPattern.PlayerIsDetectedInAttackDetectionRegion ();
//		Debug.Log ("Player detected for attack region? " + this.m_PlayerIsInRange);
		return this.m_PlayerIsInRange;
	}

	/**A function to have the enemy apply their given attack on the player.*/
	public override void Attack()
	{
		if (this.m_InhibitAttack) {
//			Debug.Log ("Inhibit Attack");
			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
		} else {
			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.MELEE;
		}
	}

	protected virtual void Update()
	{
		base.Update ();
		if (this.IsPlayerInRangeOfAttack ()) {
			this.Attack ();
		} else {
			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
		}
	}

//	public virtual void SetAttackDamage(float attack_damage)
//	{
//		//To be overridden in child classes
//	}
//
//	/**A function to set the enemy's health; virtual so we make sure to only set it later, at its proper time*/
//	public virtual void SetHealth(float health)
//	{
//		//To be overridden in children classes
//	}

}
