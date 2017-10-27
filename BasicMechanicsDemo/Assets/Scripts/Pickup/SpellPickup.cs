using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpellPickup : MonoBehaviour {

	/**A private instance of the contents of the pickup.*/
	public SpellClass m_Spell = new SpellClass();
	/**The spell name, for debugging purposes.*/
	public string m_SpellName;


//	[SerializeField] private AudioClip m_Clip;
//	/**The place the sound comes from.*/
//	private AudioSource m_AudioSource;

	void Awake()
	{
		this.gameObject.GetComponent<Collider> ().isTrigger = true;
	}

	/**A function to set the Item instance to this ItemPickup.*/
	public void SetSpell(SpellClass spell_spawned)
	{
		this.m_Spell.m_SpellName = spell_spawned.m_SpellName;
		this.m_Spell.m_SpellEffect = spell_spawned.m_SpellEffect;
		this.m_Spell.m_IsAOESpell = spell_spawned.m_IsAOESpell;
		this.m_Spell.m_IsMobileSpell = spell_spawned.m_IsMobileSpell;
		this.m_Spell.m_IsPersistent = spell_spawned.m_IsPersistent;
		this.m_SpellName = spell_spawned.m_SpellName.ToString();

//		Debug.Log ("Set spell: " + this.m_Spell.ReturnSpellInstanceInfo ());
	}//end f'n void SetItem(Item)

	//A function to trigger when the player walks into an item pickup
	void OnTriggerEnter(Collider other)
	{
		//if the object colliding with the item pickup is the player...
		if (other.gameObject.GetComponent<PlayerInventory> ()) {
//			Debug.Log ("Spell picked up: " + this.m_Spell.ReturnSpellInstanceInfo());

			other.gameObject.GetComponent<PlayerInventory> ().AddSpell (this.m_Spell);

			GameObject.Destroy (this.gameObject);
		}
	}//end f'n void OnTriggerEnter(Collider)
}
