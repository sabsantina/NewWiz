using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackPattern : AttackPattern {

	/**A reference to the default spell prefab*/
	[SerializeField] GameObject m_DefaultSpellPrefab;
	/**A reference to the spell instance we generate from the default spell prefab.*/
	public GameObject m_GeneratedSpellInstance;
	/**To be set from the children classes*/
	public SpellClass m_SpellToCast;
	[SerializeField] SpellAnimatorManager m_SpellAnimatorManager;


	protected override void ExecutePatternState ()
	{
		switch((int)this.m_AttackPatternState)
		{
		case (int)AttackPatternState.RANGED:
			{
				if (this.m_AttackTimer == 0.0f) {
					this.m_IsAttacking = true;
					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
					this.GenerateSpellPrefabInstance();
					//					this.m_Player.m_audioSource.PlayOneShot(m_playerAudio.getAudioForSpell(this.m_SpellClassToFire.m_SpellName));

					switch ((int)this.m_SpellToCast.m_SpellType) {
					case (int)SpellType.BASIC_PROJECTILE_ON_TARGET:
						{
							this.m_GeneratedSpellInstance.transform.position = this.transform.position;
							SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
							spell_movement.SetEnemyTarget (this.m_Player.gameObject);
							spell_movement.SetSpellToCast (this.m_SpellToCast);

							StartCoroutine (this.DestroySpellAfterTime (3.0f));
							break;
						}
					}

					this.m_AttackTimer += Time.deltaTime;
					this.m_IsAttacking = false;
					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
				} else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
					this.m_AttackTimer += Time.deltaTime;
				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
					this.m_AttackTimer = 0.0f;
				}
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
	}

	private IEnumerator DestroySpellAfterTime(float time)
	{
		yield return new WaitForSeconds (time);
		if (this.m_GeneratedSpellInstance != null) {
			this.DestroySpellPrefabInstance ();
		}
	}

	private void GenerateSpellPrefabInstance()
	{
		this.m_GeneratedSpellInstance = GameObject.Instantiate (this.m_DefaultSpellPrefab);
		this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ().m_SpellClassToCast = this.m_SpellToCast;
		this.m_SpellAnimatorManager.SetSpellAnimator (this.m_GeneratedSpellInstance);
	}

	public void DestroySpellPrefabInstance()
	{
		if (this.m_GeneratedSpellInstance != null) {
			GameObject.Destroy (this.m_GeneratedSpellInstance);
			this.m_GeneratedSpellInstance = null;
		}
	}
}
