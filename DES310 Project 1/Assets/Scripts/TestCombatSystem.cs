using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestCombatSystem : MonoBehaviour
{

    public GameObject enemy;

    private Combatant combatant;
    private Stats combatantStats;

    private Ability[] abilitiesUsing;
    private Ability[] abilitiesLearnt;
    private Item[] items;
    private Quirks[] quirks;

    int damage = 0;
    int poisonDamage = 0;


    private void Start()
    {
        combatantStats = GetComponent<Stats>();
        combatant = GetComponent<Combatant>();

        abilitiesUsing = combatant.abilitiesUsing;
        abilitiesLearnt = combatant.abilitiesLearnt;
        quirks = combatant.combatantQuirks;
        items = combatant.combatantItems;
    }

    public void CastAbility(int abilityNum)
    {
        Debug.Log("Casting Ability...");
        switch (abilitiesUsing[abilityNum].abilityType)
        {
            case ability_type.Damage:
                DamageAbility(abilityNum);
                break;
            case ability_type.Heal:
                HealAbility(abilityNum);
                break;
            case ability_type.Buff:
                BuffAbility(abilityNum);
                break;
            default:
                break;
        }

    }

    private void DamageAbility(int abilityNum)
    {
        Quirks[] enemyQuirks = enemy.GetComponent<Combatant>().combatantQuirks;
        Aspects.Aspect[] enemyResistances = enemy.GetComponent<Combatant>().resistances;
        Aspects.Aspect[] enemyImmunities = enemy.GetComponent<Combatant>().immunities;
        Aspects.Aspect[] enemyVulnerabilities = enemy.GetComponent<Combatant>().immunities;

        if (abilitiesUsing[abilityNum].statUsed == stat_used.Strength)
        {
            damage = abilitiesUsing[abilityNum].abilityPower + combatantStats.getStat("Str");

            for (int i = 0; i < 3; i++)
            {
                damage = CheckQuirkTypeResistances(damage, enemyQuirks[i], abilityNum);
                damage = CheckTypeResistance(damage, enemyResistances[i], enemyImmunities[i], enemyVulnerabilities[i], abilityNum, i);
            }



            if (Random.Range(1, 100) < combatantStats.getStat("Luck"))
            {
                damage *= 2;
                damage += poisonDamage;
                enemy.GetComponent<Stats>().setStat("HP", -damage);
                Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat("HP"));
            }
            else
            {
                damage += poisonDamage;
                enemy.GetComponent<Stats>().setStat("HP", -damage);
                Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat("HP"));
            }

        }
        else if (abilitiesUsing[abilityNum].statUsed == stat_used.Magic)
        {
            damage = abilitiesUsing[abilityNum].abilityPower + combatantStats.getStat("Mag");

            for (int i = 0; i < 3; i++)
            {
                damage = CheckQuirkTypeResistances(damage, enemyQuirks[i], abilityNum);
                damage = CheckTypeResistance(damage, enemyResistances[i], enemyImmunities[i], enemyVulnerabilities[i], abilityNum, i);
            }

            if (Random.Range(1, 100) < combatantStats.getStat("Luck"))
            {
                damage *= 2;
                damage += poisonDamage;
                enemy.GetComponent<Stats>().setStat("HP", -damage);
                Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat("HP"));
            }
            else
            {
                damage += poisonDamage;
                enemy.GetComponent<Stats>().setStat("HP", -damage);
                Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat("HP"));
            }

        }
        else if (abilitiesUsing[abilityNum].statUsed == stat_used.Dexterity)
        {
            damage = abilitiesUsing[abilityNum].abilityPower + combatantStats.getStat("Dex");

            for (int i = 0; i < 3; i++)
            {
                damage = CheckQuirkTypeResistances(damage, enemyQuirks[i], abilityNum);
                damage = CheckTypeResistance(damage, enemyResistances[i], enemyImmunities[i], enemyVulnerabilities[i], abilityNum, i);
            }

            if (Random.Range(1, 100) < combatantStats.getStat("Luck"))
            {
                damage *= 2;
                damage += poisonDamage;
                enemy.GetComponent<Stats>().setStat("HP", -damage);
                Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat("HP"));
            }
            else
            {
                damage += poisonDamage;
                enemy.GetComponent<Stats>().setStat("HP", -damage);
                Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat("HP"));
            }

        }
        Debug.Log("Dealt " + damage + " damage");
        poisonDamage = 0;
        damage = 0;
    }

    private void HealAbility(int abilityNum)
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

    private void BuffAbility(int abilityNum)
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

    public void UseItem()
    {
        int currentItem = combatant.currentItem;
        Debug.Log("Using Item...");        
        switch (items[currentItem].itemType)
        {
            case item_type.Poison:
                PoisonItem(currentItem);
                break;
            case item_type.Heal:
                HealItem(currentItem);
                break;
            case item_type.Buff:
                BuffItem(currentItem);
                break;
        }

    }

    private void PoisonItem(int currentItem)
    {
        Debug.Log("Added " + items[currentItem].itemPower + " points of damage to your next attack");
        poisonDamage = items[currentItem].itemPower;
        items[currentItem] = null;
    }

    private void HealItem(int currentItem)
    {
        int heal = items[currentItem].itemPower;       
        combatantStats.setStat("HP", heal);

        if (combatantStats.getStat("Con") < combatantStats.getStat("HP"))
        {
            combatantStats.setStat("HP", combatantStats.getStat("Con"));
        }

        Debug.Log("Healed " + items[currentItem].itemPower + " points of damage");
    }

    private void BuffItem(int currentItem)
    {
        switch (items[currentItem].statBoost)
        {
            case stat_boost.Strength:
                combatantStats.setStat("Str", items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat("Str"));
                break;
            case stat_boost.Dexterity:
                combatantStats.setStat("Dex", items[currentItem].itemPower);
                Debug.Log("Dexterity has been buffed to " + combatantStats.getStat("Dex"));
                break;
            case stat_boost.Magic:
                combatantStats.setStat("Mag", items[currentItem].itemPower);
                Debug.Log("Magic has been buffed to " + combatantStats.getStat("Mag"));
                break;
            case stat_boost.Defence:
                combatantStats.setStat("Def", items[currentItem].itemPower);
                Debug.Log("Defence has been buffed to " + combatantStats.getStat("Def"));
                break;
            case stat_boost.Constitution:
                combatantStats.setStat("Con", items[currentItem].itemPower);
                Debug.Log("Constitution has been buffed to " + combatantStats.getStat("Con"));
                break;
            case stat_boost.Luck:
                combatantStats.setStat("Luck", items[currentItem].itemPower);
                Debug.Log("Luck has been buffed to " + combatantStats.getStat("Luck"));
                break;
            case stat_boost.Speed:
                combatantStats.setStat("Speed", items[currentItem].itemPower);
                Debug.Log("Speed has been buffed to " + combatantStats.getStat("Speed"));
                Debug.Log("Initiative has been buffed to " + combatantStats.getStat("Init"));
                break;
            case stat_boost.Initiative:
                combatantStats.setStat("Init", items[currentItem].itemPower);
                break;
        }

    }

    private int CheckQuirkTypeResistances(int damage, Quirks enemyQuirk, int abilityNum)
    {
        if (enemyQuirk != null)
        {
            if (enemyQuirk.quirkResistance == quirk_resistant.Immunity)
            {
                if (CheckResistancesMatch(enemyQuirk.quirkAspect, abilityNum))
                {
                    return damage *= 0;
                }
            }
            else if (enemyQuirk.quirkResistance == quirk_resistant.Resistance)
            {
                if (CheckResistancesMatch(enemyQuirk.quirkAspect, abilityNum))
                {
                    return damage /= 2;
                }
            }
            else if (enemyQuirk.quirkResistance == quirk_resistant.Vulnerability)
            {
                if (CheckResistancesMatch(enemyQuirk.quirkAspect, abilityNum))
                {
                    return damage *= 2;
                }
            }
        }

        return damage;        
    }

    private int CheckTypeResistance(int damage, Aspects.Aspect enemyResistances, Aspects.Aspect enemyImmunities, Aspects.Aspect enemyVulnerabilities, int abilityNum, int i)
    {

        if(enemyResistances == abilitiesUsing[abilityNum].abilityAspect)
        {
            return damage /= 2;
        }
        else if (enemyImmunities == abilitiesUsing[abilityNum].abilityAspect)
        {
            return damage *= 0;
        }
        else if (enemyVulnerabilities == abilitiesUsing[abilityNum].abilityAspect)
        {
            return damage *= 2;
        }

        return damage;
    }

    private bool CheckResistancesMatch(Aspects.Aspect enemyResAspect, int abilityNum)
    {
        if (enemyResAspect == abilitiesUsing[abilityNum].abilityAspect)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}