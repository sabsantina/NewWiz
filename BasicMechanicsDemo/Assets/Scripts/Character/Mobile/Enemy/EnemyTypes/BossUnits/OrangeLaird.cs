using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrangeLaird : BossEnemy {
    [SerializeField] GameObject enemyHPMeter;

    public float m_OrangeLairdHealth = 250.0f;

    public float m_OrangeLairdMeleeDamage = 20.0f;

    public float m_OrangeLairdChasePlayerDuration = 20.0f;

    public float m_IntervalBetweenRangedAttacks = 0.5f;

    public float m_IntervalBetweenMeleeAttacks = 1.5f;

    private bool isHealing = false;

    private bool isOverheated = false;

    private int m_TimesHealed = 0;

    private float m_Healing_Timer = 0.0f;

    private float m_OverHeat_Timer = 0.0f;

    /**The string value of the name of the sorting layer*/
    public string sortingLayerName;


    // Use this for initialization
    void Start () {
        base.Start();

        this.SetIntervalsBetweenAttacks();
        this.SetHealth(this.m_OrangeLairdHealth);
        this.SetMeleeDamage();
        this.SetSpellToCast(this.m_AttackSpell);
        this.SetAttackDamageValue();
        this.setMaxMeter();

        this.SetChasePlayerSettings(this.m_OrangeLairdChasePlayerDuration);
        this.m_EnemyName = EnemyName.ORANGE_LAIRD;

        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingLayerName = sortingLayerName;
    }
	
	// Update is called once per frame
	void Update () {
        base.Update();

        this.Die();
        
        //Handles the two stages of healing of the boss.
        HandleHealing();

        //Handles the overheat mechanic of the boss, for the final stage
        HandleOverheat();

        //This line of code is used to get the correct draw order.
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(this.GetComponent<Transform>().transform.position.z * 100f) * -1;
    }

    public override void SetAttackDamageValue()
    {
        if (this.m_AttackPattern.m_AttackPatternState == AttackPatternState.RANGED)
        {
            this.m_AttackDamageValue = this.m_SpellToCast.m_SpellDamage + 15.0f;
        }
        else if (this.m_AttackPattern.m_AttackPatternState == AttackPatternState.MELEE)
        {
            this.m_AttackDamageValue = this.m_OrangeLairdMeleeDamage;
        }
    }

    protected override void SetMeleeDamage()
    {
        this.m_MeleeDamage = this.m_OrangeLairdMeleeDamage;
    }

    /**A function to set the spell to cast in our parent classes*/
    public override void SetSpellToCast(SpellName spell)
    {
        SpellClass spell_instance = new SpellClass();
        this.m_SpellToCast = spell_instance.GenerateInstance(spell);
        this.m_AttackPattern.m_SpellToCast = this.m_SpellToCast;
    }

    protected override void SetGeneratedSpellInstance()
    {
        this.m_GeneratedSpellCubeInstance = this.m_AttackPattern.m_GeneratedSpellInstance;
    }

    protected override void SetIntervalsBetweenAttacks()
    {
        this.m_MeleeAttackInterval = this.m_IntervalBetweenMeleeAttacks;
        this.m_RangedAttackInterval = this.m_IntervalBetweenRangedAttacks;
    }

    public override void ApplySpellEffect(SpellClass spell)
    {
        base.ApplySpellEffect(spell);
        enemyHPMeter.GetComponentInChildren<Slider>().value = m_Health;
    }

    /**This function heals the boss when it falls under a certain health amount. The boss heals a total of two times and then goes into a frenzy ^^*/
    public void HandleHealing()
    {
        if(this.m_IsHealing == true)
        {
            m_Healing_Timer += Time.deltaTime;
            enemyHPMeter.GetComponentInChildren<Slider>().value = m_Health;
        }
        if (this.m_Health > m_OrangeLairdHealth || m_Healing_Timer > 3.0f)
        {
            m_Healing_Timer = 0.0f;
            SetSpellToCast(this.m_AttackSpell);
            this.m_Health = m_OrangeLairdHealth;
            this.m_IsHealing = false;
        }
        else if (this.m_Health <= 100 && m_TimesHealed == 0)
        {
            this.m_SpellCastCount = 0;
            SetSpellToCast(SpellName.Heal);
            this.m_IsHealing = true;
            m_TimesHealed++;
            m_IntervalBetweenRangedAttacks -= 0.2f;
            SetIntervalsBetweenAttacks();
        }
        else if(this.m_Health <= 50 && m_TimesHealed == 1)
        {
            this.m_SpellCastCount = 0;
            SetSpellToCast(SpellName.Heal);
            this.m_IsHealing = true;
            m_TimesHealed++;
            m_IntervalBetweenRangedAttacks -= 0.225f;
            SetIntervalsBetweenAttacks();
        }
    }

    /**This function checks if a spell has been cast a number of times and overheats the boss if it has.*/
    public void HandleOverheat()
    {
        if (m_TimesHealed >= 2 && this.m_SpellCastCount > 40)
        {
            this.m_SpellCastCount = 0;
            this.isOverheated = true;
            this.m_RangedAttackInterval = 2.0f;
        }

        if (this.isOverheated)
        {
            this.m_OverHeat_Timer += Time.deltaTime;
        }

        if (this.m_OverHeat_Timer > 2.0f)
        {
            this.m_OverHeat_Timer = 0.0f;
            this.isOverheated = false;
            SetIntervalsBetweenAttacks();
        }
    }

    void setMaxMeter()
    {
        enemyHPMeter.GetComponentInChildren<Slider>().maxValue = m_OrangeLairdHealth;
        enemyHPMeter.GetComponentInChildren<Slider>().value = m_OrangeLairdHealth;
    }

	/**The spell to spawn on boss death*/
	protected override void ManageLootSpawnOnDeath()
	{
		this.m_Spawner.Spawn_Spell (SpellName.WaterBubble, this.transform.position);
	}
}
