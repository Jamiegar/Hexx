using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GridPosition
{
    public int x;
    public int y;


    public GridPosition(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
    }

    public static bool operator ==(GridPosition pos1, GridPosition pos2)
    {

        if (pos1.x == pos2.x && pos1.y == pos2.y)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static bool operator !=(GridPosition pos1, GridPosition pos2)
    {
        if (pos1.x == pos2.x && pos1.y == pos2.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static GridPosition operator +(GridPosition pos1, GridPosition pos2)
    {
        GridPosition newPos;

        newPos.x = pos1.x + pos2.x;
        newPos.y = pos1.y + pos2.y;
        return newPos;
    }

    public static GridPosition operator -(GridPosition pos1, GridPosition pos2)
    {
        GridPosition newPos;

        newPos.x = pos1.x - pos2.x;
        newPos.y = pos1.y - pos2.y;
        return newPos;
    }

    public static GridPosition operator *(GridPosition pos, int value)
    {
        GridPosition newPos;
        newPos.x = pos.x * value;
        newPos.y = pos.y * value;

        return newPos;
    }
    
    public static GridPosition operator *(GridPosition pos1, GridPosition pos2)
    {
        GridPosition newPos;
        newPos.x = pos1.x * pos2.x;
        newPos.y = pos1.y * pos2.y;
        return newPos;
    }

    public static GridPosition operator /(GridPosition pos, float value)
    {
        GridPosition newPos;

        newPos.x = pos.x / 2;
        newPos.y = pos.y / 2;

        return newPos;
    }

    public float Magnitude()
    {
        return Mathf.Sqrt(this.x * this.x + this.y * this.y);
    }

    public GridPosition Normalize()
    {
        float m = this.Magnitude();
        if (m == 0)
            return new GridPosition();

        GridPosition newPos;

        float newX =  this.x / m;
        float newY = this.y / m;

        newPos.x = Mathf.RoundToInt(newX);
        newPos.y = Mathf.RoundToInt(newY);
        return newPos;
    }

    public GridPosition InverseDirection()
    {
        return new GridPosition(x * -1, y * -1);
    }

    public static GridPosition GetRandomDirection()
    {
        return new GridPosition(Random.Range(-1, 1), Random.Range(-1, 1));
    }



    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }

    public static ExitPoint GetExitPointFromGridDirection(GridPosition gridPosition)
    {
        if (gridPosition == new GridPosition(1, 0))
            return ExitPoint.West;
        else if (gridPosition == new GridPosition(-1, 0))
            return ExitPoint.East;
        else if (gridPosition == new GridPosition(1, 1))
            return ExitPoint.NorthWest;
        else if (gridPosition == new GridPosition(0, 1) || gridPosition == new GridPosition(-1, 1))
            return ExitPoint.NorthEast;
        else if (gridPosition == new GridPosition(0, -1) || gridPosition == new GridPosition(1, -1))
            return ExitPoint.SouthWest;
        else if (gridPosition == new GridPosition(-1, -1))
            return ExitPoint.SouthEast;
        else
            return ExitPoint.None;
    }
}
