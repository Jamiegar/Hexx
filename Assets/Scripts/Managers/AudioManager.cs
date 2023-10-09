using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using System;


public class AudioManager : MonoBehaviour, IInitalizable
{
    [Tooltip("All Sounds are stored in (Assets/Audio/Audio Clips) during initalisation this array is populated with all sounds")]
    public Sound[] Sounds;

    [SerializeField]
    private AudioMixerGroup m_uiGroup, m_soundEffectGroup;

    [Header("Fade Options")]
    [SerializeField] private float m_fadeTime = 0.2f;
    [SerializeField] private float m_fadeAmount = 0.1f;

    
    [Header("Audio Players")]
    [SerializeField] private GameObject m_audioPlayer;
    [SerializeField] private GameObject m_uiAudioPlayer;

    private GameObject m_audioObject;
    public void Init()
    {
        //Sounds = Resources.FindObjectsOfTypeAll<Sound>();

        m_audioObject = new GameObject("Audio");
        m_audioObject.transform.SetParent(transform);

        foreach(Sound s in Sounds)
        {
            s.source = m_audioObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
            s.source.outputAudioMixerGroup = s.MixerGroup;
        }

        Play("BackgroundMusic");
    }

    public void Play(string name)
    {
        Sound currentSound = Array.Find(Sounds, sound => sound.Name == name);
        Play(currentSound);
    }

    public void Play(Sound sound)
    {
        if (sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogError("Sound of name:" + sound.name + " was not found");
        }
    }

    public void Stop(string name)
    {
        Sound currentSound = Array.Find(Sounds, sound => sound.name == name);
        Stop(currentSound);

    }

    public void Stop(Sound sound)
    {
        if (sound != null)
        {
            sound.source.Stop();
        }
        else
        {
            Debug.Log("Sound of name:" + sound.name + " was not found");
        }
    }

    public void FadeInOutSound(Sound fadeIn, Sound fadeOut)
    {
        StopAllCoroutines();

        StartCoroutine(FadeIn(fadeIn));
        StartCoroutine(FadeOut(fadeOut));
    }

    public void FadeSoundIn(string name)
    {
        Sound currentSound = Array.Find(Sounds, sound => sound.Name == name);
        FadeIn(currentSound);
    }

    public void FadeSoundIn(Sound sound)
    {
        StartCoroutine(FadeIn(sound));
    }

    public void FadeSoundOut(Sound sound)
    {
        StartCoroutine(FadeOut(sound));
    }

    private IEnumerator FadeIn(Sound sound)
    {
        Sound currentSound = sound;

        Play(currentSound);

        float targetVolume = currentSound.Volume;
        currentSound.source.volume = 0;
        float t = 0f;

        while (currentSound.source.volume < 1) //Sets the volume to 0 then lerps the volume to the required sound
        {
            t += m_fadeAmount * Time.deltaTime;
            currentSound.source.volume = Mathf.Lerp(0, targetVolume, t);
            yield return new WaitForSeconds(m_fadeTime);
        }
    }

    private IEnumerator FadeOut(Sound sound)
    {
        Sound currentSound = sound;

        float currentVolume = currentSound.source.volume;
        float t = 0f;

        while (currentSound.source.volume > 0) //Lerps the volume of the sound down
        {
            t += m_fadeAmount * Time.deltaTime;
            currentSound.source.volume = Mathf.Lerp(currentVolume, 0, t);

            yield return new WaitForSeconds(m_fadeTime);
        }
        currentSound.source.Stop();
    }

    




}
