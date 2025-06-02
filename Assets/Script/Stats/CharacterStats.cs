using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage,
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stats strength;//1点数值增加1点伤害，暴击伤害增加1%
    public Stats agility;//1点数值增加1%闪避和1%暴击率
    public Stats intelligence;//1点数值增加1点魔法伤害和3点法抗
    public Stats vitality;//1点数值增加5健康度


    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critPower;    //默认值为150%
    

    [Header("Defensive stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;


    [Header("Magic stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;



    public bool isIgnited;   //点燃
    public bool isChilled;   //减20%防
    public bool isShocked;   //减20%命中

    [SerializeField] private float ailmentsDuration = 4;//异常状态持续时间

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;
    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead {  get; private set; }
    private bool isVulnerable;

    public bool isInvincible {  get; private set; }


    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
    }

   protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
            ApplyIgniteDamage();
    }

    //buff逻辑

    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCorutine(_duration));
    }

    private IEnumerator VulnerableForCorutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier,float _duration,Stats _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier,float _duration,Stats _statToModify)
    {
        _statToModify.AddModifier(_modifier);
         yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }

    //出伤逻辑
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (_targetStats.isInvincible)
            return;
       
        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        fx.CreateHitFx(_targetStats.transform,criticalStrike);    

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);
    }

    #region 法伤及异常状态区

    //法伤计算逻辑
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();


        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);



        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }

    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyIce = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;



        while (!canApplyIgnite && !canApplyIce && !canApplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyIce, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyIce = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyIce, canApplyShock);
                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyIce, canApplyShock);
                return;
            }
        }


        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .5f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyIce, canApplyShock);
    }

  
    //异常状态实施逻辑
    public void ApplyAilments(bool _ignite,bool _chill,bool _shocked)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }

        if(_chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled =_chill;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shocked && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shocked);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();

            }

        }
               
    }

    public void ApplyShock(bool _shocked)
    {
        if (isShocked)
            return;

        shockedTimer = ailmentsDuration;
        isShocked = _shocked;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        //找到最近的敌人，实例闪电并布置好
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToenemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToenemy < closestDistance)
                {
                    closestDistance = distanceToenemy;
                    closestEnemy = hit.transform;
                }
            }
            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());

        }
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion


    #region 数值计算区（生命，伤害，护甲等）
    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if(currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.3f);

        currentHealth -= _damage;

        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString());

        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
    }


    public virtual void OnEvasion()
    {

    }
    
    //闪避逻辑
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int toatlEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            toatlEvasion += 20;


        if (Random.Range(0, 100) < toatlEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }


    //护甲逻辑
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }


    //暴击逻辑
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }



    //暴击计算逻辑
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }


    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #endregion

    public Stats GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelligence) return intelligence;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.maxHealth) return maxHealth;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.magicResistance) return magicResistance;
        else if (_statType == StatType.fireDamage) return fireDamage;
        else if (_statType == StatType.iceDamage) return iceDamage;
        else if (_statType == StatType.lightingDamage) return lightingDamage;

        return null;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvinciable(bool _invinciable)
    {
        isInvincible = _invinciable;
    }
}
