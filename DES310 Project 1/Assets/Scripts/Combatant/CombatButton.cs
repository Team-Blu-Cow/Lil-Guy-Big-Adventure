using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CombatButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateButton(Vector3 offset, Vector3 combatantPosition)
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().anchoredPosition = combatantPosition + offset;
    }

    public void deactivateButton()
    {
        gameObject.SetActive(false);
    }
}
