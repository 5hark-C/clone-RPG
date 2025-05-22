using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy_Skeleton enemy;
    private ItemDrop myDrop;
    public Stats soulsDropAmount;

    [Header("怪物等级")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;

    protected override void Start()
    {
        soulsDropAmount.SetDefaultValue(100);
        ApplyLevelModfiers();

        base.Start();

        enemy = GetComponent<Enemy_Skeleton>();
        myDrop = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModfiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        //Modify(evasion);闪避
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }

    //敌人数值增幅
    private void Modify(Stats _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        myDrop.GenerateDrop();
    }
}
