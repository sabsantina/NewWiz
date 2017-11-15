/**
* Unlike the Player class, it's very likely that the Enemy class will only wind up becoming a superclass.
*/

#define TESTING_ALWAYS_DROP_ITEM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : MonoBehaviour {
	[SerializeField] public AudioClip attackSound;
	[SerializeField] AudioClip enemyDamaged;
	public float m_Health;

	[SerializeField] public Spawner m_Spawner;

	/**A float to contain the value of the player's full health*/
    public readonly float ENEMY_FULL_HEALTH = 100.0f;
//	/**The time until the enemy thaws and can move again.*/
//	public float TIME_BEFORE_THAW = 2.5f;
//	/**The time until the enemy is no longer being shocked; at which point it can move again and stop hopping.*/
//	public float TIME_BEFORE_SHOCK_RELEASE = 1.25f;
//    /**Boolean to check if enemy can move. To be used for the IceBall effects*/
//	public bool m_IsFrozen;
//    /**Timer to be used for the freezing effect.*/
//    private float freeze_Timer = 0.0f;
//
//	/**A bool letting us know whether or not the enemy is shocked.*/
//	public bool m_IsShocked;
//
//	/**Time to be used for the shocking effect.*/
//	public float m_ShockTimer = 0.0f;
	/**A variable to keep track of how many times we've incremented the shock timer by units of 0.5.
	*The plan is for the enemy to move up/down every 0.5 seconds. So if the time before shock release is 3.0, the enemy should move a grand total of 6 times; three times upward, and three times downward.*/
	public int m_ShockTimerIncrementor = 1;
	/**A variable to keep track of the enemy start height, to ensure we don't wind up leaving them somewhere floating after shocking them.*/
	private float m_EnemyStartHeight = 0.0f;
	/**A variable to manage the distance of the height increase the enemy undergoes while shocked.*/
	public float m_ShockJumpDistance = 0.25f;
	/**A variable to manage the frequency of the shock stutters the enemies go through (the lower this value, the more frequent the stutters will be).*/
	public float m_ShockJumpFrequency = 0.0005f;
    /**A variable to control the percentage of the drop of the enemy*/
    public float m_HealthPotionDropPercentage = 58.0f;

	/**A bool to let us know whether or not to stop the enemy moving.*/
	public bool m_CanMove = true;
	/**A bool to let us know whether or not the enemy's alive.*/
	public bool m_IsAlive = true;
	/**The spell hitting the enemy*/
	public SpellClass m_SpellHittingEnemy = null;
	public SpellName m_SpellName;
	/**A float to help us keep track of a timer to regulate any extra effects (i.e., freezing, shocking)*/
	public float m_ExtraEffectTimer = 0.0f;

    void Start()
    {
		m_Health = ENEMY_FULL_HEALTH;
//        m_IsFrozen = false;
//		this.m_IsShocked = false;
		this.m_EnemyStartHeight = this.transform.position.y;
    }
    void Update()
    {
        /**Should add animation for death here later.*/
        if(this.m_Health <= 0.0f)
        {
			string message = "Enemy dead! ";
			this.m_IsAlive = false;
			#if TESTING_ALWAYS_DROP_ITEM
			this.m_Spawner.Spawn_Item(ItemName.Mana_Potion, this.transform.position + Vector3.right * 2.0f);

			this.m_Spawner.Spawn_Spell(SpellName.Iceball, this.transform.position + Vector3.left * 2.0f);
			#else
            /**Check to see if enemy will drop potion at given percentage*/
            if (Utilities.ProbabilityCheck(m_HealthPotionDropPercentage))
            {
                //Item is spawned at death position.
				this.m_Spawner.Spawn_Item(ItemName.Health_Potion, this.transform.position);
            }
			#endif
			//if the enemy has a parent with a detection area...
			if (this.gameObject.transform.GetComponentInParent<EnemyDetectionArea> () != null) {
				message += "Has enemy detection area; therefore destroying parent.";
				//...then destroy its parent, along with THIS gameobject
				GameObject.Destroy (this.gameObject.transform.parent.gameObject);
			} else {
				message += "Has no enemy detection area; therefore destroying THIS gameobject.";
				GameObject.Destroy(this.gameObject);
			}
			Debug.Log (message);
        }

		/**If the enemy's been hit by a spell and the spell has a lasting effct on the enemy...*/
		if (this.m_SpellHittingEnemy != null && this.m_SpellHittingEnemy.m_EffectDuration > 0.0f) {
			//...then ensure the enemy suffers the extra infliction that comes with the spell they've been hit with
			this.ExecuteExtraInfliction ();
		}

		return;
//
//		//if the enemy is either frozen or shocked...
//		if (m_IsFrozen || this.m_IsShocked) {
//			this.m_CanMove = false;
//			//...then stop the animator
//			this.gameObject.GetComponent<Animator> ().enabled = false;
//			//...if the enemy is specifically frozen...
//			if (m_IsFrozen) {
//				//...then increment the freeze timer and release the enemy after the alloted time.
//				freeze_Timer += Time.deltaTime;
//				if (freeze_Timer >= TIME_BEFORE_THAW) {
//					this.gameObject.GetComponent<Animator> ().enabled = true;
//					m_IsFrozen = false;
//				}//end if
//			}//end if
//			//else if the enemy is shocked...
//			else {
//				//...then increment the shock timer and release the enemy after the alloted time.
//				this.m_ShockTimer += Time.deltaTime;
//				//if the shock timer exceeds or is equal to the time of shock release (where the enemy is no longer being shocked)...
//				if (this.m_ShockTimer >= TIME_BEFORE_SHOCK_RELEASE) {
//					//...then release the enemy
//					this.gameObject.GetComponent<Animator> ().enabled = true;
//					this.m_IsShocked = false;
//					//...ensure the enemy is at the proper height
//					Vector3 position = this.transform.position;
//					position.y = this.m_EnemyStartHeight;
//					this.transform.position = position;
//				}//end if
//				//else if the shock timer is less than the time of shock release (where the enemy is still being shocked)...
//				else {
//					//...then move the enemy either up or down, with respect to the shock timer incrementor
//
//					//if the shock timer is greater-than or equal to the shock incrementor * shock jump frequency...
//					if (this.m_ShockTimer >= this.m_ShockTimerIncrementor * this.m_ShockJumpFrequency) {
//						//...then move the enemy either up or down
//
//						Vector3 position = this.transform.position;
//						//...where if the shock timer incrementor is odd, we move the enemy up
//						//...and if the shock timer incrementor is even, we move the enemy back down
//						position.y += (this.m_ShockTimerIncrementor % 2 == 1) ? this.m_ShockJumpDistance : -(this.m_ShockJumpDistance);
//						this.transform.position = position;
//
//						this.m_ShockTimerIncrementor++;
//					}//end if
//				}//end else
//			}//end else
//		}//end if
//		else {
//			this.m_CanMove = true;
//			this.m_ShockTimer = 0.0f;
////			this.freeze_Timer = 0.0f;
//			this.m_ShockTimerIncrementor = 1;
//		}
	}//end f'n void Update()


    /**A function to add [effect] to the enemys's health.*/
    public void AffectHealth(float effect)
    {
		this.gameObject.GetComponent<AudioSource> ().PlayOneShot (enemyDamaged);
        this.m_Health += effect;
    }//end f'n void AffectHealth(float)


	/**Function which applies the effect of a spell on the enemy.
     *Should make it abstract when we add a variety of enemies.*/
	public void ApplySpellEffects(SpellName spell_name)
	{
		this.m_SpellHittingEnemy = new SpellClass ();
		switch ((int)spell_name) {
		case (int)SpellName.Fireball:
			{
				this.AffectHealth (-10.0f);

				this.m_SpellHittingEnemy = this.m_SpellHittingEnemy.GenerateInstance (SpellName.Fireball);
				break;
			}//end case Fireball
        case (int)SpellName.Iceball:
            {
                this.AffectHealth(-2.0f);
                
				this.m_SpellHittingEnemy = this.m_SpellHittingEnemy.GenerateInstance (SpellName.Iceball);
				break;
            }//end case Iceball
		case (int)SpellName.Thunderball:
			{
				this.AffectHealth (-5.0f);
				this.m_ShockTimerIncrementor = 1;

				this.m_SpellHittingEnemy = this.m_SpellHittingEnemy.GenerateInstance (SpellName.Thunderball);
				break;
			}//end case Thunderball
		case (int)SpellName.Thunderstorm:
			{
				this.AffectHealth (-40.0f);
				if (this.m_ShockTimerIncrementor > 10) {
					this.m_ShockTimerIncrementor = 1;
				}

				//No point making a section for thunderstorm; thunderball can be used for the same purpose.
				//We'll make an instance of thunderstorm but let the switch fall through to thunderball
				this.m_SpellHittingEnemy = this.m_SpellHittingEnemy.GenerateInstance (SpellName.Thunderstorm);
				break;
			}
		default:
			{
				//Impossible, right now
				break;
			}
		}//end switch
		this.m_SpellName = this.m_SpellHittingEnemy.m_SpellName;

	}//end f'n void ApplySpellEffects(SpellName)

	/**A function to inflict the enemy with an extra effect (beyond losing health)*/
	private void ExecuteExtraInfliction()
	{
		/*
	There's three phases to a spell with a duration greater than 0; 
		1. the start of the timer
		2. the time where the timer has started but hasn't finished
		3. the end of the timer
		*/
		switch ((int)this.m_SpellHittingEnemy.m_SpellName) {
		case (int)SpellName.Fireball:
			{
				//do nothing
				break;
			}
		case (int)SpellName.Iceball:
			{
				//Freeze the enemy for Iceball.duration and then let them go

				//Phase 1
				if (this.m_ExtraEffectTimer == 0.0f) {
					//...stop the enemy from moving
					this.m_CanMove = false;
					//...and stop the animator
					this.gameObject.GetComponent<Animator> ().enabled = false;
					this.m_ExtraEffectTimer += Time.deltaTime;
				} 
				//Phase 2
				else if (0.0f < this.m_ExtraEffectTimer && this.m_ExtraEffectTimer < this.m_SpellHittingEnemy.m_EffectDuration) {
					//Increment timer
					this.m_ExtraEffectTimer += Time.deltaTime;
				} 
				//Phase 3
				//if the spell effect duration <= the spell effect timer
				else {
					//Unfreeze the enemy
					this.m_CanMove = true;
					this.gameObject.GetComponent<Animator> ().enabled = true;

					//Reset the effect timer
					this.m_ExtraEffectTimer = 0.0f;
					//Reset the spell we were hit with
					this.m_SpellHittingEnemy = null;
				}//end else

				break;
			}//end case Iceball
			//Let thunderstorm case fall to thunderball
		case (int)SpellName.Thunderstorm:
		case (int)SpellName.Thunderball:
			{
				//Shock the enemy for Thunderball.duration and let them go

				//Phase 1
				if (this.m_ExtraEffectTimer == 0.0f) {
					//Stop the enemy moving
					//...stop the enemy from moving
					this.m_CanMove = false;
					//...and stop the animator
					this.gameObject.GetComponent<Animator> ().enabled = false;
					this.m_ExtraEffectTimer += Time.deltaTime;
				} 
				//Phase 2
				else if (0.0f < this.m_ExtraEffectTimer && this.m_ExtraEffectTimer < this.m_SpellHittingEnemy.m_EffectDuration) {
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
					//...then release the enemy
					this.gameObject.GetComponent<Animator> ().enabled = true;
					this.m_CanMove = true;
					//...ensure the enemy is at the proper height
					Vector3 position = this.transform.position;
					position.y = this.m_EnemyStartHeight;
					this.transform.position = position;

					//Reset the shock timer incrementor
					this.m_ShockTimerIncrementor = 1;
					//Reset the effect timer
					this.m_ExtraEffectTimer = 0.0f;
					//Reset the spell we were hit with
					this.m_SpellHittingEnemy = null;
				}//end if

				break;
			}//end case thunderball
		}//end switch
	}//end f'n void ExecuteExtraInfliction()

}
