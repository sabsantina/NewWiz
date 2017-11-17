using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPattern : MobileCharacter {

	/**The state of the movement pattern.*/
	public MovementPatternState m_MovementPatternState = MovementPatternState.ROAM;
	/**A reference to the player, so we know whose gameobject to move towards.*/
	public Player m_Player;

	// Use this for initialization
	void Start () {
		this.m_Direction = new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f));
		this.m_MaximalVelocity = 7.5f;
	}
	
	// Update is called once per frame
	void Update () {
		this.ExecutePatternState ();
	}

	/**Move the gameobject with respect to the current movement pattern.*/
	void ExecutePatternState()
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
		//If the enemy's movement
		}
	}


}
