using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldManager : MonoBehaviour {

	/**A reference to our Spawner to help us spawn in enemies.*/
	[SerializeField] public Spawner m_Spawner;

	/**A gameobject to tell us where we want to spawn some wolves*/
	[SerializeField] private GameObject m_WolfDen1;
	/**A gameobject to tell us where we want to spawn some wolves*/
	[SerializeField] private GameObject m_WolfDen2;
	/**A gameobject to tell us where we want to spawn some wolves*/
	[SerializeField] private GameObject m_WolfDen3;
	/**A gameobject to tell us where we want to spawn some wolves*/
	[SerializeField] private GameObject m_WolfDen4;

	/**The number of wolves to spawn, per den.*/
	private int m_NumberofWolvesToSpawn = 5;

	/**The place where we want to spawn the rooster king.*/
	[SerializeField] private GameObject m_RoosterKingSpawnPoint;

	public static bool m_RoosterKingDead = false;


	//This function is called every time we load the scene.
	void Start () {
		this.SpawnInWolves (this.m_WolfDen1.transform.position);
		this.SpawnInWolves (this.m_WolfDen2.transform.position);
		this.SpawnInWolves (this.m_WolfDen3.transform.position);
		this.SpawnInWolves (this.m_WolfDen4.transform.position);

		if (!m_RoosterKingDead) {
//			Debug.Log ("Calling function");
			GameObject enemy = this.m_Spawner.SpawnEnemy (EnemyName.ROOSTER_KING);
			enemy.GetComponentInChildren<DefaultEnemy> ().m_Spawner = this.m_Spawner;
			enemy.transform.position = this.m_RoosterKingSpawnPoint.transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private void SpawnInWolves(Vector3 position)
	{
		GameObject enemy_container = new GameObject();
		enemy_container.name = EnemyName.WOLF + " Container";
		for (int index = 0; index < this.m_NumberofWolvesToSpawn; index++) {
			GameObject wolf = this.m_Spawner.SpawnEnemy (EnemyName.WOLF, enemy_container.transform);
			wolf.GetComponentInChildren<DefaultEnemy> ().m_Spawner = this.m_Spawner;
			wolf.transform.position = (position + Vector3.forward * index * 0.5f);
		}
	}
}
