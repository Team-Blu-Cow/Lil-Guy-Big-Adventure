using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SwapScreenParty : MonoBehaviour
{
    [SerializeField] GameObject partyMember;
    [SerializeField] GameObject[] party;
    List<GameObject> members = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Set starting to closed position
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.0f);
        GetComponentInParent<Canvas>().enabled = false;
    }

    // Open the screen and set it up
    public void OpenScreen()
    {
        // Enable canvas
        transform.GetComponentInParent<Canvas>().enabled = true;
        foreach (GameObject member in members)
        {
            Destroy(member);
        }

        // Scale in the screen
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.5f);

        // Fade in the black background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.6f), 0.5f);

        // Set what party members appear
        for(int i = 0; i <  party.Length; i++)
        {
            GameObject tempPartyMember = Instantiate(partyMember, transform.GetChild(0));
            members.Add(tempPartyMember);
            tempPartyMember.GetComponentInChildren<PartyCombatant>().SetAll(party[i], party[i].GetComponent<Stats>(), party[i].GetComponent<Combatant>(), "Name");
            tempPartyMember.GetComponentInChildren<TextMeshProUGUI>().text = transform.GetChild(0).GetComponentsInChildren<PartyCombatant>()[i].named;
            tempPartyMember.GetComponentInChildren<Image>().sprite = transform.GetChild(0).GetComponentsInChildren<PartyCombatant>()[i].GetCombatantGO().GetComponent<SpriteRenderer>().sprite;
            tempPartyMember.GetComponentInChildren<Image>().SetNativeSize();
            tempPartyMember.GetComponent<Button>().onClick.AddListener(() => { ScreenManager.screenManager.OpenCombatantScreen(tempPartyMember.GetComponentInChildren<PartyCombatant>()); });
            tempPartyMember.GetComponent<Button>().onClick.AddListener(CloseScreen);

        }
    }

    // Close the whole screen
    public void CloseScreen()
    {
        // Scale out the screen and disable once finished and hide when done
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.5f).setOnComplete(() => GetComponentInParent<Canvas>().enabled = false);

        // Fade out the black background
        Image back = transform.parent.GetComponentInChildren<Image>();
        LeanTween.value(back.gameObject, a => back.color = a, new Color(0, 0, 0, 0.6f), new Color(0, 0, 0, 0), 0.5f);
    }
}
