using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    PlayerScript playerScript;

    public SceneAudioManager audioManager;

    public FMOD.Studio.EventInstance rolling;
    public string rollEvent;

    public FMOD.Studio.EventInstance mid_air_rolling;
    public string midAirEvent;

    float RPM;

    public FMOD.Studio.EventInstance landing;
    public string landEvent;
    Vector2 landing_velocity;

    public FMOD.Studio.EventInstance jump;
    public string jumpEvent;

    public FMOD.Studio.EventInstance death;
    public string deathEvent;

    void Awake()
    {
        playerScript = GetComponent<PlayerScript>();

        jump = FMODUnity.RuntimeManager.CreateInstance(jumpEvent);
        death = FMODUnity.RuntimeManager.CreateInstance(deathEvent);
        rolling = FMODUnity.RuntimeManager.CreateInstance(rollEvent);
        mid_air_rolling = FMODUnity.RuntimeManager.CreateInstance(midAirEvent);
        landing = FMODUnity.RuntimeManager.CreateInstance(landEvent);
    }

    public void Death()
    {
        death.setVolume(1 * audioManager.globalSFXVol);
        death.start();
    }
    public void Jump()
    {
        jump.setVolume(1 * audioManager.globalSFXVol);
        jump.start();
    }

    public void Landing()
    {
        landing.setParameterByName("col_velocity", Mathf.Abs(playerScript.prev_velocity.y) * 1.2f);

        landing.setVolume(1 * audioManager.globalSFXVol);
        landing.start();
    }

    //temp
    bool t = false;
    void Update()
    {
        if (playerScript.batteryRemaining <= 0 && !t)
        {
            t = true;
            Death();
        }

        RPM = playerScript.currentSpeed;

        rolling.setParameterByName("RPM_Range", Mathf.Abs(playerScript.rb.velocity.x * 10));
        mid_air_rolling.setParameterByName("RPM_Range", Mathf.Abs(playerScript.rb.angularVelocity * .1f));

        if (playerScript.inAir)
        {
            FMOD.Studio.PLAYBACK_STATE playing;
            rolling.getPlaybackState(out playing);

            rolling.setVolume(.25f * audioManager.globalSFXVol);

            if (playing == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                //rolling.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            mid_air_rolling.getPlaybackState(out playing);

            if (Mathf.Abs(playerScript.rb.angularVelocity) > .25f)
            {
                if (playing != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    float i = 0;
                    mid_air_rolling.getVolume(out i);

                    if (i != 1 * audioManager.globalSFXVol)
                    {
                        i = 1 * audioManager.globalSFXVol;
                    }

                    mid_air_rolling.setVolume(i);
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

            rolling.setVolume(1f * audioManager.globalSFXVol);

            if (playing == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                mid_air_rolling.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            rolling.getPlaybackState(out playing);

            if (Mathf.Abs(playerScript.rb.velocity.x) > .75f)
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


