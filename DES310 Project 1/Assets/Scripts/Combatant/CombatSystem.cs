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

public class CombatSystem : MonoBehaviour
{
    [HideInInspector] public GameObject target;

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
        if (target != null)
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
            target = null;
        }
        return new AbilityResult();
    }

    private AbilityResult DamageAbility(int abilityNum)
    {
        Quirks[] enemyQuirks = target.GetComponent<Combatant>().combatantQuirks;
        float[] enemyResistances = target.GetComponent<Combatant>().resistances;
        AbilityResult abilityResult = new AbilityResult();
        bool isCrit = false;

        int aspectType = (int)abilitiesUsing[abilityNum].abilityAspect;

        float damage = 0;
        float defenceNegation = target.GetComponent<Stats>().GetStat(Combatant_Stats.Defence) / 2;

        switch (abilitiesUsing[abilityNum].statUsed)
        {
            case stat_used.Strength:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.GetFinalStat(Combatant_Stats.Strength);

                damage = damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    damage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            case stat_used.Magic:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.GetFinalStat(Combatant_Stats.Magic);

                damage = damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    damage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            case stat_used.Dexterity:
                damage = abilitiesUsing[abilityNum].abilityPower * combatantStats.GetFinalStat(Combatant_Stats.Dexterity);

                damage = damage * enemyResistances[aspectType];

                for (int i = 0; i < 3; i++)
                {
                    damage *= CheckQuirkTypeResistances(enemyQuirks[i], aspectType);
                }
                break;

            default:
                break;
        }

        if (Random.Range(1, 100) < combatantStats.GetFinalStat(Combatant_Stats.Luck))
        {
            damage *= 2;
            isCrit = true;
        }

        damage -= defenceNegation;

        damage += poisonDamage;
        target.GetComponent<Combatant>().do_damage((int)damage, abilitiesUsing[abilityNum].abilityAspect);
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
        int heal = (int)abilitiesUsing[abilityNum].abilityPower + combatantStats.GetFinalStat(Combatant_Stats.Magic);

        if (Random.Range(1, 100) < combatantStats.GetStat(Combatant_Stats.Luck))
        {
            heal *= 2;
            isCrit = true;
        }

        if (abilitiesUsing[abilityNum].statUsed == stat_used.Magic)
        {
            target.GetComponent<Stats>().SetModStat(Combatant_Stats.HP, target.GetComponent<Stats>().GetModStat(Combatant_Stats.HP) + heal);
            if (target.GetComponent<Stats>().GetStat(Combatant_Stats.Constitution) < target.GetComponent<Stats>().GetModStat(Combatant_Stats.HP))
            {
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.HP, target.GetComponent<Stats>().GetFinalStat(Combatant_Stats.Constitution));
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

        if (Random.Range(1, 100) < combatantStats.GetStat(Combatant_Stats.Luck))
        {
            buff *= 2;
            isCrit = true;
        }

        switch (abilitiesUsing[abilityNum].statUsed)
        {
            case stat_used.Strength:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Strength, buff);
                break;
            case stat_used.Dexterity:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Dexterity, buff);
                break;
            case stat_used.Magic:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Magic, buff);
                break;
            case stat_used.Defence:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Defence, buff);
                break;
            case stat_used.Constitution:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Constitution, buff);
                break;
            case stat_used.Luck:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Luck, buff);
                break;
            case stat_used.Speed:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Speed, buff);
                break;
            case stat_used.Initiative:
                target.GetComponent<Stats>().SetModStat(Combatant_Stats.Initiative, buff);
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
        //Debug.Log("Added " + items[currentItem].itemPower + " points of damage to your next attack");
        poisonDamage = items[currentItem].itemPower;
        items[currentItem] = null;
    }

    private void HealItem(int currentItem)
    {
        int heal = items[currentItem].itemPower;       
        combatantStats.SetModStat(Combatant_Stats.HP, combatantStats.GetFinalStat(Combatant_Stats.HP) + heal);

        if (combatantStats.GetFinalStat(Combatant_Stats.Constitution) < combatantStats.GetFinalStat(Combatant_Stats.HP))
        {
            combatantStats.SetModStat(Combatant_Stats.HP, combatantStats.GetFinalStat(Combatant_Stats.Constitution));
        }

        Debug.Log("Healed " + items[currentItem].itemPower + " points of damage");
        
    }

    private void BuffItem(int currentItem)
    {
        switch (items[currentItem].statBoost)
        {
            case stat_boost.Strength:
                combatantStats.SetModStat(Combatant_Stats.Strength, combatantStats.GetStat(Combatant_Stats.Strength) + items[currentItem].itemPower);
                break;
            case stat_boost.Dexterity:
                combatantStats.SetModStat(Combatant_Stats.Dexterity, combatantStats.GetStat(Combatant_Stats.Dexterity) + items[currentItem].itemPower);
                break;
            case stat_boost.Magic:
                combatantStats.SetModStat(Combatant_Stats.Magic, combatantStats.GetStat(Combatant_Stats.Magic) + items[currentItem].itemPower);
                break;
            case stat_boost.Defence:
                combatantStats.SetModStat(Combatant_Stats.Defence, combatantStats.GetStat(Combatant_Stats.Defence) + items[currentItem].itemPower);
                break;
            case stat_boost.Constitution:
                combatantStats.SetModStat(Combatant_Stats.Constitution, combatantStats.GetStat(Combatant_Stats.Constitution) + items[currentItem].itemPower);
                break;
            case stat_boost.Luck:
                combatantStats.SetModStat(Combatant_Stats.Luck, combatantStats.GetStat(Combatant_Stats.Luck) + items[currentItem].itemPower);
                break;
            case stat_boost.Speed:
                combatantStats.SetModStat(Combatant_Stats.Speed, combatantStats.GetStat(Combatant_Stats.Speed) + items[currentItem].itemPower);
                break;
            case stat_boost.Initiative:
                combatantStats.SetModStat(Combatant_Stats.Initiative, combatantStats.GetStat(Combatant_Stats.Initiative) + items[currentItem].itemPower);
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