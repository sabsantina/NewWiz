//#define TESTING_SPELLMOVEMENT
#define TESTING_SPELLCOLLISION

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;


//We need a rigidbody for collisions to go off properly
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
//Need an animator to animate the spells
[RequireComponent(typeof(Animator))]
public class SpellMovement : MonoBehaviour {

	/**A reference to the mobile character's animator.*/
	private Animator m_Animator; 

	/**A string variable containing the string name of the isMovingLeft parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGLEFT = "isMovingLeft";
	/**A string variable containing the string name of the isMovingRight parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGRIGHT = "isMovingRight";
	/**A string variable containing the string name of the isMovingUp parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGUP = "isMovingUp";
	/**A string variable containing the string name of the isMovingDown parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGDOWN = "isMovingDown";
    /**A string variable containing the string name of the isMoving parameter in the character animator.*/
    private readonly string STRINGKEY_PARAM_ISMOVING = "isMoving";

	/**A private bool to help us keep track of whether the character's moving leftward, and send the result to the character animator.*/
	protected bool m_IsMovingLeft = false;
	/**A private bool to help us keep track of whether the character's moving rightward, and send the result to the character animator.*/
	protected bool m_IsMovingRight = false;
	/**A private bool to help us keep track of whether the character's moving upward, and send the result to the character animator.*/
	protected bool m_IsMovingUp = false;
	/**A private bool to help us keep track of whether the character's moving downward, and send the result to the character animator.*/
	protected bool m_IsMovingDown = false;

	public float m_MaximalVelocity = 30.0f;
	/**A reference to the target the spell is being cast at. Set using SpellMovement::SetTarget(GameObject).*/
    public RaycastHit m_Target { get; private set; }
	/**A reference to the gameobject of [this.m_Target] - this will be for the spells to lock on in case the enemy or target is moving.*/
	public GameObject m_TargetedObj;
	/**The direction in which the spell is moving.*/
	private Vector3 m_Direction = new Vector3();
	/**A bool to let us know whether the target is a mobile character. 
	 * This variable essentially decides whether or not a basic projectile spell locks on to a given target */
	public bool m_IsMobileCharacter { get; set;}

	/**The spell we're currently casting.*/
//	public Spell m_SpellToCast;
	/**The SpellClass spell we're currently casting.*/
	public SpellClass m_SpellClassToCast;

	public SpellEffectManager m_SpellEffectManager;

	//For testing purposes
	public string m_SpellName;

//	private float m_OnTriggerEnterTimer = 0.0f;

	void Awake()
	{
		this.gameObject.GetComponent<Collider> ().isTrigger = true;
		Rigidbody spell_rigidbody = this.gameObject.GetComponent<Rigidbody> ();
		//Disable gravity
		spell_rigidbody.useGravity = false;
		//Freeze effects of physics on rotation and position
//		spell_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		spell_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		//Ensure we can use collisions properly.
		spell_rigidbody.detectCollisions = true;

		this.m_Animator = this.GetComponent<Animator> ();
	}


	// Update is called once per frame
	void FixedUpdate () {
		//if the spell we want to cast exists and is a basic projectile spell...
		if (this.m_SpellClassToCast != null 
			&& this.m_SpellClassToCast.m_SpellType == SpellType.BASIC_PROJECTILE_ON_TARGET) {
			//...if our target is mobile...
			if (this.m_IsMobileCharacter) {
				//Update direction
				this.SetDirection ();
			}//end if

			#if TESTING_SPELLMOVEMENT
			string message = "SpellMovement::Update::Direction of spell:\tx:" + this.m_Direction.x + "\ty:" + this.m_Direction.y +
			"\tz:" + this.m_Direction.z;
			Debug.Log(message);
			#endif


			Rigidbody rigidbody = this.GetComponent<Rigidbody> ();
			rigidbody.velocity = Vector3.Normalize (this.m_Direction) * this.m_MaximalVelocity;

			this.UpdateAnimatorParameters ();
		}//end if
	}//end f'n void Update()

	/**Set the target at which we're shooting the magic.*/
	public void SetTarget(RaycastHit spell_target)
	{
		//Set the target
		this.m_Target = spell_target;

		if (this.m_IsMobileCharacter) {
			//Set the target's gameobject, to be able to follow it around if it's moving
			this.m_TargetedObj = this.m_Target.collider.gameObject;
		} else {
			this.m_TargetedObj = null;
			//...then send the spell in the direction of wherever the cursor was clicked
			this.m_Direction = Vector3.Normalize(this.m_Target.point - this.transform.position) * this.m_MaximalVelocity;
			this.m_Direction.y = 0.0f;
		}
	}//end f'n void SetTarget(RaycastHit)

