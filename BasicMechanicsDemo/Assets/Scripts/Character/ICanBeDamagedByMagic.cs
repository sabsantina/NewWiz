/*
* An interface for all those capable of being damaged by magic
*/

public interface ICanBeDamagedByMagic {

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	void ApplySpellEffect (SpellClass spell);

	/**A function to tell us whether or not the thing is currently affected by magic*/
	bool IsAffectedByMagic();

	/**A function to tell us the spell currently affecting the character.*/
	SpellClass SpellAffectingCharacter ();

}
