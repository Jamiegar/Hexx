using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTexture : MonoBehaviour
{
    private MeshRenderer renderer;
    [SerializeField] private NoiseSettings settings;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = PerlinNoise.GenerateTexture(new GridPosition(50, 50), settings);
    }


}
