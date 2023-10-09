using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridTransform : MonoBehaviour
{
    public GridPosition gridPosition;
    public GridPosition chunkPosition;

    public UnityEvent<TileType> OnPositionChange;
    public UnityEvent<Chunk> OnChunkPositionChange;

    public GridTransform()
    {
        gridPosition = new GridPosition();
        chunkPosition = new GridPosition();
    }

    public GridTransform(GridPosition gridPos, GridPosition chunkPos)
    {
        gridPosition = gridPos;
        chunkPosition = chunkPos; 
    }
}