	/**A function to be called from the RangedEnemy's AttackPattern, to set the position of the target of magic.*/
	public void SetEnemyTarget(GameObject spell_target)
	{
		this.m_TargetedObj = null;
		//...then send the spell in the direction of wherever the cursor was clicked
		this.m_Direction = Vector3.Normalize(spell_target.transform.position - this.transform.position) * this.m_MaximalVelocity;
		this.m_Direction.y = 0.0f;
	}//end f'n void SetTarget(RaycastHit)

	/**A function to tell the spells where to go from another class; this will be helpful for those spells who aren't mobile, where the player may move.*/
	public void MaintainPosition(Vector3 position)
	{
		this.transform.position = position;
	}//end f'n void MaintainPosition(Vector3)

	/**A private function to set the direction to the given target [this.m_Target].*/
	private void SetDirection()
	{
		//Note on the following if: it's possible that we destroy the enemy while spells are still on their way to them
		//if the target is a mobile character and exists...
		if (this.m_IsMobileCharacter && this.m_TargetedObj != null) {
			//...then lock onto the mobile character's position
			Vector3 target_position = this.m_TargetedObj.transform.position;
			this.m_Direction = Vector3.Normalize(target_position - this.transform.position) * this.m_MaximalVelocity;
		}//end if 
		//else if the target is a mobile character but has since been destroyed...
		if (this.m_IsMobileCharacter && this.m_TargetedObj == null) {
			//...then change the [m_IsMobileCharacter] value.
			this.m_IsMobileCharacter = false;
		}//end if 
		//else if the target is not a mobile character...
		//...then make no change to the direction
	}//end f'n void SetDirection()

	/**A function to be called from PlayerCastSpell to set the SpellClass that's being cast.*/
	public void SetSpellToCast(SpellClass spell)
	{
		this.m_SpellClassToCast = spell;
		this.m_SpellName = this.m_SpellClassToCast.m_SpellName.ToString ();
	}

    /**A function to be called whenever something enters a spellmovement collider; in terms of functionality, we'll use this function to destroy the spell object prefab after it strikes with something's collider.*/
    void OnTriggerEnter(Collider other)
    {
		//if the spell we're casting exists but isn't a basic projectile...
		if (this.m_SpellClassToCast != null && this.m_SpellClassToCast.m_SpellType != SpellType.BASIC_PROJECTILE_ON_TARGET) {
			//...then we don't really care about collisions
			return;
		}//end if
		//else if the spell we're casting is a basic projectile...
		else {
			//if we hit the target and specifically NOT the enemy's detection collider...
			if (other.gameObject.GetComponent<ICanBeDamagedByMagic> () != null) {
				Vector3 vector_from_other_to_spell_instance = (this.transform.position - other.transform.position);
				//if the vector from the [other] to the spell is in roughly the same direction as the spell is moving, then [other] is the one casting the spell
				//else odds are [other] is the target of the spell
				//so if the dot product of the two vectors is negative, then the spell is hitting the target of the spell, [other]
				if (Vector3.Dot (this.m_Direction, vector_from_other_to_spell_instance) < 0) {
					Debug.Log (other.gameObject.name + " is target");
					other.gameObject.GetComponent<ICanBeDamagedByMagic> ().ApplySpellEffect (this.m_SpellClassToCast);
					GameObject.Destroy (this.gameObject);
				}//end if
			}//end if gameobject has ICanBeDamagedByMagic interface component
			//if the spell hits a part of the scenery...
			else if (other.gameObject.GetComponent<Obstructable> ()) {
				//...destroy it (we don't want stuff like spells going through trees)
				GameObject.Destroy (this.gameObject);
			}//end if
		}//end if

    }//end f'n void OnTriggerEnter(Collider)

	/**A function to update the animator parameters, with regard to motion.*/
	public void UpdateAnimatorParameters()
	{
		this.m_IsMovingRight = (this.m_Direction.x > 0) ? true : false;
		this.m_IsMovingLeft = (this.m_Direction.x < 0) ? true : false;
		this.m_IsMovingUp = (this.m_Direction.z > 0) ? true : false;
		this.m_IsMovingDown = (this.m_Direction.z < 0) ? true : false;
       
        //update for downward motion
        this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGDOWN, this.m_IsMovingDown);
		//update for upward motion
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGUP, this.m_IsMovingUp);
		//update for leftward motion
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGLEFT, this.m_IsMovingLeft);
		//update for rightward motion
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGRIGHT, this.m_IsMovingRight);
        //update for motion in general
		this.m_Animator.SetBool(STRINGKEY_PARAM_ISMOVING, this.m_IsMovingDown || this.m_IsMovingLeft || this.m_IsMovingRight || this.m_IsMovingUp);

    }//end f'n void UpdateAnimatorParameters()

	/**A function to set the animator controller with respect to the type of spell.*/
	public void SetAnimatorController(AnimatorController spell_animator_controller)
	{
		this.m_Animator.runtimeAnimatorController = spell_animator_controller as RuntimeAnimatorController;
	}
}
