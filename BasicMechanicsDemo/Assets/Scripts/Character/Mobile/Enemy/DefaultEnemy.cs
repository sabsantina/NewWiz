using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A boxcollider for spell collisions
[RequireComponent(typeof(BoxCollider))]
//An audiosource for the sounds
[RequireComponent(typeof(AudioSource))]
public abstract class DefaultEnemy : MonoBehaviour, IEnemy, ICanBeDamagedByMagic {
	/**The sound the enemy makes when damaged.
	*To be set in the inspector; respective attack sounds are found in the corresponding attack patterns*/
	[SerializeField] public AudioClip m_EnemyDamagedSound;
	/**A reference to the player, so we can get the player's position.*/
	[SerializeField] protected Player m_Player;
	/**When we spawn in an enemy, we'll just say [enemy.m_Spawner = this], or whatever.*/
	public Spawner m_Spawner;

	/**A MovementPattern to control the enemy's movement*/
	public MovementPattern m_MovementPattern;
	/**A float to keep track of health.
	*To be overridden, or set, in children classes.*/
	public float m_Health;
	/**A float to represent the enemy's attack damage.
	*To be overridden, or set, in children classes.*/
	public float m_AttackDamageValue;
	/**A bool to let us know whether or not the player is in range of the given enemy's attacks. Probably more for testing than anything else
	* For example, for ranged units, if the player is at all detected then the player is in range. For infantry units, if the player is within a given distance, then the player is in range.*/
	public bool m_PlayerIsInRange = false;
	/**A stringkey for the isDead parameter in the movement pattern animator*/
	private readonly string STRINGKEY_PARAM_ISDEAD = "IsDead";
	/**A bool to tell us whether or not the enemy's affected by a spell*/
	public bool m_IsAffectedBySpell = false;
	/**The spell we need to damage the enemy with*/
	protected SpellClass m_SpellToApply = new SpellClass ();
	/**A bool to help us inhibit movement*/
	public bool m_InhibitMovement = false;
	/**A bool to help us inhibit attack*/
	public bool m_InhibitAttack = false;
	/**A timer to help implement the spell effects*/
	public float m_ExtraEffectTimer = 0.0f;
    /**A bool to check if the boss is healing*/
    public bool m_IsHealing = false;

	/**A variable to keep track of how many times we've incremented the shock timer by units of 0.5.
	*The plan is for the enemy to move up/down every 0.5 seconds. So if the time before shock release is 3.0, the enemy should move a grand total of 6 times; three times upward, and three times downward.*/
	public int m_ShockTimerIncrementor = 1;
	/**A variable to keep track of the enemy start height, to ensure we don't wind up leaving them somewhere floating after shocking them.*/
	private float m_EnemyStartHeight;
	/**A variable to manage the distance of the height increase the enemy undergoes while shocked.*/
	public float m_ShockJumpDistance = 0.25f;
	/**A variable to manage the frequency of the shock stutters the enemies go through (the lower this value, the more frequent the stutters will be).*/
	public float m_ShockJumpFrequency = 0.0005f;

	protected Animator m_Animator;

	/**The string value of our isAttacking_Melee animator parameter*/
	protected readonly string STRINGKEY_PARAM_ISATTACKING_MELEE = "isAttacking_Melee";
	/**The string value of our isAttacking_Ranged animator parameter*/
	protected readonly string STRINGKEY_PARAM_ISATTACKING_RANGED = "isAttacking_Ranged";

	protected float m_ChasePlayerTimer;
	/**The time for this the enemy chases the player if the player attacks the enemy outside of their detection regions.
	*Set to arbitrary value 2.0f; can be modified in child classes.*/
	protected float m_ChasePlayerDuration;

	public EnemyName m_EnemyName;

	protected virtual void Start()
	{
		this.m_Animator = this.GetComponent<Animator> ();
		this.GetComponent<AudioSource> ().volume = 0.15f;
	}

	/**A function to be called from the spawner when spawning enemies.*/
	public void SetPlayer(Player player)
	{
		this.m_Player = player;
	}

	protected void SetChasePlayerSettings(float chase_player_duration)
	{
		this.m_ChasePlayerDuration = chase_player_duration;
		this.m_ChasePlayerTimer = this.m_ChasePlayerDuration;
	}

	public bool IsAffectedByMagic()
	{
		return this.m_IsAffectedBySpell;
	}

	/**A function to establish whether or not the player is in range of the enemy, for the specific enemy. 
	 * - Sets this.mPlayerIsInRange as well as returns a bool*/
	protected virtual bool IsPlayerInRangeOfAttack()
	{
		//To be overridden in children classes
		return false;
	}

	/**A function to regulate the enemy's movement and tell the enemy to move about the scene.*/
	public virtual void Move ()
	{
		//To be overridden in children classes
	}

	/**A function to have the enemy apply their given attack on the player.*/
	public virtual void Attack()
	{
		//To be overridden in children classes
	}

