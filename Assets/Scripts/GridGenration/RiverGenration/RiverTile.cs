using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ExitPoint
{ 
    None,
    NorthEast,
    East,
    SouthEast,
    SouthWest,
    West,
    NorthWest
}

[CreateAssetMenu(fileName = "River Tile", menuName = "Map/River/Create River Tile")]
public class RiverTile : Tile
{
    [Tooltip("The direction of the tiles exit points are when the tile is spawned into the world without rotation. " +
    "Looking down the world forward vector is North")]
    public List<ExitPoint> exitPointsDir = new List<ExitPoint>();
    public Dictionary<ExitPoint, bool> m_tileNeigbours { get; private set; }

    RiverTile()
    {
        m_tileNeigbours = new Dictionary<ExitPoint, bool>();
        InitaliseDirectionNeigbours();
    }


    private void InitaliseDirectionNeigbours() //The dictionary is initalised with all neigbour directions being set
                                               //to false unless they are within the exitPointDir list  
    {
        var exitPoints = Enum.GetValues(typeof(ExitPoint));
        foreach (ExitPoint ep in exitPoints)
        {
            if (ep == ExitPoint.None)
                continue;

            if(m_tileNeigbours == null)
                m_tileNeigbours = new Dictionary<ExitPoint, bool>();

            if (IsExitPointEqualToAnyExitDirection(ep))
                m_tileNeigbours.Add(ep, true);
            else
                m_tileNeigbours.Add(ep, false);
        }
    }

    private bool IsExitPointEqualToAnyExitDirection(ExitPoint exitPoint)
    {
        foreach(ExitPoint ep in exitPointsDir) //Returns true if parmater is equal to any of the exitPointDir
        {
            if (ep == exitPoint)
                return true;
        }
        return false;
    }

}
