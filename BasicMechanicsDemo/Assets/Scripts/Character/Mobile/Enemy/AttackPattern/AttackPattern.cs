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
	public Player m_Player;

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

	/**A bool for the enemy animator, to let us know whether or not we're attacking.*/
	public bool m_IsAttacking = false;
	/**The enemy's given animator*/
	private Animator m_Animator;
	/**The string value of the parameter responsible for demonstrating attacking on the enemy's behalf*/
	private readonly string STRINGKEY_PARAM_ISATTACKING = "isAttacking";

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

//	private void ExecutePatternState()
//	{
//		switch ((int)this.m_AttackPatternState) {
//		case (int)AttackPatternState.MELEE:
//			{
//				if (this.m_AttackTimer == 0.0f) {
//					this.m_IsAttacking = true;
////					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//					this.m_Player.AffectHealth (-this.m_Enemy.GetAttackDamageValue ());
//					this.m_AttackTimer += Time.deltaTime;
//					this.m_IsAttacking = false;
////					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//				} else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer += Time.deltaTime;
//				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer = 0.0f;
//				}
//				break;
//			}
//		case (int)AttackPatternState.RANGED:
//			{
//				if (this.m_AttackTimer == 0.0f) {
//					this.m_IsAttacking = true;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//					this.GenerateSpellPrefabInstance();
////					this.m_Player.m_audioSource.PlayOneShot(m_playerAudio.getAudioForSpell(this.m_SpellClassToFire.m_SpellName));
//					this.m_GeneratedSpellInstance.transform.position = this.transform.position;
//					SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
//					spell_movement.m_IsMobileCharacter = true;
//					spell_movement.m_TargetedObj = this.m_Player.gameObject;
//					spell_movement.SetSpellToCast (this.m_SpellToCast);
//
//					StartCoroutine (this.DestroySpellAfterTime (2.0f));
//
//					this.m_AttackTimer += Time.deltaTime;
//					this.m_IsAttacking = false;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//				} else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer += Time.deltaTime;
//				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer = 0.0f;
//				}
//				break;
//			}
//		case (int)AttackPatternState.DO_NOTHING:
//			{
//				//Do nothing
//
//				//In case the player is slipping in and out of the enemy's detection region, make the timer run its course anyway
//				if (0.0f < this.m_AttackTimer) {
//					this.m_AttackTimer += Time.deltaTime;
//				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer = 0.0f;
//				}
//				break;
//			}
//		}//end switch
//	}//end f'n ExecutePatternState()

	protected virtual void ExecutePatternState()
	{}
//	{
//		switch ((int)this.m_AttackPatternState) {
//		case (int)AttackPatternState.MELEE:
//			{
//				if (this.m_AttackTimer == 0.0f) {
//					this.m_IsAttacking = true;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//					this.m_Player.AffectHealth (-this.m_Enemy.GetAttackDamageValue ());
//					this.m_AttackTimer += Time.deltaTime;
//					this.m_IsAttacking = false;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//				} else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer += Time.deltaTime;
//				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer = 0.0f;
//				}
//				break;
//			}
//		case (int)AttackPatternState.RANGED:
//			{
//				if (this.m_AttackTimer == 0.0f) {
//					this.m_IsAttacking = true;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//					this.GenerateSpellPrefabInstance();
//					//					this.m_Player.m_audioSource.PlayOneShot(m_playerAudio.getAudioForSpell(this.m_SpellClassToFire.m_SpellName));
//					this.m_GeneratedSpellInstance.transform.position = this.transform.position;
//					SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
//					spell_movement.m_IsMobileCharacter = true;
//					spell_movement.m_TargetedObj = this.m_Player.gameObject;
//					spell_movement.SetSpellToCast (this.m_SpellToCast);
//
//					StartCoroutine (this.DestroySpellAfterTime (2.0f));
//
//					this.m_AttackTimer += Time.deltaTime;
//					this.m_IsAttacking = false;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//				} else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer += Time.deltaTime;
//				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer = 0.0f;
//				}
//				break;
//			}
//		case (int)AttackPatternState.DO_NOTHING:
//			{
//				//Do nothing
//
//				//In case the player is slipping in and out of the enemy's detection region, make the timer run its course anyway
//				if (0.0f < this.m_AttackTimer) {
//					this.m_AttackTimer += Time.deltaTime;
//				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer = 0.0f;
//				}
//				break;
//			}
//		}//end switch
//	}//end f'n ExecutePatternState()
//
//	private IEnumerator DestroySpellAfterTime(float time)
//	{
//		yield return new WaitForSeconds (time);
//		if (this.m_GeneratedSpellInstance != null) {
//			this.DestroySpellPrefabInstance ();
//		}
//	}
//
//	private void GenerateSpellPrefabInstance()
//	{
//		this.m_GeneratedSpellInstance = GameObject.Instantiate (this.m_DefaultSpellPrefab);
//		this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ().m_SpellClassToCast = this.m_SpellToCast;
//		this.m_SpellAnimatorManager.SetSpellAnimator (this.m_GeneratedSpellInstance);
//	}
//
//	public void DestroySpellPrefabInstance()
//	{
//		if (this.m_GeneratedSpellInstance != null) {
//			GameObject.Destroy (this.m_GeneratedSpellInstance);
//			this.m_GeneratedSpellInstance = null;
//		}
//	}
}
