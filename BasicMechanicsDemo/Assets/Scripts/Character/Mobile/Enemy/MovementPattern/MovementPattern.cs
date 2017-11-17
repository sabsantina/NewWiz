using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPattern : MobileCharacter {

	/**The enemy patrol and detection radius*/
	[SerializeField] EnemyPatrolRegion m_PatrolRegion;

//	[SerializeField] Animator m_EnemyAnimator;

	/**The state of the movement pattern.*/
	public MovementPatternState m_MovementPatternState = MovementPatternState.ROAM;
	/**A reference to the player, so we know whose gameobject to move towards.*/
	public Player m_Player;
	/**A bool to let us know whether or not the player's been detected.*/
	public bool m_PlayerDetected;
	/**The velocity at which an enemy chases a player.*/
	public float m_ChaseVelocity = 200.0f;

	public float m_CurrentVelocity = 0.0f;

	// Use this for initialization
	void Start () {
		this.m_Direction = new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f));
		this.m_MaximalVelocity = 7.5f;
//		this.m_Animator = this.m_EnemyAnimator;
	}
	
	// Update is called once per frame
	void Update () {
		this.ExecutePatternState ();
		this.m_CurrentVelocity = this.m_Direction.magnitude;
		Debug.Log (m_CurrentVelocity);
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


	/**Move the gameobject with respect to the current movement pattern.
	*Note: Updates animator parameters immediately following execution of the actual movement.*/
	private void ExecutePatternState()
	{
		switch ((int)this.m_MovementPatternState) {
		//If the enemy's movement pattern is set to roam...
		case (int)MovementPatternState.ROAM:
			{
				Vector3 current_position = this.transform.position;
				Vector3 displacement = Vector3.Normalize (this.m_Direction) * this.m_MaximalVelocity * Time.deltaTime;
				if (this.MovingLeavesPatrolRegion (displacement)) {
					displacement = Vector3.Normalize (this.m_PatrolRegion.transform.position - this.transform.position) * this.m_MaximalVelocity;
					//Update direction
					this.m_Direction = displacement;
				}
				this.transform.position = current_position + displacement;
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
				Vector3 current_position = this.transform.position;
				Vector3 vector_to_player = Vector3.Normalize (this.m_Player.gameObject.transform.position - current_position) * this.m_MaximalVelocity;
				this.m_Direction = vector_to_player;
				this.transform.position = current_position + (this.m_Direction * Time.deltaTime);
				break;
			}
		//If the enemy's movement pattern is set to chase the player, at a greater-than-normal velocity
		case (int)MovementPatternState.CHASE_PLAYER:
			{
				Vector3 current_position = this.transform.position;
				Vector3 vector_to_player = Vector3.Normalize (this.m_Player.gameObject.transform.position - current_position) * this.m_ChaseVelocity;
				this.m_Direction = vector_to_player;
				this.transform.position = current_position + (this.m_Direction * Time.deltaTime);
				break;
			}
		}//end switch
		//Update animator parameters
		this.UpdateAnimatorParameters();
	}


}
