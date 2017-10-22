/*
* A script to govern enemy movement.
* Note: The entirety of the enemy's movement is governed by a Vector3 [this.m_Direction], which gets consistently added to the
* transform.position. If you alter this variable's value anywhere at all, then the enemy will move in that direction until the 
* direction gets changed again.
*/

//#define TESTING_DETECTION
//#define STAY_STILL

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MobileCharacter {
	
	/**The prefab that defines the area (or volume, rather) of the enemy's detection capabilities.
	*Note that this object shouldn't move in the scene if the enemy is patrolling a fixed region.*/
	private Transform m_DetectionArea;
	/**We can either have the enemy wander to the very edge of the detection area, or have him stop [this.m_DetectionAreaInteriorPadding] units short of the edge, to make sure the enemy doesn't wander out of the detection area.*/
	private float m_DetectionAreaInteriorPadding = 1.0f;
	/**A velocity to be used when the enemy is chasing the player.*/
	public readonly float m_ChasingVelocity = 21.0f;
	/**A bool to tell us whether or not the enemy is chasing the player*/
	public bool m_IsChasing;


	// Use this for initialization
	void Start () {
		this.m_DetectionArea = this.transform.parent;
		this.SetAnimator (this.GetComponent<Animator> ());
		this.m_Direction = new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f));
		this.m_IsChasing = false;
	}
	
	// Update is called once per frame
	void Update () {

		#if TESTING_DETECTION
		if (Input.GetKeyDown (KeyCode.C)) {
			string message = "Detection area radius length: ";
			float result = this.m_DetectionArea.gameObject.GetComponent<CapsuleCollider>().radius;
			message += result;
			message += "\nEnemy left radius? " + this.isLeavingDetectionArea();
			Debug.Log (message);
		}
		#endif
		#if STAY_STILL
		#else
		Vector3 current_position = this.transform.position;
		//Scale up the direction
		this.m_Direction *= this.m_MaximalVelocity;
		//Clamp the direction to the maximal velocity
		this.m_Direction = (this.m_IsChasing) ? Vector3.ClampMagnitude(this.m_Direction, this.m_ChasingVelocity)
												: Vector3.ClampMagnitude(this.m_Direction, this.m_MaximalVelocity);
		//Add to current position
		current_position += this.m_Direction * Time.deltaTime;
		//Update position
		this.transform.position = current_position;

		if (this.isLeavingDetectionArea ()) {
			this.RedirectIntoDetectionArea ();
		}
		#endif
		this.SetAnimatorParameters ();

		this.UpdateAnimatorParameters ();
	}//end f'n void Update

	/**A function to change the direction of the enemy to keep it in the detection area.
	*Note that the resulting [m_Direction] is only applied to THIS gameobject in the Update() function, where Time.deltaTime is applied to it. Thus, we have no need of Time.deltaTime here.*/
	public void RedirectIntoDetectionArea()
	{
		float initial_y = this.transform.position.y;
		float detection_area_radius = this.m_DetectionArea.gameObject.GetComponent<CapsuleCollider> ().radius;
		//Get a random x value within the detection area
		float random_x = Random.Range (this.m_DetectionArea.transform.position.x - detection_area_radius,
			                 this.m_DetectionArea.transform.position.x + detection_area_radius);
		//Get a random z value within the detection area
		float random_z = Random.Range (this.m_DetectionArea.transform.position.z - detection_area_radius,
			this.m_DetectionArea.transform.position.z + detection_area_radius);
		//make a new vector representing the vector from where the enemy is to this new position
		Vector3 new_vector = new Vector3 (random_x, initial_y, random_z) - this.transform.position;

		this.m_Direction = Vector3.ClampMagnitude (new_vector, this.m_MaximalVelocity);
	}//end f'n void RedirectIntoDetectionArea()

	/**A function to check whether the enemy is leaving its alloted patrol region.*/
	private bool isLeavingDetectionArea()
	{
		Vector3 vector_difference = this.m_DetectionArea.transform.position - this.transform.position;
		float distance_from_center = Vector3.Magnitude (vector_difference);
		float detection_area_radius = this.m_DetectionArea.GetComponent<CapsuleCollider> ().radius;
		//if the distance between the enemy and the center of the detection area is greater than the radius of the
		//detection area (minus a little extra interior padding)...
		if (distance_from_center > (detection_area_radius - this.m_DetectionAreaInteriorPadding)) {
			//...then the enemy is beyond the detection area
			return true;
		}//end if
		//else if the distance between the enemy and the center of the detection area is less than the radius of the
		//detection area (minus a little extra interior padding)...
		else {
			//...then we're fine
			return false;
		}//end else
	}//end f'n bool isLeavingDetectionArea()

	/**A function to handle the updating of the animator parameters, before we update them.*/
	private void SetAnimatorParameters()
	{
		//If the direction is positive (or zero) in x, then we're moving rightward; else, we aren't moving rightward.
		this.m_IsMovingRight = (this.m_Direction.x >= 0) ? true : false;
		//If the direction is negative in x, then we're moving leftward; else, we aren't moving leftward.
		this.m_IsMovingLeft = (this.m_Direction.x < 0) ? true : false;
		//If the direction is positive in z, then we're moving upward; else, we aren't moving upward.
		this.m_IsMovingUp = (this.m_Direction.z > 0) ? true : false;
		//If the direction is negative in z, then we're moving downward; else, we aren't moving downward.
		this.m_IsMovingDown = (this.m_Direction.z < 0) ? true : false;
	}//end f'n void SetAnimatorParameters()

	/**A function to set the enemy's direction from another class; this will come in handy if ever we need to tell the enemy to chase
	*the player, or something.*/
	public void SetEnemyDirection(Vector3 direction)
	{
		this.m_Direction = direction;
	}//end f'n void SetEnemyDirection(Vector3)

}//end class EnemyMovement
