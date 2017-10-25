/**
* Unlike the Player class, it's very likely that the Enemy class will only wind up becoming a superclass.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float m_Health;

    public readonly float ENEMY_FULL_HEALTH = 100.0f;
	/**The time until the enemy thaws and can move again.*/
	public float TIME_BEFORE_THAW = 2.5f;
	/**The time until the enemy is no longer being shocked; at which point it can move again and stop hopping.*/
	public float TIME_BEFORE_SHOCK_RELEASE = 1.25f;
    /**Boolean to check if enemy can move. To be used for the IceBall effects*/
	public bool m_IsFrozen;
    /**Timer to be used for the freezing effect.*/
    private float freeze_Timer = 0.0f;

	/**A bool letting us know whether or not the enemy is shocked.*/
	public bool m_IsShocked;
	/**Time to be used for the shocking effect.*/
	private float m_ShockTimer = 0.0f;
	/**A variable to keep track of how many times we've incremented the shock timer by units of 0.5.
	*The plan is for the enemy to move up/down every 0.5 seconds. So if the time before shock release is 3.0, the enemy should move a grand total of 6 times; three times upward, and three times downward.*/
	private int m_ShockTimerIncrementor = 1;
	/**A variable to keep track of the enemy start height, to ensure we don't wind up leaving them somewhere floating after shocking them.*/
	private float m_EnemyStartHeight = 0.0f;
	/**A variable to manage the distance of the height increase the enemy undergoes while shocked.*/
	public float m_ShockJumpDistance = 0.25f;
	/**A variable to manage the frequency of the shock stutters the enemies go through (the lower this value, the more frequent the stutters will be).*/
	public float m_ShockJumpFrequency = 0.0005f;

	/**A bool to let us know whether or not to stop the enemy moving.*/
	public bool m_CanMove = true;

    void Start()
    {
		m_Health = ENEMY_FULL_HEALTH;
        m_IsFrozen = false;
		this.m_IsShocked = false;
		this.m_EnemyStartHeight = this.transform.position.y;
    }
    void Update()
    {
        /**Should add animation for death here later.*/
        if(this.m_Health <= 0.0f)
        {
			string message = "Enemy dead! ";
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


		//if the enemy is either frozen or shocked...
		if (m_IsFrozen || this.m_IsShocked) {
			this.m_CanMove = false;
			//...then stop the animator
			this.gameObject.GetComponent<Animator> ().enabled = false;
			//...if the enemy is specifically frozen...
			if (m_IsFrozen) {
				//...then increment the freeze timer and release the enemy after the alloted time.
				freeze_Timer += Time.deltaTime;
				if (freeze_Timer >= TIME_BEFORE_THAW) {
					this.gameObject.GetComponent<Animator> ().enabled = true;
					m_IsFrozen = false;
				}//end if
			}//end if
			//else if the enemy is shocked...
			else {
				//...then increment the shock timer and release the enemy after the alloted time.
				this.m_ShockTimer += Time.deltaTime;
				//if the shock timer exceeds or is equal to the time of shock release (where the enemy is no longer being shocked)...
				if (this.m_ShockTimer >= TIME_BEFORE_SHOCK_RELEASE) {
					//...then release the enemy
					this.gameObject.GetComponent<Animator> ().enabled = true;
					this.m_IsShocked = false;
					//...ensure the enemy is at the proper height
					Vector3 position = this.transform.position;
					position.y = this.m_EnemyStartHeight;
					this.transform.position = position;
				}//end if
				//else if the shock timer is less than the time of shock release (where the enemy is still being shocked)...
				else {
					//...then move the enemy either up or down, with respect to the shock timer incrementor

					//if the shock timer is greater-than or equal to the shock incrementor * shock jump frequency...
					if (this.m_ShockTimer >= this.m_ShockTimerIncrementor * this.m_ShockJumpFrequency) {
						//...then move the enemy either up or down

						Vector3 position = this.transform.position;
						//...where if the shock timer incrementor is odd, we move the enemy up
						//...and if the shock timer incrementor is even, we move the enemy back down
						position.y += (this.m_ShockTimerIncrementor % 2 == 1) ? this.m_ShockJumpDistance : -(this.m_ShockJumpDistance);
						this.transform.position = position;

						this.m_ShockTimerIncrementor++;
					}//end if
				}//end else
			}//end else
		}//end if
		else {
			this.m_CanMove = true;
		}
	}//end f'n void Update()


    /**A function to add [effect] to the enemys's health.*/
    public void AffectHealth(float effect)
    {
        this.m_Health += effect;
    }//end f'n void AffectHealth(float)


	/**Function which applies the effect of a spell on the enemy.
     *Should make it abstract when we add a variety of enemies.*/
	public void ApplySpellEffects(SpellName spell_name)
	{
		switch ((int)spell_name) {
		case (int)SpellName.Fireball:
			{
				this.AffectHealth (-10.0f);
				break;
			}//end case Fireball
        case (int)SpellName.Iceball:
            {
                this.AffectHealth(-2.0f);
                this.m_IsFrozen = true;
                freeze_Timer = 0.0f;
                break;
            }//end case Iceball
		case (int)SpellName.Thunderball:
			{
				this.AffectHealth (-5.0f);
				this.m_IsShocked = true;
				this.m_ShockTimer = 0.0f;

				break;
			}//end case Thunderball
		default:
			{
				//Impossible, right now
				break;
			}
		}


	}


}
