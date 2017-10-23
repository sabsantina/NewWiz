/*
* An enum to help us keep track of spell effects.
* Note that we can't really give the spell effects numerical values as there's no real point to indexing the variables; a spell may have
* (but likely won't have) more than one effect but it will only ever have one name. Therefore, I indexed the spell names but not the spell effects.
*/

public enum  SpellEffect
{
	//Fire damage as an example, with corresponding SpellName Fireball. 
	//We need a more comprehensive list of spells and their effects.
	Fire_Damage,
	Damage_Resistance
}
