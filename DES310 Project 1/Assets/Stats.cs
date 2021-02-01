using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Combatant_Type
{
    Human = 0, // average weights
    Dragon = 1, // Strong:(str, def) Weak:(Speed, init)
    Elf = 2 // Strong: (Dex, Mag) Weak:(str, def)
}

[System.Serializable]
public class Stats : MonoBehaviour
{
    public Combatant_Type combatant_type;

    public int mod_str;
    public int mod_dex;
    public int mod_mag;
    public int mod_def;
    public int mod_con;
    public int mod_luck;
    public int mod_speed;
    public int mod_init;

    // base stats
    private int base_str = 25;

    private int base_dex = 25;
    private int base_mag = 25;
    private int base_def = 25;
    private int base_con = 25;
    private int base_luck = 5; // regular 1 in 20 e.g. d20 roll
    private int base_speed = 25;
    private int base_init = 25;

    private void Start()
    {
        switch (combatant_type)
        {
            case Combatant_Type.Human:
                base_str += 3;
                base_dex += 3;
                base_mag += 3;
                base_def += 3;
                base_con += 3;
                base_luck += 3;
                base_speed += 3;
                base_init += 3;
                break;

            case Combatant_Type.Dragon:
                base_str += 6;
                base_def += 6;
                base_speed -= 3;
                base_init -= 3;
                break;

            case Combatant_Type.Elf:
                base_mag += 6;
                base_dex += 6;
                base_str -= 3;
                base_def -= 3;
                break;
        };

        base_str += mod_str;
        base_dex += mod_dex;
        base_mag += mod_mag;
        base_def += mod_def;
        base_con += mod_con;
        base_luck += mod_luck;
        base_speed += mod_speed;
        base_init += mod_init;

        Debug.Log("Str: " + base_str);
        Debug.Log("dex: " + base_dex);
        Debug.Log("mag: " + base_mag);
        Debug.Log("def: " + base_def);
        Debug.Log("con: " + base_con);
        Debug.Log("luck: " + base_luck);
        Debug.Log("speed: " + base_speed);
        Debug.Log("init: " + base_init);
    }

    public int getStat(string statName)
    {
        switch (statName)
        {
            case "Str":
                return base_str;
                break;

            case "Dex":
                return base_dex;
                break;

            case "Mag":
                return base_mag;
                break;

            case "Def":
                return base_def;
                break;

            case "Con":
                return base_con;
                break;

            case "Luck":
                return base_luck;
                break;

            case "Speed":
                return base_speed;
                break;

            case "Init":
                return base_init;
                break;

            default:
                Debug.Log("ERROR: Invalid Stat Name");
                return 0;
        }
    }

    public void setStat(string statName, int modStat)
    {
        switch (statName)
        {
            case "Str":
                base_str += modStat;
                break;

            case "Dex":
                base_dex += modStat;
                break;

            case "Mag":
                base_mag += modStat;
                break;

            case "Def":
                base_def += modStat;
                break;

            case "Con":
                base_con += modStat;
                break;

            case "Luck":
                base_luck += modStat;
                break;

            case "Speed":
                base_speed += modStat;
                break;

            case "Init":
                base_init += modStat;
                break;
        }
    }
}