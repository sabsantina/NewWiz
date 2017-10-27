#define TESTING_INVENTORY_CONTENTS_OUTPUT
#define TESTING_INVENTORY_SPELLPICKUP
#define TESTING_ACTIVE_SPELL
#define TESTING_INVENTORY_ITEMPICKUP


/*Activating this macro will enable the player to start the game with the Fireball spell.*/
#define START_WITH_FIREBALL
/*Activating this macro will enable the player to start the game with the Iceball spell.*/
#define START_WITH_ICEBALL
/*Activating this macro will enable the player to start the game with the Shield spell.*/
#define START_WITH_SHIELD
/*Activating this macro will enable the player to start the game with the Thunderball spell.*/
#define START_WITH_THUNDERBALL
/*Activating this macro will enable the player to start the game with the Thunderstorm spell.*/
#define START_WITH_THUNDERSTORM
/*Activating this macro will enable the player to start the game with the Heal spell*/
#define START_WITH_HEAL


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ensure the player's got his capsule collider
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerInventory : MonoBehaviour {
//	[SerializeField] private GameObject m_DefaultSpellPickupPrefab;
//	[SerializeField] private GameObject m_DefaultItemPickupPrefab;

//	public Dictionary<Item, int> m_ItemDictionary { set; get;}
	public Dictionary<ItemClass, int> m_ItemDictionary;


	/**A list of all SpellClass instances of spells in the inventory.*/
	public List<SpellClass> m_SpellClassList;


	/**A reference to a default spell prefab to contain all our active spell information.*/
	[SerializeField] public GameObject m_DefaultSpellPrefab;

	/**The active spell, of type SpellClass, used for firing spells.*/
	public SpellClass m_ActiveSpellClass = null;
	/**For testing purposes; the name of the active SpellClass instance.*/
	public string m_ActiveSpellName;

	/**A string variable containing the string name of the Input Manager variable responsible for the player switching their active spell.*/
	private readonly string STRINGKEY_INPUT_SWITCHSPELLS = "Switch Spells";

    void Awake()
	{
		//for testing
	}

	void Start()
	{
		//Initialize Item Dictionary
		this.m_ItemDictionary = new Dictionary<ItemClass, int>();

		this.m_SpellClassList = new List<SpellClass>();
		SpellClass spell_class_instance = new SpellClass();
		#if START_WITH_FIREBALL
		this.AddSpell(spell_class_instance.GenerateInstance(SpellName.Fireball));
		this.m_ActiveSpellClass = this.m_SpellClassList[0];
		#endif
		#if START_WITH_ICEBALL
		this.AddSpell (spell_class_instance.GenerateInstance(SpellName.Iceball));
		this.m_ActiveSpellClass = this.m_SpellClassList[0];
		#endif
		#if START_WITH_SHIELD
		this.AddSpell(spell_class_instance.GenerateInstance(SpellName.Shield));
		this.m_ActiveSpellClass = this.m_SpellClassList[0];
		#endif
		#if START_WITH_THUNDERBALL
		this.AddSpell(spell_class_instance.GenerateInstance(SpellName.Thunderball));
		this.m_ActiveSpellClass = this.m_SpellClassList[0];
		#endif
		#if START_WITH_THUNDERSTORM
		this.AddSpell(spell_class_instance.GenerateInstance(SpellName.Thunderstorm));
		this.m_ActiveSpellClass = this.m_SpellClassList[0];
		#endif
		#if START_WITH_HEAL
		this.AddSpell(spell_class_instance.GenerateInstance(SpellName.Heal));
		this.m_ActiveSpellClass = this.m_SpellClassList[0];
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
		else if (this.m_SpellClassList.Count == 1 && this.m_ActiveSpellClass == null)
		{
			//...then assign a default value for the active SpellClass
			this.AssignDefaultActiveSpell ();
		}
		#if TESTING_INVENTORY_CONTENTS_OUTPUT
		if (Input.GetKeyDown (KeyCode.Return)) {
			this.OutputInventoryContents();
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
				this.m_ActiveSpellName = this.m_ActiveSpellClass.m_SpellName.ToString ();

				this.GetComponent<PlayerCastSpell> ().m_SpellClassToFire = this.m_ActiveSpellClass;

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
		this.m_ActiveSpellName = this.m_ActiveSpellClass.m_SpellName.ToString ();

		this.GetComponent<PlayerCastSpell> ().m_SpellClassToFire = this.m_ActiveSpellClass;
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
		foreach (KeyValuePair<ItemClass, int> entry in this.m_ItemDictionary) {
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

	/**A function to add the item to the inventory.*/
	public void AddItem(ItemClass item_to_add)
	{
		#if TESTING_INVENTORY_ITEMPICKUP
		string message = "";
		message += "PlayerInventory::Adding item " + item_to_add.m_ItemName.ToString () +"\t";
		#endif

		foreach (KeyValuePair<ItemClass, int> entry in this.m_ItemDictionary) {
			if (item_to_add.m_ItemName.ToString () == entry.Key.m_ItemName.ToString ()) {
				this.m_ItemDictionary [entry.Key]++;
				#if TESTING_INVENTORY_ITEMPICKUP
				message += "item " + item_to_add.m_ItemName.ToString () + " found in inventory; quantity: " + this.m_ItemDictionary[entry.Key];
				Debug.Log(message);
				#endif
				return;
			}//end if
		}//end foreach
		#if TESTING_INVENTORY_ITEMPICKUP
		message += "item " + item_to_add.m_ItemName.ToString () + " not found in inventory; adding...";
		Debug.Log(message);
		#endif
		this.m_ItemDictionary.Add(item_to_add, 1);
	}//end f'n void AddItem(Item)


	/**A function to add a SpellClass object to the SpellClass list if the SpellClass is not already in the list.*/
	public void AddSpell(SpellClass spell_to_add)
	{
		#if TESTING_INVENTORY_SPELLPICKUP
		string message = "";
		message += "PlayerInventory::Adding spell " + spell_to_add.m_SpellName.ToString () +"\t";
		#endif
		foreach (SpellClass spell in this.m_SpellClassList) {
			if (spell.m_SpellName.ToString () == spell_to_add.m_SpellName.ToString ()) {

				#if TESTING_INVENTORY_SPELLPICKUP
				message += "spell " + spell_to_add.m_SpellName.ToString () +" already found in inventory; making no changes";
				Debug.Log(message);
				#endif
				return;
			}
		}
		#if TESTING_INVENTORY_SPELLPICKUP
		message += "spell " + spell_to_add.m_SpellName.ToString () +" not already found in inventory; adding...";
		Debug.Log(message);
		#endif
		this.m_SpellClassList.Add (spell_to_add);

	}//end f'n void AddSpell(SpellClass)

}//end class PlayerInventory
