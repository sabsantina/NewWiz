#define TESTING_INVENTORY_CONTENTS_OUTPUT
#define TESTING_INVENTORY_SPELLPICKUP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the player's got his capsule collider
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerInventory : MonoBehaviour {
	/**A list of all spells in the inventory.*/
	public List<Spell> m_SpellList { private set; get; }
	/**A List of all items in the inventory.*/
	public List<Item> m_ItemList {private set; get;}
    /**Int which will be used to tell the player which spell he has chosen.*/
    private int m_ActiveSpellNumber = 0;
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
                    fireball.m_SpellDamage = 30.0f;
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
		this.m_ItemDictionary = new Dictionary<Item, int> ();
//		for (int index = 0; index < System.Enum.GetValues (typeof(SpellName)).Length; index++) {
//			switch (index) {
//			//Case Fireball
//			case (int)SpellName.Fireball:
//				{
//					Spell fireball = new Spell ();
//					fireball.m_SpellEffect = SpellEffect.Fire_Damage;
//					fireball.m_SpellName = SpellName.Fireball;
//					fireball.m_HasBeenDiscovered = false;
//					this.m_SpellList.Add (fireball);
//					break;
//				}//end case Fireball
//
//				//and so on...
//			
//			default:
//				{
//					break;
//				}//end case default
//			}//end switch
//		}//end for
//
//		//Initialize Item Dictionary
//		this.m_ItemDictionary = new Dictionary<Item, int>();
//		for (int index = 0; index < System.Enum.GetValues (typeof(ItemName)).Length; index++) {
//			switch (index) {
//			//Case Health Potion
//			case (int)ItemName.Health_Potion:
//				{
//					Item health_potion = new Item ();
//					health_potion.m_ItemName = ItemName.Health_Potion;
//					health_potion.m_ItemEffect = ItemEffect.Gain_Health;
//					//Start off with none
////					health_potion.m_Quantity = 0;
//					this.m_ItemDictionary.Add(health_potion, 0);
//					break;
//				}//end case Health Potion
//
//				//and so on...
//
//			default:
//				{
//					break;
//				}//end case default
//			}//end switch
//
//		}//end for
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
			message += "Spell name: " + spell.m_SpellName.ToString () + "\tSpell Effect: " + spell.m_SpellEffect.ToString() + "\tisDiscovered? " + spell.m_HasBeenDiscovered + "\n";
		}//end foreach
		message += "Items:\n";
		foreach (KeyValuePair<Item, int> entry in this.m_ItemDictionary) {
			message += "Item name: " + entry.Key.m_ItemName.ToString () + "\tItem Effect: " + entry.Key.m_ItemEffect.ToString () + "\tItem Quantity:\t" + entry.Value + "\n";
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

    /**Function to be utilised when clicking on the spell in the inventory menu*/
    //public void SetActiveSpellNumber(int m_SpellNumber)
    //{
    //
    //}
    /**This function returns the currently chosen spell*/
    public Spell GetChosenSpell()
    {
        return m_SpellList[m_ActiveSpellNumber];
    }

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

	void OnTriggerEnter(Collider other)
	{
		//if the other's gameobject has an item component...
		if (other.gameObject.GetComponent<Item> () != null) {
			//For some reason, this function is getting called twice immediately. Uncomment the following line to see.
			Debug.Log("PlayerInventory::OnTriggerEnter::Item running");

			Item pickedup_item = other.gameObject.GetComponent<Item> ();

			//if the dictionary contains key pickedup_item...
			if (this.m_ItemDictionary.ContainsKey (pickedup_item)) {
				//...then increment to the quantity of the item
				this.m_ItemDictionary [pickedup_item]++;
			}//end if
			//else if the dictionary doesn't contain key pickedup_item...
			else {
				//...then add the Item key with quantity 1.
				this.m_ItemDictionary.Add (pickedup_item, 1);
			}//end else
			//Destroy picked-up item
			GameObject.Destroy(other.gameObject);
		}//end if

		/*
		* Note: We assume the only time the player will ever encounter a spell is if the player hasn't yet discovered them.
		* That being said, I won't take chances with the code.
		*/

		//if the other's gameobject has a spell component...
		if (other.gameObject.GetComponent<Spell> () != null) {
			Spell spell_picked_up = other.gameObject.GetComponent<Spell> ();
			//set to true if any spells have the same name
			bool spell_of_same_name = false;
			foreach (Spell spell_in_list in this.m_SpellList)
			{
				//if the spell we're picking up has the same name as another spell in the list...
				if (spell_in_list.m_SpellName == spell_picked_up.m_SpellName) {
					//update spell_of_same_name
					spell_of_same_name = true;
					//...update m_HasBeenDiscovered
					if (spell_in_list.m_HasBeenDiscovered != true) {
						spell_in_list.m_HasBeenDiscovered = true;
					}//end if
				}//end if
			}//end foreach
			//if none of the spells in the spell list had the same name...
			if (!spell_of_same_name) {
				//...then add the spell to the list
				spell_picked_up.m_HasBeenDiscovered = true;
				this.m_SpellList.Add (spell_picked_up);
			}//end if
			GameObject.Destroy(other.gameObject);
		}//end if
	}//end f'n void OnTriggerEnter(Collider)
}//end class PlayerInventory
