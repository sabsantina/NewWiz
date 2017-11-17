using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPattern : MobileCharacter {

	/**The enemy patrol and detection radius*/
	[SerializeField] EnemyDetectionArea m_DetectionArea;

	/**The state of the movement pattern.*/
	public MovementPatternState m_MovementPatternState = MovementPatternState.ROAM;
	/**A reference to the player, so we know whose gameobject to move towards.*/
	public Player m_Player;
	/**A bool to let us know whether or not the player's been detected.*/
	public bool m_PlayerDetected;
	/**The velocity at which an enemy chases a player.*/
	public float m_ChaseVelocity = 10.0f;

	// Use this for initialization
	void Start () {
		this.m_Direction = new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f));
		this.m_MaximalVelocity = 7.5f;
	}
	
	// Update is called once per frame
	void Update () {
		this.ExecutePatternState ();
	}

	/**A function to check whether or not the player was detected within the enemy's patrol area*/
	public bool IsPlayerDetected()
	{
		return false;
	}

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
				Vector3 vector_to_player = Vector3.Normalize (this.m_Player.gameObject.transform.position - current_position) * this.m_MaximalVelocity * Time.deltaTime;
				this.transform.position = current_position + vector_to_player;
				break;
			}
		//If the enemy's movement pattern is set to chase the player, at a greater-than-normal velocity
		case (int)MovementPatternState.CHASE_PLAYER:
			{
				Vector3 current_position = this.transform.position;
				Vector3 vector_to_player = Vector3.Normalize (this.m_Player.gameObject.transform.position - current_position) * this.m_ChaseVelocity * Time.deltaTime;
				this.transform.position = current_position + vector_to_player;
				break;
			}
		}//end switch
		//Update animator parameters
		this.UpdateAnimatorParameters();
	}


}
