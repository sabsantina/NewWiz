/*
* A temp class for spawning items and whatever else could potentially be spawned, for testing purposes.
*/

//#define TESTING_ITEM_PICKUP
#define TESTING_WHATAMISPAWNING

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	/**A reference to the default item prefab to be spawned, before it is set to represent a specific item.*/
	[SerializeField] private GameObject m_DefaultItemPickupPrefab;
	/**A reference to the default spell prefab to be spawned, before it is set to represent a specific spell.*/
	[SerializeField] private GameObject m_DefaultSpellPickupPrefab;

	//*** Item sprites

	/**The sprite to be used for a health potion*/
	[SerializeField] private Sprite m_HealthPotionSprite;
	/**The sprite to be used for a mana potion*/
	[SerializeField] private Sprite m_ManaPotionSprite;

	//*** Spell sprites
	/**The sprite to be used for the fireball spell when it is a pickup*/
	[SerializeField] private Sprite m_FireballSprite;
	/**The sprite to be used for the iceball spell when it is a pickup*/
	[SerializeField] private Sprite m_IceballSprite;
	/**The sprite to be used for the thunderball spell when it is a pickup*/
	[SerializeField] private Sprite m_ThunderballSprite;
	/**The sprite to be used for the shield spell when it is a pickup*/
	[SerializeField] private Sprite m_ShieldSprite;
	/**The sprite to be used for the heal spell when it is a pickup*/
	[SerializeField] private Sprite m_HealSprite;
	/**The sprite to be used for the thunderstorm spell when it is a pickup*/
	[SerializeField] private Sprite m_ThunderstormSprite;
	/**The sprite to be used for the tornado spell when it is a pickup*/
	[SerializeField] private Sprite m_TornadoSprite;
	/**The sprite to be used for the water bubble spell when it is a pickup*/
	[SerializeField] private Sprite m_WaterBubbleSprite;

	//*** Quest Item spawning requisites
	//Pretty sure all we need is one default item prefab (with the respective sprite enumerated below)
	[SerializeField] private GameObject m_DefaultQuestItemPrefab;
	/**Quest item potion of wisdom sprite; for quest POTION_MASTER*/
	[SerializeField] private Sprite m_PotionOfWisdomSprite;



	//*** Enemy-spawning requisites
	//Every enemy needs a reference to the player
	/**A reference to the player, to pass to the enemy classes on creation.*/
	[SerializeField] private Player m_Player;
	//Every magical enemy needs a reference to the spell animator manager
	[SerializeField] private SpellAnimatorManager m_SpellAnimatorManager;
    //**Infantry units
    /**Claw attack melee animation gameobject*/
    [SerializeField] private GameObject m_ClawAnimationPrefab;
    /**Normal attack melee animation gameobject*/
    [SerializeField] private GameObject m_NormalHitAnimationPrefab;
    //*Rooster
    /**Rooster gameobject prefab*/
    [SerializeField] private GameObject m_RoosterPrefab;
	//*Armored Soldier
	/**Armored soldier gameobject prefab*/
	[SerializeField] private GameObject m_ArmoredSoldierPrefab;
    /**Wolf gameobject prefab*/
    [SerializeField] private GameObject m_WolfPrefab;
    //*Rooster mage
    /**Rooster mage gameobject prefab*/
    [SerializeField] private GameObject m_RoosterMagePrefab;
	//The rooster mage uses the same melee animation as the Rooster, so there's no need to get that again



	private Sprite m_SpriteToBeUsed;

	#if TESTING_ITEM_PICKUP
	/**A reference to the player, just so we can consistently spawn something close to the player's transform.position.*/
	[SerializeField] private GameObject m_Player;
	#endif

	void Awake()
	{
		//this.m_ArmoredSoldierAnimationPrefab = this.m_RoosterAnimationPrefab;
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
				this.m_SpriteToBeUsed = this.m_HealthPotionSprite;
				break;
			}
		case (int)ItemName.Mana_Potion:
			{
				instance.GenerateInstance (ItemName.Mana_Potion);
				this.m_SpriteToBeUsed = this.m_ManaPotionSprite;
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
		//Set the sprite
		obj.transform.GetComponentInChildren<SpriteRenderer> ().sprite = this.m_SpriteToBeUsed;
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
				this.m_SpriteToBeUsed = this.m_FireballSprite;
				break;
			}
		case (int)SpellName.Iceball:
			{
				instance = instance.GenerateInstance (SpellName.Iceball);
				this.m_SpriteToBeUsed = this.m_IceballSprite;
				break;
			}
		case (int)SpellName.Thunderball:
			{
				instance = instance.GenerateInstance (SpellName.Thunderball);
				this.m_SpriteToBeUsed = this.m_ThunderballSprite;
				break;
			}
		case (int)SpellName.Shield:
			{
				instance = instance.GenerateInstance (SpellName.Shield);
				this.m_SpriteToBeUsed = this.m_ShieldSprite;
				break;
			}
		case (int)SpellName.Thunderstorm:
			{
				instance = instance.GenerateInstance (SpellName.Thunderstorm);
				this.m_SpriteToBeUsed = this.m_ThunderstormSprite;
				break;
			}
		case (int)SpellName.Heal:
			{
				instance = instance.GenerateInstance (SpellName.Heal);
				this.m_SpriteToBeUsed = this.m_HealSprite;
				break;
			}
		case (int)SpellName.Tornado:
			{
				instance = instance.GenerateInstance (SpellName.Tornado);
				this.m_SpriteToBeUsed = this.m_TornadoSprite;
				break;
			}
		case (int)SpellName.WaterBubble:
			{
				instance = instance.GenerateInstance (SpellName.WaterBubble);
				this.m_SpriteToBeUsed = this.m_WaterBubbleSprite;
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
		obj.transform.GetComponentInChildren<SpriteRenderer> ().sprite = this.m_SpriteToBeUsed;

		obj.transform.position = position;
	}

	/**A function to be called from the Quest Manager class, to return an instance of a given quest item, to be spawned from the quest manager on quest start.*/
	public GameObject SpawnQuestItem(QuestItemName quest_item_name, Transform parent = null)
	{
		//A default value, to be overwritten (Though this is literally what will be spawned if no case is reached in the switch xD)
		GameObject generated_instance = this.m_RoosterPrefab;
		if (parent != null) {//A default value - the only real thing telling the quest items apart are their names and sprites
			generated_instance = GameObject.Instantiate (this.m_DefaultQuestItemPrefab, parent);
		} else {
			generated_instance = GameObject.Instantiate (this.m_DefaultQuestItemPrefab);
		}
		generated_instance.GetComponent<QuestItemPickup> ().m_QuestItem = new QuestItem();
		switch ((int)quest_item_name) {
		case (int)QuestItemName.POTION_OF_WISDOM:
			{
				QuestItem quest_item = generated_instance.GetComponent<QuestItemPickup> ().m_QuestItem;
				quest_item.m_QuestItemName = QuestItemName.POTION_OF_WISDOM;
				quest_item.m_QuestItemSprite = this.m_PotionOfWisdomSprite;
				break;
			}//end case Potion of wisdom (POTION MASTER quest)
		}//end switch
		return generated_instance;
	}//end f'n SpawnQuestItem)QuestItemName)



	/**A function to be called from the Quest Manager class, to return an instance of a given enemy, to be spawned from the quest manager on quest start.*/
	public GameObject SpawnEnemy(EnemyName enemy_name, Transform parent = null)
	{
		//A default value, to be overwritten
		GameObject generated_instance = this.m_RoosterPrefab;

		switch ((int)enemy_name) {
		//Infantry Units
		//Case Rooster
		case (int)EnemyName.ROOSTER:
			{
				if (parent != null) {
					generated_instance = GameObject.Instantiate (this.m_RoosterPrefab, parent);
				} else {
					generated_instance = GameObject.Instantiate (this.m_RoosterPrefab);
				}
				Rooster rooster_component = generated_instance.GetComponentInChildren<Rooster> ();
				rooster_component.m_AttackPattern.m_EnemyAttackHitAnimation = this.m_ClawAnimationPrefab;
				rooster_component.SetPlayer (this.m_Player);
				break;
			}//end case Rooster
			//Case Armored Soldier
		case (int)EnemyName.ARMORED_SOLDIER:
			{
				if (parent != null) {
					generated_instance = GameObject.Instantiate (this.m_ArmoredSoldierPrefab, parent);
				} else {
					generated_instance = GameObject.Instantiate (this.m_ArmoredSoldierPrefab);
				}
				ArmoredSoldier AS_component = generated_instance.GetComponentInChildren<ArmoredSoldier> ();
				AS_component.m_AttackPattern.m_EnemyAttackHitAnimation = this.m_NormalHitAnimationPrefab;
				AS_component.SetPlayer (this.m_Player);
				break;
			}//end case ARMORED SOLDIER
             //Case Wolf
        case (int)EnemyName.WOLF:
            {
                if (parent != null)
                {
                    generated_instance = GameObject.Instantiate(this.m_WolfPrefab, parent);
                }
                else
                {
                    generated_instance = GameObject.Instantiate(this.m_WolfPrefab);
                }
                Wolf WL_component = generated_instance.GetComponentInChildren<Wolf>();
                WL_component.m_AttackPattern.m_EnemyAttackHitAnimation = this.m_ClawAnimationPrefab;
                WL_component.SetPlayer(this.m_Player);
                break;
            }//end case Wolf

            //Ranged enemies
            //Case Rooster Mage
            case (int)EnemyName.ROOSTER_MAGE:
			{
				if (parent != null) {
					generated_instance = GameObject.Instantiate (this.m_RoosterMagePrefab, parent);
				} else {
					generated_instance = GameObject.Instantiate (this.m_RoosterMagePrefab);
				}
				RoosterMage RM_component = generated_instance.GetComponentInChildren<RoosterMage> ();
				RM_component.m_AttackPattern.m_EnemyHitAnimation = this.m_ClawAnimationPrefab;
				RM_component.SetPlayer (this.m_Player);
				RM_component.m_AttackPattern.m_SpellAnimatorManager = this.m_SpellAnimatorManager;
				break;
			}//end case ROOSTER MAGE
		}//end switch
		return generated_instance;
	}//end f'n SpawnEnemy(EnemyName)


}
