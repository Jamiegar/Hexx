using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "Map/Create Biome")]
public class Biome : ScriptableObject
{
    [SerializeField] private Tile[] tilesInBiome;
    [SerializeField] private StructureBase[] structures;
    public float noiseValue;
    [Tooltip("The lower the value the less height variation, but this biome terrain will still blend between other biomes")]
    public float biomeHeight = 0.5f;
    public Material BiomeMaterial;

    public Tile GetTileFromValue(float pelinNoiseValue)
    {
        Tile tempTile = tilesInBiome[0];

        for (int i = 0; i < tilesInBiome.Length; i++)
        {
            if (pelinNoiseValue <= tilesInBiome[i].height)
            {
                tempTile = tilesInBiome[i];
            }
        }

        return tempTile;
    }

    public StructureBase GetStructureFromValue(float pelinNoiseValue)
    {
        StructureBase tempStruct = structures[0];

        if (tempStruct == null)
            return null;

        for (int i = 0; i < structures.Length; i++)
        {
            if (pelinNoiseValue <= structures[i].m_noiseValue)
            {
                tempStruct = structures[i];
            }
        }

        return tempStruct;
    }

    
}
