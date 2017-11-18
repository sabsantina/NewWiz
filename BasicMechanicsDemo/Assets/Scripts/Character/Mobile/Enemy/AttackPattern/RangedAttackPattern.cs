using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackPattern : AttackPattern {

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

//	void Update()
//	{
//		//Why isn't this running normally from its parent, like MeleeAttackPattern?
//		this.ExecutePatternState ();


//		if (this.m_SpellToCast.m_IsPersistent) {
//			//if the spell cube instance is null (which can only mean the last spell cube was destroyed)...
//			if (this.m_SpellCubeInstance == null) {
//				//...then create a new spell cube
//				this.m_SpellCubeInstance = GameObject.Instantiate (this.m_SpellCube);
//				this.m_Player.m_audioSource.PlayOneShot (m_playerAudio.getAudioForSpell (this.m_SpellClassToFire.m_SpellName));
//				this.m_SpellCubeInstance.transform.position = this.transform.position;
//				SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
//				spell_movement.SetSpellToCast (this.m_SpellClassToFire);
//				this.m_SpellAnimatorManager.SetSpellAnimator (this.m_SpellCubeInstance);
//			}//end if
//			//if the spell cube instance exists (meaning we're in the process of casting a spell)...
//			if (this.m_SpellCubeInstance != null) {
//				//...then ensure the spell's always at our chosen position
//				SpellMovement spell_movement = this.m_SpellCubeInstance.GetComponent<SpellMovement> ();
//				Ray ray = this.m_MainCamera.ScreenPointToRay (Input.mousePosition);
//				RaycastHit[] targets_hit = Physics.RaycastAll (ray);
//
//				RaycastHit target = targets_hit [0];
//				bool any_mobile_characters = false;
//				foreach (RaycastHit hit in targets_hit) {
//					//if the hit's distance is greater than that of the furthest...
//					if (hit.collider.gameObject == this.m_Floor) {
//						target = hit;
//						break;
//					}
//				}//end foreach
//				//Set position of AOE spell animation
//				Vector3 modified_target = new Vector3 (target.point.x, 0.0f, target.point.z);
//				Vector3 position = modified_target + AOE_offset;
//				spell_movement.MaintainPosition (position);
//
//				//If we're just starting to cast the spell...
//				if (this.AOE_Timer == 0.0f) {
//					//... then apply the AOE to the enemies
//					this.ApplyAOEToEnemies (position - AOE_offset);
//					//Then set AOE timer to 1, to avoid repeating the base case
//					this.AOE_Timer = 1.0f;
//				}//end if
//				//else if the AOE timer is greater than or equal to 1 + the time it takes for the specific AOE spell to wear off...
//				else if (this.AOE_Timer >= 1.0f + this.m_SpellClassToFire.m_EffectDuration) {
//					//...then apply damage to the enemies and reset AOE timer to 1.0f
//					this.ApplyAOEToEnemies (position - AOE_offset);
//					this.AOE_Timer = 1.0f;
//				}//end else if
//				//else increment AOE timer by time.delta time
//				else {
//					this.AOE_Timer += Time.deltaTime;
//				}//end else
//			}//end if the spell cube instance exists
//		}


//	}

	protected override void ExecutePatternState ()
	{
		switch((int)this.m_AttackPatternState)
		{
		case (int)AttackPatternState.RANGED:
			{
				Debug.Log ("Am I running?");

				
				if (this.m_AttackTimer == 0.0f) {
					this.m_IsAttacking = true;
					this.GenerateSpellPrefabInstance ();
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
						}
						else if (0.0f < this.m_AttackTimer && this.m_AttackTimer < this.m_IntervalBetweenAttacks) {
							this.m_AttackTimer += Time.deltaTime;
						} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
							this.m_AttackTimer = 0.0f;
						}

						break;
					}
				case (int)SpellType.AOE_ON_TARGET:
					{
						Player player = this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player;
						if (this.m_SpellToCast.m_IsPersistent) {
							if (this.m_GeneratedSpellInstance == null) {
								//...then create a new spell cube
								this.m_GeneratedSpellInstance = GameObject.Instantiate (this.m_DefaultSpellPrefab);
								//							this.m_Player.m_audioSource.PlayOneShot (m_playerAudio.getAudioForSpell (this.m_SpellClassToFire.m_SpellName));
								if (player != null) {
									//Spawn AOE halfway to player
									Vector3 halfway_to_player = player.gameObject.transform.position - this.transform.position;
									halfway_to_player.x /= 2.0f;
									halfway_to_player.z /= 2.0f;
									this.m_GeneratedSpellInstance.transform.position = this.transform.position + halfway_to_player;
								}
								SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
								spell_movement.SetSpellToCast (this.m_SpellToCast);
								this.m_SpellAnimatorManager.SetSpellAnimator (this.m_GeneratedSpellInstance);
							}//end if
							//else if the spell cube exists
							else {
								//...then ensure the spell's always at our chosen position
								SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
								Vector3 displacement = Vector3.Normalize (this.m_GeneratedSpellInstance.transform.position - player.gameObject.transform.position) * spell_movement.m_MaximalVelocity * Time.deltaTime;

								Ray ray = new Ray (this.m_GeneratedSpellInstance.transform.position + displacement, Vector3.down);
//								RaycastHit[] targets_hit = Physics.RaycastAll (ray);
//								//Arbitrary default value
//								RaycastHit furthest = targets_hit [0];
//								foreach (RaycastHit hit in targets_hit) {
//									//if the hit's distance is greater than that of the furthest...
//									if (hit.distance > furthest.distance) {
//										furthest = hit;
//									}
//								}//end foreach
								RaycastHit target;
								if (Physics.Raycast(ray, out target))
								{
									//Set position of AOE spell animation
									Vector3 modified_target = new Vector3 (target.point.x, 0.0f, target.point.z);
									Vector3 position = modified_target + AOE_offset;
									spell_movement.MaintainPosition (position);

									//If we're just starting to cast the spell...
									if (this.AOE_Timer == 0.0f) {
										//... then apply the AOE to the enemies
										this.ApplyAOEToEnemies (position - AOE_offset);
										//Then set AOE timer to 1, to avoid repeating the base case
										this.AOE_Timer = 1.0f;
									}//end if
									//else if the AOE timer is greater than or equal to 1 + the time it takes for the specific AOE spell to wear off...
									else if (this.AOE_Timer >= 1.0f + this.m_SpellToCast.m_EffectDuration) {
										//...then apply damage to the enemies and reset AOE timer to 1.0f
										this.ApplyAOEToEnemies (position - AOE_offset);
										this.AOE_Timer = 1.0f;
									}//end else if
									//else increment AOE timer by time.delta time
									else {
										this.AOE_Timer += Time.deltaTime;
									}//end else
								}
//								//Set position of AOE spell animation
//								Vector3 modified_target = new Vector3 (target.point.x, 0.0f, target.point.z);
//								Vector3 position = modified_target + AOE_offset;
//								spell_movement.MaintainPosition (position);

//								//If we're just starting to cast the spell...
//								if (this.AOE_Timer == 0.0f) {
//									//... then apply the AOE to the enemies
//									this.ApplyAOEToEnemies (position - AOE_offset);
//									//Then set AOE timer to 1, to avoid repeating the base case
//									this.AOE_Timer = 1.0f;
//								}//end if
//								//else if the AOE timer is greater than or equal to 1 + the time it takes for the specific AOE spell to wear off...
//								else if (this.AOE_Timer >= 1.0f + this.m_SpellToCast.m_EffectDuration) {
//									//...then apply damage to the enemies and reset AOE timer to 1.0f
//									this.ApplyAOEToEnemies (position - AOE_offset);
//									this.AOE_Timer = 1.0f;
//								}//end else if
//								//else increment AOE timer by time.delta time
//								else {
//									this.AOE_Timer += Time.deltaTime;
//								}//end else
							}
						}

						break;
					}
				}//end switch




//				if (this.m_AttackTimer == 0.0f) {
//					this.m_IsAttacking = true;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//					this.GenerateSpellPrefabInstance();
//					//					this.m_Player.m_audioSource.PlayOneShot(m_playerAudio.getAudioForSpell(this.m_SpellClassToFire.m_SpellName));
//
//					switch ((int)this.m_SpellToCast.m_SpellType) {
//					case (int)SpellType.BASIC_PROJECTILE_ON_TARGET:
//						{
//							this.m_GeneratedSpellInstance.transform.position = this.transform.position;
//							SpellMovement spell_movement = this.m_GeneratedSpellInstance.GetComponent<SpellMovement> ();
//							//if the player reference exists...
//							if (this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player != null) {
//								//...then continue with the spell casting
//								spell_movement.SetEnemyTarget (this.m_Enemy.m_MovementPattern.m_PatrolRegion.m_Player.gameObject);
//								spell_movement.SetSpellToCast (this.m_SpellToCast);
//
//							}
//							//Worst-case, destroy the generated spell instance after given time
////							StartCoroutine (this.DestroySpellAfterTime (2.0f));
//							break;
//						}
////					case (int)SpellType.AOE_ON_TARGET:
////						{
////							break;
////						}
//					}//end switch
//
//					this.m_AttackTimer += Time.deltaTime;
//					this.m_IsAttacking = false;
//					//					this.m_Animator.SetBool (STRINGKEY_PARAM_ISATTACKING, this.m_IsAttacking);
//				} else if (0.0f < this.m_AttackTimer && !(this.m_AttackTimer >= this.m_IntervalBetweenAttacks)) {
//					this.m_AttackTimer += Time.deltaTime;
//				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
//					this.m_AttackTimer = 0.0f;
//				}
				break;
			}
		case (int)AttackPatternState.DO_NOTHING:
			{
				//Do nothing

				//In case the player is slipping in and out of the enemy's detection region, make the timer run its course anyway
				if (0.0f < this.m_AttackTimer && !(this.m_AttackTimer >= this.m_IntervalBetweenAttacks)) {
					this.m_AttackTimer += Time.deltaTime;
				} else if (this.m_AttackTimer >= this.m_IntervalBetweenAttacks) {
					this.m_AttackTimer = 0.0f;
				}
				break;
			}
		}//end switch
	}

	/**A function to apply AOE spells effects to all nearby enemies in a given radius.*/
	private void ApplyAOEToEnemies(Vector3 position)
	{
		//Kind of a cool effect?
		//		GameObject test = GameObject.Instantiate (this.m_SpellCube);
		//		test.transform.position = position;
		//		GameObject.Destroy (test, 2.0f);

		//Get all nearby collisions in a sphere at the specified position, in a radius [this.AOE_Radius]
		Collider[] all_hit = Physics.OverlapSphere (position, this.AOE_Radius);
		foreach (Collider hit in all_hit) {
			
			if (hit is BoxCollider
				&& hit.gameObject.GetComponent<ICanBeDamagedByMagic> () != null
				&& hit.gameObject.GetComponent<Player>() != null) {
				hit.gameObject.GetComponent<Player>().ApplySpellEffect(this.m_SpellToCast);
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
