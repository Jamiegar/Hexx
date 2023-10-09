using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class MapData : PersistentSingleton<MapData>
{
    [Header("Grid Settings")]
    public NoiseSettings TileNoiseSettings;
    public NoiseSettings BiomeNoiseSettings;

    [Header("River Settings")]
    public WormSettings RiverNoiseSettings;
    [Range(0, 1)] public float ChanceOfSpawningRiver;



}
