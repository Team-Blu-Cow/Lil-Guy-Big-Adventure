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

    float damage = 0;
    float poisonDamage = 0;


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
        if (enemy != null)
        {           
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
            enemy = null;
        }

    }

    private void DamageAbility(int abilityNum)
    {
        Quirks[] enemyQuirks = enemy.GetComponent<Combatant>().combatantQuirks;
        float[] enemyResistances = enemy.GetComponent<Combatant>().resistances;

        int aspectType = (int)abilitiesUsing[abilityNum].abilityAspect;

        float tempDamage = 0;

        switch (abilitiesUsing[abilityNum].statUsed)
        {
            case stat_used.Strength:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Strength);

                tempDamage = (float)damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    tempDamage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            case stat_used.Magic:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Magic);

                tempDamage = (float)damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    tempDamage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            case stat_used.Dexterity:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Dexterity);

                tempDamage = (float)damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    tempDamage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            default:
                break;
        }

        damage = (int)tempDamage;

        if (Random.Range(1, 100) < combatantStats.getStat(Combatant_Stats.Luck))
        {
            damage *= 2;
        }

        damage += poisonDamage;
        enemy.GetComponent<Combatant>().do_damage((int)damage, abilitiesUsing[abilityNum].abilityAspect);
        Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat(Combatant_Stats.HP));

        Debug.Log("Dealt " + damage + " damage");
        poisonDamage = 0;
        damage = 0;
    }

    private void HealAbility(int abilityNum)
    {
        int heal = (int)abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Magic);
        if (abilitiesUsing[abilityNum].statUsed == stat_used.Magic)
        {
            combatantStats.setStat(Combatant_Stats.HP, combatantStats.getStat(Combatant_Stats.HP) + heal);
            if (combatantStats.getStat(Combatant_Stats.Constitution) < combatantStats.getStat(Combatant_Stats.HP))
            {
                combatantStats.setStat(Combatant_Stats.HP, combatantStats.getStat(Combatant_Stats.Constitution));
            }
        }
        Debug.Log("Healed " + heal + " points of health");
    }

    private void BuffAbility(int abilityNum)
    {
        switch (abilitiesUsing[abilityNum].statUsed)
        {
            case stat_used.Strength:
                combatantStats.setStat(Combatant_Stats.Strength, combatantStats.getStat(Combatant_Stats.Strength) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Strength));
                break;
            case stat_used.Dexterity:
                combatantStats.setStat(Combatant_Stats.Dexterity, combatantStats.getStat(Combatant_Stats.Dexterity) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Dexterity));
                break;
            case stat_used.Magic:
                combatantStats.setStat(Combatant_Stats.Magic, combatantStats.getStat(Combatant_Stats.Magic) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Magic));
                break;
            case stat_used.Defence:
                combatantStats.setStat(Combatant_Stats.Defence, combatantStats.getStat(Combatant_Stats.Defence) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Defence));
                break;
            case stat_used.Constitution:
                combatantStats.setStat(Combatant_Stats.Constitution, combatantStats.getStat(Combatant_Stats.Constitution) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Constitution));
                break;
            case stat_used.Luck:
                combatantStats.setStat(Combatant_Stats.Luck, combatantStats.getStat(Combatant_Stats.Luck) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Luck));
                break;
            case stat_used.Speed:
                combatantStats.setStat(Combatant_Stats.Speed, combatantStats.getStat(Combatant_Stats.Speed) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Speed));
                break;
            case stat_used.Initiative:
                combatantStats.setStat(Combatant_Stats.Initiative, combatantStats.getStat(Combatant_Stats.Initiative) + (int)abilitiesUsing[abilityNum].abilityPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Initiative));
                break;

        }
    }

    public void UseItem(int currentItem)
    {
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
        combatantStats.setStat(Combatant_Stats.HP, combatantStats.getStat(Combatant_Stats.HP) + heal);

        if (combatantStats.getStat(Combatant_Stats.Constitution) < combatantStats.getStat(Combatant_Stats.HP))
        {
            combatantStats.setStat(Combatant_Stats.HP, combatantStats.getStat(Combatant_Stats.Constitution));
        }

        Debug.Log("Healed " + items[currentItem].itemPower + " points of damage");
    }

    private void BuffItem(int currentItem)
    {
        switch (items[currentItem].statBoost)
        {
            case stat_boost.Strength:
                combatantStats.setStat(Combatant_Stats.Strength, combatantStats.getStat(Combatant_Stats.Strength) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Strength));
                break;
            case stat_boost.Dexterity:
                combatantStats.setStat(Combatant_Stats.Dexterity, combatantStats.getStat(Combatant_Stats.Dexterity) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Dexterity));
                break;
            case stat_boost.Magic:
                combatantStats.setStat(Combatant_Stats.Magic, combatantStats.getStat(Combatant_Stats.Magic) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Magic));
                break;
            case stat_boost.Defence:
                combatantStats.setStat(Combatant_Stats.Defence, combatantStats.getStat(Combatant_Stats.Defence) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Defence));
                break;
            case stat_boost.Constitution:
                combatantStats.setStat(Combatant_Stats.Constitution, combatantStats.getStat(Combatant_Stats.Constitution) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Constitution));
                break;
            case stat_boost.Luck:
                combatantStats.setStat(Combatant_Stats.Luck, combatantStats.getStat(Combatant_Stats.Luck) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Luck));
                break;
            case stat_boost.Speed:
                combatantStats.setStat(Combatant_Stats.Speed, combatantStats.getStat(Combatant_Stats.Speed) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Speed));
                break;
            case stat_boost.Initiative:
                combatantStats.setStat(Combatant_Stats.Initiative, combatantStats.getStat(Combatant_Stats.Initiative) + items[currentItem].itemPower);
                Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Initiative));
                break;
        }

    }

    private float CheckQuirkTypeResistances( Quirks enemyQuirk, int abilityType)
    {
        if (enemyQuirk != null)
        {
            if((int)enemyQuirk.quirkAspect == abilityType)
            {
                return enemyQuirk.quirkResistance;
            }            
        }

        return 1;        
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