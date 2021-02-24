using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Combatant_Type
{
    Human = 0, // average weights
    Mushroom = 1, // Str: Luck, Mag Weak: Def, Dex
    StoneBug = 2, // Str: Str, Def Weak: Mag, Init
    Phoenix = 3, // Str: Mag, Init Weak: Luck, Str
    Hedgehog = 4, // Str: Dex Weak: Str, Init
    Hedgehonch = 5,
    Magishroom = 6
}

public enum Combatant_Stats
{
    Strength = 0,
    Dexterity = 1,
    Magic = 2,
    Defence = 3,
    Constitution = 4,
    Luck = 5,
    Speed = 6,
    Initiative = 7,
    HP = 8
}


[System.Serializable]
public class Stats : MonoBehaviour
{
    public Combatant_Type combatant_type;
    Combatant combatant;

    int mod_str;
    int mod_dex;
    int mod_mag;
    int mod_def;
    int mod_con;
    int mod_luck;
    int mod_speed;
    int mod_init;

    // base stats
    private int base_str = 5;
    private int base_dex = 5;
    private int base_mag = 5;
    private int base_def = 5;
    private int base_con = 5;
    private int base_luck = 5; // regular 1 in 20 e.g. d20 roll
    private int base_speed = 5;
    private int base_init = 5;
    [SerializeField] private int current_hp;

    private void Start()
    {        
        combatant = GetComponent<Combatant>();
        // Racial traits
        switch (combatant_type)
        {
            case Combatant_Type.Human:
                base_str += 3;
                base_dex += 3;
                base_mag += 3;
                base_def += 3;
                base_con += 7;
                base_luck += 3;
                base_init += 3;               
                break;
            case Combatant_Type.Mushroom:
                base_luck += 8;
                base_mag += 4;
                base_con -= 2;
                base_def -= 4;
                base_dex -= 4;
                break;
            case Combatant_Type.StoneBug:
                base_str += 6;
                base_def += 6;
                base_mag -= 3;
                base_init -= 3;
                break;
            case Combatant_Type.Phoenix:
                base_mag += 6;
                base_init += 6;
                base_luck -= 3;
                base_str -= 3;
                break;
            case Combatant_Type.Hedgehog:
                base_dex += 9;
                base_def += 2;
                base_speed += 1;
                base_str -= 3;
                base_init -= 3;
                break;
            case Combatant_Type.Hedgehonch:
                base_def += 6;
                base_con += 2;
                base_mag -= -6;
                base_init -= 4;
                base_speed -= 2;
                break;
            case Combatant_Type.Magishroom:
                base_mag += 9;
                base_init += 3;
                base_con -= 1;
                base_def -= 3;
                base_str -= 4;
                break;
        };

        base_str += Random.Range(1, 5);
        base_dex += Random.Range(1, 5);
        base_mag += Random.Range(1, 5);
        base_def += Random.Range(1, 5);
        base_luck += Random.Range(1, 5);
        base_init += Random.Range(1, 5);
     
        base_con *= 10;
        current_hp = base_con;

        QuirkStats();


    }
    public int GetFinalStat(Combatant_Stats stat)
    {
        return (GetModStat(stat) + GetStat(stat));
    }

    public void ResetStats()
    {
         mod_str = 0;
         mod_dex = 0;
         mod_mag = 0;
         mod_def = 0;
         mod_con = 0;
         mod_luck = 0;
         mod_speed = 0;
         mod_init = 0;
    }

    public int GetStat(Combatant_Stats stat)
    {
        switch (stat)
        {
            case Combatant_Stats.Strength:
                return base_str;
            case Combatant_Stats.Dexterity:
                return base_dex;
            case Combatant_Stats.Magic:
                return base_mag;
            case Combatant_Stats.Defence:
                return base_def;
            case Combatant_Stats.Constitution:
                return base_con;
            case Combatant_Stats.Luck:
                return base_luck;
            case Combatant_Stats.Speed:
                return base_speed;
            case Combatant_Stats.Initiative:
                return base_init;
            case Combatant_Stats.HP:
                return current_hp;
            default:
                Debug.Log("ERROR: Invalid Stat Name");
                return 0;
        }
    }

    public int GetModStat(Combatant_Stats stat)
    {
        switch (stat)
        {
            case Combatant_Stats.Strength:
                return mod_str;
            case Combatant_Stats.Dexterity:
                return mod_dex;
            case Combatant_Stats.Magic:
                return mod_mag;
            case Combatant_Stats.Defence:
                return mod_def;
            case Combatant_Stats.Constitution:
                return mod_con;
            case Combatant_Stats.Luck:
                return mod_luck;
            case Combatant_Stats.Speed:
                return mod_speed;
            case Combatant_Stats.Initiative:
                return mod_init;
            case Combatant_Stats.HP:
                return current_hp;
            default:
                Debug.Log("ERROR: Invalid Stat Name");
                return 0;
        }
    }

    public void SetModStat(Combatant_Stats stat, int setStat)
    {
        switch (stat)
        {
            case Combatant_Stats.Strength:
                mod_str = setStat;
                break;
            case Combatant_Stats.Dexterity:
                mod_dex = setStat;
                break;
            case Combatant_Stats.Magic:
                mod_mag = setStat;
                break;
            case Combatant_Stats.Defence:
                mod_def = setStat;
                break;
            case Combatant_Stats.Constitution:
                mod_con = setStat;
                break;
            case Combatant_Stats.Luck:
                mod_luck = setStat;
                break;
            case Combatant_Stats.Speed:
                mod_speed = setStat;
                break;
            case Combatant_Stats.Initiative:
                mod_init = setStat;
                break;
            case Combatant_Stats.HP:
                current_hp = setStat;
                break;
        }
    }

    private void QuirkStats()
    {
        Quirks[] quirks = combatant.combatantQuirks;

        
        for(int i = 0; i < 3; i++)
        {
            if (quirks[i] != null)
            {
                switch (quirks[i].statBoost)
                {
                    case stat_used.Strength:
                        base_str += quirks[i].quirkPower;
                        break;
                    case stat_used.Dexterity:
                        base_dex += quirks[i].quirkPower;
                        break;
                    case stat_used.Magic:
                        base_mag += quirks[i].quirkPower;
                        break;
                    case stat_used.Defence:
                        base_def += quirks[i].quirkPower;
                        break;
                    case stat_used.Constitution:
                        base_con += quirks[i].quirkPower;
                        break;
                    case stat_used.Luck:
                        base_luck += quirks[i].quirkPower;
                        break;
                    case stat_used.Speed:
                        base_speed += quirks[i].quirkPower;
                        break;
                    case stat_used.Initiative:
                        base_init += quirks[i].quirkPower;
                        break;
                }
            }
        }
    }
}