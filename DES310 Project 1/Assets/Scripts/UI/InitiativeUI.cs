using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiativeUI : MonoBehaviour
{
    public BattleManager battleManager;
    public int initPlace;
    public GameObject[] battleQueue;
    bool imageSet = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (battleManager.getCombatantState() == CombatantState.END)
        {
            imageSet = false;
        }

        if (battleManager.getBattleQueue() != null)
        {
            if (imageSet == false)
            {
                battleQueue = battleManager.getBattleQueue().ToArray();
                if (initPlace < battleQueue.Length)
                {
                    GetComponent<Image>().sprite = battleQueue[initPlace].GetComponent<SpriteRenderer>().sprite;
                    GetComponent<Image>().SetNativeSize();
                    imageSet = true;
                }               
            }
        }

    }
}
