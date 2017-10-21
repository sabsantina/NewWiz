//#define TESTING_SPELLMOVEMENT
#define TESTING_SPELLCOLLISION

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovement : MonoBehaviour {

	public float m_MaximalVelocity = 35.0f;
	/**A reference to the target the spell is being cast at. Set using SpellMovement::SetTarget(GameObject).*/
	public RaycastHit m_Target { get; private set; }
	/**A reference to the gameobject of [this.m_Target] - this will be for the spells to lock on in case the enemy or target is moving.*/
	private GameObject m_TargetedObj;
	/**The direction in which the spell is moving.*/
	private Vector3 m_Direction = new Vector3();
	/**A bool to let us know whether or not the spell made it to the target.*/
	public bool m_TargetReached { get; private set; }
	/**A bool to notify the script that it's time to start moving the spell.*/
	public bool m_TargetFound {get; private set;}

	void Awake()
	{
		this.m_TargetReached = false;
		this.m_TargetFound = false;
	}

	// Update is called once per frame
	void Update () {
		#if TESTING_SPELLMOVEMENT
		string message = "SpellMovement::Update::Direction of spell:\tx:" + this.m_Direction.x + "\ty:" + this.m_Direction.y +
			"\tz:" + this.m_Direction.z;
		Debug.Log(message);
		#endif
		//if our direction has a magnitude greater than 0 (which will only happen after the target's been set)...
		if (this.m_TargetFound) {
			this.SetDirection ();

			#if TESTING_SPELLMOVEMENT
			message = "SpellMovement::Update:\tDirection magnitude greater than zero";
			Debug.Log(message);
			#endif

			//get our current position (which will be at whoever cast the spell)
			Vector3 current_position = this.transform.position;
			Vector3 translation = Vector3.ClampMagnitude (this.m_Direction, this.m_MaximalVelocity) * Time.deltaTime;
			//and update the current position
			current_position += translation;
			this.transform.position = current_position;

			this.UpdateTargetReached ();
			if (this.m_TargetReached) {
				GameObject.Destroy (this.gameObject);
			}
		}//end if
	}//end f'n void Update()

	/**Set the target at which we're shooting the magic.*/
	public void SetTarget(RaycastHit spell_target)
	{
		//Set the target
		this.m_Target = spell_target;

		//Set the target's gameobject, to be able to follow it around if it's moving
		this.m_TargetedObj = this.m_Target.collider.gameObject;

		this.m_TargetFound = true;
	}//end f'n void SetTarget(GameObject)

	/**A private function to set the direction to the given target [this.m_Target].*/
	private void SetDirection()
	{
		float y_coordinate = this.transform.position.y;
		Vector3 target_position = new Vector3 ();
		//if the target is a mobile character...
		if (m_TargetedObj.GetComponent<MobileCharacter> () != null) {
			//...then lock onto the mobile character's position
			target_position = this.m_TargetedObj.transform.position;
		}//end if 
		//else if the target is not a mobile character...
		else {
			//...then send the spell to wherever the cursor was clicked
			target_position = this.m_Target.point;
		}//end else
		this.m_Direction = Vector3.Normalize(target_position - this.transform.position) * this.m_MaximalVelocity;
		#if TESTING_SPELLMOVEMENT
		string message = "SpellMovement::SetDirection::Direction of spell:\tx:" + this.m_Direction.x + "\ty:" + this.m_Direction.y +
			"\tz:" + this.m_Direction.z;
		Debug.Log(message);
		#endif
	}//end f'n void SetDirection()

	/**A private function to update the bool telling us whether or not the target's been reached.*/
	private void UpdateTargetReached()
	{
		//if the spell is at the same position as the targeted object (if it was a MobileCharacter)
			//OR if the spell is at the same position as wherever the player clicked (if it wasn't a MobileCharacter...
		if (this.transform.position == this.m_TargetedObj.transform.position
			|| this.transform.position == this.m_Target.point) {
			//...then set [this.m_TargetReached] to true
			this.m_TargetReached = true;
		}//end if
	}//end f'n void UpdateTargetReached()

	void OnTriggerEnter(Collider other)
	{
		//if we hit the target...
		if (other.gameObject == this.m_Target.collider.gameObject) {
			this.m_TargetReached = true;
			#if TESTING_SPELLCOLLISION
			Debug.Log("SpellMovement::OnTriggerEnter(Collider)\tTarget " + this.m_Target.collider.gameObject.name + " hit!");
			#endif
		}
	}
}
