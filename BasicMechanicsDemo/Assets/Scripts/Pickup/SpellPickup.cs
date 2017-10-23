using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpellPickup : MonoBehaviour {

	/**A private instance of the contents of the pickup.*/
	public Spell m_Spell;
	/**The spell name, for debugging purposes.*/
	public string m_SpellName;

	void Awake()
	{
		this.gameObject.GetComponent<Collider> ().isTrigger = true;
	}

	/**A function to set the Item instance to this ItemPickup.*/
	public void SetSpell(Spell spell_spawned)
	{
		this.m_Spell = spell_spawned;
		this.m_SpellName = this.m_Spell.m_SpellName.ToString();
	}//end f'n void SetItem(Item)

	//A function to trigger when the player walks into an item pickup
	void OnTriggerEnter(Collider other)
	{
		//if the object colliding with the item pickup is the player...
		if (other.gameObject.GetComponent<PlayerInventory> ()) {
			other.gameObject.GetComponent<PlayerInventory> ().AddSpell (this.m_Spell);

			GameObject.Destroy (this.gameObject);
		}
	}//end f'n void OnTriggerEnter(Collider)
}
