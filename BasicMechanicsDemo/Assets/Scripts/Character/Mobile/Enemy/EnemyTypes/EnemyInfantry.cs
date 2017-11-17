using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfantry : DefaultEnemy {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.Move ();
	}

	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.
	* Note that in the MovementPattern class, we have a function executing movement executing in Update, and what it does depends on the MovementPatternState. So all we need do to affect movement is change the value of the movement pattern state.*/
	public override void Move ()
	{
		Debug.Log (this.m_MovementPattern.IsPlayerDetectedInPatrolRegion ());
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

	/**A function to have the enemy apply their given attack on the player.*/
	public override void Attack()
	{
		//To be overridden in children classes
	}

	/**A function to execute and control the enemy's death animation*/
	public override void Die()
	{
		//To be overridden in children classes
	}

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	public override void ApplySpellEffect (SpellClass spell)
	{
		//To be overridden in children classes
		//Note: this is virtual because certain spells may affect certain enemies differently
	}
}
