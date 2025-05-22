using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Aromr,
    Amulet,//护符
    Flask//药剂
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Date/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float ItemCoolDown;
    public ItemEffect[] itemEffects;


    [Header("Major stats")]
    public int strength;//1点数值增加1点伤害，暴击伤害增加1%
    public int agility;//1点数值增加1%闪避和1%暴击率
    public int intelligence;//1点数值增加1点魔法伤害和3点法抗
    public int vitality;//1点数值增加3健康度


    [Header("Offensive stats")]
    public int damage;
    public int critChance;
    public int critPower;    //默认值为150%


    [Header("Defensive stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;


    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int DescriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);

    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length =0;
        DescriptionLength = 0;

        AddItemDescription(strength, "Srength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit.Chance");
        AddItemDescription(critPower, "Crit.Power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resist");

        AddItemDescription(fireDamage, "Fire damage");
        AddItemDescription(iceDamage, "Ice damage");
        AddItemDescription(lightingDamage, "Lighting damage");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: " + itemEffects[i].effectDescription);
                DescriptionLength++;
            }
        }

        if (DescriptionLength < 5)
        {
            for(int i = 0; i < 5 - DescriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value,string _name)
    {
        if(_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if(_value > 0)
                sb.Append("+ " + _value + " " + _name);

            DescriptionLength++;
        }
    }
}
