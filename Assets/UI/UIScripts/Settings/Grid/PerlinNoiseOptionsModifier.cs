using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerlinNoiseOptionsModifier : MonoBehaviour, IInitalizable
{
    [Header("Noise")]
    [SerializeField] protected NoiseSettings _noiseSettings;

    [Header("UI")]
    [SerializeField] private Slider _scale;
    [SerializeField] private Slider _octaves;
    [SerializeField] private Slider _persistance;
    [SerializeField] private Slider _lacunarity;
    [SerializeField] private TMP_InputField _seed, _offsetX, _offsetY;

    private void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        new UI_InputWindow(_seed, "1234567890");
        new UI_InputWindow(_offsetX, "1234567890");
        new UI_InputWindow(_offsetY, "1234567890");

        _scale.value = _noiseSettings.noiseScale;
        _octaves.value = _noiseSettings.octaves;
        _persistance.value = _noiseSettings.persistance;
        _lacunarity.value = _noiseSettings.lacunarity;
        _seed.text = _noiseSettings.seed.ToString();
        _offsetX.text = _noiseSettings.offset.x.ToString();
        _offsetY.text = _noiseSettings.offset.y.ToString();

        _scale.onValueChanged.AddListener(OnScaleValueChanged);
        _octaves.onValueChanged.AddListener(OnOctavesValueChanged);
        _persistance.onValueChanged.AddListener(OnPersistanceValueChanged);
        _lacunarity.onValueChanged.AddListener(OnLacunarityValueChanged);

        RegenerateScene.OnRegeneration += OnMapRegenerated;
    }

    private void OnScaleValueChanged(float value)
    {
        _noiseSettings.noiseScale = value;
    }

    private void OnOctavesValueChanged(float value)
    {
        _noiseSettings.octaves = Mathf.RoundToInt(value);
    }

    private void OnPersistanceValueChanged(float value)
    {
        _noiseSettings.persistance = value;
    }

    private void OnLacunarityValueChanged(float value)
    {
        _noiseSettings.lacunarity = value;
    }

    public void OnMapRegenerated()
    {
        _noiseSettings.seed = int.Parse(_seed.text);
        _noiseSettings.offset.x = float.Parse(_offsetX.text);
        _noiseSettings.offset.y = float.Parse(_offsetY.text);
    }

}
