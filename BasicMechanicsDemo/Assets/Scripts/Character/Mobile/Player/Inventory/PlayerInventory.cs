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

    /**Hotkeys to switch spells placed on the UI. Currently don't use the UI elements.*/
    private readonly string STRINGKEY_INPUT_HOTKEY1 = "f1";
    private readonly string STRINGKEY_INPUT_HOTKEY2 = "f2";

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

        UpdateActiveSpell();
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

//	/**A function to return a dictionary filled with all spells to be displayed in the inventory when the player checks it. 
//	 * Only spells that have been discovered will be returned to the inventory ([Spell.m_HasBeenDiscovered] must be true).*/
//	public Dictionary<string, string> StringFormat_SpellList()
//	{
//		//A dictionary made up of <spell_name, spell_effect>
//		Dictionary<string, string> dictionary_to_return = new Dictionary<string, string> ();
//		foreach (Spell spell in this.m_SpellList) {
//			//if the player's discovered the given spell...
//			if (spell.m_HasBeenDiscovered) {
//				//...then add that spell to the dictionary
//				dictionary_to_return.Add (spell.m_SpellName.ToString (), spell.m_SpellEffect.ToString ());
//			}//end if
//			//else if the player hasn't discovered the given spell, then we don't care about it.
//		}//end foreach
//		return dictionary_to_return;
//	}//end f'n Dictionary<string, string> StringFormat_SpellList()

	#if ATHANASIOS
    /**Function to be utilised when clicking on the spell in the inventory menu*/
    //public void SetActiveSpellNumber(int m_SpellNumber)
    //{
    //
    //}
    /**This function returns the currently chosen spell*/
    public Spell GetChosenSpell()
    {
        return this.m_SpellList[m_ActiveSpellNumber];
    }
	#endif
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
        int current_quantity;
        //returns zero if none of the item are found.
        this.m_ItemDictionary.TryGetValue(item_to_add, out current_quantity);
        this.m_ItemDictionary[item_to_add] = current_quantity + 1;
    }//end f'n void AddItem(Item)

    /**A function to add the spell to the inventory.*/
    public void AddSpell(Spell spell_to_add)
	{
		//if the spell is contained in the inventory...
		if (this.m_SpellList.Contains (spell_to_add)) {
			//...do nothing
			return;
		}//end if 
		//else if the spell is not contained in the inventory...
		else {
			//...then add it
			this.m_SpellList.Add(spell_to_add);
		}//end else
	}//end f'n void AddSpell(Spell)

    private void UpdateActiveSpell()
    {
        /**Temporary hardcoded. Will receive inputs from SpellList and the UI to get the correct spell.*/
        if (Input.GetKeyDown(STRINGKEY_INPUT_HOTKEY1))
        {
            Debug.Log("Fireball chosen!");
            this.m_ActiveSpell = this.m_DefaultSpellPrefab.GetComponent<Spell>();
            m_ActiveSpell.GenerateInstance_Fireball(true);
            this.m_ActiveSpellName = this.m_ActiveSpell.m_SpellName.ToString();
        }

        if (Input.GetKeyDown(STRINGKEY_INPUT_HOTKEY2))
        {
            Debug.Log("IceBall chosen!");
            this.m_ActiveSpell = this.m_DefaultSpellPrefab.GetComponent<Spell>();
            m_ActiveSpell.GenerateInstance_IceBall(true);
            this.m_ActiveSpellName = this.m_ActiveSpell.m_SpellName.ToString();
        }

    }
    //	void OnTriggerEnter(Collider other)
    //	{
    //		//if the other's gameobject has an item component...
    //		if (other.gameObject.GetComponent<Item> () != null) {
    //			//For some reason, this function is getting called twice immediately. Uncomment the following line to see.
    //			Debug.Log("PlayerInventory::OnTriggerEnter::Item running");
    //
    //			Item pickedup_item = other.gameObject.GetComponent<Item> ();
    //
    //			//if the dictionary contains key pickedup_item...
    //			if (this.m_ItemDictionary.ContainsKey (pickedup_item)) {
    //				//...then increment to the quantity of the item
    //				this.m_ItemDictionary [pickedup_item]++;
    //			}//end if
    //			//else if the dictionary doesn't contain key pickedup_item...
    //			else {
    //				//...then add the Item key with quantity 1.
    //				this.m_ItemDictionary.Add (pickedup_item, 1);
    //			}//end else
    //			//Destroy picked-up item
    //			GameObject.Destroy(other.gameObject);
    //		}//end if
    //
    //		/*
    //		* Note: We assume the only time the player will ever encounter a spell is if the player hasn't yet discovered them.
    //		* That being said, I won't take chances with the code.
    //		*/
    //
    //		//if the other's gameobject has a spell component...
    //		if (other.gameObject.GetComponent<Spell> () != null) {
    //			Spell spell_picked_up = new Spell (); 
    //			spell_picked_up = spell_picked_up.CopySpell(other.gameObject.GetComponent<Spell> ());
    //			//set to true if any spells have the same name
    //			bool spell_of_same_name = false;
    //			foreach (Spell spell_in_list in this.m_SpellList)
    //			{
    //				//if the spell we're picking up has the same name as another spell in the list...
    //				if (spell_in_list.m_SpellName == spell_picked_up.m_SpellName) {
    //					//update spell_of_same_name
    //					spell_of_same_name = true;
    //					//...update m_HasBeenDiscovered
    //					if (spell_in_list.m_HasBeenDiscovered != true) {
    //						spell_in_list.m_HasBeenDiscovered = true;
    //					}//end if
    //				}//end if
    //			}//end foreach
    //			//if none of the spells in the spell list had the same name...
    //			if (!spell_of_same_name) {
    //				//...then add the spell to the list
    //				spell_picked_up.m_HasBeenDiscovered = true;
    //				this.m_SpellList.Add (spell_picked_up);
    //			}//end if
    //
    //			this.SetDefaultChosenSpell ();
    //
    //			GameObject.Destroy(other.gameObject);
    //		}//end if
    //	}//end f'n void OnTriggerEnter(Collider)

    //	/**A function to set a default chosen spell (fireball) on pickup.
    //	*The player's inventory is empty on start, so only after the player picks up a spell can there be a default spell.*/
    //	private void SetDefaultChosenSpell()
    //	{
    //		
    //		if (this.m_SpellList.Count == 1) {
    //			m_ActiveSpell = this.m_SpellList [0];
    //			m_ActiveSpell.m_SpellName = this.m_SpellList [0].m_SpellName;
    //			m_ActiveSpell.m_SpellEffect = this.m_SpellList [0].m_SpellEffect;
    //			m_ActiveSpell.m_SpellDamage = this.m_SpellList [0].m_SpellDamage;
    //			m_ActiveSpell.m_HasBeenDiscovered = this.m_SpellList [0].m_HasBeenDiscovered;
    //			Debug.Log ("Active spell: " + m_ActiveSpell.m_SpellName.ToString());
    //
    //			if (m_ActiveSpell == null) {
    //				Debug.Log ("Active spell is null");
    //			}
    //		}
    //	}

}//end class PlayerInventory
