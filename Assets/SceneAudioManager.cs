using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudioManager : MonoBehaviour
{
    public FMOD.Studio.EventInstance labBGM;
    public string labBGMEvent;

    float damp = 0.4f;

    [Range(0f, 1f)] 
    [SerializeField]
    public float globalBGMVol;

    [Range(0f, 1f)]
    [SerializeField]
    public float globalSFXVol;

    void WriteFromOptions()
    {
        // 0 - 1
        globalBGMVol = 1;
        globalSFXVol = 1;
    }

    void Start()
    {
        labBGM = FMODUnity.RuntimeManager.CreateInstance(labBGMEvent);
        labBGM.start();

        labBGM.getVolume(out BGMvol);
    }

    float BGMvol = 0;
    //float SFXvol = 0;

    void Update()
    {
        if (BGMvol != 1 * globalBGMVol)
        {
            BGMvol = 1 * globalBGMVol * damp;
            labBGM.setVolume(BGMvol);
        }
    }
}
