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
	private bool m_isCastingSpell;

	/**The player animator, including the bit about casting spells.*/
	private Animator m_Animator;
    
	/**The reference to the SpellClass which is fired by the player.*/
	public SpellClass m_SpellClassToFire;

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

	void Start()
	{
		this.m_Animator = this.GetComponent<Animator> ();
		this.m_Player = this.GetComponent<Player> ();
		this.m_SpellClassToFire = this.GetComponent<PlayerInventory> ().m_ActiveSpellClass;
    }

	// Update is called once per frame
	void Update () {
		if (this.GetComponent<PlayerInventory> ().m_ActiveSpellClass == null) {
			return;
		}

		if (Input.GetButtonDown (STRINGKEY_INPUT_CASTSPELL)) {
			this.CheckChosenSpell ();
			//if the spell to fire exists...
			if (this.m_SpellClassToFire != null) {

				//Update [this.m_isCastingSpell] for the animator
				this.m_isCastingSpell = true;

				//if the spell is mobile (meaning it's to be cast somewhere away from the player)...
				if (this.m_SpellClassToFire.m_IsMobileSpell) {
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
						this.m_SpellCubeInstance.transform.position = this.transform.position;
						SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
						spell_movement.m_IsMobileCharacter = false;
						spell_movement.SetTarget (furthest);

						spell_movement.SetSpellToCast (this.m_SpellClassToFire);

						this.m_SpellAnimatorManager.SetSpellAnimator (this.m_SpellCubeInstance);

						GameObject.Destroy (this.m_SpellCubeInstance, TIME_UNTIL_DESTROY);

					}//end if
				}//end if
			
			}//end if
		}//end if
		//else if the user holds down the mouse...
		else if (Input.GetButton(STRINGKEY_INPUT_CASTSPELL)) {
			this.CheckChosenSpell ();

			//if the spell to fire exists and is immobile...
			if (this.m_SpellClassToFire != null && !this.m_SpellClassToFire.m_IsMobileSpell) {
				//Update [this.m_isCastingSpell] for the animator
				this.m_isCastingSpell = true;

				//if the spell is not mobile
				//	AND if the spell cube instance is null (which can only mean the last spell cube was destroyed)...
				if (!this.m_SpellClassToFire.m_IsMobileSpell && this.m_SpellCubeInstance == null) {
					//...then create a new spell cube
					this.m_SpellCubeInstance = GameObject.Instantiate (this.m_SpellCube);
					this.m_SpellCubeInstance.transform.position = this.transform.position;
					SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
					spell_movement.SetSpellToCast (this.m_SpellClassToFire);
					this.m_SpellAnimatorManager.SetSpellAnimator (this.m_SpellCubeInstance);

				}//end if
				//if the spell cube instance exists (meaning we're in the process of casting a spell)...
				if (this.m_SpellCubeInstance != null) {
					//...then ensure the spell's always at our chosen position
					SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
//					//get the position to maintain with respect to the spell to cast.

					//if it's an AOE spell...
					if (this.m_SpellClassToFire.m_IsAOESpell) {
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
						Vector3 modified_target = new Vector3(target.point.x, 0.0f, target.point.z);
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
						else if (this.AOE_Timer >= 1.0f + this.GetAOESpellDuration (this.m_SpellClassToFire.m_SpellName)) {
							//...then apply damage to the enemies and reset AOE timer to 1.0f
							this.ApplyAOEToEnemies (position - AOE_offset);
							this.AOE_Timer = 1.0f;
						}//end else if
						//else increment AOE timer by time.delta time
						else {
							this.AOE_Timer += Time.deltaTime;
						}//end else
					}//end if
					//else if it isn't an AOE spell...
					else {
						spell_movement.MaintainPosition (this.transform.position);
					}//end else

					//Reset AOE timer
					this.AOE_Timer = 0.0f;
				}//end if
				//else if the spell is not mobile
				//we have no spells that fit this yet, so do nothing.

			}//end if

		}//end if
		//if the player lets go of the mouse and the spell wasn't a mobile spell (meaning that at this point they're probably still
			//holding down the mouse...
		else if (Input.GetButtonUp (STRINGKEY_INPUT_CASTSPELL) 
			&& !this.m_SpellClassToFire.m_IsMobileSpell) {
			#if TESTING_MOUSE_UP
			Debug.Log ("Mouse up");
			#endif
			//...then the spell is no longer being cast
			this.m_isCastingSpell = false;
			//...and we need to destroy the gameobject instance

			GameObject.Destroy (this.m_SpellCubeInstance);
		}//end if

		//else if the player doesn't click (doesn't fire a spell)...
		else {
			//Update [this.m_isCastingSpell] for the animator
			this.m_isCastingSpell = false;
		}//end else

		//USE THIS SPACE TO UPDATE ANY PLAYER ATTRIBUTES AS A RESULT OF MAGIC
		this.ApplyPlayerAttributesAsResultOfMagic();

		this.UpdateAnimatorParameters ();
	}//end f'n void Update()

	/**A function to return an AOE spell's duration*/
	private float GetAOESpellDuration(SpellName spell_name)
	{
		float value_to_return = 0.0f;
		switch ((int)spell_name) {
		case (int)SpellName.Thunderstorm:
			{
				//the Enemy class has this as TIME_BEFORE_SHOCK_RELEASE; might need to restructure the code...
				value_to_return = 1.25f;
				break;
			}//end case Thunderstorm
		default:
			{
				//Impossible
				break;
			}//end case default
		}//end switch
		return value_to_return;
	}//end f'n float GetAOESpellDuration(SpellName)

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
			if (hit is BoxCollider
			    && hit.gameObject.GetComponent<Enemy> () != null) {
//				Debug.Log ("Collider hit");
				//...then apply the spell damage to that enemy
				hit.gameObject.GetComponent<Enemy>().ApplySpellEffects(this.m_SpellClassToFire.m_SpellName);
			}//end if
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

