/**
* Unlike the Player class, it's very likely that the Enemy class will only wind up becoming a superclass.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float m_Health;

    public float m_FullHealth;

    void Start()
    {
        m_Health = m_FullHealth;
    }
    void Update()
    {
        /**Should add animation for death here later.*/
        if(this.m_Health <= 0.0f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    /**A function to add [effect] to the enemys's health.*/
    public void AffectHealth(float effect)
    {
        this.m_Health += effect;
    }//end f'n void AffectHealth(float)

    /**Function which applies the effect of a spell on the enemy.
     Should make it abstract when we add a variety of enemies.*/
    public void ApplySpellEffects(Spell hitSpell)
    {
        if(hitSpell.m_SpellName == SpellName.Fireball)
        {
            /**Could add further spell effects here.*/
            AffectHealth(-hitSpell.m_SpellDamage);
        }
    }
}