	/**A function to execute and control the enemy's death animation*/
	public void Die()
	{
		bool is_dead = this.m_Health <= 0.0f;
		this.m_MovementPattern.m_Animator.SetBool (STRINGKEY_PARAM_ISDEAD, is_dead);
		//Destroy gameobject after the death animation
//		if (is_dead) {
//			float animation_length = 0.0f;
//			GameObject.Destroy(this.gameObject, animation_length);
//		}
		if (is_dead) {
			this.ManageLootSpawnOnDeath ();
			GameObject.Destroy(this.transform.parent.gameObject);
		}
	}

	/**A function to manage the spawning of loot when a default enemy dies.
	*We'll say most enemies have a 66% chance of spawning either a health potion or a mana potion. They have a 33% chance to spawn anything else.*/
	protected virtual void ManageLootSpawnOnDeath()
	{
		int last_enum_index = (System.Enum.GetValues (typeof(ItemName)).Length - 1);
		float random_number = Random.Range (0.0f, (float)last_enum_index);
		//33% chance spawn health
		if (random_number <= 0.33f) {
			this.m_Spawner.Spawn_Item (ItemName.Health_Potion, this.transform.position);
		} 
		//33% chance spawn mana potion
		else if (0.33f < random_number && random_number <= 0.67f) {
			this.m_Spawner.Spawn_Item (ItemName.Mana_Potion, this.transform.position);
		} 
		//33% chance spawn anything else
		else {
			int random_index = Random.Range (2, last_enum_index);
			foreach (ItemName enum_val in System.Enum.GetValues(typeof(ItemName))) {
				if ((int)enum_val == random_index) {
					this.m_Spawner.Spawn_Item (enum_val, this.transform.position);
				}
			}
		}
	}

	/**A function to affect the enemy's health; damage should be fed as a negative number.*/
	public void AffectHealth (float effect)
	{
		this.m_Health += effect;
		if (effect < 0.0f) {
			this.gameObject.GetComponent<AudioSource> ().PlayOneShot (this.m_EnemyDamagedSound);
		}
	}

	protected virtual void Update()
	{
		if (this.m_IsAffectedBySpell && this.m_ExtraEffectTimer > 0.0f) {
			this.ApplySpellEffect (this.m_SpellToApply);
		}
		//
		if (this.m_ChasePlayerTimer < this.m_ChasePlayerDuration) {
			this.ChasePlayerForSeconds ();
		}
		this.Move ();
	}

	/**A function to tell us if the player is detected in either the attack or the movement region.*/
	protected virtual bool IsPlayerAtAllDetected()
	{
		return false;
		//A function to be overridden in children
	}

	/**A function that executes once the chase player timer's been set to less than the chase player duration.
	*Updates the movement pattern patrol region to the player's position until the duration is reached by the chase player timer.*/
	private void ChasePlayerForSeconds()
	{
		//Set movement pattern patrol region to be at player's position for duration time
		Vector3 vector_to_player = this.m_Player.transform.position - this.transform.position;
		float magnitude = vector_to_player.magnitude;
		vector_to_player = Vector3.ClampMagnitude (vector_to_player, magnitude * 0.65f);
		this.m_MovementPattern.m_PatrolRegion.transform.position = this.transform.position + vector_to_player;
		this.m_ChasePlayerTimer += Time.deltaTime;

		if (this.m_ChasePlayerTimer >= this.m_ChasePlayerDuration) {
			//Set timer to a case wherein the loop will only trigger if set to 0 again
			this.m_ChasePlayerTimer = this.m_ChasePlayerDuration;
			this.m_MovementPattern.m_MaximalVelocity /= 2.5f;
		}
	}

