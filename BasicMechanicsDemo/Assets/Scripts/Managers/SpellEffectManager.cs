using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectManager : MonoBehaviour {

	/*
	* At present, we have the following:
	* - Spells whose effects are instantaneous; they do damage, then that's the end of them.
	* - Spells whose effects are lasting but finite; a spell that freezes an enemy in place, or causes them to do something
	* 	after the damage has been applied.
	* - Spells that can be cast continuously until the player's mana runs out; in this case the spell will have a basic initial effect
	* 	and then the effect will be repeated after in iterations of the duration of the spell's initial duration, for as long as 
	* 	the user holds down the spell-casting button
	* 
	* 	So, the variables to help us differentiate these three spell types:
	* 		Case 1: if a spell's effects are instantaneous, its duration is 0.
	* 		Case 2: if a spell's effects are lasting but won't last forever, its duration is greater than 0 but is not continuous.
	* 		Case 3: if a spell's effects can be cast until the player's mana runs out, the spell has a basic duration greater than 0
	* 			but is continuous.
	*/

	/**The duration of one iteration of a spell; if the spell is instantaneous (like fireball), this is 0.*/
	private float m_SpellDuration = 0.0f;
	/**A variable to keep track of spell iterations, for Case 3*/
	private int m_Iterations = 0;
	/**A boolean to tell us whether or not a spell is continuous.*/
	private bool m_IsContinuous = false;
	/**The spell to apply to the enemy; to be set from an outside class.*/
	public SpellClass m_SpellToApply;
	/**The enemy on whom the spell must be applied; to be set from an outside class.*/
	public Enemy m_Enemy;

	//List of all spell damage, if any
	/**Fireball damage*/
	public readonly float FIREBALL_DAMAGE = -10.0f;
	/**Iceball damage*/
	public readonly float ICEBALL_DAMAGE = -2.0f;
	/**Thunderball damage*/
	public readonly float THUNDERBALL_DAMAGE = -5.0f;
	/**Thunderstorm damage per second.*/
	public readonly float THUNDERSTORM_DAMAGE = -15.0f;

	//List of all spell durations, if any
	/**The time until the enemy thaws and can move again.*/
	public float TIME_BEFORE_THAW = 2.5f;
	/**The time until the enemy is no longer being shocked; at which point it can move again and stop hopping.*/
	public float TIME_BEFORE_SHOCK_RELEASE = 1.25f;

	private float m_SpellTimer = 0.0f;

	// Update is called once per frame
	void Update () {
		//if the spell to apply exists, and so does the enemy...
		if (this.m_SpellToApply != null && this.m_Enemy != null) {

			//First check for Case 1
			//if the spell's effects are instantaneous
			if (this.m_SpellDuration == 0.0f) {
				//...then immediately apply the effects
				this.m_Enemy.ApplySpellEffects (this.m_SpellToApply.m_SpellName);
				//and that's the end of it, so reset our parameters to break from Update loop
				this.ResetParameters ();
			}//end if

			//Then check for Case 2
			//if the spell's effects are not instantaneous (meaning the duration is greater than 0), but not continuous
			else if (this.m_SpellDuration > 0 && !this.m_IsContinuous) {
				//then apply the spell effect and maintain it for [duration]

				//if the spell timer is less than the spell's duration
				if (this.m_SpellTimer < this.m_SpellDuration) {
					//Apply damage and effect
					this.ApplySpellDamage ();
					this.m_Enemy.ApplySpellEffects (this.m_SpellToApply.m_SpellName);
					//Increment timer
					this.m_SpellTimer += Time.deltaTime;
				} //end if
				//once the timer is equal-to or surpasses the duration...
				else {
					//Reset our parameters to break from Update loop
					this.ResetParameters ();
				}//end else
			}//end else if

			//Then check for Case 3
			//if the spell's effects are not instantaneous (meaning the duration is greater than 0), and continuous
			else if (this.m_SpellDuration > 0 && this.m_IsContinuous) {
				//then apply the spell effect, maintain it for [duration], and after [duration] repeat

				//if this is the very first iteration...
				if (this.m_SpellTimer == 0) {
					//apply damage and spell effect
					this.ApplySpellDamage ();
					this.m_Enemy.ApplySpellEffects (this.m_SpellToApply.m_SpellName);
				}
				//if the spell timer is less than one more than the current iteration multiplied by the spell duration
				else if (this.m_SpellTimer < (this.m_Iterations + 1) * this.m_SpellDuration) {
					//then apply spell effect
					this.m_Enemy.ApplySpellEffects (this.m_SpellToApply.m_SpellName);
					//and increment the timer
					this.m_SpellTimer += Time.deltaTime;
				}//end if
				//else if the spell timer is greater-than or equal to one more than the current iteration multiplied by the spell duration
				else {
					//Increment iterations
					this.m_Iterations++;
					//Apply damage
					this.ApplySpellDamage ();
					//And carry on
				}//end else

				/*
				* Note: There's no real end case in the case of a continuous spell because there's no real way to plan for it from here.
				* In theory, this will stop executing when the user stops sending us input. We'll need to do that from wherever we're getting
				* the user input.
				*/

			}//end else if
			//else if the spell is instantaneous and continuous, meaning the damage is just an endless stream
			else {
				//then just keep applying the damage and effects
				this.ApplySpellDamage ();
				this.m_Enemy.ApplySpellEffects (this.m_SpellToApply.m_SpellName);
			}//end else
		}//end if
	}//end f'n void Update()

	/**A function to reset parameters after each time we pass through this class.*/
	public void ResetParameters()
	{
		this.m_SpellToApply = null;
		this.m_Enemy = null;
		this.m_SpellTimer = 0.0f;
		this.m_Iterations = 0;
	}

	/**A function to be called by whoever intends to use this spell manager to damage an [enemy].
	*We set the spell to apply and initialize its variables (duration and [isContinuous]) with respect to the spell name.*/
	public void SetSpellToApply(SpellClass spell_to_apply, Enemy enemy)
	{
		this.m_SpellToApply = spell_to_apply;
		this.m_Enemy = enemy;

		switch ((int)this.m_SpellToApply.m_SpellName) {
		case (int)SpellName.Fireball:
			{
				//Fireball is instantaneous, and is not continuous
				this.m_SpellDuration = 0.0f;
				this.m_IsContinuous = false;

				break;
			}//end case fireball
		case (int)SpellName.Iceball:
			{
				//Iceball has a lasting effect on the enemy, but is not continuous
				this.m_SpellDuration = TIME_BEFORE_THAW;
				this.m_IsContinuous = false;

				break;
			}//end case iceball
		case (int)SpellName.Thunderball:
			{
				//Thunderball has a lasting effect on the enemy, but is not continuous
				this.m_SpellDuration = TIME_BEFORE_SHOCK_RELEASE;
				this.m_IsContinuous = false;

				break;
			}//end case Thunderball
		case (int)SpellName.Thunderstorm:
			{
				//Thunderstorm has a lasting effect on the enemy, and is continuous
				this.m_SpellDuration = TIME_BEFORE_SHOCK_RELEASE;
				this.m_IsContinuous = true;

				break;
			}//end case Thunderstorm
		default:
			{
				//impossible
				break;
			}//end case default
		}//end switch
	}//end f'n void SetSpellToApply(SpellClass, Enemy)

	/**A function to apply damage to the enemy.*/
	private void ApplySpellDamage()
	{
		switch ((int)this.m_SpellToApply.m_SpellName) {
		case (int)SpellName.Fireball:
			{
				//Fireball's damage is applied instantaneously
				this.m_Enemy.AffectHealth (FIREBALL_DAMAGE);

				break;
			}//end case fireball
		case (int)SpellName.Iceball:
			{
				//Iceball's damage is applied instantaneously and then the target is frozen for a bit
				this.m_Enemy.AffectHealth (ICEBALL_DAMAGE);

				break;
			}//end case iceball
		case (int)SpellName.Thunderball:
			{
				//Thunderball's damage is applied instantaneously and then the target is shocked for a bit
				this.m_Enemy.AffectHealth (THUNDERBALL_DAMAGE);

				break;
			}//end case Thunderball
		case (int)SpellName.Thunderstorm:
			{
				//Thunderstorm's damage is consistently applied in the following manner:
				//- The target is damaged
				//- The target does its little [isShocked] dance
				//Rinse and repeat
				this.m_Enemy.AffectHealth (THUNDERSTORM_DAMAGE);

				break;
			}//end case Thunderstorm
		default:
			{
				//impossible
				break;
			}//end case default
		}
	}
}
