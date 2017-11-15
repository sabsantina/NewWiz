//#define TESTING_ENEMY_ATTACK

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the Enemy has a capsule collider to define the range in which the player can suffer damage from the enemy.
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyAttack : MonoBehaviour {

	public AudioSource m_audioSource;
	public float m_AttackDamage = -5.0f;
	/**A timer to ensure we don't relentlessly spam the player's health.*/
	public float m_AttackTimer = 0.0f;
	/**A variable to keep track of the value of the last second at which the player was attacked.*/
	public int m_LastSecond = 0;

	private Enemy m_THIS_Enemy;

	void Awake()
	{
		m_audioSource = GetComponent<AudioSource> ();
		//Ensure our capsule collider is how we like it
		CapsuleCollider this_capsule_collider = this.GetComponent<CapsuleCollider> ();
		this_capsule_collider.radius = 2.5f;
		this_capsule_collider.height = 1.0f;
		this_capsule_collider.isTrigger = true;

		this.m_THIS_Enemy = this.GetComponent<Enemy> ();
	}

	/**A private bool to tell us whether or not the enemy is capable of damaging [other].
	*Returns true if the enemy can damage [other]; false, otherwise.*/
	private bool CanDamageOther(GameObject other)
	{
		//if THIS enemy is frozen...
		if (!this.m_THIS_Enemy.m_CanMove) {
			//...then the enemy cannot damage [other].
			return false;
		}//end if
		//if [other] is the player...
		if (other.GetComponent<Player> () != null) {
			Player player = other.GetComponent<Player> ();
			//...and if the player is shielded
			if (player.m_IsShielded) {
				player.m_audioSource.PlayOneShot(player.GetComponent<PlayerAudio>().shieldHitSound);
				//...then the enemy cannot damage the player.
				return false;
			}//end if
		}//end if
		return true;
	}//end f'n bool CanDamageOther(GameObject other)

	void OnTriggerEnter(Collider other)
	{
		//if the enemy is capable of attacking...
		if (this.CanDamageOther (other.gameObject)) {
			//if the player enters the collider...
			if (other.gameObject.GetComponent<Player> () != null) {
				//... then attack the player
				Player player = other.gameObject.GetComponent<Player>();
				m_audioSource.PlayOneShot (m_THIS_Enemy.attackSound);
				player.AffectHealth (this.m_AttackDamage);
				//...and start the timer
				this.m_AttackTimer += Time.deltaTime;
				this.m_LastSecond = 0;
				#if TESTING_ENEMY_ATTACK
				Debug.Log("Player detected! Attacking...\tTimer: " + this.m_AttackTimer);
				#endif
			}//end if
		}//end if
	}//end f'n void OnTriggerEnter(Collider)

	void OnTriggerStay(Collider other)
	{
		//if the enemy is capable of attacking...
		if (this.CanDamageOther (other.gameObject)) {
			//if the player stays in the collider...
			if (other.gameObject.GetComponent<Player> () != null) {
				//...and if the attack timer is greater than or equal to one more than the last second at which the player was attacked...
				if (this.m_AttackTimer >= this.m_LastSecond + 1) {
					//...then attack the player
					Player player = other.gameObject.GetComponent<Player>();
					m_audioSource.PlayOneShot (m_THIS_Enemy.attackSound);
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
		}//end if
	}//end f'n void OnTriggerStay(Collider)

	void OnTriggerExit(Collider other)
	{
		//if the enemy is capable of attacking...
		if (this.CanDamageOther (other.gameObject)) {
			//...and if the other is the player...
			if (other.gameObject.GetComponent<Player> () != null) {
				//then reset temporal variables
				this.m_AttackTimer = 0.0f;
				this.m_LastSecond = 0;
				#if TESTING_ENEMY_ATTACK
				Debug.Log("Player escaped! Resetting timer...");
				#endif
			}//end if
		}//end if
	}//end f'n void OnTriggerExit(Collider)

}//end class EnemyAttack : MonoBehaviour
