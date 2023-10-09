using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerlinWormOptionsModifier : PerlinNoiseOptionsModifier
{
    [SerializeField] private Slider _weight;

    [Header("River Chance")]
    [SerializeField] private Slider _percentageChance;


    private WormSettings wormSettings;

    public override void Init()
    {
        base.Init();
        wormSettings = _noiseSettings as WormSettings;
        _weight.value = wormSettings.weight;
        _percentageChance.value = MapData.instance.ChanceOfSpawningRiver;

        _percentageChance.onValueChanged.AddListener(OnChanceOfSpawningChanged);
        _weight.onValueChanged.AddListener(OnWeightChanged);
    }

    private void OnWeightChanged(float value)
    {
        wormSettings.weight = value;
    }

    private void OnChanceOfSpawningChanged(float value)
    {
        MapData.instance.ChanceOfSpawningRiver = value;
    }

}
