using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPartyManager : MonoBehaviour
{
    public GameObject[] party = new GameObject[4];
    public int totalAreasVisited = 1;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Remove()
    {
        foreach (GameObject item in GetComponentsInChildren<GameObject>())
        {
            Destroy(item);
        }
    }

    public bool AddCombatant(GameObject combatant)
    {
        bool partyFull = true;

        for (int i = 0; i < party.Length; i++)
        {
            if (party[i] == combatant)
            {
                partyFull = false;
                break;
            }

            if (party[i] == null)
            {
                partyFull = false;
                party[i] = combatant;
                combatant.tag = "Ally";
                combatant.gameObject.SetActive(true);                
                combatant.transform.parent = transform;
                combatant.GetComponent<Stats>().SetModStat(Combatant_Stats.HP, combatant.GetComponent<Stats>().GetFinalStat(Combatant_Stats.Constitution));
                FindObjectOfType<MapGeneration>().placedEnemies.Remove(combatant);
                break;
            }
        }

        return partyFull;
    }
    public void RemoveCombatant(GameObject combatant)
    {
        for (int i = 0; i < party.Length; i++)
        {
            if (party[i] == combatant)
            {
                party[i] = null;
                Destroy(combatant);
                break;
            }
        }
    }

    public void RemoveCombatant(int memberIndex, GameObject newPartyMember)
    {
        if(party[memberIndex] != null)
        {
            Destroy(party[memberIndex]);
            party[memberIndex] = null;
            AddCombatant(newPartyMember);
            ScreenManager.instance.HideRemove();
        }
    }
}
