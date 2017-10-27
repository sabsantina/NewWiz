/*
* A script to manage the player's status.
*/

#define TESTING_ZERO_HEALTH
#define TESTING_MANA_REGEN

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[SerializeField] public GameObject healthMeter;
	[SerializeField] public GameObject manaMeter;

	/**A variable to keep track of the player's health.*/
	public float m_Health;
	/**A variable to keep track of the player's mana.*/
	public float m_Mana;
	/**A variable to help us know what the player's full health is; public for accessibility in the Item classes (think health potions).*/
	public readonly float PLAYER_FULL_HEALTH = 100.0f;
	/**A variable to help us know what the player's full mana count is.*/
	public readonly float PLAYER_FULL_MANA = 100.0f;
	/**A bool to tell us whether or not the player is capable of being damaged.*/
	public bool m_IsShielded = false;
	/**A bool to tell us whether or not the player is capable of casting spells, with respect to mana.*/
	public bool m_CanCastSpells = true;
	/**A multiplier to help with mana regeneration.*/
	public float m_ManaRegenMultiplier = 1.125f;

	void Start()
	{
		//Start off with full health
		this.m_Health = PLAYER_FULL_HEALTH;
		setMaxMeter (healthMeter, this.m_Health);
		setMeterValue (healthMeter, this.m_Health);
		
		//Start off with full mana
		this.m_Mana = PLAYER_FULL_MANA;
		setMaxMeter (manaMeter, this.m_Mana);
		setMeterValue (manaMeter, this.m_Mana);
	}

	void Update()
	{
		if (this.m_Health <= 0)
		{
			#if TESTING_ZERO_HEALTH
			Debug.Log("Zero health; player dead\tResurrection time!");
			this.m_Health = PLAYER_FULL_HEALTH;
			setMeterValue(healthMeter, this.m_Health);
			#endif
			//Game Over
			//Should we respawn the player at a checkpoint? How do we want to do this?
		}//end if
		//if the player runs out of mana, they can't cast spells anymore
		if (this.m_Mana <= 0
		    || this.GetComponent<PlayerInventory> ().m_ActiveSpellClass.m_ManaCost > this.m_Mana) {
			this.m_CanCastSpells = false;
		}//end if
		else {
			this.m_CanCastSpells = true;
		}

		if (this.m_Mana < PLAYER_FULL_MANA) {
			this.m_Mana += Time.deltaTime * this.m_ManaRegenMultiplier;
			if (this.m_Mana > PLAYER_FULL_MANA) {
				this.m_Mana = PLAYER_FULL_MANA;
				setMeterValue (manaMeter, this.m_Mana);
			}//end if
		}//end if
	}

	/**A function to add [effect] to the player's health.*/
	public void AffectHealth(float effect)
	{
		this.m_Health += effect;
		setMeterValue (healthMeter, this.m_Health);

	}//end f'n void AffectHealth(float)

	public void AffectMana(float effect)
	{
		this.m_Mana += effect;
		setMeterValue (manaMeter, this.m_Mana);
	}
		
	public void setMaxMeter(GameObject meter, float value)
	{
		meter.GetComponent<Slider> ().maxValue = value;
	}

	public void setMeterValue(GameObject meter, float value)
	{
		meter.GetComponent<Slider> ().value = value;
	}
}

