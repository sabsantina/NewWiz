using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour {

	/**The state of the attack pattern; do nothing, by default*/
	public AttackPatternState m_AttackPatternState = AttackPatternState.DO_NOTHING;
	/**The area for which the enemy can detect the player*/
	[SerializeField] EnemyDetectionRegion m_AttackDetectionRegion;
	/**We need a reference to the actual enemy to be able to inflict their specific damage.*/
	[SerializeField] DefaultEnemy m_Enemy;
	/**A reference to the player so we can apply the damage to them.*/
	public Player m_Player;
	/**The timer to intersperse the attacks.*/
	public float m_AttackTimer = 0.0f;
	/**The time in seconds between each attack.*/
	public float m_IntervalBetweenAttacks = 1.0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.ExecutePatternState ();
	}

	public bool PlayerIsDetectedInAttackDetectionRegion()
	{
		return this.m_AttackDetectionRegion.m_PlayerInRegion;
	}

	private void ExecutePatternState()
	{
		switch ((int)this.m_AttackPatternState) {
		case (int)AttackPatternState.MELEE:
			{
				if (this.m_AttackTimer == 0.0f) {
					this.m_Player.AffectHealth (-this.m_Enemy.GetAttackDamageValue ());
					this.m_AttackTimer += Time.deltaTime;
				} else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
					this.m_AttackTimer += Time.deltaTime;
				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
					this.m_AttackTimer = 0.0f;
				}
				break;
			}
		case (int)AttackPatternState.RANGED:
			{
				
				break;
			}
		case (int)AttackPatternState.DO_NOTHING:
			{
				//Do nothing

				//In case the player is slipping in and out of the enemy's detection region, make the timer run its course anyway
				if (0.0f < this.m_AttackTimer) {
					this.m_AttackTimer += Time.deltaTime;
				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
					this.m_AttackTimer = 0.0f;
				}
				break;
			}
		}//end switch
	}//end f'n ExecutePatternState()
}
