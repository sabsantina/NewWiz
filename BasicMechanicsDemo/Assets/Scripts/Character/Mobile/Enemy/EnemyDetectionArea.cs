//#define TESTING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class EnemyDetectionArea : MonoBehaviour {

	/**A reference to the script where we defined the enemy movement functions.*/
	[SerializeField] private EnemyMovement m_EnemyMovement;

	void Start()
	{
		this.GetComponent<CapsuleCollider> ().isTrigger = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Player> () != null) {
			this.m_EnemyMovement.m_IsChasing = true;
		}//end if
	}

	void OnTriggerStay(Collider other)
	{
		//If the player's within the zone...
		if (other.gameObject.GetComponent<Player> () != null) {
			//...then adjust the enemy's movement to give chase!
			Vector3 new_direction = other.transform.position - this.m_EnemyMovement.gameObject.transform.position;
			this.m_EnemyMovement.SetEnemyDirection (new_direction);
		}//end if
	}//end f'n void OnTriggerStay(Collider)

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.GetComponent<Player> () != null) {
			this.m_EnemyMovement.m_IsChasing = false;
		}//end if
	}
}
