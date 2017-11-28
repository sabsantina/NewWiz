using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour {

	[SerializeField] private Spawner m_Spawner;

	/**A gameobject to tell us where we want to spawn some soldiers*/
	[SerializeField] private GameObject m_Barracks1;
	/**A gameobject to tell us where we want to spawn some soldiers*/
	[SerializeField] private GameObject m_Barracks2;
	/**A gameobject to tell us where we want to spawn some soldiers*/
	[SerializeField] private GameObject m_Barracks3;
	/**A gameobject to tell us where we want to spawn some soldiers*/
	[SerializeField] private GameObject m_Barracks4;
	/**A gameobject to tell us where we want to spawn some soldiers*/
	[SerializeField] private GameObject m_Barracks5;

	/**The number of soldiers to spawn, per barracks.*/
	private int m_NumberofSoldiersToSpawn = 7;

	/**The place where we want to spawn the Orange Laird, Teine.*/
	[SerializeField] private GameObject m_TeineSpawnPoint;

	public static bool m_TeineIsDead = false;

	// Use this for initialization
	void Start () {
		this.SpawnInSoldiers (this.m_Barracks1.transform.position);
		this.SpawnInSoldiers (this.m_Barracks2.transform.position);
		this.SpawnInSoldiers (this.m_Barracks3.transform.position);
		this.SpawnInSoldiers (this.m_Barracks4.transform.position);
		this.SpawnInSoldiers (this.m_Barracks5.transform.position);


		if (!m_TeineIsDead) {
			GameObject enemy = this.m_Spawner.SpawnEnemy (EnemyName.ORANGE_LAIRD);
			enemy.GetComponentInChildren<DefaultEnemy> ().m_Spawner = this.m_Spawner;
			enemy.transform.position = this.m_TeineSpawnPoint.transform.position;
		}
	}

	private void SpawnInSoldiers(Vector3 position)
	{
		GameObject enemy_container = new GameObject();
		enemy_container.name = EnemyName.ARMORED_SOLDIER + " Container";
		for (int index = 0; index < this.m_NumberofSoldiersToSpawn; index++) {
			GameObject enemy = this.m_Spawner.SpawnEnemy (EnemyName.ARMORED_SOLDIER, enemy_container.transform);
			enemy.GetComponentInChildren<DefaultEnemy> ().m_Spawner = this.m_Spawner;
			enemy.transform.position = (position + Vector3.forward * index * 0.5f);
		}
	}

	private void SpawnInTeine(Vector3 position)
	{

	}

}
