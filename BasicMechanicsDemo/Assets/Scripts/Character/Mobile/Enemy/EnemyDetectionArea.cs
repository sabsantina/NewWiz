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

	void OnTriggerStay(Collider other)
	{
		#if TESTING
		string message = "OnTriggerStay::";
		#endif
		if (other.gameObject.GetComponent<Player> () != null && this.gameObject.GetComponent<EnemyMovement>() != null) {
			this.m_EnemyMovement.SetEnemyDirection (other.transform.position - this.m_EnemyMovement.gameObject.transform.position);

		}
		#if TESTING
		Debug.Log (message);
		#endif
	}
}
