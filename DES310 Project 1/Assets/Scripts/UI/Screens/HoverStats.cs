using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class HoverStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    GameObject hovered;
    [SerializeField] GameObject member;
    [SerializeField] PlayerPartyManager party;
    float hoverTime;
    int count = 0;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < party.party.Length; i++)
        {
            if (party.party[i])
            {
                GameObject tempMember = Instantiate(member, transform.GetChild(0));
                tempMember.GetComponent<PartyCombatant>().SetAll(party.party[i]);
                tempMember.transform.GetChild(2).GetComponent<Image>().sprite = party.party[i].GetComponent<SpriteRenderer>().sprite;
                tempMember.transform.GetChild(2).GetComponent<Image>().SetNativeSize();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hovered)
        {
            if (hoverTime > 0.5f && !hovered.activeSelf)
            {
                ShowStats();
            }
            else if (hoverTime < 0.5f)
            {
                hoverTime += Time.deltaTime;
            }
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        switch (count)
        {            
            case 0:
                LeanTween.delayedCall(0.3f, OpenCombatant).setOnCompleteParam(eventData.pointerCurrentRaycast.gameObject);
                count++;
                break;
            case 1:
                LeanTween.cancelAll();
                OpenParty();
                count = 0;
                break;
            default:
                break;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.CompareTag("UIPartyMember"))
        {
            hovered = eventData.pointerCurrentRaycast.gameObject.transform.parent.GetChild(0).gameObject;
            if (LeanTween.isTweening(hovered))
                ShowStats();
        }
    }

    IEnumerator Exit()
    {
        GameObject temp = hovered;
        yield return new WaitForSeconds(0.0f);
        LeanTween.scale(temp, new Vector3(0, 0, 0), 0.5f).setOnComplete(HideStats).setOnCompleteParam(temp);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (hovered)
            StartCoroutine(Exit());
        hovered = null;
    }

    void ShowStats()
    {              
        hovered.SetActive(true);
        LeanTween.cancel(hovered); 
        LeanTween.scale(hovered, new Vector3(1, 1, 1), 0.5f);
    }

    void HideStats(object hovered)
    {
        GameObject i = hovered as GameObject;
        i.SetActive(false);
        hoverTime = 0;
    }

    public void ToggleCanvas(bool toggle)
    {
        GetComponent<Canvas>().enabled = toggle;
    }

    void OpenCombatant(object combatant)
    {
        GameObject goCombatant = combatant as GameObject;
        ScreenManager.instance.OpenCombatantScreen(goCombatant.GetComponentInParent<PartyCombatant>());
        count = 0;
    }

    void OpenParty()
    {
        ScreenManager.instance.OpenPartyScreen();
    }

}
