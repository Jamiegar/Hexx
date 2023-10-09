using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perlin Worms Settings", menuName = "Perlin Noise/Worms Settings")]
public class WormSettings : NoiseSettings
{
    [Range(0.5f, 0.9f)]
    public float weight = 0.6f;

}
