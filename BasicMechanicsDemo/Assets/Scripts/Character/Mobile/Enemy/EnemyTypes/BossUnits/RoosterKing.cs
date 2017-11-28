using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoosterKing : BossEnemy {
	[SerializeField] GameObject enemyHPMeter;
	/**The health of a RoosterMage*/
	public float m_RoosterKingHealth = 100.0f;

	public float m_RoosterKingMeleeDamage = 15.0f;

	public float m_RoosterKingChasePlayerDuration = 15.0f;

	public float m_IntervalBetweenRangedAttacks = 0.25f;

	public float m_IntervalBetweenMeleeAttacks = 0.5f;

	// Use this for initialization
	void Start () {
		base.Start ();

		this.SetIntervalsBetweenAttacks ();
		this.SetHealth (this.m_RoosterKingHealth);
		this.SetMeleeDamage ();
		this.SetSpellToCast (this.m_AttackSpell);
		this.SetAttackDamageValue ();
		this.setMaxMeter ();

		this.SetChasePlayerSettings (this.m_RoosterKingChasePlayerDuration);
		this.m_EnemyName = EnemyName.ROOSTER_KING;
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

	void setMaxMeter()
	{
		enemyHPMeter.GetComponentInChildren<Slider> ().maxValue = m_RoosterKingHealth;
		enemyHPMeter.GetComponentInChildren<Slider> ().value = m_RoosterKingHealth;
	}

//	public override void AffectHealth(float effect)
//	{
//		base.AffectHealth (effect);
//		enemyHPMeter.GetComponentInChildren<Slider> ().value = m_Health;
//	}

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
			this.m_AttackDamageValue = this.m_RoosterKingMeleeDamage;
		}
	}

	protected override void SetMeleeDamage ()
	{
		this.m_MeleeDamage = this.m_RoosterKingMeleeDamage;
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
		enemyHPMeter.GetComponentInChildren<Slider> ().value = m_Health;
	}

	protected override void ManageLootSpawnOnDeath()
	{
		this.m_Spawner.Spawn_Spell (SpellName.Heal, this.transform.position);
	}
}
