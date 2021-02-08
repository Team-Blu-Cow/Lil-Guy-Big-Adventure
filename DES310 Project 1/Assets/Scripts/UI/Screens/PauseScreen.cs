using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    Canvas pauseMenu;
    GraphicRaycaster[] rayCasters;

    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            pauseMenu = canvas;
            pauseMenu.enabled = false;
        }

        rayCasters = transform.parent.GetComponentsInChildren<GraphicRaycaster>();
    }

    public void TogglePauseGame(bool toggle)
    {
        if (toggle)
        {
            Time.timeScale = 0f;
            pauseMenu.enabled = true;
            foreach (GraphicRaycaster caster in rayCasters)
            {
                caster.enabled = false;
            }
            GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.enabled = false;
            foreach (GraphicRaycaster caster in rayCasters)
            {
                caster.enabled = true;
            }
        }
    }
}
