using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TileDirectionalInfo : TileInfo
{
    public List<ExitPoint> m_tilesExitDirection = new List<ExitPoint>();

    public TileDirectionalInfo(GridPosition gridPos, Vector3 pos, Tile typeOfTile, GameObject go, GridPosition chunkPosition, Biome biome, List<ExitPoint> exitPoints) 
        : base(gridPos, pos, typeOfTile, go, chunkPosition, biome)
    {
        m_tilesExitDirection = exitPoints;
        
    }

    public TileDirectionalInfo(TileInfo tileInfo, List<ExitPoint> exitPoints) 
        :base(tileInfo.m_gridPosition, tileInfo.m_worldPos, tileInfo.m_tileType, tileInfo.m_tileObject, tileInfo.m_chunkPos, tileInfo.m_biome)
    {
        m_tilesExitDirection = exitPoints;
        
    }
    
    public TileDirectionalInfo(TileInfo tileInfo) 
        :base(tileInfo.m_gridPosition, tileInfo.m_worldPos, tileInfo.m_tileType, tileInfo.m_tileObject, tileInfo.m_chunkPos, tileInfo.m_biome)
    {
        
    }

    public ExitPoint GetOppositeDirection(ExitPoint direction) //Returns what the opposite direction is to the parameter
    {
        switch(direction)
        {
            case ExitPoint.None:
                return ExitPoint.None;

            case ExitPoint.NorthEast:
                return ExitPoint.SouthWest;

            case ExitPoint.East:
                return ExitPoint.West;

            case ExitPoint.SouthEast:
                return ExitPoint.NorthWest;

            case ExitPoint.SouthWest:
                return ExitPoint.NorthEast;

            case ExitPoint.West:
                return ExitPoint.East;

            case ExitPoint.NorthWest:
                return ExitPoint.SouthEast;
        }
        return ExitPoint.None;
    }



}
