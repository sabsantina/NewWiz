#define OUTPUT_RETURNED_SPELL

public class SpellClass {

	/**An enum variable for the spell name. See SpellName.cs for all spell names.*/
	public SpellName m_SpellName;
	/**An enum variable for the spell effect. See SpellEffect.cs for all spell effects.*/
	public SpellEffect m_SpellEffect;
	/**A bool to tell us whether or not a spellclass instance is mobile.
	*A mobile spell is a spell that is cast by the player at an enemy (i.e. Fireball), that moves around.
	*An immobile spell is a spell that is cast by the player on the player (i.e. Heal, Shield), that stays fixed on the player.*/
	public bool m_IsMobileSpell;
	/**A bool to tell us whether or not a spell is an AOE spell.*/
	public bool m_IsAOESpell = false;

	/**A function to return a SpellClass instance based on the spell name, from the SpellName enum, in its enum form.*/
	public SpellClass GenerateInstance(SpellName spell_name)
	{
		SpellClass spellinstance_to_return = new SpellClass ();
		switch ((int)spell_name) {
		case (int)SpellName.Fireball:
			{
				spellinstance_to_return.m_SpellName = SpellName.Fireball;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Fire_Damage;
				spellinstance_to_return.m_IsMobileSpell = true;
				break;
			}//end case Fireball
		case (int)SpellName.Shield:
			{
				spellinstance_to_return.m_SpellName = SpellName.Shield;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Damage_Resistance;
				spellinstance_to_return.m_IsMobileSpell = false;
				break;
			}//end case Shield
		case (int)SpellName.Iceball:
			{
				spellinstance_to_return.m_SpellName = SpellName.Iceball;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Ice_Freeze;
				spellinstance_to_return.m_IsMobileSpell = true;
				break;
			}//end case Iceball
		case (int)SpellName.Thunderball:
			{
				spellinstance_to_return.m_SpellName = SpellName.Thunderball;
				spellinstance_to_return.m_SpellEffect = SpellEffect.Shock_Damage;
				spellinstance_to_return.m_IsMobileSpell = true;
				break;
			}//end case Thunderball
		case (int)SpellName.Thunderstorm:
			{
				spellinstance_to_return.m_SpellName = SpellName.Thunderstorm;
				spellinstance_to_return.m_SpellEffect = SpellEffect.AOE_Shock;
				spellinstance_to_return.m_IsMobileSpell = false;
				spellinstance_to_return.m_IsAOESpell = true;
				break;
			}//end case Thunderstorm
		default:
			{
				//Impossible
				break;
			}//end case default
		}//end switch
		return spellinstance_to_return;
	}//end f'n SpellClass GenerateInstance(SpellName)

//	/**A function to simplify the spell application process.*/
//	public void ApplySpellEffect(Enemy enemy, float delay_in_seconds)
//	{
//		switch ((int)this.m_SpellName) {
//			case (int)SpellName.Fireball:
//			{
//				//Fireball is instantaneous, so far, so the delay doesn't bother us so much.
//
//			}
//		}
//	}

	/**A function to return a SpellClass instance's properties in a string for testing purposes.*/
	public string ReturnSpellInstanceInfo()
	{
		string message = "SpellClass::ReturnSpellInstanceInfo()\t:\tSpell Name: " + this.m_SpellName.ToString ()
			+ "\tSpell Effect: " + this.m_SpellEffect.ToString () + "\tisMobile? " + this.m_IsMobileSpell + "\tisAOE? " 
			+ this.m_IsAOESpell;
		return message;
	}//end f'n string ReturnSpellInstanceInfo()
}//end class SpellClass
