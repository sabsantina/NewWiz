using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Serializable_Player {
	/**A list containing the string value of every item in the player inventory*/
	public List<string> m_ItemNames = new List<string>();
	/**A list containing the int value of every quantity of every item in the player inventory*/
	public List<int> m_ItemQuantities = new List<int>();
	/**A list containing all spell name strings in the player inventory*/
	public List<string> m_SpellNames = new List<string>();
	/**A string list of all quest item names. Repeated names do not share a slot.*/
	public List<string> m_QuestItemNames = new List<string> ();

	/**A float to store the player's x coordinate at the time of the save.*/
	public float m_PlayerPositionInWorld_X;
	/**A float to store the player's y coordinate at the time of the save.*/
	public float m_PlayerPositionInWorld_Y;
	/**A float to store the player's z coordinate at the time of the save*/
	public float m_PlayerPositionInWorld_Z;

	/**A variable to store the value of the player health*/
	public float m_PlayerHealth;
	/**A variable to store the maximal value of the player health*/
	public float m_MaxPlayerHealth;
	/**A variable to store the value of the player mana*/
	public float m_PlayerMana;
	/**A variable to store the maximal value of the player mana*/
	public float m_MaxPlayerMana;
	/**A variable to store the value of the player;s magic affinity*/
	public float m_PlayerMagicAffinity;

	/**A function to store the contents of the quest item list.*/
	public void ParseQuestItemList(GameObject player)
	{
		PlayerInventory player_inventory = player.GetComponent<PlayerInventory> ();
		foreach (QuestItem item in player_inventory.m_QuestItems) {
			this.m_QuestItemNames.Add (item.m_QuestItemName.ToString ());
		}//end foreach
	}//end f'n void ParseQuestItemList(GameObject)

	/**A function to set the player inventory quest item list based on saved content.
	*Clears player quest item list before adding anything.*/
	public void SetQuestItemList(GameObject player)
	{
		PlayerInventory player_inventory = player.GetComponent<PlayerInventory> ();
		player_inventory.m_QuestItems.Clear ();

		for (int index = 0; index < this.m_QuestItemNames.Count; index++) {
			QuestItem item_to_add = new QuestItem ();
			foreach (QuestItemName name in System.Enum.GetValues(typeof(QuestItemName))) {
				if (this.m_QuestItemNames [index] == name.ToString ()) {
					item_to_add.GenerateQuestItem (name);
					player_inventory.m_QuestItems.Add (item_to_add);
				}//end if
			}//end foreach
		}//end for
	}//end f'n void SetQuestItemList(GameObject)

	/**A function to go through the contents of the player item dictionary and separate its item names and quantities into serializable formats.*/
	public void ParseItemDictionary(Dictionary<ItemClass, int> all_items_in_inventory)
	{
//		Debug.Log ("all items in inventory exists? " + (all_items_in_inventory != null));
		//foreach keyvaluepair in all items...
		foreach (KeyValuePair<ItemClass, int> entry in all_items_in_inventory) {
			//add the item name
			this.m_ItemNames.Add (entry.Key.m_ItemName.ToString ());
			//then the quantity for that item (order will matter in the loading process).
			this.m_ItemQuantities.Add (entry.Value);
		}//end foreach
	}//end f'n void ParseItemDictionary(Dictionary<Item, int>)

	/**A function to set the contents of the player inventory item dictionary.
	*Clears player item dictionary before adding anything.*/
	public void SetItemDictionary(GameObject player)
	{
		PlayerInventory player_inventory = player.GetComponent<PlayerInventory> ();
		player_inventory.m_ItemDictionary.Clear ();

		for (int index = 0; index < this.m_ItemNames.Count; index++) {
			ItemClass item_to_add = new ItemClass ();

			//for every item name...
			foreach (ItemName name in System.Enum.GetValues(typeof(ItemName))) {
				//...if the name in the enum matches that of the list...
				if (this.m_ItemNames [index] == name.ToString ()) {
					//...then create the item and add it with its respective quantity
					item_to_add.GenerateInstance (name);
					player_inventory.m_ItemDictionary.Add (item_to_add, this.m_ItemQuantities [index]);
				}//end if
			}//end foreach

		}//end for
	}//end f'n void SetItemDictionary(GameObject)

	/**A function to go through the spell list and gather the spell names found in the player inventory.*/
	public void ParseSpellList(List<SpellClass> all_spells_in_inventory)
	{
		//foreach spell in the player's inventory
		foreach (SpellClass spell in all_spells_in_inventory) {
			//add the spell's name to the list of spells
			this.m_SpellNames.Add (spell.m_SpellName.ToString ());
		}//end foreach
	}//end f'n void ParseSpellList(List<SpellClass>)

	/**A function to fill the player spell list according to the save.
	*Clears player spell list before adding anything.*/
	public void SetSpellList(GameObject player)
	{
		PlayerInventory player_inventory = player.GetComponent<PlayerInventory> ();
		player_inventory.m_SpellClassList.Clear ();

		for(int index = 0; index < this.m_SpellNames.Count; index++) {
			SpellClass spell_to_add = new SpellClass ();

			//for every spell name...
			foreach (SpellName name in System.Enum.GetValues(typeof(SpellName))) {
				//...if the name in the enum matches that of the list...
				if (this.m_SpellNames [index] == name.ToString ()) {
					//...then create the item from the enum value and add it to the spell list
					spell_to_add = spell_to_add.GenerateInstance (name);
					player_inventory.AddSpell (spell_to_add);
				}//end if
			}//end foreach

		}//end foreach
	}//end f'n void SetSpellList(GameObject player)

	/**A function to gather the player position in the world 
	 * Note: Vector3 is not serializable, so we need to break down each component and send it along as a float.*/
	public void GatherPlayerPosition(GameObject player)
	{
//		this.m_PlayerPositionInWorld = player.transform.position;
		this.m_PlayerPositionInWorld_X = player.transform.position.x;
		this.m_PlayerPositionInWorld_Y = player.transform.position.y;
		this.m_PlayerPositionInWorld_Z = player.transform.position.z;
	}//end f'n void GatherPlayerPosition(GameObject)

	/**A function to set the player position in the world, gathering the individual floats and setting a Vector3 from them*/
	public void SetPlayerPosition(GameObject player)
	{
		Vector3 player_position = new Vector3 (this.m_PlayerPositionInWorld_X, this.m_PlayerPositionInWorld_Y, this.m_PlayerPositionInWorld_Z);
		player.transform.position = player_position;
	}//end f'n void SetPlayerPosition(GameObject)

	/**A function to gather all player attributes (health and mana, as well as the full possible value of each) to get a given GameObject's Player component to store its values.*/
	public void GatherPlayerAttributes(GameObject player_obj)
	{
		Player player = player_obj.GetComponent<Player> ();
		this.m_PlayerHealth = player.m_Health;
		this.m_PlayerMana = player.m_Mana;
		this.m_MaxPlayerHealth = player.PLAYER_FULL_HEALTH;
		this.m_MaxPlayerMana = player.PLAYER_FULL_MANA;
		this.m_PlayerMagicAffinity = player.m_MagicAffinity;
	}//end f'n void GatherPlayerAttributes(GameObject)

	/**A function to execute after loading, to set all player attributes.*/
	public void SetPlayerAttributes(GameObject player_obj)
	{
		Player player = player_obj.GetComponent<Player> ();
		player.m_Health = this.m_PlayerHealth;
		player.m_Mana = this.m_PlayerMana;
		player.PLAYER_FULL_HEALTH = this.m_MaxPlayerHealth;
		player.PLAYER_FULL_MANA = this.m_MaxPlayerMana;
		player.m_MagicAffinity = this.m_PlayerMagicAffinity;
	}//end f'n SetPlayerAttributes(GameObject)

	/**A function to generate our serializable instance.*/
	public Serializable_Player GenerateSerializableInstance(GameObject player)
	{
		Serializable_Player serializable_player = new Serializable_Player ();
		serializable_player.GatherPlayerAttributes (player);
		serializable_player.GatherPlayerPosition (player);

		PlayerInventory inventory = player.GetComponent<PlayerInventory> ();
//		Debug.Log ("Player inventory exists? " + (inventory != null));
//		inventory.OutputInventoryContents ();
//		Debug.Log ("Player dictionary exists? " + (inventory.m_ItemDictionary != null));
		serializable_player.ParseItemDictionary (inventory.m_ItemDictionary);
		serializable_player.ParseSpellList (inventory.m_SpellClassList);
		serializable_player.ParseQuestItemList (player);
		return serializable_player;
	}//end f'n Serializable_Player GenerateSerializableInstance(GameObject)

	/**A function to set all player information.*/
	public void SetAllPlayerInformation(GameObject player)
	{
		this.SetPlayerPosition (player);
		this.SetPlayerAttributes (player);
		this.SetSpellList (player);
		this.SetItemDictionary (player);
		this.SetQuestItemList (player);
	}//end f'n void SetAllPlayerInformation(GameObject)

	/**For testing purposes; a function to use in a debug, to ensure all variables are well-set.*/
	public string ReturnAllInformation()
	{
		string message = "Items:\n";
		for (int index = 0; index < this.m_ItemNames.Count; index++) {
			message += "\tName:\t" + this.m_ItemNames [index] + "\tQuantity:\t" + this.m_ItemQuantities [index] + "\n"; 
		}//end for
		message += "\n";

		message += "Spells:\n";
		for (int index = 0; index < this.m_SpellNames.Count; index++)
		{
			message += "\tSpell:\t" + this.m_SpellNames [index] + "\n";
		}
		message += "\n";

		message += "Player information:\n";
		message += "\tHealth:\t" + this.m_PlayerHealth + "\tMax health:\t" + this.m_MaxPlayerHealth + "\n";
		message += "\tMana:\t" + this.m_PlayerMana + "\tMax mana:\t" + this.m_MaxPlayerMana + "\n";

		return message;
	}//end f'n string ReturnAllInformation()


}//end class Serializable_Player
