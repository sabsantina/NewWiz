using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefaultEnemy : MonoBehaviour, IEnemy {

	/**A MovementPattern to control the enemy's movement*/
	public MovementPattern m_MovementPattern;
	/**An AttackPattern to control the enemy's attacks*/
	public AttackPattern m_AttackPattern;
	/**A float to keep track of health.
	*To be overridden, or set, in children classes.*/
	public float m_Health;
	/**A float to represent the enemy's attack damage.
	*To be overridden, or set, in children classes.*/
	public float m_AttackDamageValue;
	/**A bool to let us know whether or not the player is in range of the given enemy's attacks. Probably more for testing than anything else
	* For example, for ranged units, if the player is at all detected then the player is in range. For infantry units, if the player is within a given distance, then the player is in range.*/
	public bool m_PlayerIsInRange = false;

	/**A function to establish whether or not the player is in range of the enemy, for the specific enemy. 
	 * - Sets this.mPlayerIsInRange as well as returns a bool*/
	protected virtual bool IsPlayerInRangeOfAttack()
	{
		//To be overridden in children classes
		return false;
	}

	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.*/
	public virtual void Move ()
	{
		//To be overridden in children classes
	}

	/**A function to have the enemy apply their given attack on the player.*/
	public virtual void Attack()
	{
		//To be overridden in children classes
	}

	/**A function to execute and control the enemy's death animation*/
	public virtual void Die()
	{
		//To be overridden in children classes
	}

	/**A function to affect the enemy's health; damage should be fed as a negative number.*/
	public void AffectHealth (float effect)
	{
		this.m_Health += effect;
	}

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	public virtual void ApplySpellEffect (SpellClass spell)
	{
		//To be overridden in children classes
		//Note: this is virtual because certain spells may affect certain enemies differently
	}

	/**A function to set the enemy's health; virtual so we make sure to only set it later, at its proper time*/
	public virtual void SetHealth()
	{
		//To be overridden in children classes
	}

	public virtual void SetAttackDamageValue()
	{
		//To be overridden in children classes
	}

	public virtual float GetAttackDamageValue()
	{
		//To be overridden in children classes
		return 0.0f;
	}

//	/**A function to set the enemy that's attacking, so we can apply the specific damage to the target*/
//	public virtual void SetAttacker(DefaultEnemy enemy)
//	{
//		//To be overridden in children classes
//	}
}
