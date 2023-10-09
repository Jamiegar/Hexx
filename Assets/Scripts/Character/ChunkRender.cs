using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(GridTransform))]
public class ChunkRender : MonoBehaviour, IInitalizable
{
    [SerializeField]
    private int m_renderDistance = 3;

    [Header("Run Time Sets")]
    [SerializeField]
    private ChunkManagerSet m_chunkManagerRunTimeSet;
    private ChunkManager m_chunkManager;

    public void Init()
    {
        m_chunkManager = m_chunkManagerRunTimeSet.GetItemIndex(0); //Set run time set
        if (m_chunkManager == null) //Throw exception if it is null
            throw new System.NullReferenceException("Chunk Manager needs to be set as singleton");
    }

    public void ManageChunkPositions(Chunk newChunk) //Called in Unity event when chunk position is chnaged
    {
        GridPosition playerChunkPos = newChunk.ChunkGridPosition;

        int minColumIndex = playerChunkPos.x - m_renderDistance;
        int maxColumIndex = playerChunkPos.x + m_renderDistance;

        List<Chunk> chunks = m_chunkManager.SpawnedChunks.Values.ToList();

        foreach (Chunk c in chunks)
        {
            if (c.ChunkGridPosition.x < minColumIndex)
            {
                GridPosition gridPos = c.ChunkGridPosition;
                m_chunkManager.MoveChunk(m_chunkManager.GetChunkFromUpdatedPosition(gridPos), 1);
                c.gameObject.SetActive(false);
            }
            else if(c.ChunkGridPosition.x > maxColumIndex)
            {
                GridPosition gridPos = c.ChunkGridPosition;
                m_chunkManager.MoveChunk(m_chunkManager.GetChunkFromUpdatedPosition(gridPos), -1);
                c.gameObject.SetActive(false);
            }
            else
            {
                c.gameObject.SetActive(true);
                continue;
            }
        }
    }
}
