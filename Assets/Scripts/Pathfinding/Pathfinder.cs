using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathPointStatus
{
    NOTEXPLORED,
    SEEN,
    VISITED
}

public enum PathfinderStatus
{
    NOTCALCULATING,
    CALCULATING,
}

[System.Serializable]
public struct PathfindingData
{
    public PathPoint parent;
    public float fCost;
    public float gCost; // The distance between the start and the node
    public float hCost;
    public PathPointStatus pointStatus;
    public int pathfindingIndex;

    private PathfindingData(PathPoint newParent)
    {
        this.parent = newParent;
        this.fCost = 0;
        this.gCost = 0;
        this.hCost = 0;
        this.pointStatus = PathPointStatus.NOTEXPLORED;
        pathfindingIndex = 0;
    }
}

public class Pathfinder : MonoBehaviour
{
    List<PathPoint> currentPath;
    IMovementController controller;
    bool isCalculatingPath = false;

    // pathfinder variables
    List<PathPoint> seenPoints = new List<PathPoint>();
    List<PathPoint> visitedPoints = new List<PathPoint>();

    List<PathPoint> openSet = new List<PathPoint>();
    HashSet<PathPoint> closedSet = new HashSet<PathPoint>();

    PathPoint currentPoint;
    PathPoint startPoint;
    PathPoint targetPoint;
    public PathCreator pathCreator;
    int pathfindingIterations;

    //static uint8 pathfinders;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<IMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCalculatingPath)
        {
            return;
        }

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        while(sw.ElapsedMilliseconds < 2)
        {
            bool foundPoint = PathfindingIteration();
            if (foundPoint)
            {
                CalculatePath();
                break;
            }
        }
        sw.Stop();
    }

    public List<PathPoint> GetCurrentPath()
    {
        return currentPath;
    }

    public Vector2 GetRandomPoint()
    {
        int pointsLength = pathCreator.points.Count;
        return pathCreator.points[(int)Random.Range(0, pointsLength)].location;
    }

    public void FindPath(Vector2 endPoint)
    {
        isCalculatingPath = true;

        //Debug.Log("START PATHFINDING");

        startPoint = PointFromWorldLocation(transform.position);
        pathfindingIterations = 0;

        if (startPoint == null)
        {
            Debug.Log("Start Point is NULL");
        }

        targetPoint = PointFromWorldLocation(endPoint);

        if(targetPoint == null)
        {
            Debug.LogError("Target Point is NULL");
            return;
        }

        currentPoint = startPoint;

        openSet = new List<PathPoint>();
        closedSet = new HashSet<PathPoint>();
        openSet.Add(startPoint);

        for (int i = 0; i < 5; i++)
        {
            bool foundPoint = PathfindingIteration();
            if(foundPoint)
            {
                CalculatePath();
                break;
            }
        }
    }

    bool PathfindingIteration()
    {
        if(openSet.Count == 0)
        {
            openSet.Add(startPoint);
        }
        PathPoint node = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].pathfindingData.fCost < node.pathfindingData.fCost || Mathf.Approximately(openSet[i].pathfindingData.fCost,node.pathfindingData.fCost))
            {
                if (openSet[i].pathfindingData.hCost < node.pathfindingData.hCost)
                    node = openSet[i];
            }
        }

        openSet.Remove(node);
        closedSet.Add(node);

        if (node == targetPoint)
        {
            return true;
        }

        foreach (int index in node.connections)
        {
            PathPoint neighbour = pathCreator.points[index];
            if (closedSet.Contains(neighbour))
            {
                continue;
            }

            float newCostToNeighbour = node.pathfindingData.gCost + GetDistance(node, neighbour);
            if (newCostToNeighbour < neighbour.pathfindingData.gCost || !openSet.Contains(neighbour))
            {
                neighbour.pathfindingData.gCost = newCostToNeighbour;
                neighbour.pathfindingData.hCost = GetDistance(neighbour, targetPoint);
                neighbour.pathfindingData.parent = node;

                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);
            }
        }
        return false;
    }

    void CalculatePath()
    {
        currentPath = new List<PathPoint>();
        currentPoint = targetPoint;
        if(currentPoint == null)
        {
            Debug.Log("TARGET POINT is NULL");
        }

        int iterations = 0;
        while (!PathPoint.IsEqual(currentPoint, startPoint))
        {
            iterations++;
            currentPath.Insert(0, currentPoint);
            currentPoint = currentPoint.pathfindingData.parent;
            if(iterations >= 45)
            {
                break;
            }
            if(currentPoint == null)
            {
                break;
            }
        }
        currentPath.Insert(0, startPoint);

        ModifyPath();

        //Debug.Log("END PATHFINDING");

        if (controller == null)
            controller = GetComponent<IMovementController>();
        controller.RecievePath(currentPath);
        isCalculatingPath = false;
    }

    void ModifyPath()
    {
        // If Count is 0 or 1, return
        if(currentPath.Count == 0 || currentPath.Count == 1)
        {
            return;
        }
        // Remove 0th Point if between two points
        float angleBetweenPoints = Vector2.Angle(currentPath[0].location - (Vector2)transform.position, currentPath[1].location - (Vector2)transform.position);
        if(angleBetweenPoints > 145)
        {
            currentPath.RemoveAt(0);
        }

        // If duplicates

        // Actually go to the desired point
    }

    void AttemptRecalculation()
    {
        FindPath(targetPoint.location);
    }

    PathPoint PointFromWorldLocation(Vector2 location)
    {
        List<PathPoint> pathPoints = pathCreator.points;

        float minDistance = 58315047.0f;
        int minIndex = -1;
        for(int i = 0; i < pathPoints.Count; i++)
        {
            float sqrDistance = (location - pathPoints[i].location).sqrMagnitude;
            if(sqrDistance < minDistance)
            {
                minIndex = i;
                minDistance = sqrDistance;
            }
        }
        return pathPoints[minIndex];
    }

    float GetDistance(PathPoint a, PathPoint b)
    {
        return Vector2.Distance(a.location, b.location);
    }
}
