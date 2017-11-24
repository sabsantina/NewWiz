/*
* A script for the player's magic casting mechanics.
* The way this is supposed to work is wherever the user clicks on the screen, provided they aren't clicking on a UI element, then
* a spell should be cast at that spot.
*/
//A macro for testing; comment out to remove testing functionalities
//#define TESTING_SPELLCAST
//#define TESTING_SPELLMOVEMENT
//#define TESTING_MOUSE_UP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerCastSpell : MonoBehaviour {

	#if TESTING_SPELLCAST
	[SerializeField] private GameObject m_MagicCubePrefab;
	#endif
	/**A reference to our default spell prefab.*/
	[SerializeField] public GameObject m_SpellCube;
	/**A reference to an instantiated spell cube (instantiated from the reference to the default spell prefab), to have its Spell component set accordingly.*/
    public GameObject m_SpellCubeInstance;

	/**A manager for the spell animator controllers*/
	[SerializeField] private SpellAnimatorManager m_SpellAnimatorManager;

	[SerializeField] private SpellEffectManager m_SpellEffectManager;

    private GameObject m_Target;
	/**A reference to our main camera.*/
	[SerializeField] private Camera m_MainCamera;
	/**A reference to the player, so we can affect his parameters as a result of magic usage.*/
	private Player m_Player;

	/**A string variable containing the string name of the Input Manager variable responsible for player firing off spells.*/
	private readonly string STRINGKEY_INPUT_CASTSPELL = "Cast Spell";
	/**A string variable containing the string name of the isCastingSpell parameter in the player animator.*/
	private readonly string STRINGKEY_PARAM_CASTSPELL = "isCastingSpell";
	/**A bool variable to let us know whether or not the player's in the process of casting a spell.*/
	public bool m_isCastingSpell;

	/**The player animator, including the bit about casting spells.*/
	private Animator m_Animator;
    
	/**The reference to the SpellClass which is fired by the player.*/
	public SpellClass m_SpellClassToFire;

	public PlayerAudio m_playerAudio;

	/**A variable for testing purposes*/
	public string m_SpellName;

	/**The number of seconds until we destroy the spell gameobject.*/
	private readonly float TIME_UNTIL_DESTROY = 1.25f;
	/**A vector to help us know what the offset is for a given AOE animation, with respect to the player click position.*/
	public Vector3 AOE_offset = new Vector3(0.0f, 2.8f, 4.20f);
	/**A variable to keep track of the AOE radius.*/
	public float AOE_Radius = 15.0f;
	/**A variable to ensure AOE attacks don't apply themselves to the enemy for every frame.*/
	public float AOE_Timer = 0.0f;
	/**A reference to the floor.*/
	[SerializeField] private GameObject m_Floor;

	/**A multiplier for mana drain for when the user holds down the mouse casting a given spell*/
	public float m_ManaDrainTimer = 0.0f;
	/**A variable to ensure the spell class instance gameobject is destroyed.*/
	public float m_ManaToDrain = 0.0f;
	/**A bool to tell us whether or not a given menu is open; if so, the player shouldn't be able to cast spells.*/
	public bool m_MenuOpen = true;

	private float m_InputTimer = 0.0f;

	void Start()
	{
		this.m_Animator = this.GetComponent<Animator> ();
		this.m_Player = this.GetComponent<Player> ();
		this.m_SpellClassToFire = this.GetComponent<PlayerInventory> ().m_ActiveSpellClass;
		this.m_playerAudio = this.GetComponent<PlayerAudio> ();
    }

	/**A function to be called if the long list of conditions for spell-casting to be valid are met; assigns the respective movement for the corresponding spell type. 
	 * When this function is called the player could be either pressing or holding the mouse.*/
	private void AssignSpellMovement()
	{
		//If the input button is tapped
		if (Input.GetButtonDown (STRINGKEY_INPUT_CASTSPELL)) {
//			Debug.Log ("Active spell: " + this.m_Player.GetComponent<PlayerInventory> ().m_ActiveSpellClass.m_SpellName.ToString ());

			if (!this.m_SpellClassToFire.m_IsPersistent) {
//				Debug.Log ("The mouse was pressed");
				switch ((int)this.m_SpellClassToFire.m_SpellType) {
				case (int)SpellType.BASIC_PROJECTILE_ON_TARGET:
					{
						//All basic projectile spells have the same movement pattern

						Ray ray = this.m_MainCamera.ScreenPointToRay (Input.mousePosition);
						RaycastHit[] targets_hit = Physics.RaycastAll (ray);
						//We need to find the raycast hit furthest from the camera in the event that none of the raycasthits are 
						//mobile character as the furthest raycast hit will be the ground.
						RaycastHit furthest = targets_hit [0];
						bool any_mobile_characters = false;
						foreach (RaycastHit hit in targets_hit) {

							//if the hit's distance is greater than that of the furthest...
							if (hit.distance > furthest.distance) {
								//...then update the furthest
								furthest = hit;
							}//end if

							//if the hit has a MobileCharacter component...
							if (hit.collider.gameObject.GetComponent<MobileCharacter> () != null) {
								m_Target = hit.collider.gameObject;
								this.m_SpellCubeInstance = GameObject.Instantiate (this.m_SpellCube);
								this.m_Player.m_audioSource.PlayOneShot(this.m_Player.m_PlayerAudio.getAudioForSpell(this.m_SpellClassToFire.m_SpellName));
								this.m_SpellCubeInstance.transform.position = this.transform.position;
								SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
								spell_movement.m_IsMobileCharacter = true;
								spell_movement.SetTarget (hit);
								any_mobile_characters = true;
								spell_movement.SetSpellToCast (this.m_SpellClassToFire);

								this.m_SpellAnimatorManager.SetSpellAnimator (this.m_SpellCubeInstance);
								//return to ensure we only launch one spell
								return;
							}//end if
						}//end foreach

						//if none of the gameobjects found in the raycastall were mobile characters...
						if (!any_mobile_characters) {
							//...then send the spell to the furthest Raycast hit
							#if TESTING_SPELLMOVEMENT
							Debug.Log("PlayerCastSpell::Update\tNo mobile characters found\tRay hit\tx: " + furthest.point.x
							+ " y: " + furthest.point.y + " z: " + furthest.point.z);
							#endif
							this.m_SpellCubeInstance = GameObject.Instantiate (this.m_SpellCube);
							this.m_Player.m_audioSource.PlayOneShot(this.m_Player.m_PlayerAudio.getAudioForSpell(this.m_SpellClassToFire.m_SpellName));
							this.m_SpellCubeInstance.transform.position = this.transform.position;
							SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
							spell_movement.m_IsMobileCharacter = false;
							spell_movement.SetTarget (furthest);
//							spell_movement.m_MaximalVelocity = 10.0f;
							spell_movement.SetSpellToCast (this.m_SpellClassToFire);

							this.m_SpellAnimatorManager.SetSpellAnimator (this.m_SpellCubeInstance);

							GameObject.Destroy (this.m_SpellCubeInstance, TIME_UNTIL_DESTROY);
						}//end if
							

						this.m_Player.AffectMana (-this.m_SpellClassToFire.m_ManaCost / this.m_Player.m_MagicAffinity);

						break;
					}//end case basic projectile on target
				}//end switch
			}//end if active spell is not persistent
		}//end if mouse is pressed
		//else if mouse is held
		else if ((Input.GetButton (STRINGKEY_INPUT_CASTSPELL)))
		{
			//if the active spell is persistent
			if (this.m_SpellClassToFire.m_IsPersistent) {
//				Debug.Log ("The mouse was held");
				switch ((int)this.m_SpellClassToFire.m_SpellType) {
				case (int)SpellType.ON_PLAYER:
					{
						//Any spell cast on the player has the same basic movement pattern

						//if the spell cube instance is null (which can only mean the last spell cube was destroyed)...
						if (this.m_SpellCubeInstance == null) {
							//...then create a new spell cube
							this.m_SpellCubeInstance = GameObject.Instantiate (this.m_SpellCube);
							this.m_Player.m_audioSource.PlayOneShot (this.m_Player.m_PlayerAudio.getAudioForSpell (this.m_SpellClassToFire.m_SpellName));
							this.m_SpellCubeInstance.transform.position = this.transform.position;
							SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
							spell_movement.SetSpellToCast (this.m_SpellClassToFire);
							this.m_SpellAnimatorManager.SetSpellAnimator (this.m_SpellCubeInstance);
						}//end if
						//if the spell cube instance exists (meaning we're in the process of casting a spell)...
						if (this.m_SpellCubeInstance != null) {
							//...then ensure the spell's always at our chosen position
							SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
							spell_movement.MaintainPosition (this.transform.position);
						}//end if the spell cube instance exists



						//However, most (if not all) on-player spells have different effects on both enemies and player mana.
						this.m_Player.AffectMana(-(this.m_SpellClassToFire.m_ManaCost / this.m_Player.m_MagicAffinity) * Time.deltaTime);
						switch ((int)this.m_SpellClassToFire.m_SpellName) {
						case (int)SpellName.Shield:
							{
//								this.m_Player.AffectMana (-this.m_SpellClassToFire.SHIELD_MANA_COST);
								this.m_Player.m_IsShielded = true;
								break;
							}//end case Shield
						case (int)SpellName.Heal:
							{
//								this.m_Player.AffectMana (-this.m_SpellClassToFire.HEAL_MANA_COST);
								//Some arbitrary value just to test
								this.m_Player.AffectHealth (2.0f);
								break;
							}//end case Heal
						}//end switch

						break;
					}//end case ON PLAYER
				case (int)SpellType.AOE_ON_TARGET:
					{
						//Any AOE spell cast on a target has the same basic movement pattern

						//if the spell cube instance is null (which can only mean the last spell cube was destroyed)...
						if (this.m_SpellCubeInstance == null) {
							//...then create a new spell cube
							this.m_SpellCubeInstance = GameObject.Instantiate (this.m_SpellCube);
							this.m_Player.m_audioSource.PlayOneShot (this.m_Player.m_PlayerAudio.getAudioForSpell (this.m_SpellClassToFire.m_SpellName));
							this.m_SpellCubeInstance.transform.position = this.transform.position;
							SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
							spell_movement.SetSpellToCast (this.m_SpellClassToFire);
							this.m_SpellAnimatorManager.SetSpellAnimator (this.m_SpellCubeInstance);
						}//end if
						//if the spell cube instance exists (meaning we're in the process of casting a spell)...
						if (this.m_SpellCubeInstance != null) {
							//...then ensure the spell's always at our chosen position
							SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
							Ray ray = this.m_MainCamera.ScreenPointToRay (Input.mousePosition);
							RaycastHit[] targets_hit = Physics.RaycastAll (ray);

							RaycastHit target = targets_hit [0];
							bool any_mobile_characters = false;
							foreach (RaycastHit hit in targets_hit) {
								//if the hit's distance is greater than that of the furthest...
								if (hit.collider.gameObject == this.m_Floor) {
									target = hit;
									break;
								}
							}//end foreach
							//Set position of AOE spell animation
							Vector3 modified_target = new Vector3 (target.point.x, 0.0f, target.point.z);
							Vector3 position = modified_target + AOE_offset;
							spell_movement.MaintainPosition (position);

							//If we're just starting to cast the spell...
							if (this.AOE_Timer == 0.0f) {
								//... then apply the AOE to the enemies
								this.ApplyAOEToEnemies (position - AOE_offset);
								//Then set AOE timer to 1, to avoid repeating the base case
								this.AOE_Timer = 1.0f;
							}//end if
							//else if the AOE timer is greater than or equal to 1 + the time it takes for the specific AOE spell to wear off...
							else if (this.AOE_Timer >= 1.0f + this.m_SpellClassToFire.m_EffectDuration) {
								//...then apply damage to the enemies and reset AOE timer to 1.0f
								this.ApplyAOEToEnemies (position - AOE_offset);
								this.AOE_Timer = 1.0f;
							}//end else if
							//else increment AOE timer by time.delta time
							else {
								this.AOE_Timer += Time.deltaTime;
							}//end else
						}//end if the spell cube instance exists

						//However, most (if not all) AOE on-target spells have different effects on both enemies and player mana.
						this.m_Player.AffectMana (-(this.m_SpellClassToFire.m_ManaCost / this.m_Player.m_MagicAffinity) * Time.deltaTime);
						switch ((int)this.m_SpellClassToFire.m_SpellName) {
						case (int)SpellName.Thunderstorm:
							{
								//Nothing really special about thunderstorm
//								this.m_Player.AffectMana (-this.m_SpellClassToFire.THUNDERSTORM_MANA_COST);
								break;
							}//end case thunderstorm
						}//end switch

						break;
					}//end case AOE ON TARGET
				}//end switch
			}//end if active spell is persistent
		}//end if mouse is held
	}

	/**A function to return the player to normal if they're casting an active spell and run out of mana or let go of the spellcasting button*/
	private void NormalizePlayerStatus()
	{
		if (this.m_SpellClassToFire.m_SpellType == SpellType.ON_PLAYER) {
			switch ((int)this.m_SpellClassToFire.m_SpellName) {
			case (int)SpellName.Shield:
				{
					this.m_Player.m_IsShielded = false;
					break;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {

//		if (this.m_MenuOpen) {
//			Debug.Log ("A menu is open");
//		}

		//If the player has no active spell...
		if (this.GetComponent<PlayerInventory> ().m_ActiveSpellClass == null) {
			//don't waste your time executing further
//			Debug.Log("Active spell is null");
			return;
		}//end if

		//Same story if the player is having a conversation...
		if (this.GetComponent<PlayerInteraction> ().m_IsTalking) {
			return;
		}

		//And don't cast spells if the player touches a menu button
		if (this.m_MenuOpen) {
			return;
		}

//		Debug.Log (this.m_SpellClassToFire != null
//			&& this.m_SpellCubeInstance != null
//			&& this.m_SpellClassToFire.m_IsPersistent
//			&& (Input.GetButtonUp (STRINGKEY_INPUT_CASTSPELL) || !this.m_Player.m_CanCastSpells));
//		
		//if the active spell exists
		//AND the spell cube instance (representative of the spell instance in the scene) exists
		//AND the active spell in question is persistent
		//AND either the user lets go of the mouse OR the player runs out of mana
		if (this.m_SpellClassToFire != null
			&& this.m_SpellCubeInstance != null
			&& this.m_SpellClassToFire.m_IsPersistent
			&& (Input.GetButtonUp (STRINGKEY_INPUT_CASTSPELL) || !this.m_Player.m_CanCastSpells))
		{
			//...then destroy the spell instance in the scene
			GameObject.Destroy(this.m_SpellCubeInstance);
			this.m_isCastingSpell = false;
			this.NormalizePlayerStatus ();
		}//end if

		//Input management

		//If the player at all touches the spell-casting input button...
		if (Input.GetButton (STRINGKEY_INPUT_CASTSPELL)) {
			//Increment input timer to differentiate between a tap and a hold
//			this.m_InputTimer += Time.deltaTime;
			//...and so long as the player wasn't clicking a UI button...
			if (!this.PlayerIsClickingUIButton ()) {
				//...and if the player has a currently active spell at this point...
				if (this.m_SpellClassToFire != null) {
					//...and if the player is capable of casting a spell mana-wise...
					if (this.m_Player.m_CanCastSpells) {
						this.m_isCastingSpell = true;
						//...then set up the spell movement for the corresponding spell
						this.AssignSpellMovement ();
					}//end if player has mana

				}//end if active spell exists
			
			}//end if player isn't clicking UI button
			//else if the player is clicking on a UI button...
			else {
				//...then update the corresponding bool and break us out of here
//				this.m_MenuOpen = true;
				return;
			}
		}//end if player touched mouse input
		//else if player finishes casting a spell
		else if (Input.GetButtonUp (STRINGKEY_INPUT_CASTSPELL)) {
			this.m_isCastingSpell = false;
			this.AOE_Timer = 0.0f;
		}
//		//else if player didn't touch mouse input
//		else {
//			this.m_isCastingSpell = false;
//			this.AOE_Timer = 0.0f;
//		}


		this.UpdateAnimatorParameters ();

		return;



	}//end f'n void Update()

	/**A private helper function to tell us whether or not when the user clicks they are in fact clicking on a UI button*/
	private bool PlayerIsClickingUIButton()
	{
		//First check to see if we're trying to toggle a menu
		RaycastHit menu_check_hit;
		Ray menu_check_ray = this.m_MainCamera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast(menu_check_ray, out menu_check_hit))
		{
			if (menu_check_hit.collider.gameObject.GetComponent<UnityEngine.UI.Button> () != null) {
				return true;
			}
		}
		return false;
	}

//	/**A function to return an AOE spell's duration*/
//	private float GetAOESpellDuration(SpellName spell_name)
//	{
//		float value_to_return = 0.0f;
//		switch ((int)spell_name) {
//		case (int)SpellName.Thunderstorm:
//			{
//				//the Enemy class has this as TIME_BEFORE_SHOCK_RELEASE; might need to restructure the code...
//				value_to_return = 1.25f;
//				break;
//			}//end case Thunderstorm
//		default:
//			{
//				//Impossible
//				break;
//			}//end case default
//		}//end switch
//		return value_to_return;
//	}//end f'n float GetAOESpellDuration(SpellName)
		
	/**A function to apply AOE spells effects to all nearby enemies in a given radius.*/
	private void ApplyAOEToEnemies(Vector3 position)
	{
		//Kind of a cool effect?
//		GameObject test = GameObject.Instantiate (this.m_SpellCube);
//		test.transform.position = position;
//		GameObject.Destroy (test, 2.0f);

		//Get all nearby collisions in a sphere at the specified position, in a radius [this.AOE_Radius]
		Collider[] all_hit = Physics.OverlapSphere (position, this.AOE_Radius);
		foreach (Collider hit in all_hit) {
			//Note: enemies are all surrounded by a box collider for their actual person; capsule colliders are for their
			//detection regions. We only want this to apply to the box collider
			//So if the hit collider is a box collider
				//AND the hit collider has an Enemy component...
//			if (hit is BoxCollider
//			    && hit.gameObject.GetComponent<Enemy> () != null) {
//				Debug.Log ("Enemy Collider hit");
//				//...then apply the spell damage to that enemy
//				hit.gameObject.GetComponent<Enemy>().ApplySpellEffects(this.m_SpellClassToFire.m_SpellName);
//			}//end if

			if (hit is BoxCollider
			    && hit.gameObject.GetComponent<ICanBeDamagedByMagic> () != null
				&& hit.gameObject.GetComponent<Player>() == null) {
				hit.gameObject.GetComponent<DefaultEnemy>().ApplySpellEffect(this.m_SpellClassToFire);
			}
		}//end foreach
	}//end f'n void ApplyAOEToEnemies(Vector3)

	/**A function to return the position to maintain of an immobile spell (an immobile spell could be cast either on the player or on an enemy, as it turns out).*/
	private Vector3 PositionToMaintain()
	{
		Vector3 position_to_maintain = new Vector3 ();
		switch ((int)this.m_SpellClassToFire.m_SpellName) {
		case (int)SpellName.Shield:
			{
				position_to_maintain = this.transform.position;
				break;
			}//end case Shield
		case (int)SpellName.Heal:
			{
				position_to_maintain = this.transform.position;
				break;
			}
		default:
			{
				//impossible
				break;
			}//end case default
		}//end switch
		return position_to_maintain;
	}//end f'n Vector3 PositionToMaintain()

	/**A function to neatly apply all player attributes as a result of a given magic.
	*For instance, we'll use this function to apply the [IsShielded] to the Player class.*/
	private void ApplyPlayerAttributesAsResultOfMagic()
	{
		//***SHIELD
		//if we're casting a spell and the spell we're firing is the shield then the player is shielded.
		this.m_Player.m_IsShielded = (this.m_isCastingSpell && 
										this.m_SpellClassToFire.m_SpellName == SpellName.Shield) ? true : false;
	}//end f'n void ApplyPlayerAttributesAsResultOfMagic()


	/**Used to retrieve the current spell from the inventory.*/
	private void UpdateChosenSpell()
	{
		this.m_SpellClassToFire = this.gameObject.GetComponent<PlayerInventory>().m_ActiveSpellClass;
		this.m_SpellName = this.m_SpellClassToFire.m_SpellName.ToString ();
			

	}//end f'n void UpdateChosenSpell()

	/**A function to update the player animator with regards to the player spell casting animations.*/
	private void UpdateAnimatorParameters()
	{
		this.m_Animator.SetBool (STRINGKEY_PARAM_CASTSPELL, this.m_isCastingSpell);

	}
}//end class PlayerCastSpell
