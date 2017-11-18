/*
* A script to manage the player's status.
*/

#define TESTING_ZERO_HEALTH
#define TESTING_MANA_REGEN

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, ICanBeDamagedByMagic {
	[SerializeField] public GameObject healthMeter;
	[SerializeField] public GameObject manaMeter;

	/**A reference to the HotKey button gameobject representative of HotKey1*/
	[SerializeField] public UnityEngine.UI.Button m_HotKey1_Obj;
	/**A reference to the HotKey button gameobject representative of HotKey2*/
	[SerializeField] public UnityEngine.UI.Button m_HotKey2_Obj;
	/**A reference to the HotKey button gameobject representative of HotKey3*/
	[SerializeField] public UnityEngine.UI.Button m_HotKey3_Obj;

	/**A string value of the input axis name for HotKey1*/
	private readonly string STRINGKEY_INPUT_HOTKEY1 = "HotKey1";
	/**A string value of the input axis name for HotKey2*/
	private readonly string STRINGKEY_INPUT_HOTKEY2 = "HotKey2";
	/**A string value of the input axis name for HotKey3*/
	private readonly string STRINGKEY_INPUT_HOTKEY3 = "HotKey3";

	private HotKeys m_HotKey1;
	private HotKeys m_HotKey2;
	private HotKeys m_HotKey3;

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
    /**The string value of the name of the sorting layer*/
    public string sortingLayerName;

	void Awake()
	{
		this.m_HotKey1 = this.m_HotKey1_Obj.GetComponentInChildren<HotKeys> ();
		this.m_HotKey2 = this.m_HotKey2_Obj.GetComponentInChildren<HotKeys> ();
		this.m_HotKey3 = this.m_HotKey3_Obj.GetComponentInChildren<HotKeys> ();
	}

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
        //The sorting layer name is retrieved from unity
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = sortingLayerName;
    }

	void Update()
	{
		//check for input from the player for use of hotkeyed items
		this.CheckForHotKeyButtonInput ();
        
        //This line calculates the sorting order value for the sprite renderer while in play.
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(this.GetComponent<Transform>().transform.position.z * 100f) * -1;
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

	/**A function to check for keyboard input for use of hotkeyed items.*/
	private void CheckForHotKeyButtonInput()
	{
		//if the user hits the input button for HotKey1 item consumption...
		if (Input.GetButtonDown (STRINGKEY_INPUT_HOTKEY1)) {
			this.m_HotKey1.useItem ();
		}//end if
		//else if the user hits the input button for HotKey2 item consumption...
		else if (Input.GetButtonDown (STRINGKEY_INPUT_HOTKEY2)) {
			this.m_HotKey2.useItem ();
		}//end else if
		//else if the user hits the input button for HotKey3 item consumption...
		else if (Input.GetButtonDown (STRINGKEY_INPUT_HOTKEY3)) {
			this.m_HotKey3.useItem ();
		}//end else if
		//else do nothing
	}//end f'n void CheckForHotKeyButtonInput()

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

	public void ApplySpellEffect (SpellClass spell)
	{
		this.AffectHealth (-spell.m_SpellDamage);
	}
}

