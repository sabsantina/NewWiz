#define TESTING_SPELLMOVEMENT
#define TESTING_SPELLCOLLISION

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Collider to ensure the spell can be picked up if it's in the world
[RequireComponent(typeof(Collider))]
public class Spell : MonoBehaviour {
	/**An enum variable for the spell name. See SpellName.cs for all spell names.*/
	public SpellName m_SpellName { set; get;}
	/**An enum variable for the spell effect. See SpellEffecr.cs for all spell effects.*/
	public SpellEffect m_SpellEffect { set; get;}
    /**A float which stores the initial damage of the spell*/
    public float m_SpellDamage { set; get; }
	/**A bool to let us know whether or not the player has discovered the spell yet.
	*It'd be simpler to just keep all the spells in the inventory from the get-go, but only show them to the player if they've been discovered.
	*We can do the same basic thing with the items; if the player has 0 quantity of a given item, we won't show it.*/
	public bool m_HasBeenDiscovered{ set; get;}

	void Awake()
	{
		//ensure the box collider's isTrigger is set to true
		this.GetComponent<Collider> ().isTrigger = true;
	}//end f'n void Awake()

	/**Return a copy of [spell_to_copy].*/
	public Spell CopySpell(Spell spell_to_copy)
	{
		Spell new_spell = new Spell ();
		new_spell.m_HasBeenDiscovered = spell_to_copy.m_HasBeenDiscovered;
		new_spell.m_SpellDamage = spell_to_copy.m_SpellDamage;
		new_spell.m_SpellEffect = spell_to_copy.m_SpellEffect;
		new_spell.m_SpellName = spell_to_copy.m_SpellName;
		return new_spell;
	}

	public void GenerateInstance_Fireball(bool has_been_discovered, float damage)
	{
		this.m_HasBeenDiscovered = has_been_discovered;
		this.m_SpellDamage = damage;
		this.m_SpellName = SpellName.Fireball;
		this.m_SpellEffect = SpellEffect.Fire_Damage;
	}

    public void GenerateInstance_IceBall(bool has_been_discovered, float damage)
    {
        this.m_HasBeenDiscovered = has_been_discovered;
        this.m_SpellDamage = damage;
        this.m_SpellName = SpellName.Iceball;
        this.m_SpellEffect = SpellEffect.Ice_Freeze;
    }
}
