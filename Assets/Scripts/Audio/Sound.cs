using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Sound", menuName = "Audio/Create New Audio Sound")]
public class Sound : ScriptableObject
{
    public string Name;
    public AudioClip Clip;

    [Range(0, 1f)]
    public float Volume = 0.8f;
    [Range(0.1f, 3f)]
    public float Pitch = 1f;
    public bool Loop = false;
    [HideInInspector]
    public AudioSource source;
    public AudioMixerGroup MixerGroup;
    public float PitchChange;

    public Sound(AudioClip sClip, float soundVolume, float soundPitch, bool isLooping, AudioMixerGroup mixGroup, float maxPitchChange, string sName)
    {
        Name = sName;
        Clip = sClip;
        Volume = soundVolume;
        Pitch = soundPitch;
        Loop = isLooping;
        MixerGroup = mixGroup;
        PitchChange = maxPitchChange;
    }
}
