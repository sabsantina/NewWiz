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
	#if TESTING_SPELLMOVEMENT
	[SerializeField] private GameObject m_SpellCube;
	private GameObject m_SpellCubeInstance;
	#endif

	/**A reference to our main camera.*/
	[SerializeField] private Camera m_MainCamera;

	/**A string variable containing the string name of the Input Manager variable responsible for player firing off spells.*/
	private readonly string STRINGKEY_INPUT_CASTSPELL = "Cast Spell";
	/**A string variable containing the string name of the isCastingSpell parameter in the player animator.*/
	private readonly string STRINGKEY_PARAM_CASTSPELL = "isCastingSpell";
	/**A bool variable to let us know whether or not the player's in the process of casting a spell.*/
	private bool m_isCastingSpell;
	/**The player animator, including the bit about casting spells.*/
	private Animator m_Animator;

	private SpellMovement m_SpellMovement;

	/**The number of seconds until we destroy the spell gameobject.*/
	private readonly float TIME_UNTIL_DESTROY = 2.0f;

	void Start()
	{
		this.m_Animator = this.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		//if the player clicks mouse 0, the button responsible for firing off magic...
		if (Input.GetButtonDown (STRINGKEY_INPUT_CASTSPELL)) {
			//Update [this.m_isCastingSpell] for the animator
			this.m_isCastingSpell = true;

			//...then cast a ray to wherever the player clicked on...
			Ray ray_to_target = this.m_MainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit target_hit;
			//if the ray hits something...
			if (Physics.Raycast (ray_to_target, out target_hit)) {
				#if TESTING_SPELLCAST
				//and if we're testing the spellcast and instantiating a cube...
				//...then instantiate the cube...
				this.m_MagicCubePrefab = GameObject.Instantiate(this.m_MagicCubePrefab);
				//...and move it over to where we clicked
				float cube_height = this.m_MagicCubePrefab.transform.lossyScale.z;
				this.m_MagicCubePrefab.transform.position = new Vector3(target_hit.point.x, target_hit.point.y + cube_height / 2.0f, target_hit.point.z);
				#endif
				#if TESTING_SPELLMOVEMENT
				this.m_SpellCubeInstance = GameObject.Instantiate(this.m_SpellCube);
				this.m_SpellCubeInstance.transform.position = this.transform.position;
				SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement>();
//				Debug.Log("PlayerCastSpell::Update()\tTarget: " + target_hit.collider.gameObject.name);
				spell_movement.SetTarget(target_hit);
				GameObject.Destroy(this.m_SpellCubeInstance, TIME_UNTIL_DESTROY);
				#endif

			}//end if
		}//end if
		//else if the player doesn't click (doesn't fire a spell)...
		else {
			//Update [this.m_isCastingSpell] for the animator
			this.m_isCastingSpell = false;
		}


		this.UpdateAnimatorParameters ();
	}//end f'n void Update()

	/**A function to update the player animator with regards to the player spell casting animations.*/
	private void UpdateAnimatorParameters()
	{
		this.m_Animator.SetBool (STRINGKEY_PARAM_CASTSPELL, this.m_isCastingSpell);

	}
}//end class PlayerCastSpell
