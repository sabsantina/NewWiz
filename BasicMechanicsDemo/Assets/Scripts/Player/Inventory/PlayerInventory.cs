#define TESTING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : MonoBehaviour {
	/**A list of all spells in the inventory.*/
	public List<Spell> m_SpellList { private set; get; }
	/**A dictionary of all items in the inventory, where the Item is the key, and the quantity is the value.*/
	public Dictionary<Item, int> m_ItemDictionary {private set; get;}


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
		this.m_ItemDictionary = new Dictionary<Item, int>();
		for (int index = 0; index < System.Enum.GetValues (typeof(ItemName)).Length; index++) {
			switch (index) {
			//Case Health Potion
			case (int)ItemName.Health_Potion:
				{
					Item health_potion = new Item ();
					health_potion.m_ItemName = ItemName.Health_Potion;
					health_potion.m_ItemEffect = ItemEffect.Gain_Health;
					int quantity = 0;
					//Start off with none
					this.m_ItemDictionary.Add (health_potion, quantity);
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
		foreach (Item item in this.m_ItemDictionary.Keys) {
			message += "Item name: " + item.m_ItemName.ToString () + "\tItem Effect: " + item.m_ItemEffect.ToString () + "\tItem Quantity:\t" + this.m_ItemDictionary[item] + "\n";
		}//end foreach
		Debug.Log(message);
	}//end f'n void OutputInventoryContents()

}
