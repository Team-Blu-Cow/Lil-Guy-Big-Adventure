using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Animator buttons;
    public Animator title;

    // Start is called before the first frame update

    private void Start()
    {
        buttons.SetTrigger("UI Trigger Start");
        title.SetTrigger("UI Trigger Title");
    }

    // Update is called once per frame
    private void Update()
    {
    }
}