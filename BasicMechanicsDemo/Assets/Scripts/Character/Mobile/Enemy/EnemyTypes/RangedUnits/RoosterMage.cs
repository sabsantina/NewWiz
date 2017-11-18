//Uncommnt this macro to be able to switch the spell the RoosterMage casts in real time, from the inspector.
#define TESTING_SPELL_SELECTION

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoosterMage : RangedEnemy {
	/**The health of a RoosterMage*/
	public float m_RoosterMageHealth = 25.0f;
	/**A public SpellName, to set our attack spell, with default value Fireball*/
	public SpellName m_AttackSpell = SpellName.Fireball;

	// Use this for initialization
	void Start () {
		this.SetHealth ();
		this.SetSpellToCast (this.m_AttackSpell);
		this.SetAttackDamageValue ();
	}


	// Update is called once per frame
	protected override void Update () {
		base.Update ();

//		this.Move ();
//		this.ManageAttack ();
//		#if TESTING_SPELL_SELECTION
//		this.SetSpellToCast (this.m_AttackSpell);
//		#endif
	}

	/**A function to set the spell to cast in our parent classes*/
	public override void SetSpellToCast (SpellName spell)
	{
		SpellClass spell_instance = new SpellClass ();
		this.m_SpellToCast = spell_instance.GenerateInstance (spell);
		this.m_AttackPattern.m_SpellToCast = this.m_SpellToCast;
	}

	/**A function to set the enemy health in our parent classes*/
	public override void SetHealth ()
	{
		this.m_Health = this.m_RoosterMageHealth;
	}

	public override void SetAttackDamageValue ()
	{
		this.m_AttackDamageValue = this.m_SpellToCast.m_SpellDamage;
	}

	protected override void SetGeneratedSpellInstance ()
	{
		this.m_GeneratedSpellCubeInstance = this.m_AttackPattern.m_GeneratedSpellInstance;
	}

//	protected override void ManageAttack ()
//	{
//		base.ManageAttack ();
//		Debug.Log ("Child");
//	}
}
