using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneAudioManager : MonoBehaviour
{
    public AudioOptionsScriptableObject audioOptions;

    public FMOD.Studio.EventInstance labBGM;
    public string labBGMEvent;

    public FMOD.Studio.EventInstance mainMenuBGM;
    public string mainMenuBGMEvent;

    public FMOD.Studio.EventInstance currentBGM;
    public string currentBGMEvent;

    public PlayerScript player;
    public WorldGenScript world;
    string biome;

    float damp = 0.64f;

    //e
    public bool checkingBiome = false;
    public GameObject forestBG, beachBG; 

    [Range(0f, 1f)]
    [SerializeField]
    public float globalBGMVol;

    [Range(0f, 1f)]
    [SerializeField]
    public float globalSFXVol;

    public Slider bgm_slider;
    public Slider sfx_slider;

    public void ApplyChanges()
    {
        audioOptions.background_vol = bgm_slider.value;
        audioOptions.sfx_vol = sfx_slider.value;
    }

    void WriteFromOptions()
    {
        // 0 - 1
        globalBGMVol = audioOptions.background_vol;
        globalSFXVol = audioOptions.sfx_vol;
    }

    public string scene;
    float BGMvol = 0;
    //float SFXvol = 0;

    void Start()
    {
        if (bgm_slider || sfx_slider)
        {
            bgm_slider.value = audioOptions.background_vol;
            sfx_slider.value = audioOptions.sfx_vol;
        }

        if (scene.Contains("Tutorial"))
        {
            currentBGM = labBGM;
            currentBGMEvent = labBGMEvent;
        }
        else if (scene.Contains("MainMenu"))
        {
            currentBGM = mainMenuBGM;
            currentBGMEvent = mainMenuBGMEvent;
        }
        else
        {
            return;
        }

        FMOD.Studio.Bus bus;
        bus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        currentBGM = FMODUnity.RuntimeManager.CreateInstance(currentBGMEvent);
        currentBGM.getVolume(out BGMvol);
        currentBGM.start();
    }

    void Update()
    {
        if (globalBGMVol != audioOptions.background_vol || globalSFXVol != audioOptions.sfx_vol)
        {
            WriteFromOptions();
        }
        if (BGMvol != 1 * globalBGMVol * damp)
        {
            BGMvol = 1 * globalBGMVol * damp;
            currentBGM.setVolume(BGMvol);
        }
        if (bgm_slider || sfx_slider)
        {
            if (globalBGMVol != bgm_slider.value || globalSFXVol != sfx_slider.value)
            {
                ApplyChanges();
            }
        }

        if (checkingBiome)
        {
            if (biome != player.currentBiome)
            {
                biome = player.currentBiome;

                FMOD.Studio.Bus bus;
                bus = FMODUnity.RuntimeManager.GetBus("Bus:/");
                bus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

                switch (biome)
                {
                    case "beach":
                        currentBGM = FMODUnity.RuntimeManager.CreateInstance(world.biomes[0].biomeBGMEvent);
                        forestBG.SetActive(false);
                        beachBG.SetActive(true);
                        break;
                    case "forest":
                        currentBGM = FMODUnity.RuntimeManager.CreateInstance(world.biomes[1].biomeBGMEvent);
                        forestBG.SetActive(true);
                        beachBG.SetActive(false);
                        break;

                    default:
                        break;
                }

                currentBGM.getVolume(out BGMvol);
                currentBGM.start();
            }
        }
    }
}
