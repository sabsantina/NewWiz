using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPattern : MobileCharacter {

	/**The enemy patrol and detection radius*/
	[SerializeField] public EnemyDetectionRegion m_PatrolRegion;


	/**The state of the movement pattern.*/
	public MovementPatternState m_MovementPatternState = MovementPatternState.ROAM;

	/**The velocity at which an enemy chases a player.*/
	public float m_ChaseVelocity = 200.0f;
	//for testing
	public float m_CurrentVelocity = 0.0f;

	// Use this for initialization
	void Start () {
		this.m_MaximalVelocity = 2.5f;
		this.m_Direction = Vector3.Normalize(new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f))) * this.m_MaximalVelocity;

	}
	
	// Update is called once per frame
	void Update () {
		this.ExecutePatternState ();
	}

	/**A function to check whether or not the player was detected within the enemy's patrol area*/
	public bool IsPlayerDetectedInPatrolRegion()
	{
		return this.m_PatrolRegion.m_PlayerInRegion;
	}

	/**A function to check whether the enemy is leaving its alloted patrol region.*/
	private bool MovingLeavesPatrolRegion(Vector3 displacement)
	{
		Vector3 vector_difference = this.m_PatrolRegion.transform.position - this.transform.position;
		float distance_from_center = Vector3.Magnitude((this.transform.position + displacement) - this.m_PatrolRegion.transform.position);
		float detection_area_radius = this.m_PatrolRegion.GetComponent<SphereCollider> ().radius * this.m_PatrolRegion.transform.lossyScale.x;
		//if the distance between the enemy and the center of the detection area is greater than the radius of the
		//detection area (minus a little extra interior padding)...
		if (distance_from_center > (detection_area_radius - this.m_PatrolRegion.m_InteriorPadding)) {
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

	/**A function to return true if the enemy is moving in the direction of an obstructable body*/
	private bool MovingLeadsToObstructable(Vector3 displacement)
	{
		foreach (RaycastHit hit in Physics.RaycastAll(this.transform.position, displacement, displacement.magnitude)) {
			if (hit.collider.gameObject.GetComponent<Obstructable> () != null) {
				return true;
			}
		}
		return false;
	}

	private bool MovingLeadsToPlayer(Vector3 displacement)
	{
		foreach (RaycastHit hit in Physics.RaycastAll(this.transform.position, displacement, displacement.magnitude)) {
			if (hit.collider.gameObject.GetComponent<Player> () != null) {
				return true;
			}
		}
		return false;
	}


	/**Move the gameobject with respect to the current movement pattern.
	*Note: Updates animator parameters immediately following execution of the actual movement.*/
	private void ExecutePatternState()
	{
		Vector3 displacement = Vector3.zero;
		Vector3 current_position = this.transform.position;

		switch ((int)this.m_MovementPatternState) {
		//If the enemy's movement pattern is set to roam...
		case (int)MovementPatternState.ROAM:
			{
//				Vector3 current_position = this.transform.position;
				displacement = Vector3.Normalize (this.m_Direction) * this.m_MaximalVelocity;
//				this.transform.position = current_position + (displacement * Time.deltaTime);
				break;
			}
		//If the enemy's movement pattern is set to stay still...
		case (int)MovementPatternState.STAY_STILL:
			{
				//Do nothing
				break;
			}
		//If the enemy's movement pattern is set to follow the player, at normal velocity
		case (int)MovementPatternState.MOVE_TOWARDS_PLAYER:
			{
//				Vector3 current_position = this.transform.position;
				displacement = Vector3.Normalize (this.m_PatrolRegion.m_Player.gameObject.transform.position - current_position) * this.m_MaximalVelocity;
				//Update direction
				this.m_Direction = displacement;
//				this.transform.position = current_position + (displacement * Time.deltaTime);
				break;
			}
		//If the enemy's movement pattern is set to chase the player, at a greater-than-normal velocity
		case (int)MovementPatternState.CHASE_PLAYER:
			{
//				Vector3 current_position = this.transform.position;
				displacement = Vector3.Normalize (this.m_PatrolRegion.m_Player.gameObject.transform.position - current_position) * this.m_ChaseVelocity;
				//Update direction
				this.m_Direction = displacement;
//				this.transform.position = current_position + (displacement * Time.deltaTime);
				break;
			}
		}//end switch

		//if moving leads to the enemy leaving the patrol region...
		if (this.MovingLeavesPatrolRegion (displacement * Time.deltaTime)) {
			displacement = Vector3.Normalize (this.m_PatrolRegion.transform.position - this.transform.position) * this.m_MaximalVelocity;
			//Update direction
			this.m_Direction = displacement;
		}
		//if moving leads to the enemy running into an Obstructable...
		if (this.MovingLeadsToObstructable (displacement * Time.deltaTime)) {
			//move in some other direction
			displacement = Vector3.Normalize(Vector3.Cross(displacement, this.transform.up)) * this.m_MaximalVelocity;
			this.m_Direction = displacement;
		}
		//if moving leads to the enemy running into the player
		if (this.MovingLeadsToPlayer (displacement * 2.0f * Time.deltaTime)) {
			//don't move
			displacement = Vector3.zero;
		}

		//Apply movement
		this.transform.position = current_position + (displacement * Time.deltaTime);

		//Update animator parameters
		this.ProperlyUpdateAnimatorParameters();
	}

	/**A function to set the new values of the animator bools and update the animator parameters.*/
	private void ProperlyUpdateAnimatorParameters()
	{
		this.m_IsMovingLeft = this.m_Direction.x < 0 && this.m_MovementPatternState != MovementPatternState.STAY_STILL;
		this.m_IsMovingRight = this.m_Direction.x > 0 && this.m_MovementPatternState != MovementPatternState.STAY_STILL;
		this.m_IsMovingUp = this.m_Direction.z > 0 && this.m_MovementPatternState != MovementPatternState.STAY_STILL;
		this.m_IsMovingDown = this.m_Direction.z < 0 && this.m_MovementPatternState != MovementPatternState.STAY_STILL;

		//The function in MobileCharacter
		base.UpdateAnimatorParameters ();
	}


}
