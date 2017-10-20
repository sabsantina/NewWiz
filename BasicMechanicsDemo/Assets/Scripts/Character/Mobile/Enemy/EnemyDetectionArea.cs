using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionArea : MonoBehaviour {

	/**The enemy whose detection area this belongs to.*/
	[SerializeField] private Enemy m_Enemy;

	void OnTriggerExit(Collider other)
	{

	}
}
