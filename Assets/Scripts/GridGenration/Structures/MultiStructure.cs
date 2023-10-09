using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Single Structure", menuName = "Map/Structure Types/Create Multi Structure")]
public class MultiStructure : StructureBase
{
    [Header("Structure Prefabs")]
    public List<GameObject> prefabs;

    public override void SpawnStructurePrefabOnTile(GameObject tile)
    {
        if (m_singleTileStructure)
        {
            SpawnSinglePrefab(tile);
            return;
        }

        SpawnMultiPrefabs(tile);
    }

    private GameObject SelectPrefab()
    {
        int rnd = Random.Range(0, prefabs.Count);
        return prefabs[rnd];
    }

    private void SpawnSinglePrefab(GameObject tile)
    {
        if(m_randomRotation)
        {
            Instantiate(SelectPrefab(),tile.transform.position, GetRandomYRotation(), tile.transform);
            return;
        }
        else
        {
            Instantiate(SelectPrefab(), tile.transform);
            return;
        }

    }

    private void SpawnMultiPrefabs(GameObject tile)
    {
        Vector3[] points = Tile.GetNumberOfSpawnPoints();

        foreach (Vector3 pos in points)
        {
            Vector3 structPos = pos + tile.transform.position;

            if(m_randomRotation)
                Instantiate(SelectPrefab(), structPos, GetRandomYRotation(), tile.transform);
            else
                Instantiate(SelectPrefab(), structPos, Quaternion.identity, tile.transform);
        }
    }
}
