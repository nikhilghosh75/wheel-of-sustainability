using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathPoint
{
    public Vector2 location;

    public List<int> connections;

    public PathfindingData pathfindingData;

    public PathPoint()
    {
        connections = new List<int>();
    }

    public PathPoint(Vector2 newLocation)
    {
        location = newLocation;
        connections = new List<int>();
    }

    public void MovePosition(Vector2 newPosition)
    {
        location = newPosition;
    }

    public static bool IsEqual(PathPoint a, PathPoint b)
    {
        if(a == null)
        {
            Debug.Log("A is NULL");
            return true;
        }
        else if(b == null)
        {
            Debug.Log("B is NULL");
            return true;
        }
        return Vector2.SqrMagnitude(a.location - b.location) < 0.01f;
    }
}
