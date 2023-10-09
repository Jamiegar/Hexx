using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour, IInitalizable
{
    #region Variables 
    #region Private
    [Header("Grid Settings")]
    [SerializeField] private GridPosition m_chunksOnGridDimentions = new GridPosition(11, 11);

    [Header("Noise")]
    [SerializeField] private NoiseSettings m_tileNoise;
    [SerializeField] private NoiseSettings m_biomeNoise;

    [Header("Tile Types")]
    public Biome[] BiomeData;

    [Header("Editor Settings")]
    public bool AutoUpdate;

    public float[,] m_tileNoiseMap { get; private set; }
    public float[,] m_biomeNoiseMap { get; private set; }
    private Dictionary<GridPosition, Chunk> m_chunks = new Dictionary<GridPosition, Chunk>();
    private Dictionary<GridPosition, TileInfo> m_tilesOnGrid;

    [Header("Run Time Sets")]
    [SerializeField]
    private ChunkManagerSet m_ChunkManagerRunTimeSet;
    private ChunkManager m_chunkManager;
    [SerializeField]
    private RiverManagerSet m_riverMangerSetTimeSet;
    private RiverManager m_riverManager;
    
    #endregion

    #region Public
    public static TileInfo centreTile { get; private set; }
    public static GridPosition TotalMapDimentions { get; private set; }

    #region Getters
    public GridPosition GridDimention { get => m_chunksOnGridDimentions; }
    #endregion
    #endregion
    #endregion

    #region Grid Setup
    public void Init()
    {
        m_chunkManager = m_ChunkManagerRunTimeSet.GetItemIndex(0);
        m_riverManager = m_riverMangerSetTimeSet.GetItemIndex(0);
        TotalMapDimentions = m_chunksOnGridDimentions * m_chunkManager.ChunkDimentions;
    }

    public void GenrateWorldTileGrid()
    {
        //Create dictionary to store tile information at grid location
        m_tilesOnGrid = new Dictionary<GridPosition, TileInfo>();
        
        //Generates Perlin Noise Maps
        CreatePerlinNoiseMaps();

        for (int x = 0; x < m_chunksOnGridDimentions.x; x++) 
        {
            for (int y = 0; y < m_chunksOnGridDimentions.y; y++)
            {
                //Create new chunk setting the chunk position, size of chunk, tile noise map and biome noise map
                Chunk createdChunk = m_chunkManager.CreateChunk(new GridPosition(x, y), m_chunkManager.ChunkDimentions, m_tileNoiseMap, m_biomeNoiseMap);
                m_chunks.Add(new GridPosition(x, y), createdChunk);
                createdChunk.gameObject.transform.SetParent(transform);
            }
        }

        centreTile = GetCentreOfMap();
    }

    public void GetRiverPoints(out List<GridPosition> riverStarts, out List<GridPosition> riverEnd)
    {
        riverStarts = PerlinNoise.GetLocalHighest(m_tileNoiseMap);
        riverEnd = PerlinNoise.GetLocalLowest(m_tileNoiseMap);
    }

    public void ClearGrid()
    {
        foreach (Chunk c in m_chunks.Values)
        {
            c.DestroyChunk();
        }
        m_tilesOnGrid.Clear();
        m_chunks.Clear();
    }
    

    #region Perlin Noise
    private void CreatePerlinNoiseMaps()
    {
        GridPosition dimentions = 
            new GridPosition(m_chunksOnGridDimentions.x * m_chunkManager.ChunkDimentions.x, m_chunksOnGridDimentions.y * m_chunkManager.ChunkDimentions.y);

        m_tileNoiseMap = PerlinNoise.GenrateNoiseMap(dimentions, m_tileNoise);
        m_biomeNoiseMap = PerlinNoise.GenrateNoiseMap(dimentions, m_biomeNoise);
        PerlinNoise.MakeSeamlessHorizontally(m_biomeNoiseMap, 5);

    }
    #endregion
    #endregion

    #region Getters and Setters

    #region Public
    public GridPosition GetGridDimentions()
    {
        return m_chunksOnGridDimentions;
    }

    public Biome[] GetBiomes()
    {
        return BiomeData;
    }

    public void AddToTile(GridPosition pos, TileInfo obj)
    {
        m_tilesOnGrid.Add(pos, obj);
    }
    
    public TileInfo GetTileAtGridPosition(GridPosition gridPosition)
    {
        if(m_tilesOnGrid.TryGetValue(gridPosition, out TileInfo tile) == true)
        {
            return tile;
        }
        else
        {
            Debug.LogError("Did not find tile: " + gridPosition.ToString());
            return null;
        }
    }

    public void AmendMap(GridPosition gridPosition, TileInfo newTileInfo)
    {
        TileInfo oldTileInfo = GetTileAtGridPosition(gridPosition);
        DestroyImmediate(oldTileInfo.m_tileObject);
        oldTileInfo.ClearInfo();

        m_tilesOnGrid.Remove(gridPosition);
        m_tilesOnGrid.Add(gridPosition, newTileInfo);

        Chunk chunk = GetChunkAtChunkPosition(newTileInfo.m_chunkPos);

        newTileInfo.m_tileObject.transform.parent = chunk.transform;
    }
    
    public Chunk GetChunkAtChunkPosition(GridPosition chunkPos)
    {
        if (m_chunks.TryGetValue(chunkPos, out Chunk chunk) == true)
        {
            return chunk;
        }
        else
        {
            Debug.LogError("Did not find chunk: " + chunkPos.ToString());
            return null;
        }
    }
    #endregion

    #region Private
    private TileInfo GetCentreOfMap()
    {
        GridPosition centre = TotalMapDimentions / 2;

        return GetTileAtGridPosition(centre);
    }


    #endregion
    #endregion
}
