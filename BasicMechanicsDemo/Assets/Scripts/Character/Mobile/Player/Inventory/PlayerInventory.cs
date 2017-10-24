#define TESTING_INVENTORY_CONTENTS_OUTPUT
#define TESTING_INVENTORY_SPELLPICKUP


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the player's got his capsule collider
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerInventory : MonoBehaviour {
	[SerializeField] private GameObject m_DefaultSpellPickupPrefab;
	[SerializeField] private GameObject m_DefaultItemPickupPrefab;

	/**A list of all spells in the inventory.*/
	public List<Spell> m_SpellList {  set; get; }
	/**A reference to a default spell prefab to contain all our active spell information.*/
	[SerializeField] public GameObject m_DefaultSpellPrefab;
	private GameObject m_DefaultSpellInstance;
	public Spell m_ActiveSpell;
	public string m_ActiveSpellName;
	public Dictionary<Item, int> m_ItemDictionary { set; get;}

	void Awake()
	{
		//for testing
		this.m_ActiveSpell = this.m_DefaultSpellPrefab.GetComponent<Spell> ();
		m_ActiveSpell.GenerateInstance_Fireball (true);
		this.m_ActiveSpellName = this.m_ActiveSpell.m_SpellName.ToString ();
	}

	void Start()
	{
		//Initialize Spell List
		this.m_SpellList = new List<Spell> ();

		//Initialize Item Dictionary
		this.m_ItemDictionary = new Dictionary<Item, int>();
	}//end f'n void Start()

	void Update()
	{
		#if TESTING_INVENTORY_CONTENTS_OUTPUT
		if (Input.GetKeyDown (KeyCode.Return)) {
			this.OutputInventoryContents ();
		}
		#endif
	}

	/**A function, for testing purposes, to print out the inventory contents.*/
	private void OutputInventoryContents()
	{
		string message = "Inventory Contents:\n";
		message += "Spells:\n";
		foreach (Spell spell in this.m_SpellList) {
			message += "Spell name: " + spell.m_SpellName.ToString () + "\tSpell Effect: " + spell.m_SpellEffect.ToString() + "\n";
		}//end foreach
		message += "Items:\n";
		foreach (KeyValuePair<Item, int> entry in this.m_ItemDictionary) {
			message += "Item name: " + entry.Key.m_ItemName.ToString () + "\tItem Effect: " + entry.Key.m_ItemEffect.ToString () + "\tItem Quantity:\t" + entry.Value + "\n";
		}//end foreach
		message += "Active spell:\t";
		if (m_ActiveSpell == null) {
			message += "None";
		} else {
			message += "Spell name: " + m_ActiveSpell.m_SpellName.ToString () + "\tSpell Effect: " + m_ActiveSpell.m_SpellEffect.ToString () + "\n";
		}
		Debug.Log(message);
	}//end f'n void OutputInventoryContents()


	/**This function returns the currently chosen spell.
	*Accounts for an empty inventory by returning null if the active spell can't be found.*/
	public Spell GetChosenSpell()
	{
		//if the spell is contained in the spell list...
		foreach (Spell spell in this.m_SpellList) {
			if (spell.m_SpellName == m_ActiveSpell.m_SpellName) {
				return spell;
			}
		}
		return null;

	}//end f'n Spell GetChosenSpell()

	/**A function to add the item to the inventory.*/
	public void AddItem(Item item_to_add)
	{
		int value;
		//if the key was found in the dictionary...
		if (this.m_ItemDictionary.TryGetValue (item_to_add, out value)) {
			//...then increment quantity
			this.m_ItemDictionary [item_to_add] = value + 1;
		} else {
			this.m_ItemDictionary.Add (item_to_add, 1);
		}

//		bool match_found = false;
//		//for all items in the dictionary...
//		foreach (KeyValuePair<Item, int> entry in this.m_ItemDictionary) {
//			//...if the item to add has the same name as that of the current entry...
//			if (item_to_add.isEqual (entry.Key)) {
//				//...then a match for a common name was found
//				match_found = true;
//			}//end if
//		}//end foreach
//		//if there was a match for the item name...
//		if (match_found) {
//			//...then add to the current quantity
//			int current_quantity;
//			//returns zero if none of the item are found.
//			this.m_ItemDictionary.TryGetValue (item_to_add, out current_quantity);
//			this.m_ItemDictionary [item_to_add] = current_quantity + 1;
//		}//end if
//		//else if there was no match for the item name...
//		else
//		{
//			//...then add the entry into the dictionary
//			this.m_ItemDictionary.Add (item_to_add, 1);
//		}//end else
		Debug.Log ("PlayerInventory::Adding item " + item_to_add.m_ItemName.ToString () + "\n"
			+ "Item already in list? " + this.m_ItemDictionary.TryGetValue (item_to_add, out value));

	}//end f'n void AddItem(Item)

	/**A function to add the spell to the inventory.*/
	public void AddSpell(Spell spell_to_add)
	{
		bool match_found = false;
		//for all spells in the spell list...
		foreach (Spell spell in this.m_SpellList) {
			//...if any of them have the same name as the one we're trying to add...
			if (spell_to_add.isEqual (spell)) {
				//...then a match was found
				match_found = true;
			}//end if
		}//end foreach
		//if no match was found...
		if (!match_found) {
			//...then the spell is not already in the spell list, and we can add it, now.
			this.m_SpellList.Add (spell_to_add);
		}//end if
		Debug.Log ("PlayerInventory::Adding spell " + spell_to_add.m_SpellName.ToString () + "\n"
			+ "Spell already in list? " + match_found);

	}//end f'n void AddSpell(Spell)



}//end class PlayerInventory
