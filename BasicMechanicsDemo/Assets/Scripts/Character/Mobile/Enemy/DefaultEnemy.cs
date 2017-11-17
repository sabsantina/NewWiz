using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefaultEnemy : MonoBehaviour, IEnemy {

	public MovementPattern m_MovementPattern;
	/**A float to keep track of health*/
	public float m_Health;
	/**A bool to let us know whether or not the player is in range of the given enemy's attacks.
	* For example, for ranged units, if the player is at all detected then the player is in range. For infantry units, if the player is within a given distance, then the player is in range.*/
	public bool m_PlayerIsInRange = false;

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
