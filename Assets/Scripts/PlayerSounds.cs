using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public FMOD.Studio.EventInstance rolling;
    public string rollEvent;

    public FMOD.Studio.EventInstance mid_air_rolling;
    public string midAirEvent;

    float RPM;

    public FMOD.Studio.EventInstance landing;
    public string landEvent;
    Vector2 landing_velocity;

    public string jumpEvent;
    PlayerScript playerScript;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();

        rolling = FMODUnity.RuntimeManager.CreateInstance(rollEvent);
        mid_air_rolling = FMODUnity.RuntimeManager.CreateInstance(midAirEvent);
        landing = FMODUnity.RuntimeManager.CreateInstance(landEvent);
    }

    void Update()
    {
        RPM = playerScript.currentSpeed;

        rolling.setParameterByName("RPM_Range", Mathf.Abs(playerScript.rb.velocity.x * 10));
        mid_air_rolling.setParameterByName("RPM_Range", Mathf.Abs(playerScript.rb.velocity.x * 10));

        if (playerScript.inAir)
        {
            FMOD.Studio.PLAYBACK_STATE playing;
            rolling.getPlaybackState(out playing);

            if (playing == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                rolling.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            mid_air_rolling.getPlaybackState(out playing);

            if (Mathf.Abs(playerScript.rb.velocity.x) > 0)
            {
                if (playing != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    mid_air_rolling.start();
                }
            }
            else
            {
                mid_air_rolling.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
        }
        else
        {
            FMOD.Studio.PLAYBACK_STATE playing;
            mid_air_rolling.getPlaybackState(out playing);

            if (playing == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                mid_air_rolling.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            rolling.getPlaybackState(out playing);

            if (Mathf.Abs(playerScript.rb.velocity.x) > 0)
            {
                if (playing != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    rolling.start();
                }
            }
            else
            {
                rolling.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }
}


