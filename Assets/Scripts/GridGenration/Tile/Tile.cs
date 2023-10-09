using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


[CreateAssetMenu(fileName = "New Tile Type", menuName = "Map/Create New Tile")]
public class Tile : ScriptableObject
{
    private string tileName;
    public GameObject prefab;
    public float height;
    public bool canHaveStructures;
    public Sound AudioOnTile;
    public static Vector2 TileDimention { get => m_dimentions; }
    private static Vector2 m_dimentions;

    public void WorkOutTileDimentions()
    {
        //Work out tile dimensions here
        Vector3 tileSize = prefab.GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
        m_dimentions.x = tileSize.z;
        m_dimentions.y = tileSize.x;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        tileName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
    }
#endif

    public static Vector3[] GetNumberOfSpawnPoints()
    {
        List<Vector3> tempList = new List<Vector3>();
        int value = Random.Range(1, 10);

        for (int i = 0; i < value; i++)
        {
            tempList.Add(new Vector3(Random.Range(0f, 2.5f), 0, Random.Range(0f, 2f)));
        }

        return tempList.ToArray();
    }

    private void OnEnable()
    {
        if(m_dimentions == Vector2.zero)
            WorkOutTileDimentions();
    }

}
