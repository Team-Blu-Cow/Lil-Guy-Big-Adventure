using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public string[] spells;
    public Spell_Library library;
    public Stats combatantStats;

    public float CastDamageSpell(int spellToUse)
    {
        if (spells.Length == 0)
        {
            Debug.Log("ERROR: No Known Spells");
            return 0;
        }

        float finalDamage = 0;
        switch (spells[spellToUse])
        {
            case "Melee":
                finalDamage += library.Melee(combatantStats.getStat("Str"), combatantStats.getStat("Luck"));
                break;

            case "Magic":
                finalDamage += library.Magic(combatantStats.getStat("Mag"), combatantStats.getStat("Luck"));
                break;

            case "Ranged":
                finalDamage += library.Ranged(combatantStats.getStat("Dex"), combatantStats.getStat("Luck"));
                break;
        }

        return finalDamage;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Melee: " + CastDamageSpell(0));
            Debug.Log("Magic: " + CastDamageSpell(1));
            Debug.Log("Ranged: " + CastDamageSpell(2));
        }
    }
}