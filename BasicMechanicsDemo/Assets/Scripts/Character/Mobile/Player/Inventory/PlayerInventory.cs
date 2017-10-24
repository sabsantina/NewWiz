#define TESTING_INVENTORY_CONTENTS_OUTPUT
#define TESTING_INVENTORY_SPELLPICKUP
#define TESTING_ACTIVE_SPELL
#define START_WITH_FIREBALL
#define START_WITH_ICEBALL

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
	public Dictionary<Item, int> m_ItemDictionary { set; get;}


	/**A list of all SpellClass instances of spells in the inventory.*/
	public List<SpellClass> m_SpellClassList;


	/**A reference to a default spell prefab to contain all our active spell information.*/
	[SerializeField] public GameObject m_DefaultSpellPrefab;
	private GameObject m_DefaultSpellInstance;
	public Spell m_ActiveSpell;

	/**The active spell, of type SpellClass, used for firing spells.*/
	public SpellClass m_ActiveSpellClass = null;
	/**For testing purposes; the name of the active SpellClass instance.*/
	public string m_ActiveSpellName;

    /**Hotkeys to switch spells placed on the UI. Currently don't use the UI elements.*/
    private readonly string STRINGKEY_INPUT_HOTKEY1 = "f1";
    private readonly string STRINGKEY_INPUT_HOTKEY2 = "f2";
	/**A string variable containing the string name of the Input Manager variable responsible for the player switching their active spell.*/
	private readonly string STRINGKEY_INPUT_SWITCHSPELLS = "Switch Spells";

    void Awake()
	{
		//for testing
		this.m_DefaultSpellInstance = GameObject.Instantiate(this.m_DefaultSpellPrefab);
//		this.m_ActiveSpell = this.m_DefaultSpellPrefab.GetComponent<Spell> ();
//		m_ActiveSpell.GenerateInstance_Fireball ();
//		this.m_ActiveSpellName = this.m_ActiveSpellClass.m_SpellName.ToString ();
	}

	void Start()
	{
		//Initialize Spell List
		this.m_SpellList = new List<Spell> ();
		Spell spell_to_add = this.m_DefaultSpellInstance.GetComponent<Spell> ();

		//Initialize Item Dictionary
		this.m_ItemDictionary = new Dictionary<Item, int>();


		//**********
		this.m_SpellClassList = new List<SpellClass>();
		SpellClass spell_class_instance = new SpellClass();
		#if START_WITH_FIREBALL
		this.AddSpell(spell_class_instance.GenerateInstance(SpellName.Fireball));
		#endif

	}//end f'n void Start()

	void Update()
	{
		//If there's more than one spell in the player's inventory...
		if (this.m_SpellClassList.Count > 1) {
			//...then check for player input and switch SpellClass instance on command.
			this.UpdateActiveSpell ();
		} //end if
		//if there's only one spell in the player's inventory and an active SpellClass instance hasn't yet been set...
		if (this.m_SpellClassList.Count == 1 && this.m_ActiveSpellClass == null)
		{
			//...then assign a default value for the active SpellClass
			this.AssignDefaultActiveSpell ();
		}
		#if TESTING_INVENTORY_CONTENTS_OUTPUT
		if (Input.GetKeyDown (KeyCode.Return)) {
			this.OutputInventoryContents();
		#if START_WITH_ICEBALL
			SpellClass spell_class_instance = new SpellClass();
			this.AddSpell (spell_class_instance.GenerateInstance(SpellName.Iceball));
		#endif
		}
		#endif
	}//end f'n void Update()

	/**A function to update the player's active SpellClass instance with respect to mousewheel input.
	*Switches to next rightward spell if the input is forward; switches to next leftward spell if the input is backward.*/
	private void UpdateActiveSpell()
	{
		//the user needs to have at least two spells to be able to switch between them
		if (this.m_SpellClassList.Count > 1) {

			float input = Input.GetAxis (STRINGKEY_INPUT_SWITCHSPELLS);

			//If the user wants to switch their active spells...
			if (input != 0) {
				int next_index = 0;
				int index_of_active_spell = this.m_SpellClassList.IndexOf(this.m_ActiveSpellClass);
				//if the user scrolls forward...
				if (input > 0.0f) {
					//...then switch to the next rightward spell

					//if the index of the active spell is equal to that of the last spell in the list...
					if (index_of_active_spell == this.m_SpellClassList.Count - 1) {
						//...then the next rightward spell is the first one.
						next_index = 0;
					}//end if
					//else if the index of the active spell is not the last spell in the list...
					else {
						//...then the next index is simply one further on.
						next_index = index_of_active_spell + 1;
					}//end else
				}//end if
				//else if the user scrolls backward...
				else { //if input < 0.0f
					//...then switch to the next leftward spell

					//if the index of the active spell is equal to that of the first spell in the list...
					if (index_of_active_spell == 0) {
						//...then the next leftward spell is the last one.
						next_index = this.m_SpellClassList.Count - 1;
					}//end if
					//else if the index of the active spell is not that of the first spell in the list...
					else {
						//...then the next index is simply one preceding the one we're at.
						next_index = index_of_active_spell - 1;
					}//end else
				}//end else

				//Update active spell
				this.m_ActiveSpellClass = this.m_SpellClassList [next_index];
				Debug.Log (next_index);
				#if TESTING_ACTIVE_SPELL
				Debug.Log ("UpdateActiveSpell::Active SpellClass: " + this.m_ActiveSpellClass.m_SpellName.ToString ());
				#endif
			}//end if
		}//end if
	}//end f'n void UpdateActiveSpell()

	/**A function to assign the default active SpellClass instance of the player.
	*Assumes the size of the spell class list is equal to 1.*/
	private void AssignDefaultActiveSpell()
	{
		this.m_ActiveSpellClass = this.m_SpellClassList [0];
	}//end f'n void AssignDefaultActiveSpell()

	/**A function to return the string containing all of the SpellClass instances of the [this.m_SpellClassList]*/
	private string OutputSpellClassList()
	{
		string all_messages = "";
		foreach (SpellClass spell_class in this.m_SpellClassList) {
			all_messages += spell_class.ReturnSpellInstanceInfo () + "\n";
		}
		return all_messages;
	}

	/**A function, for testing purposes, to print out the inventory contents.*/
	private void OutputInventoryContents()
	{
		string message = "Inventory Contents:\n";
		message += "Spells:\n";
		message += this.OutputSpellClassList ();

		message += "Items:\n";
		foreach (KeyValuePair<Item, int> entry in this.m_ItemDictionary) {
			message += "Item name: " + entry.Key.m_ItemName.ToString () + "\tItem Effect: " + entry.Key.m_ItemEffect.ToString () + "\tItem Quantity:\t" + entry.Value + "\n";
		}//end foreach
		message += "Active spell:\t";
		if (this.m_ActiveSpellClass == null) {
			message += "None";
		} else {
			message += this.m_ActiveSpellClass.ReturnSpellInstanceInfo();
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

//	/**A function to add the spell to the inventory.*/
//    public void AddSpell(Spell spell_to_add)
//
//    {
//        bool match_found = false;
//        //for all spells in the spell list...
//        foreach (Spell spell in this.m_SpellList) {
//            //...if any of them have the same name as the one we're trying to add...
//            if (spell_to_add.isEqual (spell)) {
//                //...then a match was found
//                match_found = true;
//            }//end if
//        }//end foreach
//        //if no match was found...
//        if (!match_found) {
//            //...then the spell is not already in the spell list, and we can add it, now.
//            this.m_SpellList.Add (spell_to_add);
//        }//end if
//        Debug.Log ("PlayerInventory::Adding spell " + spell_to_add.m_SpellName.ToString () + "\n"
//            + "Spell already in list? " + match_found);
//
//    }//end f'n void AddSpell(Spell)

	/**A function to add a Spell object to the SpellClass list, converting it into a new instance and adding it afterwards, if necessary.*/
	public void AddSpell(Spell spell_to_add)
	{
		SpellClass converted_spell = new SpellClass ().GenerateInstance (spell_to_add.m_SpellName);
		foreach (SpellClass spell in this.m_SpellClassList) {
			if (spell.m_SpellName.ToString () == spell_to_add.m_SpellName.ToString ()) {
				return;
			}
		}
		this.m_SpellClassList.Add (converted_spell);
	}//end f'n void AddSpell(Spell)

	/**A function to add a SpellClass object to the SpellClass list if the SpellClass is not already in the list.*/
	public void AddSpell(SpellClass spell_to_add)
	{
////		Debug.Log (this.m_SpellClassList.Contains (spell_to_add));
//		if (this.m_SpellClassList.Contains (spell_to_add)) {
//			return;
//		} else {
//			this.m_SpellClassList.Add (spell_to_add);
//		}
		foreach (SpellClass spell in this.m_SpellClassList) {
			if (spell.m_SpellName.ToString () == spell_to_add.m_SpellName.ToString ()) {
				return;
			}
		}
		this.m_SpellClassList.Add (spell_to_add);

	}//end f'n void AddSpell(SpellClass)


    

}//end class PlayerInventory
