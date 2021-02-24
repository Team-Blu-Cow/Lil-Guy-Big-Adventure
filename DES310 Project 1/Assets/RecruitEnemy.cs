using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecruitEnemy : MonoBehaviour
{
    [SerializeField] GameObject buttonBase;
    [SerializeField] BattleManager battleManager;
    List<GameObject> buttons = new List<GameObject>();

    public void Recruit()
    {
        foreach (GameObject dead in battleManager.deadCombatants)
        {
            GameObject enemyButton = Instantiate(buttonBase, transform.GetChild(0)).gameObject;
            enemyButton.GetComponent<Button>().onClick.AddListener(() => { battleManager.AddParty(dead); });
            enemyButton.GetComponent<Button>().onClick.AddListener(DeleteButtons);
            enemyButton.GetComponentsInChildren<Image>()[1].sprite = ScreenManager.GetFirstSprite(dead); 
            enemyButton.GetComponentsInChildren<Image>()[1].SetNativeSize();
            buttons.Add(enemyButton);
        }
    }
    
    public void DeleteButtons()
    {
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }

        buttons = new List<GameObject>();
    }
}
