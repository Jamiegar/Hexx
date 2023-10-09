using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid
{
    private int m_gridWidth = 0;
    private int m_gridHeight = 0;
    private float m_offset = 0.0f;

    private Vector3 m_startPos;
    private Tile[,] m_tileGrid;
    private Vector2 m_tileDimentions;
    

    public TileGrid(GridPosition dimentions, float gapBetweenTiles, Vector3 chunkPos, Vector2 tileDimention)
    {
        m_gridHeight = dimentions.x;
        m_gridWidth = dimentions.y;
        m_offset = gapBetweenTiles;
        m_tileGrid = new Tile[m_gridHeight, m_gridWidth];
        m_tileDimentions = tileDimention;

        //AddGap();
        m_startPos = FindStartPosition(chunkPos);
    }

    public void SetTileTypeOnGrid(GridPosition position, Tile tile)
    {
        m_tileGrid[position.x, position.y] = tile;
    }

    private void AddGap()
    {
        m_tileDimentions.x += m_tileDimentions.x * m_offset;
        m_tileDimentions.y += m_tileDimentions.y * m_offset;
    }

    private Vector3 FindStartPosition(Vector3 chunkPos)
    {
        float offset = 0;
        if ((m_gridHeight / 2) % 2 != 0) // Checks if the center hex is in an even row, if not the start position needs a small offset
        {
            offset = m_tileDimentions.y / 2;
        }

        float x = (-m_tileDimentions.x * (m_gridWidth / 2f) - offset) + chunkPos.x; // Moves the map negativity down the x axis 
        float z = (m_tileDimentions.y * 0.75f * (m_gridHeight / 2f)) + chunkPos.z; // The distance between a hex tile is 75% of a
                                                                                        //whole hex tile so times that by the grid hight 

        return new Vector3(x, 0, z); // Creates the new start position which is the center of the map
    }

    public Vector3 CalcWorldPositionOnGrid(GridPosition gridPosition)
    {
        float offset = 0;
        if (gridPosition.y % 2 != 0) // if the grid position is not dividable by 2 the offset is changed 
        {
            offset = m_tileDimentions.x / 2; // Moves every second row along the X axis so that it aligns on the grid
        }

        float x = m_startPos.x + gridPosition.x * m_tileDimentions.x + offset;    // Gets the X coordinate of the tile
        float z = m_startPos.z - gridPosition.y * m_tileDimentions.y * 0.75f;    // Gets the Z coordinate of the tile
                                                                                      // The hex tile doesn't move a whole hexagon down, only 75% of a whole Hex tile

       return new Vector3(x, 0, z); // returns a Vector 3 of the hex tile position
    }

}
