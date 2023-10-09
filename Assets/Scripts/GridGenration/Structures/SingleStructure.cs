using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Single Structure", menuName = "Map/Structure Types/Create Single Structure")]
public class SingleStructure : StructureBase
{
    [Header("Structure Prefab")]
    public GameObject prefab;

    public override void SpawnStructurePrefabOnTile(GameObject tile)
    {
        if(m_singleTileStructure)
        {
            if(m_randomRotation)
            {
                Instantiate(prefab,tile.transform.position, GetRandomYRotation(), tile.transform);
            }

            Instantiate(prefab, tile.transform);
            return;
        }

        Vector3[] points = Tile.GetNumberOfSpawnPoints();

        foreach (Vector3 pos in points)
        {
            Vector3 structPos = pos + tile.transform.position;

            if (m_randomRotation)
            {
                Instantiate(prefab, structPos, GetRandomYRotation(), tile.transform);
                continue;
            }
            
            Instantiate(prefab, structPos, Quaternion.identity, tile.transform);
        }

    }

}
