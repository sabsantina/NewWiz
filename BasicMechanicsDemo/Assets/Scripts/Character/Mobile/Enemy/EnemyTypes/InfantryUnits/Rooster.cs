using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooster : EnemyInfantry {

	public float m_RoosterAttackDamage = 2.5f;

	public float m_RoosterHealth = 20.0f;

	// Use this for initialization
	void Start () {
		this.SetAttackDamage (this.m_RoosterAttackDamage);
		this.SetHealth (this.m_RoosterHealth);
	}
	
	// Update is called once per frame
	void Update () {
		this.Move ();
		if (this.IsPlayerInRangeOfAttack ()) {
			this.Attack ();
		} else {
			this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
		}
	}

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	public override void ApplySpellEffect (SpellClass spell)
	{
		//To be overridden in children classes
		//Note: this is virtual because certain spells may affect certain enemies differently
	}

	public override void SetAttackDamage(float attack_damage)
	{
		this.m_AttackDamageValue = attack_damage;
	}

	public override void SetHealth(float health)
	{
		this.m_Health = health;
	}

	public override float GetAttackDamageValue ()
	{
		return this.m_AttackDamageValue;
	}


}
