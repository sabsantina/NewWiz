using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour {

	/**The state of the attack pattern; do nothing, by default*/
	public AttackPatternState m_AttackPatternState = AttackPatternState.DO_NOTHING;
	/**The area for which the enemy can detect the player*/
	[SerializeField] protected EnemyDetectionRegion m_AttackDetectionRegion;
	/**We need a reference to the actual enemy to be able to inflict their specific damage.*/
	[SerializeField] protected DefaultEnemy m_Enemy;

	/**The timer to intersperse the attacks.*/
	public float m_AttackTimer = 0.0f;
	/**The time in seconds between each attack.*/
	public float m_IntervalBetweenAttacks = 1.0f;

	/**The enemy's given animator*/
	protected Animator m_Animator;

	/**The string value of our isAttacking_Melee animator parameter*/
	protected readonly string STRINGKEY_PARAM_ISATTACKING_MELEE = "isAttacking_Melee";
	/**The string value of our isAttacking_Ranged animator parameter*/
	protected readonly string STRINGKEY_PARAM_ISATTACKING_RANGED = "isAttacking_Ranged";

	// Use this for initialization
	void Awake () {
		this.m_Animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.ExecutePatternState ();
	}

	public bool PlayerIsDetectedInAttackDetectionRegion()
	{
		return this.m_AttackDetectionRegion.m_PlayerInRegion;
	}

	protected virtual void ExecutePatternState()
	{}

}
