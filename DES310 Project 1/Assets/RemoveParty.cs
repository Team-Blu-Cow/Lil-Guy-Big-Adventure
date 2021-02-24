using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveParty : MonoBehaviour
{
    [SerializeField] GameObject buttonBase;
    
    public void Remove()
    {
        GameObject[] partyList = ScreenManager.instance.partyManager.party;
        for (int i = 1; i < partyList.Length; i++)
        {
            if (partyList[i] != null)
            {
                GameObject partyButton = transform.GetChild(0).GetChild(i).gameObject;
                partyButton.GetComponent<Button>().onClick.AddListener(() => {ScreenManager.instance.partyManager.RemoveCombatant(ScreenManager.instance.partyManager.party[partyButton.GetComponent<Button>().name[0]]); });
                partyButton.GetComponentsInChildren<Image>()[1].sprite = ScreenManager.GetFirstSprite(partyList[i]);
                partyButton.GetComponentsInChildren<Image>()[1].SetNativeSize();
            }
        }
    }
}
