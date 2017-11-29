using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpell : MonoBehaviour {

    public Spawner m_Spawner;
	// Use this for initialization
	void Start () {
        m_Spawner.Spawn_Spell(SpellName.Thunderstorm, this.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
