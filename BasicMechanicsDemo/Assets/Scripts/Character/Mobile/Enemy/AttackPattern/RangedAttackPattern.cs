using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackPattern : AttackPattern {

	/**The sound the enemy makes on ranged attack*/
	[SerializeField] public AudioClip m_EnemyMeleeAttackSound;
	[SerializeField] public AudioClip m_EnemySpellSound;

	public AudioSource m_AudioSource;

	/**A reference to the default spell prefab*/
	[SerializeField] GameObject m_DefaultSpellPrefab;
	/**A reference to the spell instance we generate from the default spell prefab.*/
	public GameObject m_GeneratedSpellInstance;
	/**To be set from the children classes*/
	public SpellClass m_SpellToCast;
	[SerializeField] SpellAnimatorManager m_SpellAnimatorManager;

	/**A vector to help us know what the offset is for a given AOE animation, with respect to the player click position.*/
	public Vector3 AOE_offset = new Vector3(0.0f, 2.8f, 4.20f);
	/**A variable to keep track of the AOE radius.*/
	public float AOE_Radius = 15.0f;
	/**A variable to ensure AOE attacks don't apply themselves to the enemy for every frame.*/
	public float AOE_Timer = 0.0f;

	public float m_EffectManagerTimer = 0.0f;

	void Start()
	{
		this.m_AudioSource = this.GetComponent<AudioSource> ();
	}

	protected override void ExecutePatternState ()
	{
		switch((int)this.m_AttackPatternState)
		{
		case (int)AttackPatternState.RANGED:
			{
//				Debug.Log ("Am I running?");

				
				if (this.m_AttackTimer == 0.0f) {
					this.m_IsAttacking = true;
					this.GenerateSpellPrefabInstance ();
					Player player_detected = this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player;
					this.m_EnemySpellSound = player_detected.m_PlayerAudio.getAudioForSpell (this.m_SpellToCast.m_SpellName);
					this.m_AudioSource.PlayOneShot(this.m_EnemySpellSound);
				}
				switch ((int)this.m_SpellToCast.m_SpellType) {
				case (int)SpellType.BASIC_PROJECTILE_ON_TARGET:
					{
						
						if (this.m_AttackTimer == 0.0f) {
							this.m_GeneratedSpellInstance.transform.position = this.transform.position;
							SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
							//if the player reference exists...
							if (this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player != null) {
								//...then continue with the spell casting
								spell_movement.SetEnemyTarget (this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player.gameObject);
								spell_movement.SetSpellToCast (this.m_SpellToCast);


							}
							//Worst-case, destroy the generated spell instance after given time
							StartCoroutine (this.DestroySpellAfterTime (2.0f));

							this.m_AttackTimer += Time.deltaTime;
						} else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
							this.m_AttackTimer += Time.deltaTime;
						} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
							this.m_AttackTimer = 0.0f;
						}

						break;
					}
				case (int)SpellType.AOE_ON_TARGET:
					{
						Player player = this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player;
						if (this.m_AttackTimer == 0.0f) {
							if (player != null) {
								//Spawn AOE halfway to player
								Vector3 halfway_to_player = player.gameObject.transform.position - this.transform.position;
								halfway_to_player.x /= 2.0f;
								halfway_to_player.z /= 2.0f;
//								this.m_GeneratedSpellInstance.transform.position = this.transform.position + halfway_to_player;
								this.m_GeneratedSpellInstance.transform.position = this.transform.position;
							}
							SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
							spell_movement.SetSpellToCast (this.m_SpellToCast);
							//Update animator
							this.m_SpellAnimatorManager.SetSpellAnimator (this.m_GeneratedSpellInstance);
							//Update timer
							this.m_AttackTimer += Time.deltaTime;
						} 
						//if the timer's greater than 0, then the player's still in the region and the spell instance is created.
						//So all we need to do is make the spell follow the player
						else if (this.m_AttackTimer > 0.0f) {

							//...then ensure the spell's always at our chosen position
							SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
							Vector3 displacement = Vector3.Normalize (player.gameObject.transform.position - this.m_GeneratedSpellInstance.transform.position) * spell_movement.m_MaximalVelocity * Time.deltaTime * (Time.deltaTime * 4.0f);
							Vector3 spell_current_position = this.m_GeneratedSpellInstance.transform.position;
							this.m_GeneratedSpellInstance.transform.position = spell_current_position + displacement;

							this.ApplyAOEToAllInRange(spell_current_position);

							this.m_AttackTimer += Time.deltaTime;
						}

						//***********

						break;
					}//end case AOE_ON_TARGET
				}//end switch spelltype

				break;
					
			}//end case RANGED
		case (int)AttackPatternState.DO_NOTHING:
			{
				//Do nothing


				//In case the player is slipping in and out of the enemy's detection region, make the timer run its course anyway
				if (0.0f < this.m_AttackTimer && !(this.m_AttackTimer >= this.m_IntervalBetweenAttacks)) {
					this.m_AttackTimer += Time.deltaTime;
				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
					this.m_AttackTimer = 0.0f;

					//Ensure generated spell instance is nullified
					if (this.m_GeneratedSpellInstance != null) {
						GameObject.Destroy (this.m_GeneratedSpellInstance);
					}
				}
				break;
			}
		}//end switch

		this.m_IsAttacking = false;
	}

	private float FindFloorDistance(Vector3 position)
	{
		float furthest = 0.0f;
		foreach (RaycastHit hit in Physics.RaycastAll(position, Vector3.down, 10.0f)) {
			if (hit.distance > furthest) {
				furthest = hit.distance;
			}
		}
		return furthest;
	}

	/**A function to apply AOE spells effects to all nearby enemies in a given radius.*/
	private void ApplyAOEToAllInRange(Vector3 position)
	{
		//Kind of a cool effect?
		//		GameObject test = GameObject.Instantiate (this.m_SpellCube);
		//		test.transform.position = position;
		//		GameObject.Destroy (test, 2.0f);

		//Get all nearby collisions in a sphere at the specified position, in a radius [this.AOE_Radius]
		Collider[] all_hit = Physics.OverlapSphere (position, 4.0f);
		foreach (Collider hit in all_hit) {

			bool caster_hit = (hit.gameObject == this.gameObject);

			//if we hit a boxcollider
			//AND the hit collider belongs to a thing that can be damaged by magic
			//AND whatever was hit was not the enemy who cast the spell
			if (hit.gameObject.GetComponent<ICanBeDamagedByMagic> () != null
				&& !caster_hit) {
				Debug.Log ("Applying spell damage to " + hit.gameObject.name);
				ICanBeDamagedByMagic target_hit = hit.gameObject.GetComponent<ICanBeDamagedByMagic> ();
				//if the target hit is already affected by magic...
				if (target_hit.IsAffectedByMagic()) {
					//...and if the spell affecting the target is NOT the same as the one we're casting now...
					if (target_hit.SpellAffectingCharacter () != this.m_SpellToCast) {
						//...then apply spell effects
						target_hit.ApplySpellEffect (this.m_SpellToCast);
					} 
					//else if it's the same spell, only apply half damage
					else {
						Player player_component = hit.gameObject.GetComponent<Player> ();
						DefaultEnemy enemy_component = hit.gameObject.GetComponent<DefaultEnemy> ();
						if (player_component != null) {
							player_component.AffectHealth (this.m_SpellToCast.m_SpellDamage / 2.0f);
						} else if (enemy_component != null) {
							enemy_component.AffectHealth (this.m_SpellToCast.m_SpellDamage / 2.0f);
						}
					}

				} 
				//else if the target is not already affected by magic
				else {
					//just apply spell effects
					hit.gameObject.GetComponent<ICanBeDamagedByMagic> ().ApplySpellEffect (this.m_SpellToCast);
				}
			}
		}//end foreach
	}//end f'n void ApplyAOEToEnemies(Vector3)

	private IEnumerator DestroySpellAfterTime(float time)
	{
		yield return new WaitForSeconds (time);
		if (this.m_GeneratedSpellInstance != null) {
			this.DestroySpellPrefabInstance ();
		}
	}

	private void GenerateSpellPrefabInstance()
	{
		this.m_GeneratedSpellInstance = GameObject.Instantiate (this.m_DefaultSpellPrefab);
		this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ().m_SpellClassToCast = this.m_SpellToCast;
		this.m_SpellAnimatorManager.SetSpellAnimator (this.m_GeneratedSpellInstance);
	}

	public void DestroySpellPrefabInstance()
	{
		if (this.m_GeneratedSpellInstance != null) {
			GameObject.Destroy (this.m_GeneratedSpellInstance);
			this.m_GeneratedSpellInstance = null;
		}
	}
}
