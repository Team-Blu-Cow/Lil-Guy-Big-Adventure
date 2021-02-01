using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum spellType
{
    Damage = 0,
    Heal = 1,
    Buff = 2
};

public class Spell_Library : MonoBehaviour
{
    // luck
    // speed
    // def
    // str
    // dex
    // magic

    public string spell_Name;
    public float base_Power;
    public string stat_modifier;
    public spellType spell_type;

    public void castSpell(/*get ref to target*/)
    {
        switch (spell_type)
        {
            case spellType.Damage:
                //Do Damage @ /*ref to enemy*/
                break;

            case spellType.Heal:
                //Do Heal @ /*ref to target*/
                break;

            case spellType.Buff:
                //Do buff @ /*ref to target*/
                break;

            default:
                break;
        }
    }

    public float Melee(int str, int luck)
    {
        //Debug.Log("Casting Melee...");
        //Debug.Log("input Str: " + str);
        //Debug.Log("input Luck: " + luck);
        float damage = 0;
        if (Random.Range(1, 100) < luck)
        {
            damage += (str + Melee(str, luck));
        }
        else
        {
            damage += str;
        }

        return damage;
    }

    public float Magic(int mag, int luck)
    {
        //Debug.Log("Casting Magic...");
        //Debug.Log("input Mag: " + mag);
        //Debug.Log("input Luck: " + luck);
        if (Random.Range(1, 100) < luck)
        {
            return mag * 2;
        }
        else
        {
            return mag;
        }
    }

    public float Ranged(int dex, int luck)
    {
        //Debug.Log("Casting Ranged...");
        //Debug.Log("input Dex: " + dex);
        //Debug.Log("input Luck: " + luck);

        if (Random.Range(1, 100) < luck)
        {
            return dex * 2;
        }
        else
        {
            return dex;
        }
    }
}