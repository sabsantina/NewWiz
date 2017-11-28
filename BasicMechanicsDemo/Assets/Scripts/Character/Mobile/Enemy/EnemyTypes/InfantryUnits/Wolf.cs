using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : EnemyInfantry {

    /**The damage a Wolf enemy does on attack.*/
    public float m_WolfAttackDamage = 10.0f;
    /**The amount of health a Wolf enemy has.*/
    public float m_WolfHealth = 40.0f;
    /**Length of time for which the wolf will chase the player if the player attacks the wolf without being detected first*/
    public float m_WolfChasePlayerDuration = 3.0f;
    /**The string value of the name of the sorting layer*/
    public string sortingLayerName;

    // Use this for initialization
    void Start()
    {
        base.Start();
        this.SetAttackDamageValue(this.m_WolfAttackDamage);
        this.SetHealth(this.m_WolfHealth);

        this.SetChasePlayerSettings(this.m_WolfChasePlayerDuration);
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = sortingLayerName;

		this.m_EnemyName = EnemyName.WOLF;
    }


    // Update is called once per frame
    protected override void Update()
    {
        //Take care of movement and attack patterns
        base.Update();
        //Manage death
        this.Die();
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(this.GetComponent<Transform>().transform.position.z * 100f) * -1;


        #if TESTING_MELEE_PARAM
		Debug.Log("Wolf attacking Melee? " + this.m_Animator.GetBool("isAttacking_Melee"));
        #endif
        #if TESTING_RANGED_PARAM
		Debug.Log("Wolf attacking Ranged? " + this.m_Animator.GetBool("isAttacking_Ranged"));
        #endif
    }

    /**A function to apply a given spell's effects on the enemy, including damage.*/
    public override void ApplySpellEffect(SpellClass spell)
    {
        base.ApplySpellEffect(spell);
    }


    public override void SetAttackDamageValue()
    {
        this.m_AttackDamageValue = this.m_WolfAttackDamage;
    }

    public override float GetAttackDamageValue()
    {
        return this.m_AttackDamageValue;
    }

	protected override void ManageLootSpawnOnDeath()
	{
		this.m_Spawner.Spawn_Item (ItemName.Health_Potion, this.transform.position);
	}
}
