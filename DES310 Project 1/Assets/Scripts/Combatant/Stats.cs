using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Combatant_Type
{
    Human = 0, // average weights
    Mushroom = 1, // Str: Luck, Mag Weak: Def, Dex
    StoneBug = 2, // Str: Str, Def Weak: Mag, Init
    Phoenix = 3, // Str: Mag, Init Weak: Luck, Str
    Hedgehog = 4 // Str: Dex Weak: Str, Init
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

    public int mod_str;
    public int mod_dex;
    public int mod_mag;
    public int mod_def;
    public int mod_con;
    public int mod_luck;
    public int mod_speed;
    public int mod_init;

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
                base_speed += 3;
                base_init += 3;               
                break;

            case Combatant_Type.Mushroom:
                base_luck += 6;
                base_mag += 6;
                base_def -= 3;
                base_dex -= 3;
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
                base_str -= 3;
                base_init -= 3;
                break;
        };

        //mod_str += Random.Range(1, 5);
        //mod_dex += Random.Range(1, 5);
        //mod_mag += Random.Range(1, 5);
        //mod_def += Random.Range(1, 5);
        //mod_luck += Random.Range(1, 5);
        //mod_init += Random.Range(1, 5);
     
        base_str += mod_str;
        base_dex += mod_dex;
        base_mag += mod_mag;
        base_def += mod_def;
        base_con += mod_con;
        base_luck += mod_luck;
        base_speed += mod_speed;
        base_init += mod_init;

        base_con *= 10;
        current_hp = base_con;


        quirkStats();

        // pls dont, thanks, you can see this shit in the editor

        //Debug.Log("Str: " + base_str);
        //Debug.Log("dex: " + base_dex);
        //Debug.Log("mag: " + base_mag);
        //Debug.Log("def: " + base_def);
        //Debug.Log("con: " + base_con);
        //Debug.Log("luck: " + base_luck);
        //Debug.Log("speed: " + base_speed);
        //Debug.Log("init: " + base_init);
        //Debug.Log("HP: " + current_hp);
    }

    public int getStat(Combatant_Stats stat)
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

    public void setStat(Combatant_Stats stat, int setStat)
    {
        switch (stat)
        {
            case Combatant_Stats.Strength:
                base_str = setStat;
                break;
            case Combatant_Stats.Dexterity:
                base_dex = setStat;
                break;
            case Combatant_Stats.Magic:
                base_mag = setStat;
                break;
            case Combatant_Stats.Defence:
                base_def = setStat;
                break;
            case Combatant_Stats.Constitution:
                base_con = setStat;
                break;
            case Combatant_Stats.Luck:
                base_luck = setStat;
                break;
            case Combatant_Stats.Speed:
                base_speed = setStat;
                break;
            case Combatant_Stats.Initiative:
                base_init = setStat;
                break;
            case Combatant_Stats.HP:
                current_hp = setStat;
                break;
        }
    }

    private void quirkStats()
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