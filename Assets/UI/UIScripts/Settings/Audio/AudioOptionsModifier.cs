using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptionsModifier : MonoBehaviour, IInitalizable
{
    [SerializeField] private Slider _sfx, _music, _ui;
    [SerializeField] private AudioMixer _sfxMixer, _musicMixer, _uiMixer;

    public void Init()
    {
        _sfx.onValueChanged.AddListener(OnSXFChanged);
        _music.onValueChanged.AddListener(OnMusicChanged);
        _ui.onValueChanged.AddListener(OnUIChanged);

    }


    private void OnSXFChanged(float value)
    {
        _sfxMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    private void OnMusicChanged(float value)
    {
        _musicMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    private void OnUIChanged(float value)
    {
        _uiMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }
}
