using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveParty : MonoBehaviour
{
    [SerializeField] GameObject buttonBase;
    [SerializeField] GameObject[] buttons;
    
    public void Remove(GameObject newPartyMember)
    {
        buttons[0].GetComponent<Button>().onClick.AddListener(() => { ScreenManager.instance.partyManager.RemoveCombatant(1, newPartyMember); });
        buttons[0].transform.GetChild(0).GetComponent<Image>().sprite = ScreenManager.GetFirstSprite(ScreenManager.instance.partyManager.party[1]);
        buttons[0].transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        buttons[1].GetComponent<Button>().onClick.AddListener(() => { ScreenManager.instance.partyManager.RemoveCombatant(2, newPartyMember); });
        buttons[1].transform.GetChild(0).GetComponent<Image>().sprite = ScreenManager.GetFirstSprite(ScreenManager.instance.partyManager.party[2]);
        buttons[1].transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        buttons[2].GetComponent<Button>().onClick.AddListener(() => { ScreenManager.instance.partyManager.RemoveCombatant(3, newPartyMember); });
        buttons[2].transform.GetChild(0).GetComponent<Image>().sprite = ScreenManager.GetFirstSprite(ScreenManager.instance.partyManager.party[3]);
        buttons[2].transform.GetChild(0).GetComponent<Image>().SetNativeSize();

        /*GameObject[] partyList = ScreenManager.instance.partyManager.party;
        for (int i = 1; i < partyList.Length; i++)
        {
            if (partyList[i] != null)
            {
                GameObject partyButton = transform.GetChild(0).GetChild(i).gameObject;
                partyButton.GetComponent<Button>().onClick.AddListener(() => {ScreenManager.instance.partyManager.RemoveCombatant(ScreenManager.instance.partyManager.party[partyButton.GetComponent<Button>().name[0]]); });
                partyButton.GetComponentsInChildren<Image>()[1].sprite = ScreenManager.GetFirstSprite(partyList[i]);
                partyButton.GetComponentsInChildren<Image>()[1].SetNativeSize();
            }
        }*/
    }
}
