using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBase : ScriptableObject
{
    [Header("Noise Settings")]
    public float m_noiseValue;
    [Header("Frequency")]
    [Range(0, 1)]
    public float percentageChanceOfSpawn;
    public bool m_singleTileStructure;
    public bool m_replaceTile;
    public bool m_randomRotation = false;

    public bool CanPlaceStructure()
    {
        if (Random.value < percentageChanceOfSpawn)
        {
            return true;
        }
        return false;
    }

    public Quaternion GetRandomYRotation()
    {
        Quaternion rot = Quaternion.Euler(0, Random.rotation.eulerAngles.y, 0);
        return rot;
    }

    public abstract void SpawnStructurePrefabOnTile(GameObject tile);


}
