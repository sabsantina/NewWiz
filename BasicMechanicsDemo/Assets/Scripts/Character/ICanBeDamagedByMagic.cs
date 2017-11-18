/*
* An interface for all those capable of being damaged by magic
*/

public interface ICanBeDamagedByMagic {

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	void ApplySpellEffect (SpellClass spell);

}