//	/**A function to return the medium-most time for a clip in the animator.
//	*This is to help us know when to destroy the gameobject such that the fade-out animation can play.*/
//	private float TimeOfEndClip()
//	{
//		float shortest_clip_time = 1000.0f, medium_clip_time = 0.0f, longest_clip_time = 0.0f;
//		foreach(AnimationClip clip in this.m_Animator.runtimeAnimatorController.animationClips)
//		{
//			//...if a given clip's length is greater than that of the longest clip's length...
//			if (clip.length > longest_clip_time) {
//				//...then update the longest clip
//				longest_clip_time = clip.length;
//			}//end if
//			//...if a given clip's length is less than that of the shortest clip's length...
//			if (clip.length < shortest_clip_time) {
//				//...then update the shortest clip
//				shortest_clip_time = clip.length;
//			}//end if
//			//...if a given clip's length is greater-than or equal to the shortest clip length
//			//		AND that same given clip's length is less-than or equal to the longest clip length...
//			if (clip.length >= shortest_clip_time && clip.length <= longest_clip_time) {
//				//...then update the medium clip
//				medium_clip_time = clip.length;
//			}//end if
//		}//end foreach
//		Debug.Log(medium_clip_time);
//		return medium_clip_time + 0.1f;
//	}//end f'n float TimeOfEndClip()

	/**Used to retrieve the current spell from the inventory.*/
	private void CheckChosenSpell()
	{
		this.m_SpellClassToFire = this.gameObject.GetComponent<PlayerInventory>().m_ActiveSpellClass;
		this.m_SpellName = this.m_SpellClassToFire.m_SpellName.ToString ();
			

	}//end f'n void CheckChosenSpell()

	/**A function to update the player animator with regards to the player spell casting animations.*/
	private void UpdateAnimatorParameters()
	{
		this.m_Animator.SetBool (STRINGKEY_PARAM_CASTSPELL, this.m_isCastingSpell);

	}
}//end class PlayerCastSpell
