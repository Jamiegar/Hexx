using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileInfo
{
    public GridPosition m_gridPosition { get; protected set; }
    public Vector3 m_worldPos { get; protected set; }
    public Tile m_tileType { get; protected set; }
    public GameObject m_tileObject { get; protected set; }
    public GridPosition m_chunkPos { get; protected set; }
    public Biome m_biome { get; protected set; }

    public TileInfo(GridPosition gridPos, Vector3 pos, Tile typeOfTile, GameObject go, GridPosition chunkPosition, Biome biome)
    {
        m_gridPosition = gridPos;
        m_worldPos = pos;
        m_tileType = typeOfTile;
        m_tileObject = go;
        m_chunkPos = chunkPosition;
        m_biome = biome;
    }

    public void ClearInfo()
    {
        m_gridPosition = new GridPosition();
        m_worldPos = new Vector3();
        m_tileType = null;
        m_tileObject = null;
    }
}
