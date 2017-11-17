using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfantry : DefaultEnemy {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		this.Move ();
//		if (this.IsPlayerInRangeOfAttack ()) {
//			this.Attack ();
//		} else {
//			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
//		}
	}

	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.
	* Note that in the MovementPattern class, we have a function executing movement executing in Update, and what it does depends on the MovementPatternState. So all we need do to affect movement is change the value of the movement pattern state.*/
	public override void Move ()
	{
		Debug.Log ("Player detected for movement region? " + this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ());
		//If the player is detected...
		if (this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ()) {
			//...then an infantry enemy should chase the player
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.CHASE_PLAYER;
		} 
		//else if the player isn't detected...
		else {
			//...then an infantry enemy should just roam
			this.m_MovementPattern.m_MovementPatternState = MovementPatternState.ROAM;
		}
	}//end f'n void Move()

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
		this.m_AttackPattern.m_AttackPatternState = AttackPatternState.MELEE;
	}

	/**A function to execute and control the enemy's death animation*/
	public override void Die()
	{
		//Set off animator
	}

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	public virtual void ApplySpellEffect (SpellClass spell)
	{
		//To be overridden in children classes
		//Note: this is virtual because certain spells may affect certain enemies differently
	}

	public virtual void SetAttackDamage(float attack_damage)
	{
		//To be overridden in child classes
	}

	/**A function to set the enemy's health; virtual so we make sure to only set it later, at its proper time*/
	public virtual void SetHealth(float health)
	{
		//To be overridden in children classes
	}

}
