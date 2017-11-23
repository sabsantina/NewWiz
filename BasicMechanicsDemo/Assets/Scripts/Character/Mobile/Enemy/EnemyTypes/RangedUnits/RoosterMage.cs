////Uncommnt this macro to be able to switch the spell the RoosterMage casts in real time, from the inspector.
//#define TESTING_SPELL_SELECTION

//#define TESTING_MELEE_PARAM
//#define TESTING_RANGED_PARAM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoosterMage : RangedEnemy {
	/**The health of a RoosterMage*/
	public float m_RoosterMageHealth = 25.0f;

	public float m_RoosterMageMeleeDamage = 1.5f;

	public float m_RoosterMageChasePlayerDuration = 1.5f;

	public float m_IntervalBetweenRangedAttacks = 2.5f;

	public float m_IntervalBetweenMeleeAttacks = 1.0f;


	// Use this for initialization
	void Start () {
		base.Start ();

		this.SetIntervalsBetweenAttacks ();
		this.SetHealth (this.m_RoosterMageHealth);
		this.SetMeleeDamage ();
		this.SetSpellToCast (this.m_AttackSpell);
		this.SetAttackDamageValue ();

		this.SetChasePlayerSettings (this.m_RoosterMageChasePlayerDuration);
	}


	// Update is called once per frame
	protected override void Update () {
		//Take care of movement and attack patterns
		base.Update ();
		//Manage death
		this.Die ();

		#if TESTING_SPELL_SELECTION
		this.SetSpellToCast (this.m_AttackSpell);
		#endif
		#if TESTING_MELEE_PARAM
		Debug.Log("Rooster attacking Melee? " + this.m_Animator.GetBool("isAttacking_Melee"));
		#endif
		#if TESTING_RANGED_PARAM
		Debug.Log("Rooster attacking Ranged? " + this.m_Animator.GetBool("isAttacking_Ranged"));
		#endif
	}

	/**A function to set the spell to cast in our parent classes*/
	public override void SetSpellToCast (SpellName spell)
	{
		SpellClass spell_instance = new SpellClass ();
		this.m_SpellToCast = spell_instance.GenerateInstance (spell);
		this.m_AttackPattern.m_SpellToCast = this.m_SpellToCast;
	}

	public override void SetAttackDamageValue ()
	{
		if (this.m_AttackPattern.m_AttackPatternState == AttackPatternState.RANGED) {
			this.m_AttackDamageValue = this.m_SpellToCast.m_SpellDamage;
		} else if (this.m_AttackPattern.m_AttackPatternState == AttackPatternState.MELEE) {
			this.m_AttackDamageValue = this.m_RoosterMageMeleeDamage;
		}
	}

	protected override void SetMeleeDamage ()
	{
		this.m_MeleeDamage = this.m_RoosterMageMeleeDamage;
	}

	protected override void SetGeneratedSpellInstance ()
	{
		this.m_GeneratedSpellCubeInstance = this.m_AttackPattern.m_GeneratedSpellInstance;
	}

	/**A function to set different intervals between each successive strike of a type of attack*/
	protected override void SetIntervalsBetweenAttacks()
	{
		this.m_MeleeAttackInterval = this.m_IntervalBetweenMeleeAttacks;
		this.m_RangedAttackInterval = this.m_IntervalBetweenRangedAttacks;
	}

	public override void ApplySpellEffect (SpellClass spell)
	{
		base.ApplySpellEffect (spell);
	}

}
