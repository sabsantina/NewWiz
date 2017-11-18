using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefaultEnemy : MonoBehaviour, IEnemy, ICanBeDamagedByMagic {

	/**A MovementPattern to control the enemy's movement*/
	public MovementPattern m_MovementPattern;
	/**An AttackPattern to control the enemy's attacks*/
//	public AttackPattern m_AttackPattern;
	/**A float to keep track of health.
	*To be overridden, or set, in children classes.*/
	public float m_Health;
	/**A float to represent the enemy's attack damage.
	*To be overridden, or set, in children classes.*/
	public float m_AttackDamageValue;
	/**A bool to let us know whether or not the player is in range of the given enemy's attacks. Probably more for testing than anything else
	* For example, for ranged units, if the player is at all detected then the player is in range. For infantry units, if the player is within a given distance, then the player is in range.*/
	public bool m_PlayerIsInRange = false;
	/**A stringkey for the isDead parameter in the movement pattern animator*/
	private readonly string STRINGKEY_PARAM_ISDEAD = "IsDead";

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
	public void Die()
	{
		this.m_MovementPattern.m_Animator.SetBool (STRINGKEY_PARAM_ISDEAD, this.m_Health <= 0.0f);
		//Destroy gameobject after the death animation
//		float animation_length = 0.0f;
//		GameObject.Destroy(this.gameObject, animation_length);
	}

	/**A function to affect the enemy's health; damage should be fed as a negative number.*/
	public void AffectHealth (float effect)
	{
		this.m_Health += effect;
	}

	/**A function to apply a given hostile spell's effects on the enemy, including damage.*/
	public virtual void ApplySpellEffect (SpellClass spell)
	{
		this.AffectHealth (-spell.m_SpellDamage);

//		switch((int)spell.m_SpellName)
//		{
//		case 
//		}
		//To be overridden in children classes. We'll keep a default version here, though.
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
