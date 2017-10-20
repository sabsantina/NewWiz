#define TESTING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : MonoBehaviour {
	/**A list of all spells in the inventory.*/
	public List<Spell> m_SpellList { private set; get; }
	/**A List of all items in the inventory.*/
	public List<Item> m_ItemList {private set; get;}


	void Start()
	{
		//Initialize Spell List
		this.m_SpellList = new List<Spell> ();
		for (int index = 0; index < System.Enum.GetValues (typeof(SpellName)).Length; index++) {
			switch (index) {
			//Case Fireball
			case (int)SpellName.Fireball:
				{
					Spell fireball = new Spell ();
					fireball.m_SpellEffect = SpellEffect.Fire_Damage;
					fireball.m_SpellName = SpellName.Fireball;
					this.m_SpellList.Add (fireball);
					break;
				}//end case Fireball

				//and so on...
			
			default:
				{
					break;
				}//end case default
			}//end switch
		}//end for

		//Initialize Item Dictionary
		this.m_ItemList = new List<Item>();
		for (int index = 0; index < System.Enum.GetValues (typeof(ItemName)).Length; index++) {
			switch (index) {
			//Case Health Potion
			case (int)ItemName.Health_Potion:
				{
					Item health_potion = new Item ();
					health_potion.m_ItemName = ItemName.Health_Potion;
					health_potion.m_ItemEffect = ItemEffect.Gain_Health;
					//Start off with none
					health_potion.m_Quantity = 0;
					this.m_ItemList.Add(health_potion);
					break;
				}//end case Health Potion

				//and so on...

			default:
				{
					break;
				}//end case default
			}//end switch

		}//end for
	}//end f'n void Start()

	void Update()
	{
		#if TESTING
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
		foreach (Item item in this.m_ItemList) {
			message += "Item name: " + item.m_ItemName.ToString () + "\tItem Effect: " + item.m_ItemEffect.ToString () + "\tItem Quantity:\t" + item.m_Quantity + "\n";
		}//end foreach
		Debug.Log(message);
	}//end f'n void OutputInventoryContents()

	/**A function to return a dictionary filled with all spells to be displayed in the inventory when the player checks it. 
	 * Only spells that have been discovered will be returned to the inventory ([Spell.m_HasBeenDiscovered] must be true).*/
	public Dictionary<string, string> StringFormat_SpellList()
	{
		//A dictionary made up of <spell_name, spell_effect>
		Dictionary<string, string> dictionary_to_return = new Dictionary<string, string> ();
		foreach (Spell spell in this.m_SpellList) {
			//if the player's discovered the given spell...
			if (spell.m_HasBeenDiscovered) {
				//...then add that spell to the dictionary
				dictionary_to_return.Add (spell.m_SpellName.ToString (), spell.m_SpellEffect.ToString ());
			}//end if
			//else if the player hasn't discovered the given spell, then we don't care about it.
		}//end foreach
		return dictionary_to_return;
	}//end f'n Dictionary<string, string> StringFormat_SpellList()

//	public Dictionary<string, string, int> StringFormat_ItemList()
//	{
//		//A dictionary made up of <item_name, item_effect, item_quantity>
//		Dictionary<string, string, int> dictionary_to_return = new Dictionary<string, string, int> ();
//		foreach (Item item in this.m_ItemList) {
//			//if the player's discovered the given spell...
//			if (item.m_Quantity > 0) {
//				//...then add that spell to the dictionary
//				dictionary_to_return.Add (item.m_ItemName.);
//			}//end if
//			//else if the player hasn't discovered the given spell, then we don't care about it.
//		}//end foreach
//		return dictionary_to_return;
//	}
}
