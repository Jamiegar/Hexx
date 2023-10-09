using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    private GridPosition m_chunkSize;
    private TileGrid m_grid;
    
    [SerializeField]
    private GridManagerSet m_gridManagerSet;
    private GridManager m_gridManager;

    private GridPosition m_chunkGridPosition;
    public GridPosition ChunkGridPosition 
    { 
        get => m_chunkGridPosition; 
        set => m_chunkGridPosition = value;
    }
    public TileGrid ChunkGrid { get => m_grid; }

    public List<GameObject> ChunkTiles { get; private set; }

    public void InitalizeChunk(GridPosition chunkPosition, GridPosition chunkDimentions, Vector3 chunkWorldPosition, float[,] tileNoiseMap, float[,] biomeNoiseMap)
    {
        #region Setting Variables
        m_chunkGridPosition = chunkPosition;
        m_chunkSize = chunkDimentions;

        //Rename the chunk GameObject to its position
        gameObject.name = "Chunk" + "(" + chunkPosition.x + "," + chunkPosition.y + ")";

        //Set variable for grid manager
        m_gridManager = m_gridManagerSet.GetItemIndex(0);
        #endregion

        ChunkTiles = new List<GameObject>();

        transform.position = chunkWorldPosition;

        m_grid = new TileGrid(m_chunkSize, 0, transform.position, Tile.TileDimention);

        for (int x = 0; x < m_chunkSize.x; x++)
        {
            for (int y = 0; y < m_chunkSize.y; y++)
            {
                //Get the location for the tile on the grid
                GridPosition tileGridPos = SetTileCoordinate(chunkPosition, new GridPosition(x, y)); 

                //Gets the biome type from the biome noise map
                Biome b = GetBiomeFromValue(biomeNoiseMap[tileGridPos.x, tileGridPos.y]);                
                
                //From the biome get the tile that should be spawned from the noise value
                Tile t = b.GetTileFromValue(tileNoiseMap[tileGridPos.x, tileGridPos.y]);
                
                //Calaculate the tiles grid position
                Vector3 tilePos = CalcTileWorldPos(new GridPosition(x, y), tileGridPos, tileNoiseMap, biomeNoiseMap, b);

                //Spawns tile
                GameObject tile = Instantiate(t.prefab, tilePos, Quaternion.Euler(0, 90, 0), transform); //Spawn the tile

                tile.name = tileGridPos.ToString(); //Set the name of the tile to its grid position
                TileType componentOnTile = tile.AddComponent<TileType>();
                componentOnTile.InitaliseTile(t, tileGridPos, chunkPosition);
                ChunkTiles.Add(tile); //Add tile to list of all the tiles in chunk

                AddTileToGrid(tileGridPos, tilePos, t, tile, b);

                if (t.canHaveStructures == false)
                    continue;

                StructureBase s = b.GetStructureFromValue(tileNoiseMap[tileGridPos.x, tileGridPos.y]);
                HandleStructures(s, tile);
            }
        }
    }

    public void DestroyChunk()
    {
        DestroyImmediate(gameObject);
        DestroyImmediate(this);
    }

    private void HandleStructures(StructureBase s, GameObject tileToSpawnOn)
    {
        if (s == null)
            return;

        if (s.CanPlaceStructure() == false)
            return;

        s.SpawnStructurePrefabOnTile(tileToSpawnOn);
    }


    private GridPosition SetTileCoordinate(GridPosition chunkPos, GridPosition loopCoord)
    {
        GridPosition tilePos = new GridPosition(); 

        tilePos.x = loopCoord.x + chunkPos.x + (chunkPos.x * (m_chunkSize.x - 1)); // -1 because the loop starts at 0
        tilePos.y = loopCoord.y + chunkPos.y + (chunkPos.y * (m_chunkSize.y - 1));

        return tilePos;  
    }
    
    private Biome GetBiomeFromValue(float value)
    {
        Biome[] biomes = m_gridManager.GetBiomes();

        Biome tempBiome = biomes[0];
        float closetVal = Mathf.Abs(value - tempBiome.noiseValue);

        foreach(Biome biome in biomes)
        {
            float tempValue = Mathf.Abs(value - biome.noiseValue);

            if (tempValue < closetVal)
            {
                tempBiome = biome;
                closetVal = tempValue;
            }
        }

        return tempBiome;
    }

    private Vector3 CalcTileWorldPos(GridPosition loopValue, GridPosition gridPosition, float[,] tileNoise, float[,] biomeNoise, Biome biome)
    {
        Vector3 pos = m_grid.CalcWorldPositionOnGrid(loopValue); //Get the world location for the tile to spawn

        float sumOfHeights = 0f;
        for (int i = 0; i < m_gridManager.GetBiomes().Length; i++)
        {
            float weightBiome = biomeNoise[gridPosition.x, gridPosition.y];

            sumOfHeights += (tileNoise[gridPosition.x, gridPosition.y] * weightBiome) * m_gridManager.GetBiomes()[i].biomeHeight;
        }

        float weight = biomeNoise[gridPosition.x, gridPosition.y];
        float height = (tileNoise[gridPosition.x, gridPosition.y] * weight * sumOfHeights) * biome.biomeHeight;

        pos.y = Mathf.FloorToInt(height);

        return pos;
    }

    private void AddTileToGrid(GridPosition gridPosition, Vector3 worldPos, Tile tileType, GameObject tileGameObject, Biome biome)
    {
        TileInfo info = new TileInfo(gridPosition, worldPos, tileType, tileGameObject, m_chunkGridPosition, biome);
        m_gridManager.AddToTile(gridPosition, info);
    }

}
