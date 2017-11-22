//#define TESTING_MELEE_PARAM
//#define TESTING_RANGED_PARAM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooster : EnemyInfantry {

	/**The damage a Rooster enemy does on attack.*/
	public float m_RoosterAttackDamage = 2.5f;
	/**The amount of health a Rooster enemy has.*/
	public float m_RoosterHealth = 20.0f;
	/**Length of time for which the rooster will chase the player if the player attacks the rooster without being detected first*/
	public float m_RoosterChasePlayerDuration = 2.0f;

	// Use this for initialization
	void Start () {
		base.Start ();
		this.SetAttackDamageValue (this.m_RoosterAttackDamage);
		this.SetHealth (this.m_RoosterHealth);

		this.SetChasePlayerSettings (this.m_RoosterChasePlayerDuration);
	}


	// Update is called once per frame
	protected override void Update () {
		//Take care of movement and attack patterns
		base.Update ();
		//Manage death
		this.Die ();


		#if TESTING_MELEE_PARAM
		Debug.Log("Rooster attacking Melee? " + this.m_Animator.GetBool("isAttacking_Melee"));
		#endif
		#if TESTING_RANGED_PARAM
		Debug.Log("Rooster attacking Ranged? " + this.m_Animator.GetBool("isAttacking_Ranged"));
		#endif
	}

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	public override void ApplySpellEffect (SpellClass spell)
	{
		base.ApplySpellEffect (spell);
		//To be overridden in children classes
		//Note: this is virtual because certain spells may affect certain enemies differently
	}


	public override void SetAttackDamageValue()
	{
		this.m_AttackDamageValue = this.m_RoosterAttackDamage;
	}

	public override float GetAttackDamageValue ()
	{
		return this.m_AttackDamageValue;
	}


}
