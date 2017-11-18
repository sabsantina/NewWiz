using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackPattern : AttackPattern {
	
	protected override void ExecutePatternState ()
	{
		switch ((int)this.m_AttackPatternState) {
		case (int)AttackPatternState.MELEE:
			{
				if (this.m_AttackTimer == 0.0f) {
					this.m_IsAttacking = true;
					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
					this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player.AffectHealth(-this.m_Enemy.GetAttackDamageValue ());
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
		}
	}
}
