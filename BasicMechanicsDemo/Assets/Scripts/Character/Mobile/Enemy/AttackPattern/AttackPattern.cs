using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour {

	/**The state of the attack pattern; do nothing, by default*/
	AttackPatternState m_AttackPatternState = AttackPatternState.DO_NOTHING;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.ExecutePatternState ();
	}

	private void ExecutePatternState()
	{
		switch ((int)this.m_AttackPatternState) {
		case (int)AttackPatternState.MELEE:
			{
				
				break;
			}
		case (int)AttackPatternState.RANGED:
			{
				
				break;
			}
		case (int)AttackPatternState.DO_NOTHING:
			{
				
				break;
			}
		}
	}
}
