using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredSoldier : EnemyInfantry {
    /**The health of an armored soldier*/
    public float m_ArmoredSoldierHealth = 60.0f;
    /**The attack damage of an armored soldier*/
    public float m_ArmoredSoldierAttackDamage = 15.0f;
    /**The string value of the name of the sorting layer*/
    public string sortingLayerName;

    // Use this for initialization
    void Start () {
        this.SetHealth();
        this.SetAttackDamageValue();
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = sortingLayerName;
    }
	
	// Update is called once per frame
	void Update () {
        //This line of code is used to get the correct draw order.
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(this.GetComponent<Transform>().transform.position.z * 100f) * -1;
        this.Move();
        if (this.IsPlayerInRangeOfAttack())
        {
            this.Attack();
        }
        else
        {
            this.m_AttackPattern.m_AttackPatternState = AttackPatternState.DO_NOTHING;
        }
    }
    /**A function to set the enemy health in our parent classes*/
    public override void SetHealth()
    {
        this.m_Health = this.m_ArmoredSoldierHealth;
    }

    public override void SetAttackDamageValue()
    {
        this.m_AttackDamageValue = this.m_ArmoredSoldierAttackDamage;
    }

    public override float GetAttackDamageValue()
    {
        return this.m_AttackDamageValue;
    }
}
