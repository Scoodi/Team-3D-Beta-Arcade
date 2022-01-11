using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingConfigurationScript : MonoBehaviour {
    #region Attributes

    #region Player Pref Key Constants

    private const string RESOLUTION_PREF_KEY = "resolution";

    #endregion

    #region Resolution

    [SerializeField]
    private Text resolutionText;
    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;

    #endregion

    #region BGM Audio

    [SerializeField] Slider volumeSlider;

    #endregion


    #endregion

    // Start is called before the first frame update
    void Start()
    { 
        resolutions = Screen.resolutions;

        currentResolutionIndex = PlayerPrefs.GetInt(RESOLUTION_PREF_KEY, 0);

        SetResolutionText(resolutions[currentResolutionIndex]);

        #region BGM Volume

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            load();
        }
        else
        {
            load(); 
        }

        #endregion 
    }

    #region Resolution Cycling

    private void SetResolutionText(Resolution resolution)
    {
        resolutionText.text = resolution.width + "x" + resolution.height;
    }

    public void SetNextResolution()
    {
        currentResolutionIndex = GetNextWrappedIndex(resolutions, currentResolutionIndex);
        SetResolutionText(resolutions[currentResolutionIndex]);
    }

    public void SetPreviousResolution()
    {
        currentResolutionIndex = GetPreviousWrappedIndex(resolutions, currentResolutionIndex);
        SetResolutionText(resolutions[currentResolutionIndex]);
    }


    #endregion

    #region Apply Resolution

    public void SetAndApplyResolution(int newResolutionIndex)
    {
        currentResolutionIndex = newResolutionIndex;
        ApplyCurrentResolution();
    }

    public void ApplyCurrentResolution()
    {
        ApplyResolution(resolutions[currentResolutionIndex]);
    }

    public void ApplyResolution(Resolution resolution)
    {
        SetResolutionText(resolution);

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(RESOLUTION_PREF_KEY, currentResolutionIndex);
    }

    #endregion

    #region Misc Helpers

    #region Index Wrap Helpers
    private int GetNextWrappedIndex<T>(IList<T> collection, int currentIndex)
    {
        if (collection.Count < 1) return 0;
        return (currentIndex + 1) % collection.Count;
    }

    private int GetPreviousWrappedIndex<T>(IList<T> collection, int currentIndex)
    {
        if (collection.Count < 1) return 0;
        if ((currentIndex + 1) < 0) return collection.Count - 1;
        return (currentIndex - 1) % collection.Count;
    }
    #endregion

    #endregion

    #region Apply Changes
    public void ApplyChanges()
    {
        SetAndApplyResolution(currentResolutionIndex);
        load();
    }
    #endregion

    #region Display Mode
    
    public void ToggleFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        if (Screen.fullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }




    #endregion

    #region BGM audio

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        save();
    }

    public void load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    public void save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }









    #endregion

}
