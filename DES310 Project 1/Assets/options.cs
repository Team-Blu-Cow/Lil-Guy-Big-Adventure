using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class options : MonoBehaviour
{
    public TMPro.TMP_Dropdown resDropdown;

    //public Dropdown resDropdown;
    private List<Resolution> resolutions;

    // Start is called before the first frame update
    private void Start()
    {
        resolutions = new List<Resolution>();
        resDropdown.ClearOptions();

        Resolution[] array = Screen.resolutions;
        List<string> resolutionStrings = new List<string>();
        int curResIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            string option = array[i].width + " x " + array[i].height + ", " + array[i].refreshRate + "hz";

            if (Mathf.Approximately((float)array[i].width / (float)array[i].height, 16f / 9f))
            {
                resolutionStrings.Add(option);
                resolutions.Add(array[i]);
                if (array[i].width == Screen.currentResolution.width && array[i].height == Screen.currentResolution.height)
                {
                    curResIndex = i;
                }
            }
        }
        resDropdown.AddOptions(resolutionStrings);
        resDropdown.value = curResIndex;
        resDropdown.RefreshShownValue();
    }

    public void setResolution(int resolution)
    {
        Resolution res = resolutions[resolution];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
    }

    public void setFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
}