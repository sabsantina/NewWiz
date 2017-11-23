using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : DefaultEnemy {

	/**The spell the ranged enemy will be casting.
	*To be set in children classes.*/
	public SpellClass m_SpellToCast;
	/**A public SpellName, to set our attack spell, with default value Fireball*/
	public SpellName m_AttackSpell = SpellName.Fireball;

	public RangedAttackPattern m_AttackPattern;

	protected GameObject m_GeneratedSpellCubeInstance;

	protected float m_MeleeDamage;

	public float m_MeleeAttackInterval;

	public float m_RangedAttackInterval;

	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.
	* Note that in the MovementPattern class, we have a function executing movement executing in Update, and what it does depends on the MovementPatternState. So all we need do to affect movement is change the value of the movement pattern state.*/
	public override void Move ()
	{
		//		Debug.Log (this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ());
		if (this.m_InhibitMovement) {
			//			Debug.Log ("Inhibit movement");
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.STAY_STILL;
		}
		//If the player is detected in the patrol region...
		else if (this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ()) {
			//...then a boss enemy should edge closer to the player
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.MOVE_TOWARDS_PLAYER;
			//(All the while attacking from far away but switching to melee when close enough)
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
		if (this.m_InhibitAttack) {
			//			Debug.Log ("Inhibit Attack");
			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
			return;
		}
		if (this.m_AttackPattern.m_MeleeDetectionRegion.m_PlayerInRegion) {
			this.m_AttackPattern.m_IntervalBetweenAttacks = this.m_MeleeAttackInterval;
			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.MELEE;
		}
		else if (this.IsPlayerInRangeOfAttack ()) {
			this.m_AttackPattern.m_IntervalBetweenAttacks = this.m_RangedAttackInterval;
			this.Attack ();
		} 
		else if (!this.IsPlayerInRangeOfAttack()) {
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

	protected virtual void Update()
	{
		base.Update ();
		this.ManageAttack ();
	}

	/**A function to set different intervals between each successive strike of a type of attack*/
	protected virtual void SetIntervalsBetweenAttacks()
	{
		//To be overridden in children classes
	}

	/**A function to be defined in ranged enemies, since what is called "melee damage" to them is just called regular dmaage in the infantry units*/
	protected virtual void SetMeleeDamage()
	{
		//To be overridden in children classes
	}

	/**A function to tell us if the player is detected in either the attack or the movement region.*/
	protected override bool IsPlayerAtAllDetected()
	{
		return this.m_MovementPattern.IsPlayerDetectedInPatrolRegion() || this.m_AttackPattern.PlayerIsDetectedInAttackDetectionRegion();
	}
}
