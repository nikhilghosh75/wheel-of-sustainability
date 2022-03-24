using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public List<PathPoint> points;

    public Vector2 gridSize;

    void Awake()
    {

    }

    void Start()
    {
        for(int i = 0; i < points.Count; i++)
        {
            points[i].pathfindingData.pathfindingIndex = i;
        }
    }

    public void AddPoint(Vector2 point)
    {
        PathPoint pathPoint = new PathPoint();
        pathPoint.location = point;

        points.Add(pathPoint);
    }

    public void MakeConnection(int point1, int point2)
    {
        points[point1].connections.Add(point2);
        points[point2].connections.Add(point1);
    }

    public void BreakConnection(int point1, int point2)
    {
        for(int i = 0; i < points[point1].connections.Count; i++)
        {
            if(points[point1].connections[i] == point2)
            {
                points[point1].connections.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < points[point2].connections.Count; i++)
        {
            if (points[point2].connections[i] == point1)
            {
                points[point2].connections.RemoveAt(i);
                break;
            }
        }
    }

    public void ClearPathfindingData()
    {
        for(int i = 0; i < points.Count; i++)
        {
            points[i].pathfindingData.pointStatus = PathPointStatus.NOTEXPLORED;
            points[i].pathfindingData.parent = null;
            points[i].pathfindingData.gCost = 0;
        }
    }

    public PathPoint PointFromWorldLocation(Vector2 location)
    {
        float minDistance = 58315047.0f;
        int minIndex = -1;
        for (int i = 0; i < points.Count; i++)
        {
            float sqrDistance = (location - points[i].location).sqrMagnitude;
            if (sqrDistance < minDistance)
            {
                minIndex = i;
                minDistance = sqrDistance;
            }
        }
        return points[minIndex];
    }
}
