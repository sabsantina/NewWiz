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
	/**A vector to store the player's position in the world at the time of the save.*/
	public Vector3 m_PlayerPositionInWorld = new Vector3();
	/**A variable to store the value of the player health*/
	public float m_PlayerHealth;
	/**A variable to store the maximal value of the player health*/
	public float m_MaxPlayerHealth;
	/**A variable to store the value of the player mana*/
	public float m_PlayerMana;
	/**A variable to store the maximal value of the player mana*/
	public float m_MaxPlayerMana;

	/**A function to go through the contents of the player item dictionary and separate its item names and quantities into serializable formats.*/
	public void ParseItemDictionary(Dictionary<ItemClass, int> all_items_in_inventory)
	{
		//foreach keyvaluepair in all items...
		foreach (KeyValuePair<ItemClass, int> entry in all_items_in_inventory) {
			//add the item name
			this.m_ItemNames.Add (entry.Key.m_ItemName.ToString ());
			//then the quantity for that item (order will matter in the loading process).
			this.m_ItemQuantities.Add (entry.Value);
		}//end foreach
	}//end f'n void ParseItemDictionary(Dictionary<Item, int>)

	/**A function to go through the spell list and gather the spell names found in the player inventory.*/
	public void ParseSpellList(List<SpellClass> all_spells_in_inventory)
	{
		//foreach spell in the player's inventory
		foreach (SpellClass spell in all_spells_in_inventory) {
			//add the spell's name to the list of spells
			this.m_SpellNames.Add (spell.m_SpellName.ToString ());
		}//end foreach
	}//end f'n void ParseSpellList(List<SpellClass>)

	/**A function to gather the player position in the world (little to do here, since the Vector3 format is serializable as it is).*/
	public void GatherPlayerPosition(GameObject player)
	{
		this.m_PlayerPositionInWorld = player.transform.position;
	}//end f'n void GatherPlayerPosition(GameObject)

	/**A function to gather all player attributes (health and mana, as well as the full possible value of each).*/
	public void GatherPlayerAttributes(Player player)
	{
		this.m_PlayerHealth = player.m_Health;
		this.m_PlayerMana = player.m_Mana;
		this.m_MaxPlayerHealth = player.PLAYER_FULL_HEALTH;
		this.m_MaxPlayerMana = player.PLAYER_FULL_MANA;
	}//end f'n void GatherPlayerAttributes(Player)

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
