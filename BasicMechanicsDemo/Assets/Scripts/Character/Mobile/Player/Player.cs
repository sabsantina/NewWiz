/*
* A script to manage the player's status.
*/

#define TESTING_ZERO_HEALTH

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[SerializeField] public GameObject healthMeter;
	[SerializeField] public GameObject manaMeter;

	/**A variable to keep track of the player's health.*/
	public float m_Health;
	/**A variable to help us know what the player's full health it; public for accessibility in the Item classes (think health potions).*/
	public readonly float PLAYER_FULL_HEALTH = 100.0f;

	public bool m_IsShielded = false;

	void Start()
	{
		//Start off with full health
		this.m_Health = PLAYER_FULL_HEALTH;
		setMaxMeter (healthMeter, this.m_Health);
	}

	void Update()
	{
		if (this.m_Health == 0)
		{
			#if TESTING_ZERO_HEALTH
			Debug.Log("Zero health; player dead\tResurrection time!");
			this.m_Health = PLAYER_FULL_HEALTH;
			setMeterValue(healthMeter, this.m_Health);
			#endif
			//Game Over
			//Should we respawn the player at a checkpoint? How do we want to do this?
		}//end if
	}

	/**A function to add [effect] to the player's health.*/
	public void AffectHealth(float effect)
	{
		this.m_Health += effect;
		setMeterValue (healthMeter, this.m_Health);

	}//end f'n void AffectHealth(float)

	public void setMaxMeter(GameObject meter, float value)
	{
		meter.GetComponent<Slider> ().maxValue = value;
	}

	public void setMeterValue(GameObject meter, float value)
	{
		meter.GetComponent<Slider> ().value = value;
	}


}

