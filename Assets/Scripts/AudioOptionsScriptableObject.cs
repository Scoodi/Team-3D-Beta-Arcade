using UnityEngine;

[CreateAssetMenu(fileName = "AudioOptions", menuName = "ScriptableObjects/AudioOptionsScriptableObject", order = 1)]
public class AudioOptionsScriptableObject : ScriptableObject
{
    public float background_vol;
    public float sfx_vol;


}