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

	public AudioSource m_audioSource;

	/**A variable to keep track of the player's health.*/
	public float m_Health;
	/**A variable to keep track of the player's mana.*/
	public float m_Mana;
	/**A variable to help us know what the player's full health is; public for accessibility in the Item classes (think health potions).*/
	public float PLAYER_FULL_HEALTH = 100.0f;
	/**A variable to help us know what the player's full mana count is.*/
	public float PLAYER_FULL_MANA = 100.0f;
	/**A bool to tell us whether or not the player is capable of being damaged.*/
	public bool m_IsShielded = false;
	/**A bool to tell us whether or not the player is capable of casting spells, with respect to mana.*/
	public bool m_CanCastSpells = true;
	/**A multiplier to help with mana regeneration.*/
	public float m_ManaRegenMultiplier = 1.125f;
	/**A bool to let us know whether or not we're alive.*/
	public bool m_IsAlive = false;
	/**The player animator*/
	private Animator m_Animator;
	/**The string value of the player animator parameter isAlive*/
	public readonly string STRINGKEY_PARAM_ISALIVE = "isAlive";
	/**A vector to contain the player's respawn position*/
	public Vector3 m_PlayerRespawnPosition = new Vector3(-6.5f, 0.55f, -0.43f);

	void Start()
	{
		this.m_Animator = this.GetComponent<Animator> ();
		this.m_audioSource = GetComponent<AudioSource> ();
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
			#endif

			setMeterValue(healthMeter, this.m_Health);
			this.m_IsAlive = false;
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
			}//end if
			setMeterValue (manaMeter, this.m_Mana);
		}//end if

		if (!this.m_IsAlive) {
			this.Resurrect ();
		}

		this.UpdateAnimatorParameters ();
	}//end f'n void Update

	private void UpdateAnimatorParameters()
	{
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISALIVE, this.m_IsAlive);
	}

	/**A function to resurrect the player to their respawn point*/
	private void Resurrect()
	{
		this.m_IsAlive = true;
//		this.UpdateAnimatorParameters ();
		this.m_Health = this.PLAYER_FULL_HEALTH;
		this.setMeterValue(healthMeter, this.m_Health);
		this.m_Mana = this.PLAYER_FULL_MANA;
		this.setMeterValue (manaMeter, this.m_Mana);
		this.transform.position = this.m_PlayerRespawnPosition;
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

