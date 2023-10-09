using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridTransform))]
public class TileType : MonoBehaviour
{
    public Tile m_tile { get; private set; }
    public GridTransform gridTransform { get; private set; }

    public void InitaliseTile(Tile tile, GridPosition gridPosition, GridPosition chunkPosition)
    {
        gridTransform = GetComponent<GridTransform>();
        gridTransform.gridPosition = gridPosition;
        gridTransform.chunkPosition = chunkPosition;

        m_tile = tile;
    }
}
