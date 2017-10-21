using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Spell : MonoBehaviour {
	/**An enum variable for the spell name. See SpellName.cs for all spell names.*/
	public SpellName m_SpellName { set; get;}
	/**An enum variable for the spell effect. See SpellEffecr.cs for all spell effects.*/
	public SpellEffect m_SpellEffect { set; get;}
	/**A bool to let us know whether or not the player has discovered the spell yet.
	*It'd be simpler to just keep all the spells in the inventory from the get-go, but only show them to the player if they've been discovered.
	*We can do the same basic thing with the items; if the player has 0 quantity of a given item, we won't show it.*/
	public bool m_HasBeenDiscovered{ set; get;}

	void Awake()
	{
		//ensure the box collider's isTrigger is set to true
		this.GetComponent<BoxCollider> ().isTrigger = true;
	}//end f'n void Awake()
}
