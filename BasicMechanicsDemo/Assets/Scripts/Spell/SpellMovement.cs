//#define TESTING_SPELLMOVEMENT
#define TESTING_SPELLCOLLISION

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMovement : MonoBehaviour {

	public float m_MaximalVelocity = 35.0f;
	/**A reference to the target the spell is being cast at. Set using SpellMovement::SetTarget(GameObject).*/
	public RaycastHit m_Target { get; private set; }
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
		}//end if
	}//end f'n void Update()

	/**Set the target at which we're shooting the magic.*/
	public void SetTarget(RaycastHit spell_target)
	{
		this.m_Target = spell_target;
		this.m_TargetFound = true;
//		this.SetDirection ();
	}//end f'n void SetTarget(GameObject)

	/**A private function to set the direction to the given target [this.m_Target].*/
	private void SetDirection()
	{
		float y_coordinate = this.transform.position.y;
		Vector3 target_position = new Vector3 (this.m_Target.point.x, y_coordinate, this.m_Target.point.z);
		this.m_Direction = (target_position - this.transform.position) * this.m_MaximalVelocity;
		#if TESTING_SPELLMOVEMENT
		string message = "SpellMovement::SetDirection::Direction of spell:\tx:" + this.m_Direction.x + "\ty:" + this.m_Direction.y +
			"\tz:" + this.m_Direction.z;
		Debug.Log(message);
		#endif
	}//end f'n void SetDirection()

	void OnTriggerEnter(Collider other)
	{
		//if we hit the target...
		if (other.gameObject == this.m_Target.collider.gameObject) {
			this.m_TargetReached = true;
			#if TESTING_SPELLCOLLISION
			Debug.Log("SpellMovement::OnTriggerEnter(Collider)\tTarget hit!");
			#endif
		}
	}
}
