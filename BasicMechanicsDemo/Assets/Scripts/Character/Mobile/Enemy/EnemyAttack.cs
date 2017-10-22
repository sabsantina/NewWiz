//#define TESTING_ENEMY_ATTACK

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the Enemy has a capsule collider to define the range in which the player can suffer damage from the enemy.
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyAttack : MonoBehaviour {

	public float m_AttackDamage = -5.0f;
	/**A timer to ensure we don't relentlessly spam the player's health.*/
	public float m_AttackTimer = 0.0f;
	/**A variable to keep track of the value of the last second at which the player was attacked.*/
	public int m_LastSecond = 0;

	void Awake()
	{
		//Ensure our capsule collider is how we like it
		CapsuleCollider this_capsule_collider = this.GetComponent<CapsuleCollider> ();
		this_capsule_collider.radius = 2.5f;
		this_capsule_collider.height = 1.0f;
		this_capsule_collider.isTrigger = true;
	}

	void OnTriggerEnter(Collider other)
	{
		//if the player enters the collider...
		if (other.gameObject.GetComponent<Player> () != null) {
			//... then attack the player
			Player player = other.gameObject.GetComponent<Player>();
			player.AffectHealth (this.m_AttackDamage);
			//...and start the timer
			this.m_AttackTimer += Time.deltaTime;
			this.m_LastSecond = 0;
			#if TESTING_ENEMY_ATTACK
			Debug.Log("Player detected! Attacking...\tTimer: " + this.m_AttackTimer);
			#endif
		}//end if
	}//end f'n void OnTriggerEnter(Collider)

	void OnTriggerStay(Collider other)
	{
		//if the player stays in the collider...
		if (other.gameObject.GetComponent<Player> () != null) {
			//...and if the attack timer is greater than or equal to one more than the last second at which the player was attacked...
			if (this.m_AttackTimer >= this.m_LastSecond + 1) {
				//...then attack the player
				Player player = other.gameObject.GetComponent<Player>();
				player.AffectHealth (this.m_AttackDamage);
				//... and increase last second value for the next iteration
				this.m_LastSecond++;
			}//end if
			//Update timer
			this.m_AttackTimer += Time.deltaTime;
			#if TESTING_ENEMY_ATTACK
			Debug.Log("Timer: " + this.m_AttackTimer);
			#endif
		}//end if
	}//end f'n void OnTriggerStay(Collider)

	void OnTriggerExit(Collider other)
	{
		//if the player exits the collider...
		if (other.gameObject.GetComponent<Player> () != null) {
			//then reset temporal variables
			this.m_AttackTimer = 0.0f;
			this.m_LastSecond = 0;
			#if TESTING_ENEMY_ATTACK
			Debug.Log("Player escaped! Resetting timer...");
			#endif
		}//end if
	}//end f'n void OnTriggerExit(Collider)

}
