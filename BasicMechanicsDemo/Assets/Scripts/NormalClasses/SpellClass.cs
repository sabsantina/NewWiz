﻿#define OUTPUT_RETURNED_SPELL

public class SpellClass {

	/**An enum variable for the spell name. See SpellName.cs for all spell names.*/
	public SpellName m_SpellName;
	/**An enum variable for the spell effect. See SpellEffect.cs for all spell effects.*/
	public SpellEffect m_SpellEffect;
	/**An enum variable for the spell type. See SpellType.cs for all spell types.*/
	public SpellType m_SpellType;
	/**A float to denote the duration of the spell's effect; note: the duration of a spell is defined as being the that which occurs after the spell hits the enemy.
	*So in the case of a spell like Iceball, we ask ourselves "For how long is the enemy frozen after being hit?".
	*Also note that in the context of spells that are held down, like Shield or Heal, this parameter is infinite and has no real relevance. We therefore set it to 0 in those cases.*/
	public float m_EffectDuration = 0.0f;
	/**A float to denote the spell damage.
	*If a spell is persistent, this should be the initial damage dealt by the spell, while the other damage will be dealt with respect to time..*/
	public float m_SpellDamage = 0.0f;
	/**A float to manage the spell's mana cost*/
	public float m_ManaCost = 0.0f;
	/**A bool to tell us whether or not the player can hold down the mouse to cast the spell. This is important for mana.*/
	public bool m_IsPersistent = false;
	public float m_SpellVelocity = 0.0f;

	//the following variables to help for spell testing and balancing from the inspector.
	//Fireball
	/**A variable to represent the mana cost of the fireball spell.
	*At starting mana of 100, the player can cast 50 of these before needing a mana refill.*/
	public float FIREBALL_MANA_COST = 2.0f;
	public float FIREBALL_DAMAGE = 10.0f;
	public float FIREBALL_VELOCITY = 15.0f;
	//Iceball
	/**A variable to represent the mana cost of the iceball spell.
	*At starting mana of 100, the player can cast 33 of these before needing a mana refill.*/
	public float ICEBALL_MANA_COST = 3.0f;
	public float ICEBALL_DAMAGE = 7.5f;
	public float ICEBALL_VELOCITY = 20.0f;
	//Thunderball
	/**A variable to represent the mana cost of the thunderball spell.
	*At starting mana of 100, the player can cast 20 of these before needing a mana refill.*/
	public float THUNDERBALL_MANA_COST = 5.0f;
	public float THUNDERBALL_DAMAGE = 17.5f;
	public float THUNDERBALL_VELOCITY = 25.0f;
	//Shield
	/**A variable to represent the mana cost of the shield spell. This is a persisting spell, so the player will be expending 15 mana * a value increasing by Time.deltatime every frame.
	*At starting mana of 100, the player can cast this for a couple of seconds before needing a mana refill.*/
	public float SHIELD_MANA_COST = 50.0f;
	//Thunderstorm
	/**A variable to represent the mana cost of the thunderstorm spell. This is a persisting spell, so the player will be expending 10 mana * a value increasing by Time.deltatime every frame.
	*At starting mana of 100, the player can cast this for a couple of seconds before needing a mana refill.*/
	public float THUNDERSTORM_MANA_COST = 40.0f;
	public float THUNDERSTORM_DAMAGE = 40.0f;
	//velocity?
	//Heal
	/**A variable to represent the mana cost of the heal spell. This is a persisting spell, so the player will be expending 0.5 mana * a value increasing by Time.deltatime every frame.
	*At starting mana of 100, the player can cast this for several seconds before needing a mana refill.*/
	public float HEAL_MANA_COST = 15.0f;
	//Tornado
    /**A variable to represent the mana cost of the tornado spell.
     *At starting mana of 100, the player can cast 14 of these before needing a mana refill.*/
    public float TORNADO_MANA_COST = 4.0f;
	public float TORNADO_DAMAGE = 10.0f;
	public float TORNADO_VELOCITY = 15.0f;
    /**A variable to represent the mana cost of the water bubble spell.
     *At starting mana of 100, the player can cast 25 of these before needing a mana refill.*/
    public float WATER_BUBBLE_MANA_COST = 4.0f;
	public float WATER_BUBBLE_DAMAGE = 8.0f;
	public float WATER_BUBBLE_VELOCITY = 25.0f;

