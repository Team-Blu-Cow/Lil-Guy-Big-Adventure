using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitEnemy : MonoBehaviour
{
    [SerializeField] GameObject buttonBase;
    [SerializeField] BattleManager battleManager;

    public void Recruit()
    {
        foreach (GameObject dead in battleManager.deadCombatants)
        {
            GameObject enemyButton = Instantiate(buttonBase, transform.GetChild(0)).gameObject;
            enemyButton.GetComponent<Button>().onClick.AddListener(() => { battleManager.AddParty(dead); });
            enemyButton.GetComponentsInChildren<Image>()[1].sprite = ScreenManager.GetFirstSprite(dead); 
            enemyButton.GetComponentsInChildren<Image>()[1].SetNativeSize();
        }
    }    
}
