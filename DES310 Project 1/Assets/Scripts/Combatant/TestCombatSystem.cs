using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public struct AbilityResult
{
    private float m_overallDamage;
    private bool m_crit;
    private GameObject m_target;
    private int m_abilityIndex;

    public float oDamage
    {
        get
        {
            return m_overallDamage;
        }
        set
        {
            m_overallDamage = value;
        }
    }

    public bool crit
    {
        get
        {
            return m_crit;
        }
        set
        {
            m_crit = value;
        }
    }

    public GameObject target
    {
        get
        {
            return m_target;
        }

        set
        {
            m_target = value;
        }
    }

    public int abilityIndex
    {
        get
        {
            return m_abilityIndex;
        }

        set
        {
            m_abilityIndex = value;
        }
    }


    
}

public class TestCombatSystem : MonoBehaviour
{
    [HideInInspector] public GameObject enemy;

    private Combatant combatant;
    private Stats combatantStats;

    private Ability[] abilitiesUsing;
    private Ability[] abilitiesLearnt;
    private Item[] items;
    private Quirks[] quirks;

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


    public AbilityResult CastAbility(int abilityNum)
    {
        Debug.Log("Casting Ability...");
        if (enemy != null)
        {           
            switch (abilitiesUsing[abilityNum].abilityType)
            {
                case ability_type.Damage:
                    return DamageAbility(abilityNum);
                case ability_type.Heal:
                    return HealAbility(abilityNum);
                case ability_type.Buff:
                    return BuffAbility(abilityNum);
                default:
                    break;
            }
            enemy = null;
        }
        return new AbilityResult();
    }

    private AbilityResult DamageAbility(int abilityNum)
    {
        Quirks[] enemyQuirks = enemy.GetComponent<Combatant>().combatantQuirks;
        float[] enemyResistances = enemy.GetComponent<Combatant>().resistances;
        AbilityResult abilityResult = new AbilityResult();
        bool isCrit = false;

        int aspectType = (int)abilitiesUsing[abilityNum].abilityAspect;

        float damage = 0;
        float defenceNegation = enemy.GetComponent<Stats>().getStat(Combatant_Stats.Defence) / 2;

        switch (abilitiesUsing[abilityNum].statUsed)
        {
            case stat_used.Strength:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Strength);

                damage = damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    damage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            case stat_used.Magic:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Magic);

                damage = damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    damage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            case stat_used.Dexterity:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Dexterity);

                damage = damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    damage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            default:
                break;
        }

        if (Random.Range(1, 100) < combatantStats.getStat(Combatant_Stats.Luck))
        {
            damage *= 2;
            isCrit = true;
        }

        damage -= defenceNegation;

        damage += poisonDamage;
        enemy.GetComponent<Combatant>().do_damage((int)damage, abilitiesUsing[abilityNum].abilityAspect);
        //Debug.Log("Enemy HP: " + enemy.GetComponent<Stats>().getStat(Combatant_Stats.HP));
        //Debug.Log("Dealt " + damage + " damage");

        abilityResult.oDamage = damage;
        abilityResult.crit = isCrit;

        poisonDamage = 0;

        return abilityResult;
    }

    private AbilityResult HealAbility(int abilityNum)
    {
        bool isCrit = false;
        AbilityResult abilityResult = new AbilityResult();
        int heal = (int)abilitiesUsing[abilityNum].abilityPower * combatantStats.getStat(Combatant_Stats.Magic);

        if (Random.Range(1, 100) < combatantStats.getStat(Combatant_Stats.Luck))
        {
            heal *= 2;
            isCrit = true;
        }

        if (abilitiesUsing[abilityNum].statUsed == stat_used.Magic)
        {
            combatantStats.setStat(Combatant_Stats.HP, combatantStats.getStat(Combatant_Stats.HP) + heal);
            if (combatantStats.getStat(Combatant_Stats.Constitution) < combatantStats.getStat(Combatant_Stats.HP))
            {
                combatantStats.setStat(Combatant_Stats.HP, combatantStats.getStat(Combatant_Stats.Constitution));
            }
        }

        abilityResult.oDamage = heal;
        abilityResult.crit = isCrit;
        
        //Debug.Log("Healed " + heal + " points of health");

        return abilityResult;
    }

    private AbilityResult BuffAbility(int abilityNum)
    {
        bool isCrit = false;
        AbilityResult abilityResult = new AbilityResult();
        int buff = (int)abilitiesUsing[abilityNum].abilityPower;

        if (Random.Range(1, 100) < combatantStats.getStat(Combatant_Stats.Luck))
        {
            buff *= 2;
            isCrit = true;
        }

        switch (abilitiesUsing[abilityNum].statUsed)
        {
            case stat_used.Strength:
                combatantStats.setStat(Combatant_Stats.Strength, combatantStats.getStat(Combatant_Stats.Strength) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Strength));
                break;
            case stat_used.Dexterity:
                combatantStats.setStat(Combatant_Stats.Dexterity, combatantStats.getStat(Combatant_Stats.Dexterity) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Dexterity));
                break;
            case stat_used.Magic:
                combatantStats.setStat(Combatant_Stats.Magic, combatantStats.getStat(Combatant_Stats.Magic) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Magic));
                break;
            case stat_used.Defence:
                combatantStats.setStat(Combatant_Stats.Defence, combatantStats.getStat(Combatant_Stats.Defence) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Defence));
                break;
            case stat_used.Constitution:
                combatantStats.setStat(Combatant_Stats.Constitution, combatantStats.getStat(Combatant_Stats.Constitution) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Constitution));
                break;
            case stat_used.Luck:
                combatantStats.setStat(Combatant_Stats.Luck, combatantStats.getStat(Combatant_Stats.Luck) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Luck));
                break;
            case stat_used.Speed:
                combatantStats.setStat(Combatant_Stats.Speed, combatantStats.getStat(Combatant_Stats.Speed) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Speed));
                break;
            case stat_used.Initiative:
                combatantStats.setStat(Combatant_Stats.Initiative, combatantStats.getStat(Combatant_Stats.Initiative) + buff);
                //Debug.Log("Strength has been buffed to " + combatantStats.getStat(Combatant_Stats.Initiative));
                break;
        }

        abilityResult.oDamage = buff;
        abilityResult.crit = isCrit;

        return abilityResult;
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