using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River : Object
{
    public Dictionary<GridPosition, TileDirectionalInfo> m_riverTiles;

    private PerlinWorm m_perlinWorm;
    private GridManager m_gridManager;
    private bool createRiver = true;

    public River(WormSettings noiseSettings, GridPosition startGridPos, GridPosition convrgancePos, GridManager gridManager)
    {
        m_gridManager = gridManager;
        m_riverTiles = new Dictionary<GridPosition, TileDirectionalInfo>();

        Debug.Log("River Starting At: " + startGridPos);
        m_perlinWorm = new PerlinWorm(noiseSettings, startGridPos, convrgancePos, gridManager); //Construct Perlin Worm
        m_perlinWorm.MoveLength(10);
        
        InitaliseRiverTiles();

        if(createRiver == true)
            PlaceTilesForRiver();
    }

    private void InitaliseRiverTiles()
    {
        foreach(TileDirectionalInfo info in m_perlinWorm.m_tiledPath) //Get the path of Tiles and loop through each tile
        {
            m_riverTiles.Add(info.m_gridPosition, info);

            if (CheckTile(info, out RiverTile neigbourTile)) // if truw then dont create the river as they will cross
            {
                if (neigbourTile != null)
                    createRiver = false;
            } 
        }
    }

    public bool PlaceTilesForRiver()
    {
        GameObject riverTile;
        RiverTile type;

        foreach (TileDirectionalInfo info in m_riverTiles.Values)
        {
            if (info.m_tilesExitDirection.Count == 1) //if the exit directions the tile has is equal to 1 then it is the start or end tile 
            {
                type = RiverManager.GetRiverTileWithExitPoint(info.m_tilesExitDirection[0]);
                riverTile = Instantiate(type.prefab, info.m_worldPos, Quaternion.identity);
            }
            else // if there are more than 1 then is it a tile in the middle of the river
            {
                type = RiverManager.GetRiverTileWithExitPoint(info.m_tilesExitDirection[0], info.m_tilesExitDirection[1]);
                riverTile = Instantiate(type.prefab, info.m_worldPos, Quaternion.identity);
            }

            var meshRenders = riverTile.GetComponentsInChildren<MeshRenderer>();

            foreach(MeshRenderer mesh in meshRenders)
            {
                mesh.material = info.m_biome.BiomeMaterial;
            }

            riverTile.name = "River: " + info.m_gridPosition.ToString();
            m_gridManager.AmendMap(info.m_gridPosition, new TileInfo(info.m_gridPosition, info.m_worldPos, type, riverTile, info.m_chunkPos, info.m_biome));
        }

        return false;
    }

    private bool CheckTile(TileDirectionalInfo info, out RiverTile riverTileNeibour)
    {
        GridSearch gridSearch = new GridSearch();

        var neighbours = gridSearch.GetNeighboursOfTile(info.m_gridPosition);  //Gets neighbours of tile

        foreach(TileInfo tileInfo in neighbours) //loop through neighbours 
        {
            if(tileInfo.m_tileType.GetType() == typeof(RiverTile)) //if a neighbour is of type River Tile
            {
                riverTileNeibour = tileInfo.m_tileType as RiverTile; // Return a tile that tile
                return true;
            }
        }
        riverTileNeibour = null;
        return false;
    }



}
