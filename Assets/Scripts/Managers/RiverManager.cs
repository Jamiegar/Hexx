using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;
using System.Linq;

public class RiverManager : MonoBehaviour, IInitalizable
{
    [SerializeField, Range(0, 1)]
    private float m_percentageChanceOfRiver;

    [Header("Minima / Maxima")]
    [MinMaxSlider(0, 1), SerializeField]
    private Vector2 m_rangeForMinima = new Vector2(0.4f, 0.8f);
    [MinMaxSlider(0, 1), SerializeField]
    private Vector2 m_rangeForMaxima = new Vector2(0.8f, 0.8f); 

    [Header("Tiles"), SerializeField] 
    private RiverTile[] riverTiles;
    static private RiverTile[] allRiverTiles;

    [SerializeField]
    private WormSettings riverSettings;

    [SerializeField]
    private bool debug = false;

    private List<River> rivers;

    [SerializeField, Header("Run Time Sets")]
    private GridManagerSet m_gridManagerRunTimeSet;
    private GridManager m_gridManager;


    public void Init()
    {
        m_percentageChanceOfRiver = MapData.instance.ChanceOfSpawningRiver;
        allRiverTiles = riverTiles; // Set static array to the array that is set in the inspector
        m_gridManager = m_gridManagerRunTimeSet.GetItemIndex(0);
        
        rivers = new List<River>();
    }

    public void GenerateRivers() 
    {
        m_gridManager.GetRiverPoints(out List<GridPosition> riverStarts, out List<GridPosition> riverEnds);

        List<GridPosition> max = OrderMaxima(riverStarts); //Order the maximas, order settings are set to get values that are between a noise value

        foreach (var item in max)
        {
            TileInfo info = m_gridManager.GetTileAtGridPosition(item); //Get starting tile info

            #region Debug
            if (debug == true)
                Debug.DrawLine(info.m_worldPos, info.m_worldPos + new Vector3(0, 100, 0), Color.red, Mathf.Infinity);
            #endregion
        }

        //Find the percentage chance of river spawning
        float chance = Random.Range(0f, 1f);

        for(int i = 0; i < max.Count; i++)
        {
            if (chance > m_percentageChanceOfRiver) // if the chance is more then the set percentage chance of rivers then this river is not spawned
                continue;

            TileInfo endTile = OrderMinima(riverEnds, max[i]);
            River river = new River(riverSettings, max[i], endTile.m_gridPosition, m_gridManager);
            rivers.Add(river);
        }

    }

    private TileInfo OrderMinima(List<GridPosition> minima, GridPosition startPosition)
    {
        minima = minima.Where(pos => m_gridManager.m_biomeNoiseMap[pos.x, pos.y] < m_rangeForMinima.y && m_gridManager.m_biomeNoiseMap[pos.x, pos.y] > m_rangeForMinima.x 
            && pos.x > 1 && pos.x < (GridManager.TotalMapDimentions.x - 1)).OrderBy(pos => m_gridManager.m_tileNoiseMap[pos.x, pos.y]).ToList();


        List<TileInfo> minimaInfo = new List<TileInfo>();
        foreach (GridPosition gridPosition in minima)
        {
            TileInfo info = m_gridManager.GetTileAtGridPosition(gridPosition);
            minimaInfo.Add(info);

            #region Debug
            if (debug == true) 
                Debug.DrawLine(info.m_worldPos, info.m_worldPos + new Vector3(0, 100, 0), Color.blue, Mathf.Infinity);
            #endregion
        }

        TileInfo startingPositionInfo = m_gridManager.GetTileAtGridPosition(startPosition);
        TileInfo endTile = minimaInfo.OrderBy(info => Vector3.Distance(info.m_worldPos, startingPositionInfo.m_worldPos)).First();

        return endTile;
    }

    private List<GridPosition> OrderMaxima(List<GridPosition> maxima)
    {
        var orderedMaxima = maxima.Where(pos => m_gridManager.m_biomeNoiseMap[pos.x, pos.y] > m_rangeForMaxima.x && pos.x > 1 
        && pos.x < (GridManager.TotalMapDimentions.x - 1)).OrderBy(pos => m_gridManager.m_tileNoiseMap[pos.x, pos.y]).ToList();

        return orderedMaxima;
    }

    public static RiverTile GetRiverTileWithExitPoint(ExitPoint exitPointOfTile)
    {
        //Only 1 exit point
        foreach(RiverTile riverTile in allRiverTiles)
        {
            if (riverTile.exitPointsDir.Count <= 1)
            {
                if (riverTile.exitPointsDir[0] == exitPointOfTile)
                {
                    return riverTile;
                }
            }
        }
        return null;
    }

    public static RiverTile GetRiverTileWithExitPoint(ExitPoint exitPoint1, ExitPoint exitPoint2)
    {
        //If there is more than one exit point
        foreach(RiverTile riverTile in allRiverTiles)
        {
            if(riverTile.exitPointsDir.Count > 1)
            {
                if(riverTile.exitPointsDir[0] == exitPoint1 || riverTile.exitPointsDir[0] == exitPoint2)
                {
                    if(riverTile.exitPointsDir[1] == exitPoint1 || riverTile.exitPointsDir[1] == exitPoint2)
                    {
                        return riverTile;
                    }
                }
            }
        }
        return null;
    }

    public static RiverTile GetRiverTileWithExitPoint(ExitPoint exitPoint1, ExitPoint exitPoint2, ExitPoint exitPoint3)
    {
        //If there is more than one exit point
        foreach (RiverTile riverTile in allRiverTiles)
        {
            if (riverTile.exitPointsDir.Count > 2)
            {
                if (riverTile.exitPointsDir[0] == exitPoint1 || riverTile.exitPointsDir[0] == exitPoint2 || riverTile.exitPointsDir[0] == exitPoint3)
                {
                    if (riverTile.exitPointsDir[1] == exitPoint1 || riverTile.exitPointsDir[1] == exitPoint2 || riverTile.exitPointsDir[1] == exitPoint3)
                    {
                        if (riverTile.exitPointsDir[2] == exitPoint1 || riverTile.exitPointsDir[2] == exitPoint2 || riverTile.exitPointsDir[2] == exitPoint3)
                        {
                            return riverTile;
                        }
                    }
                }
            }
        }
        return null;
    }
}
