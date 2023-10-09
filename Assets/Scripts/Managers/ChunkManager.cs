using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkManager : MonoBehaviour, IInitalizable
{
    #region Variables
    #region Private
    [Header("Chunk Settings")]
    [SerializeField] private Chunk m_chunkPrefab;
    [SerializeField] private GridPosition m_chunkTileDimentions;

    [Header("Run Time Sets")]
    [SerializeField] private GridManagerSet m_gridMangerSet;

    private Dictionary<GridPosition, Chunk> m_Chunks;
    private GridManager m_gridManager;
    #endregion

    #region Public
    #region Getters
    public Dictionary<GridPosition, Chunk> SpawnedChunks => m_Chunks;
    public GridPosition ChunkDimentions => m_chunkTileDimentions;
    #endregion
    #endregion
    #endregion

    public void Init()
    {
        m_gridManager = m_gridMangerSet.GetItemIndex(0);
        m_Chunks = new Dictionary<GridPosition, Chunk>();
    }

    public Chunk CreateChunk(GridPosition chunkPosition, GridPosition chunkSize, float[,] tileNoiseMap, float[,] biomeNoiseMap)
    {
        Chunk newlyCreatedChunk = Instantiate(m_chunkPrefab);
        newlyCreatedChunk.InitalizeChunk(chunkPosition, chunkSize, CalcChunkWorldPosition(chunkPosition), tileNoiseMap, biomeNoiseMap);
        
        m_Chunks.Add(chunkPosition, newlyCreatedChunk);

        return newlyCreatedChunk;
    }

    public Vector3 CalcChunkWorldPosition(GridPosition gridPosition)
    {
        Vector2 tileDim = Tile.TileDimention;

        float offset = 0;
        if (gridPosition.y % 2 != 0)
            offset = tileDim.x / 2;

        float x = m_chunkTileDimentions.x * tileDim.x * gridPosition.x - offset;
        float z = (-m_chunkTileDimentions.y * tileDim.y * gridPosition.y) * 0.75f;

        return new Vector3(x, 0, z);
    }

    public void MoveChunk(Chunk movingChunk, int offset)
    {
        GridPosition newGridPosition = movingChunk.ChunkGridPosition;
        Vector3 newChunkPos = movingChunk.transform.position;

        if (offset < 0) 
        {
            newGridPosition = new GridPosition(movingChunk.ChunkGridPosition.x, movingChunk.ChunkGridPosition.y);
            newChunkPos = CalcChunkWorldPosition(newGridPosition);

            float x = (m_gridManager.GridDimention.x * m_chunkTileDimentions.x) * Tile.TileDimention.x;

            newChunkPos += new Vector3(-x, 0, 0);
            newGridPosition = new GridPosition(movingChunk.ChunkGridPosition.x - m_gridManager.GridDimention.x, movingChunk.ChunkGridPosition.y);
        }
        else if(offset > 0)
        {
            newGridPosition = new GridPosition(movingChunk.ChunkGridPosition.x, movingChunk.ChunkGridPosition.y);
            newChunkPos = CalcChunkWorldPosition(newGridPosition);

            float x = (m_gridManager.GridDimention.x * m_chunkTileDimentions.x) * Tile.TileDimention.x;

            newChunkPos += new Vector3(x, 0, 0);
            newGridPosition = new GridPosition(movingChunk.ChunkGridPosition.x + m_gridManager.GridDimention.x, movingChunk.ChunkGridPosition.y);
            
        }

        ChangeChunkGridPosition(movingChunk.ChunkGridPosition, newGridPosition);
        movingChunk.transform.position = newChunkPos;
    }

    public Chunk GetChunkFromUpdatedPosition(GridPosition position)
    {
        if(m_Chunks.TryGetValue(position, out Chunk chunk ))
        {
            return chunk;
        }

        Debug.LogError("No Chunk");
        return null;
    }

    private void ChangeChunkGridPosition(GridPosition oldGridPosition, GridPosition newGridPosition)
    {
        if(m_Chunks.TryGetValue(oldGridPosition, out Chunk currentChunk))
        {
            currentChunk.ChunkGridPosition = newGridPosition;
            m_Chunks.Remove(oldGridPosition);
            m_Chunks.Add(newGridPosition, currentChunk);
        }
    }

}