	/**A function to apply a given hostile spell's effects on the enemy, including damage.*/
	public virtual void ApplySpellEffect (SpellClass spell)
	{
		//If this is the first iteration through this function, no matter the spell...
		if (this.m_ExtraEffectTimer == 0.0f) {
			if (!spell.m_IsPersistent) {
				this.AffectHealth (-spell.m_SpellDamage * this.m_Player.m_MagicAffinity);
			}
			this.m_IsAffectedBySpell = true;
			this.m_SpellToApply = spell;

			//Also, if the enemy is being attacked by the player, but the player hasn't been detected in either the movement or attack region
			//then move the region 

			//If this function is called, the enemy is necessarily being attacked, so just check for player detection
			if (!this.IsPlayerAtAllDetected ()) {
				//if not detected then set chase player timer to 0 to tell enemy to chase player
				this.m_ChasePlayerTimer = 0.0f;
				this.m_MovementPattern.m_MaximalVelocity *= 2.5f;
			}
		}

		if (this.m_SpellToApply.m_IsPersistent) {
			this.AffectHealth (-spell.m_SpellDamage * this.m_Player.m_MagicAffinity * Time.deltaTime);
		}

		/*
	There's three phases to a spell with a duration greater than 0; 
		1. the start of the timer
		2. the time where the timer has started but hasn't finished
		3. the end of the timer
		*/
		switch ((int)spell.m_SpellName) {
		case (int)SpellName.Fireball:
			{
				//Damage is already applied, so just 
				this.m_IsAffectedBySpell = false;
				//do nothing
				break;
			}
		case (int)SpellName.Iceball:
			{
				//Freeze the enemy for Iceball.duration and then let them go

				//Phase 1
				//this.InhibitMovement(float time)
				if (this.m_ExtraEffectTimer == 0.0f) {
					//...stop the enemy from moving
					this.m_InhibitMovement = true;
					//...stop the enemy from attacking
					this.m_InhibitAttack = true;
					//...and stop the animator
					this.gameObject.GetComponent<Animator> ().enabled = false;
					this.m_ExtraEffectTimer += Time.deltaTime;
				} 
				//Phase 2
				else if (0.0f < this.m_ExtraEffectTimer && this.m_ExtraEffectTimer < spell.m_EffectDuration) {
					//Increment timer
					this.m_ExtraEffectTimer += Time.deltaTime;
				} 
				//Phase 3
				//if the spell effect duration <= the spell effect timer
				else if (spell.m_EffectDuration <= this.m_ExtraEffectTimer) {
					this.m_IsAffectedBySpell = false;

					//Unfreeze the enemy
					this.m_InhibitMovement = false;
					this.m_InhibitAttack = false;
					this.gameObject.GetComponent<Animator> ().enabled = true;

					//Reset the effect timer
					this.m_ExtraEffectTimer = 0.0f;
				}//end else

				break;
			}//end case Iceball
			//Let thunderstorm case fall to thunderball
		case (int)SpellName.Thunderstorm:
			{
//				//Thunderstorm is persistent, so affect differently with respect to damage
//				this.AffectHealth(-spell.m_SpellDamage * Time.deltaTime);
				//Transfer flow of control to case Thunderball
				goto case (int)SpellName.Thunderball;
			}
		case (int)SpellName.Thunderball:
			{
				//Shock the enemy for Thunderball.duration and let them go

				//Phase 1
				if (this.m_ExtraEffectTimer == 0.0f) {
					this.m_EnemyStartHeight = this.transform.position.y;
					//Stop the enemy moving
					//...stop the enemy from moving
					this.m_InhibitMovement = true;
					this.m_InhibitAttack = true;
					//...and stop the animator
					this.gameObject.GetComponent<Animator> ().enabled = false;
					this.m_ExtraEffectTimer += Time.deltaTime;
				} 
				//Phase 2
				else if (0.0f < this.m_ExtraEffectTimer && this.m_ExtraEffectTimer < spell.m_EffectDuration) {
					//if the shock timer is greater-than or equal to the shock incrementor * shock jump frequency...
					if (this.m_ExtraEffectTimer >= this.m_ShockTimerIncrementor * this.m_ShockJumpFrequency) {
						//...then move the enemy either up or down

						Vector3 position = this.transform.position;
						//...where if the shock timer incrementor is odd, we move the enemy up
						//...and if the shock timer incrementor is even, we move the enemy back down
						position.y += (this.m_ShockTimerIncrementor % 2 == 1) ? this.m_ShockJumpDistance : -(this.m_ShockJumpDistance);
						this.transform.position = position;

						this.m_ShockTimerIncrementor++;
					}
					this.m_ExtraEffectTimer += Time.deltaTime;
				}
				//Phase 3
				//if the spell effect duration <= the spell effect timer
				else
				{
					this.m_IsAffectedBySpell = false;
					//...then release the enemy
					this.gameObject.GetComponent<Animator> ().enabled = true;
					this.m_InhibitAttack = false;
					this.m_InhibitMovement = false;
					//...ensure the enemy is at the proper height
					Vector3 position = this.transform.position;
					position.y = this.m_EnemyStartHeight;
					this.transform.position = position;

					//Reset the shock timer incrementor
					this.m_ShockTimerIncrementor = 1;
					//Reset the effect timer
					this.m_ExtraEffectTimer = 0.0f;
				}//end if

				break;
			}//end case thunderball
		case (int)SpellName.WaterBubble:
			{
				//Do nothing
				break;
			}
		}//end switch
		
		//if this is the last iteration of the function, no matter the spell...
		if (!this.m_IsAffectedBySpell) {
			this.m_SpellToApply = null;

		}

		//To be overridden in children classes. We'll keep a default version here, though.
		//Note: this is virtual because certain spells may affect certain enemies differently
	}

	public void SetHealth(float health)
	{
		this.m_Health = health;
	}

	public virtual void SetAttackDamageValue()
	{
		//To be overridden in children classes
	}

	public void SetAttackDamageValue(float damage)
	{
		this.m_AttackDamageValue = damage;
	}

	public virtual float GetAttackDamageValue()
	{
		//To be overridden in children classes
		return 0.0f;
	}

	public SpellClass SpellAffectingCharacter ()
	{
		return this.m_SpellToApply;
	}

//	/**A function to set the enemy that's attacking, so we can apply the specific damage to the target*/
//	public virtual void SetAttacker(DefaultEnemy enemy)
//	{
//		//To be overridden in children classes
//	}
}
