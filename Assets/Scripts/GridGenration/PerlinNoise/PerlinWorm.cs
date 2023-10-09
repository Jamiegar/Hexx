using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinWorm
{
    private Vector2 m_currenDir;
    private Vector2 m_currentPosition;
    private Vector2 m_destinationPoint;

    private GridPosition m_currentGridPos;
    private GridPosition m_destinationGridPos;
    private WormSettings m_wormSettings;
    private float[,] m_noiseMap;

    private TileInfo m_startingTileInfo;
    private TileInfo m_destinationTileInfo;
    
    private bool m_reachedDestination = false;
    private GridManager m_gridManger;

    public List<TileDirectionalInfo> m_tiledPath { get; private set; }
    private ExitPoint prevTileDirectionToNextTile;

    public PerlinWorm(WormSettings noiseSettings, GridPosition startGridPos, GridPosition convrgancePos, GridManager gridManager)
    {
        #region SetUpVariables
        //Set the positions in grid space
        m_currentGridPos = startGridPos;
        m_destinationGridPos = convrgancePos;

        //Set worms settings variable
        m_wormSettings = noiseSettings;

        //Set gridManager reference 
        m_gridManger = gridManager;
        #endregion

        //Set the starting and destination info variables, these are used to get any information about a tile
        m_startingTileInfo = m_gridManger.GetTileAtGridPosition(m_currentGridPos);
        m_destinationTileInfo = m_gridManger.GetTileAtGridPosition(m_destinationGridPos);

        //Gets a random direction 
        m_currenDir = Random.insideUnitCircle.normalized;

        //When setting the current and destination world point, the X & Z need to be set not the X & Y 
        m_currentPosition = new Vector2(m_startingTileInfo.m_worldPos.x, m_startingTileInfo.m_worldPos.z);
        m_destinationPoint = new Vector2(m_destinationTileInfo.m_worldPos.x, m_destinationTileInfo.m_worldPos.z);

        //Create a new noise map for the perlin worm 
        m_noiseMap = PerlinNoise.GenrateNoiseMap(new GridPosition(GridManager.TotalMapDimentions.x, GridManager.TotalMapDimentions.y), noiseSettings);
        m_reachedDestination = false;

        m_tiledPath = new List<TileDirectionalInfo>();
    }

    private Vector3 GetNoiseDirection()
    {
        float noiseValue = m_noiseMap[m_currentGridPos.x, m_currentGridPos.y]; //Gets the noise value between (0-1) for the current grid position
        float degrees = PerlinNoise.RangeMap(noiseValue, 0, 1, -90, 90); //Calculates a degrees value between -90 & 90 from the noise value
        m_currenDir = (Quaternion.AngleAxis(degrees, Vector3.up) * m_currenDir).normalized; //Finds the direction rotated aroun the up vector

        return m_currenDir;
    }

    private Vector2 MoveToDestinationPoint()
    {
        Vector3 direction = GetNoiseDirection(); //Calaculates the noise direction from the current world position
        Vector2 directionToDestinationPoint = (m_destinationPoint - m_currentPosition).normalized; //Direction to end
        
        //nextDirection = noiseDirection * weight + the destination * weight
        Vector2 endDirection = ((Vector2)direction * (1 - m_wormSettings.weight) + directionToDestinationPoint * m_wormSettings.weight).normalized;

        return endDirection;
    }

    private TileInfo FindTileInDirection(GridPosition dir, out ExitPoint exitPointToNextTile)
    {
        ExitPoint ep = GridPosition.GetExitPointFromGridDirection(dir); //Gets the compass direction from the direction 
        if(ep == ExitPoint.None)
        {
            Debug.Log(dir);
        }
        exitPointToNextTile = ep;

        GridSearch gridSearch = new GridSearch();
        TileInfo info = gridSearch.GetTileFromDirection(ep, m_currentGridPos);

        Debug.Log("River Tile Direction: " + ep);
        
        return info;
    }


    #region Helper functions
    private void SetNewCurrentLocation(Vector2 directionResult, Vector3 resultWorldPos) //Sets the current position from the new tile
    {
        m_currentPosition = new Vector2(resultWorldPos.x, resultWorldPos.z);
        m_currentPosition += directionResult;
    }
    #endregion

    public List<GridPosition> MoveLength(int length)
    {
        //Init local var
        List<GridPosition> gridPosOfRiverTiles = new List<GridPosition>();
        List<Vector3> worldPosOfRiverTiles = new List<Vector3>();

        //Add the starting tile to the list
        gridPosOfRiverTiles.Add(m_startingTileInfo.m_gridPosition);

        for (int i = 0; i < length; i++)
        {
            Vector2 result = MoveToDestinationPoint(); //The result is the noise direction 
            Vector2Int dir = Vector2Int.RoundToInt(result); //This rounds the direction to the nearest whole number

            //Finds the grid position of the next tile
            TileInfo nextTileInfo = FindTileInDirection(new GridPosition(dir.x, dir.y * -1), out ExitPoint exitPointToNextTile);

            //Setting the current world position to the new tile 
            Vector3 resultTileWorldPos = m_gridManger.GetTileAtGridPosition(nextTileInfo.m_gridPosition).m_worldPos; //Gets the new tiles world position
            worldPosOfRiverTiles.Add(resultTileWorldPos); //Add world position of tile to list
            SetNewCurrentLocation(result, resultTileWorldPos);

            if (!gridPosOfRiverTiles.Contains(nextTileInfo.m_gridPosition)) 
            {
                TileInfo currentTileInfo = m_gridManger.GetTileAtGridPosition(m_currentGridPos);
                AddTileToPath(currentTileInfo, exitPointToNextTile);

                if(i == length - 1 || nextTileInfo.m_gridPosition == m_destinationGridPos)
                {
                    AddTileToPath(nextTileInfo, ExitPoint.None);
                }

                m_currentGridPos = nextTileInfo.m_gridPosition; //set the new current grid pos
                gridPosOfRiverTiles.Add(nextTileInfo.m_gridPosition);

                if (nextTileInfo.m_gridPosition == m_destinationGridPos) //if the next position is == to the destination then we have reached the finish
                {
                    m_reachedDestination = true;
                    Debug.Log("Finished");
                    break;
                }
            }
        }
        return gridPosOfRiverTiles;
    }

    private void AddTileToPath(TileInfo currentTileinformation, ExitPoint directionToNextTile) // need to add last tile when on final index
    {
        //Create Directional Tile "tile with Exit Points" and with a Tile info
        TileDirectionalInfo tile = new TileDirectionalInfo(currentTileinformation);

        if(m_tiledPath.Count <= 0) //If true it is the first tile
        {
            tile.m_tilesExitDirection.Add(directionToNextTile); //Add the direction to the next tile for the current tile
            m_tiledPath.Add(tile); //Add current tile to the path list
            prevTileDirectionToNextTile = directionToNextTile;
            return;
        }

        int previousTileIndex = m_tiledPath.Count - 1; //Store the previous index
        TileDirectionalInfo prevTile = m_tiledPath[previousTileIndex]; //Gets ref to previous Tile Direction
        //Gets the oposite dirtection to the previous tile next direction this will get the direction to the previous
        ExitPoint directionToPrevious = prevTile.GetOppositeDirection(prevTileDirectionToNextTile); 
                        

        if (directionToNextTile == ExitPoint.None) //if there is no next direction then it is the end of the tile
        {
            //Add way to add end tile
            tile.m_tilesExitDirection.Add(directionToPrevious);
            prevTileDirectionToNextTile = directionToNextTile;
            m_tiledPath.Add(tile);
            return;
        }

        tile.m_tilesExitDirection.Add(directionToNextTile);
        tile.m_tilesExitDirection.Add(directionToPrevious);
        m_tiledPath.Add(tile);

        prevTileDirectionToNextTile = directionToNextTile;
    }

}
