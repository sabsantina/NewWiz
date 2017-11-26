/*
* An interface to be implemented in each enemy type.
*/
public interface IEnemy {
	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.*/
	void Move ();

	/**A function to have the enemy apply their given attack on the player.*/
	void Attack();

	/**A function to execute and control the enemy's death animation*/
	void Die();

	/**A function to affect the enemy's health*/
	void AffectHealth (float effect);

	/**A function to apply a given spell's effects on the enemy, including damage.*/
	void ApplySpellEffect (SpellClass spell);
}
