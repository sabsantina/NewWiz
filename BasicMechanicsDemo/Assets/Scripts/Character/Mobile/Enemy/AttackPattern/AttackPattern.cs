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
	/**A reference to the player so we can apply the damage to them.*/
//	[SerializeField] public Player m_Player;

//	/**A reference to the default spell prefab*/
//	[SerializeField] GameObject m_DefaultSpellPrefab;
//	/**A reference to the spell instance we generate from the default spell prefab.*/
//	public GameObject m_GeneratedSpellInstance;
//	/**To be set from the children classes*/
//	public SpellClass m_SpellToCast;
//	[SerializeField] SpellAnimatorManager m_SpellAnimatorManager;

	/**The timer to intersperse the attacks.*/
	public float m_AttackTimer = 0.0f;
	/**The time in seconds between each attack.*/
	public float m_IntervalBetweenAttacks = 1.0f;

	/**A bool for the enemy animator, to let us know whether or not we're attacking via melee.*/
	public bool m_IsAttacking_Melee = false;
	/**A bool for the enemy animator, to let us know whether or not we're attacking via ranged attack.*/
	public bool m_IsAttacking_Ranged = false;
	/**The enemy's given animator*/
	protected Animator m_Animator;

	/**The string value of our isAttacking_Melee animator parameter*/
	protected readonly string STRINGKEY_PARAM_ISATTACKING_MELEE = "isAttacking_Melee";
	/**The string value of our isAttacking_Ranged animator parameter*/
	protected readonly string STRINGKEY_PARAM_ISATTACKING_RANGED = "isAttacking_Ranged";

	// Use this for initialization
	void Start () {
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

	protected void UpdateAnimatorParameters_Attack()
	{
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING_MELEE, this.m_IsAttacking_Melee);
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING_RANGED, this.m_IsAttacking_Ranged);
	}

	protected virtual void ExecutePatternState()
	{}

}
