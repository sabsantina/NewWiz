/*
* A script for the player's magic casting mechanics.
* The way this is supposed to work is wherever the user clicks on the screen, provided they aren't clicking on a UI element, then
* a spell should be cast at that spot.
*/
//A macro for testing; comment out to remove testing functionalities
//#define TESTING_SPELLCAST
#define TESTING_SPELLMOVEMENT


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCastSpell : MonoBehaviour {

	#if TESTING_SPELLCAST
	[SerializeField] private GameObject m_MagicCubePrefab;
	#endif
	public GameObject m_SpellCube;
    private GameObject m_SpellCubeInstance;

    private GameObject m_Target;
	/**A reference to our main camera.*/
	[SerializeField] private Camera m_MainCamera;

	/**A string variable containing the string name of the Input Manager variable responsible for player firing off spells.*/
	private readonly string STRINGKEY_INPUT_CASTSPELL = "Cast Spell";
	/**A string variable containing the string name of the isCastingSpell parameter in the player animator.*/
	private readonly string STRINGKEY_PARAM_CASTSPELL = "isCastingSpell";
	/**A bool variable to let us know whether or not the player's in the process of casting a spell.*/
	private bool m_isCastingSpell;

    private bool m_hasCastHittingSpell;
	/**The player animator, including the bit about casting spells.*/
	private Animator m_Animator;
    /**The reference to the spell which is fired by the player.*/
	public Spell m_SpellToFire;
	/**A variable for testing purposes*/
	public string m_SpellName;


	/**The number of seconds until we destroy the spell gameobject.*/
	private readonly float TIME_UNTIL_DESTROY = 1.25f;

	void Awake()
	{
		this.m_SpellToFire = this.gameObject.GetComponent<PlayerInventory>().m_ActiveSpell;
		this.m_SpellName = m_SpellToFire.m_SpellName.ToString();
	}

	void Start()
	{
		this.m_Animator = this.GetComponent<Animator> ();
    }


	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown (STRINGKEY_INPUT_CASTSPELL)) {
			this.CheckChosenSpell ();
			//if the spell to fire exists...
			if (this.m_SpellToFire != null) {
				//Update [this.m_isCastingSpell] for the animator
				this.m_isCastingSpell = true;

				//if the spell is mobile (meaning it's to be cast somewhere away from the player)...
				if (this.m_SpellToFire.m_IsMobileSpell) {
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
							this.m_hasCastHittingSpell = true;
							SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
							spell_movement.m_IsMobileCharacter = true;
							spell_movement.SetTarget (hit);
							any_mobile_characters = true;

							spell_movement.SetSpellToCast (this.m_SpellToFire);

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

						spell_movement.SetSpellToCast (this.m_SpellToFire);
						GameObject.Destroy (this.m_SpellCubeInstance, TIME_UNTIL_DESTROY);

					}//end if
				}//end if
				//else if the spell is not mobile (meaning it's cast at the player's location...)
				//we have no spells that fit this yet, so do nothing.
			
			}//end if
		}//end if
		//else if the user holds down the mouse...
		else if (Input.GetButton(STRINGKEY_INPUT_CASTSPELL)) {
			this.CheckChosenSpell ();
			//if the spell to fire exists...
			if (this.m_SpellToFire != null) {
				//Update [this.m_isCastingSpell] for the animator
				this.m_isCastingSpell = true;

				//if the spell is not mobile (meaning it's to be cast at the player)...
				if (!this.m_SpellToFire.m_IsMobileSpell) {
					/*
					* We need to figure out how to cast immobile spells (i.e. healing, shield).
					*/


				}//end if
				//else if the spell is not mobile (meaning it's cast at the player's location...)
				//we have no spells that fit this yet, so do nothing.

			}//end if
		}
		//else if the player doesn't click (doesn't fire a spell)...
		else {
			//Update [this.m_isCastingSpell] for the animator
			this.m_isCastingSpell = false;
		}//end else



		this.UpdateAnimatorParameters ();
	}//end f'n void Update()

    /**Used to retrieve the current spell from the inventory.*/
    private void CheckChosenSpell()
    {
		this.m_SpellToFire = this.gameObject.GetComponent<PlayerInventory>().m_ActiveSpell;
		if (this.m_SpellToFire != null) {
			Debug.Log ("Chosen spell: " + this.gameObject.GetComponent<PlayerInventory> ().m_ActiveSpell.m_SpellName.ToString());

		}
    }

	/**A function to update the player animator with regards to the player spell casting animations.*/
	private void UpdateAnimatorParameters()
	{
		this.m_Animator.SetBool (STRINGKEY_PARAM_CASTSPELL, this.m_isCastingSpell);

	}
}//end class PlayerCastSpell
