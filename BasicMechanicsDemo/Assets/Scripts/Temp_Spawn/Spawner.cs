/*
* A temp class for spawning items and whatever else could potentially be spawned, for testing purposes.
*/

#define TESTING_ITEM_PICKUP
#define TESTING_WHATAMISPAWNING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	#if TESTING_ITEM_PICKUP
	/**A reference to the default item prefab to be spawned, before it is set to represent a specific item.*/
	[SerializeField] private GameObject m_DefaultItemPickupPrefab;
	/**A reference to the default spell prefab to be spawned, before it is set to represent a specific spell.*/
	[SerializeField] private GameObject m_DefaultSpellPickupPrefab;
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
			this.Spawn_Item (ItemName.Health_Potion, this.m_Player.transform.position + Vector3.left * 2.0f);
//			Debug.Log("Spawner:\nSpawning an item with properties:\t" + this.m_ItemInstances[0].m_Item.m_ItemName.ToString());
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			this.Spawn_Spell (SpellName.Fireball, this.m_Player.transform.position + Vector3.right * 2.0f);
//			Debug.Log("Spawner:\nSpawning a spell with properties:\t" + this.m_SpellInstances[0].m_Spell.m_SpellName.ToString());
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			this.Spawn_Item (ItemName.Mana_Potion, this.m_Player.transform.position + Vector3.forward * 2.0f);
//			Debug.Log("Spawner:\nSpawning an item with properties:\t" + this.m_ItemInstances[1].m_Item.m_ItemName.ToString());
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			this.Spawn_Spell (SpellName.Thunderball, this.m_Player.transform.position + Vector3.back * 2.0f);
//			Debug.Log("Spawner:\nSpawning a spell with properties:\t" + this.m_SpellInstances[1].m_Spell.m_SpellName.ToString());
		}
		#endif
	}

	public void Spawn_Item(ItemName item_name, Vector3 position)
	{
		#if TESTING_WHATAMISPAWNING
		Debug.Log ("Spawning a " + item_name.ToString ());
		#endif
		ItemClass instance = new ItemClass ();
		switch ((int)item_name) {
		case (int)ItemName.Health_Potion:
			{
				instance.GenerateInstance (ItemName.Health_Potion);
				break;
			}
		case (int)ItemName.Mana_Potion:
			{
				instance.GenerateInstance (ItemName.Mana_Potion);
				break;
			}
		default:
			{
				//impossible
				break;
			}
		}//end switch

		GameObject obj = GameObject.Instantiate (this.m_DefaultItemPickupPrefab);
		obj.GetComponent<ItemPickup> ().SetItem(instance);

		obj.transform.position = position;
	}

	public void Spawn_Spell(SpellName spell_name, Vector3 position)
	{
		#if TESTING_WHATAMISPAWNING
		Debug.Log ("Spawning a " + spell_name.ToString ());
		#endif
		SpellClass instance = new SpellClass ();
		switch ((int)spell_name) {
		case (int)SpellName.Fireball:
			{
				instance = instance.GenerateInstance (SpellName.Fireball);
				break;
			}
		case (int)SpellName.Iceball:
			{
				instance = instance.GenerateInstance (SpellName.Iceball);
				break;
			}
		case (int)SpellName.Thunderball:
			{
				instance = instance.GenerateInstance (SpellName.Thunderball);
				break;
			}
		case (int)SpellName.Shield:
			{
				instance = instance.GenerateInstance (SpellName.Shield);
				break;
			}
		case (int)SpellName.Thunderstorm:
			{
				instance = instance.GenerateInstance (SpellName.Thunderstorm);
				break;
			}
		default:
			{
				//impossible
				break;
			}
		}//end switch

		GameObject obj = GameObject.Instantiate (this.m_DefaultSpellPickupPrefab);

//		Debug.Log ("Spawner:: instance: " + instance.ReturnSpellInstanceInfo ());

		obj.GetComponent<SpellPickup> ().SetSpell(instance);

		obj.transform.position = position;
	}




}
