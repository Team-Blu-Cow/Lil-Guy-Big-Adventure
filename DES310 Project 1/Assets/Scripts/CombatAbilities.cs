using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAbilities : MonoBehaviour
{
    public Ability[] abilitiesUsing;
    public GameObject enemy;
    public Stats combatantStats;


    public void CastAbility(int abilityNum)
    {
        Debug.Log("Casting Ability...");
        if (abilitiesUsing[abilityNum].abilityType == ability_type.Damage)
        {
            DamageAbility(abilityNum);
        }
        else if (abilitiesUsing[abilityNum].abilityType == ability_type.Heal)
        {
            HealAbility(abilityNum);
        }
        else if (abilitiesUsing[abilityNum].abilityType == ability_type.Buff)
        {
            BuffAbility(abilityNum);
        }
    }

    public void DamageAbility(int abilityNum)
    {
        int damage = 0;

        if (abilitiesUsing[abilityNum].statUsed == stat_used.Strength)
        {
            damage = abilitiesUsing[abilityNum].abilityPower + combatantStats.getStat("Str");

            if (Random.Range(1, 100) < combatantStats.getStat("Luck"))
            {
                damage *= 2;
                enemy.GetComponent<Stats>().setStat("HP", -damage);
            }
            else
            {
                enemy.GetComponent<Stats>().setStat("HP", -damage);
            }

        }
        else if (abilitiesUsing[abilityNum].statUsed == stat_used.Magic)
        {
            damage = abilitiesUsing[abilityNum].abilityPower + combatantStats.getStat("Mag");

            if (Random.Range(1, 100) < combatantStats.getStat("Luck"))
            {
                enemy.GetComponent<Stats>().setStat("HP", -damage * 2);
            }
            else
            {
                enemy.GetComponent<Stats>().setStat("HP", -damage);
            }

        }
        else if (abilitiesUsing[abilityNum].statUsed == stat_used.Dexterity)
        {
            damage = abilitiesUsing[abilityNum].abilityPower + combatantStats.getStat("Dex");

            if (Random.Range(1, 100) < combatantStats.getStat("Luck"))
            {
                enemy.GetComponent<Stats>().setStat("HP", -damage * 2);
            }
            else
            {
                enemy.GetComponent<Stats>().setStat("HP", -damage);
            }

        }
        Debug.Log("Dealt " + damage + " damage");
    }

    public void HealAbility(int abilityNum)
    {
        int heal = abilitiesUsing[abilityNum].abilityPower + combatantStats.getStat("Mag");
        if (abilitiesUsing[abilityNum].statUsed == stat_used.Magic)
        {
            combatantStats.setStat("HP", heal);
            if (combatantStats.getStat("Con") < combatantStats.getStat("HP"))
            {
                combatantStats.setStat("HP", combatantStats.getStat("Con"));
            }
        }
        Debug.Log("Healed " + heal + " points of health");
    }

    public void BuffAbility(int abilityNum)
    {
        switch (abilitiesUsing[abilityNum].statUsed)
        {
            case stat_used.Strength:
                combatantStats.setStat("Str", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat("Str"));
                break;
            case stat_used.Dexterity:
                combatantStats.setStat("Dex", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Dexterity has been buffed to " + combatantStats.getStat("Dex"));
                break;
            case stat_used.Magic:
                combatantStats.setStat("Mag", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Magic has been buffed to " + combatantStats.getStat("Mag"));
                break;
            case stat_used.Defence:
                combatantStats.setStat("Def", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Defence has been buffed to " + combatantStats.getStat("Def"));
                break;
            case stat_used.Constitution:
                combatantStats.setStat("Con", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Constitution has been buffed to " + combatantStats.getStat("Con"));
                break;
            case stat_used.Luck:
                combatantStats.setStat("Luck", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Luck has been buffed to " + combatantStats.getStat("Luck"));
                break;
            case stat_used.Speed:
                combatantStats.setStat("Speed", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Speed has been buffed to " + combatantStats.getStat("Speed"));
                break;
            case stat_used.Initiative:
                combatantStats.setStat("Init", abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Initiative has been buffed to " + combatantStats.getStat("Init"));
                break;

        }
    }

}
