using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour 
{
	//***		Spell-related sounds...

	/**A sound to be played when the player is shielded and attacked by enemies.*/
	[SerializeField] public AudioClip shieldHitSound;
	/**A sound to be played when the fireball spell is cast*/
	[SerializeField] AudioClip fireSpellSound;
	/**A sound to be played when the thunderball spell is cast*/
	[SerializeField] AudioClip thunderSpellSound;
	/**A sound to be played while the thunderstorm spell is cast*/
	[SerializeField] AudioClip thunderStormSound;
	/**A sound to be played while the heal spell is cast*/
	[SerializeField] AudioClip healSpellSound;
	/**A sound to be played when the rock spell is cast*/
	[SerializeField] AudioClip rockSpellSound;
	/**A sound to be played while the shield is being cast*/
	[SerializeField] AudioClip shieldCastSound;
	/**A sound to be played when the water spell is being cast*/
	[SerializeField] AudioClip waterCastSound;

	//***		Pick-up related sounds...

	/**An audio clip to tell us an item got picked up*/
	[SerializeField] private AudioClip m_ItemPickedUpSound;
	/**An audio clip to tell us a spell got picked up/acquired*/
	[SerializeField] private AudioClip m_SpellPickedUpSound;
	/**A player-audio for pickups; we might be picking something up while casting a spell, or being attacked, so keeping them separate is good.*/
	public PlayerAudio m_PlayerAudio_Pickups;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**A function to return spell-related audio clips*/
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
	}//end f'n AudioClip getAudioForSpell(SpellName)

//	public AudioClip GetAudioForPickup(bool pickup_item)
//	{
//		if (pickup_item) {
//			return this.m_ItemPickedUpSound;
//		} else {
//			return this.m_SpellPickedUpSound;
//		}
//	}
//
//	public void PlaySoundOnce(AudioClip clip)
//	{
//		Player player = this.GetComponent<Player> ();
//		player.m_audioSource.PlayOneShot(
////		this.m_Player.m_audioSource.PlayOneShot(m_playerAudio.getAudioForSpell(this.m_SpellClassToFire.m_SpellName));
//	}
}
