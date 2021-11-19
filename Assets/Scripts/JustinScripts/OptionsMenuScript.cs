using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine UI;


public class OptionsMenuScript : MonoBehaviour
{
    public Dropdown resolutionDropdown;

    resolution[] resolutions;

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullscreen = isFullscreen;
    }

    void Start ()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string.option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
    }



}
