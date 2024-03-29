using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class SwapScreenParty : MonoBehaviour
{
    [SerializeField] GameObject partyMember;
    PlayerPartyManager party;
    List<GameObject> members = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Set starting to closed position
        party = ScreenManager.instance.partyManager;
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
        for(int i = 0; i <  party.party.Length; i++)
        {
            if (party.party[i])
            {
                GameObject tempPartyMember = Instantiate(partyMember, transform.GetChild(0));
                members.Add(tempPartyMember);
                tempPartyMember.GetComponentInChildren<PartyCombatant>().SetAll(party.party[i]);

                // Setup Text
                tempPartyMember.GetComponentInChildren<TextMeshProUGUI>().text = party.party[i].GetComponent<Combatant>().combatantName;
                tempPartyMember.GetComponentInChildren<TextMeshProUGUI>().font = ScreenManager.instance.activeFont;

                // Setup Image
                tempPartyMember.GetComponentInChildren<Image>().sprite = ScreenManager.GetFirstSprite(party.party[i]);
                tempPartyMember.GetComponentInChildren<Image>().SetNativeSize();

                // Setup button
                tempPartyMember.GetComponent<Button>().onClick.AddListener(() => { ScreenManager.instance.OpenCombatantScreen(tempPartyMember.GetComponentInChildren<PartyCombatant>()); });
                tempPartyMember.GetComponent<Button>().onClick.AddListener(CloseScreen);
            }
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
