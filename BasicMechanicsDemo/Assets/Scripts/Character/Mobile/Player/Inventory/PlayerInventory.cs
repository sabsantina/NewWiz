#define TESTING_INVENTORY_CONTENTS_OUTPUT
#define TESTING_INVENTORY_SPELLPICKUP
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
	public List<SpellClass> m_SpellClassList;
	/**A reference to a default spell prefab to contain all our active spell information.*/
	[SerializeField] public GameObject m_DefaultSpellPrefab;
	private GameObject m_DefaultSpellInstance;
	public Spell m_ActiveSpell;
	public string m_ActiveSpellName;
	public Dictionary<Item, int> m_ItemDictionary { set; get;}

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
		this.m_ActiveSpellName = this.m_ActiveSpell.m_SpellName.ToString ();
	}

	void Start()
	{
		//Initialize Spell List
		this.m_SpellList = new List<Spell> ();
		Spell spell_to_add = this.m_DefaultSpellInstance.GetComponent<Spell> ();
		#if START_WITH_FIREBALL
		spell_to_add.GenerateInstance_Fireball();
		this.m_SpellList.Add(spell_to_add);
		#endif
		#if START_WITH_ICEBALL
		spell_to_add.GenerateInstance_IceBall();
		this.m_SpellList.Add(spell_to_add);
		#endif

		//Initialize Item Dictionary
		this.m_ItemDictionary = new Dictionary<Item, int>();


		//**********
		this.m_SpellClassList = new List<SpellClass>();
		SpellClass spell_class_instance = new SpellClass();
		this.m_SpellClassList.Add (spell_class_instance.GenerateInstance(SpellName.Fireball));
		this.m_SpellClassList.Add (spell_class_instance.GenerateInstance(SpellName.Iceball));


	}//end f'n void Start()

	void Update()
	{
		#if TESTING_INVENTORY_CONTENTS_OUTPUT
		if (Input.GetKeyDown (KeyCode.Return)) {
//			this.OutputInventoryContents ();
			this.OutputSpellClass();
		}
		#endif

        UpdateActiveSpell();
	}

	private void OutputSpellClass()
	{
		string all_messages = "";
		foreach (SpellClass spell_class in this.m_SpellClassList) {
			all_messages += spell_class.ReturnSpellInstanceInfo () + "\n";
		}
		Debug.Log (all_messages);
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

//    /**A function to add the item to the inventory.*/
//    public void AddItem(Item item_to_add)
//    {
//        int current_quantity;
//        //returns zero if none of the item are found.
//        this.m_ItemDictionary.TryGetValue(item_to_add, out current_quantity);
//        this.m_ItemDictionary[item_to_add] = current_quantity + 1;
//    }//end f'n void AddItem(Item)

    
    private void UpdateActiveSpell()
    {
//        /**Temporary hardcoded. Will receive inputs from SpellList and the UI to get the correct spell.*/
//        if (Input.GetKeyDown(STRINGKEY_INPUT_HOTKEY1))
//        {
//            Debug.Log("Fireball chosen!");
//            this.m_ActiveSpell = this.m_DefaultSpellPrefab.GetComponent<Spell>();
//            m_ActiveSpell.GenerateInstance_Fireball(true);
//            this.m_ActiveSpellName = this.m_ActiveSpell.m_SpellName.ToString();
//        }
//
//        if (Input.GetKeyDown(STRINGKEY_INPUT_HOTKEY2))
//        {
//            Debug.Log("IceBall chosen!");
//            this.m_ActiveSpell = this.m_DefaultSpellPrefab.GetComponent<Spell>();
//            m_ActiveSpell.GenerateInstance_IceBall(true);
//            this.m_ActiveSpellName = this.m_ActiveSpell.m_SpellName.ToString();
//        }

		//the user needs to have at least two spells to be able to switch between them
		if (this.m_SpellList.Count > 1) {

			float input = Input.GetAxis (STRINGKEY_INPUT_SWITCHSPELLS);

			//If the user wants to switch their active spells...
			if (input != 0) {
				int next_index = 0;
				int index_of_active_spell = this.m_SpellList.IndexOf(this.m_ActiveSpell);
				//if the user scrolls forward...
				if (input > 0.0f) {
					//...then switch to the next rightward spell

					//if the index of the active spell is equal to that of the last spell in the list...
					if (index_of_active_spell == this.m_SpellList.Count - 1) {
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
						next_index = this.m_SpellList.Count - 1;
					}//end if
					//else if the index of the active spell is not that of the first spell in the list...
					else {
						//...then the next index is simply one preceding the one we're at.
						next_index = index_of_active_spell - 1;
					}//end else
				}//end else

				//Update active spell
				this.m_ActiveSpell = this.m_SpellList [next_index];
				Debug.Log ("Active spell: " + this.m_ActiveSpell.m_SpellName.ToString ());
			}//end if
		}//end if

    }
    

}//end class PlayerInventory
