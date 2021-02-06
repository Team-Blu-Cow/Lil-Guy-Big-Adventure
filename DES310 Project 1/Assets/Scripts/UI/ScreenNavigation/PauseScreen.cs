using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    Canvas pauseMenu;
    public InputManager controls;
    GraphicRaycaster[] rayCasters;

    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            pauseMenu = canvas;
            pauseMenu.enabled = false;
        }
        
        controls = new InputManager();
        controls.Keyboard.Pause.performed += ctx => PauseGame();

        rayCasters = transform.parent.GetComponentsInChildren<GraphicRaycaster>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void PauseGame()
    {
        if (!pauseMenu.enabled)
        {
            Time.timeScale = 0f;
            pauseMenu.enabled = true;
            foreach (GraphicRaycaster caster in rayCasters)
            {
                caster.enabled = false;
            }
            GetComponent<GraphicRaycaster>().enabled = true;
        }
        else if (pauseMenu.enabled)
        {
            Time.timeScale = 1f;
            pauseMenu.enabled = false;
            foreach (GraphicRaycaster caster in rayCasters)
            {
                caster.enabled = true;
            }
        }
    }

    public void MainMenu()
    {
        Debug.Log("Main menu");
    }
}
