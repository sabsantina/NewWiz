using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour 
{
	[SerializeField] public AudioClip shieldHitSound;
	[SerializeField] AudioClip fireSpellSound;
	[SerializeField] AudioClip thunderSpellSound;
	[SerializeField] AudioClip thunderStormSound;
	[SerializeField] AudioClip healSpellSound;
	[SerializeField] AudioClip rockSpellSound;
	[SerializeField] AudioClip shieldCastSound;
	[SerializeField] AudioClip waterCastSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public AudioClip getAudioForSpell(SpellName name)
	{
		switch (name) 
		{
		case SpellName.Fireball:
			return fireSpellSound;
		case SpellName.Iceball:
			return waterCastSound;
		case SpellName.Shield:
			return shieldCastSound;
		case SpellName.Thunderball:
			return thunderSpellSound;
		case SpellName.Thunderstorm:
			return thunderStormSound; //will change it later
		//Add more as new spells are created
		}
		return shieldCastSound;
	}
}
