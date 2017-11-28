using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilleForestManager : MonoBehaviour {

	[SerializeField] private Spawner m_SceneSpawner;

	private Vector3 m_WolfPosition = new Vector3(8.559856f, 0.3f, 2.978876f);

	// Use this for initialization
	void Start () {
		GameObject wolf_enemy_obj = this.m_SceneSpawner.SpawnEnemy (EnemyName.WOLF);
		wolf_enemy_obj.transform.position = this.m_WolfPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
