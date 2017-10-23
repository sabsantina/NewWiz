/*
* A temp class for spawning items and whatever else could potentially be spawned, for testing purposes.
*/

#define TESTING_ITEM_PICKUP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	#if TESTING_ITEM_PICKUP
	/**A reference to the default item prefab to be spawned, before it is set to represent a specific item.*/
	[SerializeField] private GameObject m_TempItemPrefab;
	/**A reference to the default spell prefab to be spawned, before it is set to represent a specific spell.*/
	[SerializeField] private GameObject m_TempSpellPrefab;
	/**A reference to the player, just so we can consistently spawn something close to the player's transform.position.*/
	[SerializeField] private GameObject m_Player;
	#endif

	/**A list of all Item instances we spawn.*/
	private List<ItemPickup> m_ItemInstances;
	/**A list of all Spell instances we spawn.*/
	private List<SpellPickup> m_SpellInstances;

	void Start()
	{
		this.m_ItemInstances = new List<ItemPickup> ();
		this.m_SpellInstances = new List<SpellPickup>();
	}

	// Update is called once per frame
	void Update () {
		#if TESTING_ITEM_PICKUP
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			this.Spawn_Item_HealthPotion (this.m_Player.transform.position + Vector3.left * 2.0f);
			Debug.Log("Spawner:\nSpawning an item with properties:\t" + this.m_ItemInstances[0].m_Item.m_ItemName.ToString());
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			this.Spawn_Spell_Fireball (this.m_Player.transform.position + Vector3.right * 2.0f);
			Debug.Log("Spawner:\nSpawning a spell with properties:\t" + this.m_SpellInstances[0].m_Spell.m_SpellName.ToString());
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			this.Spawn_Item_ManaPotion (this.m_Player.transform.position + Vector3.forward * 2.0f);
			Debug.Log("Spawner:\nSpawning an item with properties:\t" + this.m_ItemInstances[1].m_Item.m_ItemName.ToString());
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			this.Spawn_Spell_Shield (this.m_Player.transform.position + Vector3.back * 2.0f);
			Debug.Log("Spawner:\nSpawning a spell with properties:\t" + this.m_SpellInstances[1].m_Spell.m_SpellName.ToString());
		}
		#endif
	}

	/**Spawns a health potion at the given position.*/
	public void Spawn_Item_HealthPotion(Vector3 position)
	{
		GameObject health_potion = GameObject.Instantiate (this.m_TempItemPrefab);
		health_potion.transform.position = position;
		ItemPickup health_potion_item = health_potion.GetComponent<ItemPickup> ();
		health_potion_item.SetItem(this.GenerateInstance_Item_HealthPotion ());
		this.m_ItemInstances.Add (health_potion_item);
	}//end f'n void Spawn_Item_HealthPotion(Vector3)

	public void Spawn_Item_ManaPotion(Vector3 position)
	{
		GameObject mana_potion = GameObject.Instantiate (this.m_TempItemPrefab);
		mana_potion.transform.position = position;
		ItemPickup mana_potion_item = mana_potion.GetComponent<ItemPickup> ();
		mana_potion_item.SetItem(this.GenerateInstance_Item_ManaPotion ());
		this.m_ItemInstances.Add (mana_potion_item);
	}

	public void Spawn_Spell_Fireball(Vector3 position)
	{
		GameObject fireball = GameObject.Instantiate (this.m_TempSpellPrefab);
		fireball.transform.position = position;
		SpellPickup fireball_spell = fireball.GetComponent<SpellPickup> ();
		fireball_spell.SetSpell(this.GenerateInstance_Spell_Fireball ());
		this.m_SpellInstances.Add (fireball_spell);
	}//end f'n void Spawn_Spell_Fireball(Vector3)

	public void Spawn_Spell_Shield(Vector3 position)
	{
		GameObject shield = GameObject.Instantiate (this.m_TempSpellPrefab);
		shield.transform.position = position;
		SpellPickup shield_spell = shield.GetComponent<SpellPickup> ();
		shield_spell.SetSpell(this.GenerateInstance_Spell_Shield ());
		this.m_SpellInstances.Add (shield_spell);
	}//end f'n void Spawn_Spell_Fireball(Vector3)

	/**A private function to spawn a default health potion item; returns the instance of the Item.*/
	private Item GenerateInstance_Item_HealthPotion()
	{
		Item health_potion = new Item ();
		health_potion.m_ItemEffect = ItemEffect.Gain_Health;
		health_potion.m_ItemName = ItemName.Health_Potion;
		return health_potion;
	}//end f'n Item GenerateInstance_Item_HealthPotion()

	/**A private function to spawn a default mana potion item; returns the instance of the Item.*/
	private Item GenerateInstance_Item_ManaPotion()
	{
		Item mana_potion = new Item ();
		mana_potion.m_ItemEffect = ItemEffect.Gain_Mana;
		mana_potion.m_ItemName = ItemName.Mana_Potion;
		return mana_potion;
	}//end f'n Item GenerateInstance_Item_ManaPotion()

	/**A private function to spawn a default fireball spell; returns the instance of the Spell.*/
	private Spell GenerateInstance_Spell_Fireball()
	{
		Spell fireball = new Spell ();
		fireball.m_SpellName = SpellName.Fireball;
		fireball.m_SpellEffect = SpellEffect.Fire_Damage;
		fireball.m_IsMobileSpell = true;
		return fireball;
	}//end f'n Spell GenerateInstance_Spell_Fireball()

	/**A private function to spawn a default shield spell; returns the instance of the Spell.*/
	private Spell GenerateInstance_Spell_Shield()
	{
		Spell shield = new Spell ();
		shield.m_SpellName = SpellName.Shield;
		shield.m_SpellEffect = SpellEffect.Damage_Resistance;
		shield.m_IsMobileSpell = true;
		return shield;
	}//end f'n Spell GenerateInstance_Spell_Shield()
}
