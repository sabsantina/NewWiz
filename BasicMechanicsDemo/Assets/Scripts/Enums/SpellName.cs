/*
* An enum to help us keep track of spell names.
* Note that we can't really give the spell effects numerical values as there's no real point to indexing the variables; a spell may have
* more than one effect but it will only ever have one name. Therefore, I indexed the spell names but not the spell effects.
*/

public enum SpellName {
	//Fireball as an example, with corresponding effect Fire_Damage.
	//We need a more comprehensive list of spells and their effects.
	Fireball = 0,
	Shield,
    Iceball,
	Thunderball,
	Thunderstorm,
	Heal,
    Tornado,
    WaterBubble
}
