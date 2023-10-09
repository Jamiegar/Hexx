using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSearch
{
    private static List<TileInfo> m_neighbours = new List<TileInfo>();
    private static GridManager m_gridManager;
    private static ChunkManager m_chunkManager;

    public GridSearch()
    {
        GameManager gameManager = GameManager.instance;
        m_gridManager = gameManager.GridManager;
        m_chunkManager = gameManager.ChunkManager;
    }

    public GridSearch(GridManager gridManager, ChunkManager chunkManager)
    {
        m_gridManager = gridManager;
        m_chunkManager = chunkManager;
    }

    public List<TileInfo> GetNeighboursOfTile(GridPosition currentTile)
    {
        m_neighbours.Clear(); //clear list each time the function is ran so that neigbour list does not have tiles from last search 

        var exitPoints = Enum.GetValues(typeof(ExitPoint));

        foreach(ExitPoint ep in exitPoints)
        {
            if (ep == ExitPoint.None)
                continue;

            TileInfo neighbour = GetTileFromDirection(ep, currentTile);
            if (neighbour != null)
                FindTileAndAddToNeighbours(neighbour.m_gridPosition);
        }

        return m_neighbours;
    }

    /// <summary>
    /// Returns all the neigbours of current tile execpt the direction 
    /// </summary>
    /// <param name="currentTile"> Tile to get neighbours </param>
    /// <param name="direction">Direction to not add to neighbours </param>
    /// <returns>Returns neighbours of tile</returns>
    public List<TileInfo> GetNeighboursOfTile(GridPosition currentTile, ExitPoint direction)
    {
        m_neighbours.Clear(); //clear list each time the function is ran so that neigbour list does not have tiles from last search 

        var exitPoints = Enum.GetValues(typeof(ExitPoint));

        foreach (ExitPoint ep in exitPoints)
        {
            if (ep == ExitPoint.None || ep == direction)
                continue;

            TileInfo neighbour = GetTileFromDirection(ep, currentTile);
            if (neighbour != null)
                FindTileAndAddToNeighbours(neighbour.m_gridPosition);
        }

        return m_neighbours;
    }

    public TileInfo GetTileFromDirection(ExitPoint direction, GridPosition currentPosition)
    {
        TileInfo currentTileInfo = m_gridManager.GetTileAtGridPosition(currentPosition);
        TileInfo neigbourTileInDirection;

        #region Even Chunk
        if (currentTileInfo.m_chunkPos.y % 2 == 0)
        {
            neigbourTileInDirection = GetEvenChunkTile(direction, currentTileInfo);
        }
        #endregion
        #region Odd Chunk
        else
        {
            neigbourTileInDirection = GetOddChunkTile(direction, currentTileInfo);
        }
        #endregion

        if (neigbourTileInDirection == null)
            return null;

        if (neigbourTileInDirection.m_chunkPos == currentTileInfo.m_chunkPos)
            return neigbourTileInDirection;
        

        if (neigbourTileInDirection.m_chunkPos.y < currentTileInfo.m_chunkPos.y)
        {
            if (currentTileInfo.m_chunkPos.y % 2 == 0)
            {
                //Do calc for odd
                neigbourTileInDirection = GetOddChunkTile(direction, currentTileInfo);
            }
            else if (currentTileInfo.m_chunkPos.y % 2 != 0)
            {
                if (currentTileInfo.m_gridPosition.y % 2 != 0)
                {
                    //Do calc for Even
                    neigbourTileInDirection = GetOddChunkTile(direction, currentTileInfo);
                }
                else
                {
                    neigbourTileInDirection = GetEvenChunkTile(direction, currentTileInfo);
                }
            }
        }
        
        return neigbourTileInDirection;
    }

    private TileInfo GetOddChunkTile(ExitPoint direction, TileInfo currentTileInfo)
    {
        GridPosition currentPosition = currentTileInfo.m_gridPosition;
        TileInfo foundTileInfo;

        #region Even Tile
        if (currentPosition.y % 2 == 0)
        {
            foundTileInfo = GetOddChunkEvenTile(direction, currentPosition);
        }
        #endregion
        #region Odd Tile
        else if (currentTileInfo.m_chunkPos.y % 2 != 0)
        {
            foundTileInfo = GetOddChunkOddTile(direction, currentPosition);
        }
        #endregion
        else
        {
            Debug.LogError(currentTileInfo.m_chunkPos + " is not Odd");
            return null;
        }
        return foundTileInfo;
    }
    
    private TileInfo GetEvenChunkTile(ExitPoint direction, TileInfo currentTileInfo)
    {
        GridPosition currentPosition = currentTileInfo.m_gridPosition;
        TileInfo foundTileInfo;

        #region Even Tile
        if (currentPosition.y % 2 == 0)
        {
            foundTileInfo = GetEvenChunkEvenTile(direction, currentPosition);
        }
        #endregion
        #region Odd Tile
        else if(currentPosition.y % 2 != 0)
        {
            foundTileInfo = GetEvenChunkOddTile(direction, currentPosition);
        }
        #endregion
        else
        {
            Debug.LogError(currentTileInfo.m_chunkPos + " is not Even");
            return null;
        }
        return foundTileInfo;
    }

    private TileInfo GetOddChunkOddTile(ExitPoint direction, GridPosition currentPosition)
    {
        TileInfo returnedTile = null;
        #region Odd Tile
        switch (direction)
        {
            case ExitPoint.NorthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y + 1));
                break;

            case ExitPoint.NorthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y + 1));
                break;

            case ExitPoint.SouthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y - 1));
                break;

            case ExitPoint.SouthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y - 1));
                break;

            case ExitPoint.West:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y));
                break;

            case ExitPoint.East:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y));
                break;

            case ExitPoint.None:
                break;
        }
        #endregion
        return returnedTile;
    }

    private TileInfo GetOddChunkEvenTile(ExitPoint direction, GridPosition currentPosition)
    {
        TileInfo returnedTile = null;
        #region Even Tile
        switch (direction)
        {
            case ExitPoint.NorthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y + 1));
                break;

            case ExitPoint.NorthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y + 1));
                break;

            case ExitPoint.SouthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y - 1));
                break;

            case ExitPoint.SouthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y - 1));
                break;

            case ExitPoint.West:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y));
                break;

            case ExitPoint.East:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y));
                break;

            case ExitPoint.None:
                break;
        }
        #endregion
        return returnedTile;
    }

    private TileInfo GetEvenChunkEvenTile(ExitPoint direction, GridPosition currentPosition)
    {
        TileInfo returnedTile = null;
        #region Even Tile
        switch (direction)
        {
            case ExitPoint.NorthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y + 1));
                break;

            case ExitPoint.NorthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y + 1));
                break;

            case ExitPoint.SouthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y - 1));
                break;

            case ExitPoint.SouthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y - 1));
                break;

            case ExitPoint.West:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y));
                break;

            case ExitPoint.East:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y));
                break;

            case ExitPoint.None:
                break;
        }
        #endregion
        return returnedTile;
    }

    private TileInfo GetEvenChunkOddTile(ExitPoint direction, GridPosition currentPosition)
    {
        TileInfo returnedTile = null;

        #region Odd Tile
        switch (direction)
        {
            case ExitPoint.NorthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y + 1));
                break;
                

            case ExitPoint.NorthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y + 1));
                break;
                

            case ExitPoint.SouthEast:
                returnedTile = FindTile(new GridPosition(currentPosition.x, currentPosition.y - 1));
                break;

            case ExitPoint.SouthWest:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y - 1));
                break;

            case ExitPoint.West:
                returnedTile = FindTile(new GridPosition(currentPosition.x + 1, currentPosition.y));
                break;

            case ExitPoint.East:
                returnedTile = FindTile(new GridPosition(currentPosition.x - 1, currentPosition.y));
                break;

            case ExitPoint.None:
                break;
        }
        #endregion
        return returnedTile;
    }

    private static void FindTileAndAddToNeighbours(GridPosition pos)
    {
        if (pos.x > (m_gridManager.GetGridDimentions().x * m_chunkManager.ChunkDimentions.x) - 1 ||
            pos.y > (m_gridManager.GetGridDimentions().y * m_chunkManager.ChunkDimentions.y) - 1)
            return;

        if (pos.x < 0 || pos.y < 0)
            return;


        TileInfo tileInfo = m_gridManager.GetTileAtGridPosition(pos);

        m_neighbours.Add(tileInfo);
    }

    private static TileInfo FindTile(GridPosition pos)
    {
        if (pos.x > (m_gridManager.GetGridDimentions().x * m_chunkManager.ChunkDimentions.x) - 1 ||
            pos.y > (m_gridManager.GetGridDimentions().y * m_chunkManager.ChunkDimentions.y) - 1)
            return null;

        if (pos.x < 0 || pos.y < 0)
            return null;

        TileInfo tileInfo = m_gridManager.GetTileAtGridPosition(pos);
        return tileInfo;
    }

}