    /**A function to return a SpellClass instance based on the spell name, from the SpellName enum, in its enum form.*/
    public SpellClass GenerateInstance(SpellName spell_name)
	{
		SpellClass spellinstance_to_return = new SpellClass ();
		switch ((int)spell_name) {
		case (int)SpellName.Fireball:
			{
				spellinstance_to_return.m_SpellName = SpellName.Fireball;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Fire_Damage;
				spellinstance_to_return.m_SpellType = SpellType.BASIC_PROJECTILE_ON_TARGET;
				spellinstance_to_return.m_EffectDuration = 0.0f;
				spellinstance_to_return.m_SpellDamage = FIREBALL_DAMAGE;
				spellinstance_to_return.m_ManaCost = FIREBALL_MANA_COST;
				spellinstance_to_return.m_SpellVelocity = FIREBALL_VELOCITY;
				spellinstance_to_return.m_IsPersistent = false;
				break;
			}//end case Fireball
		case (int)SpellName.Shield:
			{
				spellinstance_to_return.m_SpellName = SpellName.Shield;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Damage_Resistance;
				spellinstance_to_return.m_SpellType = SpellType.ON_PLAYER;
				spellinstance_to_return.m_EffectDuration = 0.0f;
				//No damage
				spellinstance_to_return.m_ManaCost = SHIELD_MANA_COST;
				spellinstance_to_return.m_IsPersistent = true;
				break;
			}//end case Shield
		case (int)SpellName.Iceball:
			{
				spellinstance_to_return.m_SpellName = SpellName.Iceball;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Ice_Freeze;
				spellinstance_to_return.m_SpellType = SpellType.BASIC_PROJECTILE_ON_TARGET;
				spellinstance_to_return.m_EffectDuration = 2.5f;
				spellinstance_to_return.m_SpellDamage = ICEBALL_DAMAGE;
				spellinstance_to_return.m_SpellVelocity = ICEBALL_VELOCITY;
				spellinstance_to_return.m_ManaCost = ICEBALL_MANA_COST;
				spellinstance_to_return.m_IsPersistent = false;
				break;
			}//end case Iceball
		case (int)SpellName.Thunderball:
			{
				spellinstance_to_return.m_SpellName = SpellName.Thunderball;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Shock_Damage;
				spellinstance_to_return.m_SpellType = SpellType.BASIC_PROJECTILE_ON_TARGET;
				spellinstance_to_return.m_EffectDuration = 1.25f;
				spellinstance_to_return.m_SpellDamage = THUNDERBALL_DAMAGE;
				spellinstance_to_return.m_SpellVelocity = THUNDERBALL_VELOCITY;
				spellinstance_to_return.m_ManaCost = THUNDERBALL_MANA_COST;
				spellinstance_to_return.m_IsPersistent = false;
				break;
			}//end case Thunderball
		case (int)SpellName.Thunderstorm:
			{
				spellinstance_to_return.m_SpellName = SpellName.Thunderstorm;
				spellinstance_to_return.m_SpellEffect = SpellEffect.AOE_Shock;
				spellinstance_to_return.m_SpellType = SpellType.AOE_ON_TARGET;
				//While it is true that thunderstorm can be used indefinitely, the effects of this spell are electrocution for a given 
				//time; it therefore has a duration value 
				spellinstance_to_return.m_EffectDuration = 1.25f;
				spellinstance_to_return.m_SpellDamage = THUNDERSTORM_DAMAGE;
				spellinstance_to_return.m_ManaCost = THUNDERSTORM_MANA_COST;
				spellinstance_to_return.m_IsPersistent = true;
				break;
			}//end case Thunderstorm
		case (int)SpellName.Heal:
			{
				spellinstance_to_return.m_SpellName = SpellName.Heal;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Heal_Player;
				spellinstance_to_return.m_SpellType = SpellType.ON_PLAYER;
				spellinstance_to_return.m_EffectDuration = 0.0f;
				spellinstance_to_return.m_ManaCost = HEAL_MANA_COST;
				spellinstance_to_return.m_IsPersistent = true;
				break;
			}//end case Heal
        case (int)SpellName.Tornado:
            {
                spellinstance_to_return.m_SpellName = SpellName.Tornado;
                spellinstance_to_return.m_SpellEffect = SpellEffect.Knockback;
                spellinstance_to_return.m_SpellType = SpellType.BASIC_PROJECTILE_ON_TARGET;
                spellinstance_to_return.m_EffectDuration = 0.5f;
                spellinstance_to_return.m_ManaCost = TORNADO_MANA_COST;
				spellinstance_to_return.m_SpellDamage = TORNADO_DAMAGE;
				spellinstance_to_return.m_SpellVelocity = TORNADO_VELOCITY;
                spellinstance_to_return.m_IsPersistent = false;
                break;
            }//end case Tornado
        case (int)SpellName.WaterBubble:
            {
                spellinstance_to_return.m_SpellName = SpellName.WaterBubble;
                spellinstance_to_return.m_SpellEffect = SpellEffect.Water_Damage;
                spellinstance_to_return.m_SpellType = SpellType.BASIC_PROJECTILE_ON_TARGET;
                spellinstance_to_return.m_EffectDuration = 0.0f;
                spellinstance_to_return.m_ManaCost = WATER_BUBBLE_MANA_COST;
				spellinstance_to_return.m_SpellDamage = WATER_BUBBLE_DAMAGE;
				spellinstance_to_return.m_SpellVelocity = WATER_BUBBLE_VELOCITY;
                spellinstance_to_return.m_IsPersistent = false;
                break;
            }//end case Tornado
		default:
			{
				//Impossible
				break;
			}//end case default
		}//end switch
			
		return spellinstance_to_return;
	}//end f'n SpellClass GenerateInstance(SpellName)


	/**A function to return a SpellClass instance's properties in a string for testing purposes.*/
	public string ReturnSpellInstanceInfo()
	{
		string message = "SpellClass::ReturnSpellInstanceInfo()\t:\tSpell Name: " + this.m_SpellName.ToString ()
			+ "\tSpell Effect: " + this.m_SpellEffect.ToString () + " Spell type: " + this.m_SpellType.ToString();
		return message;
	}//end f'n string ReturnSpellInstanceInfo()
}//end class SpellClass
