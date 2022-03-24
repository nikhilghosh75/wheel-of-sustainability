using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour, IMovementController
{
    Pathfinder pathfinder;
    List<PathPoint> points;
    int currentPoint = 0;

    public ConservationMinigame minigame;
    public RectTransform player;
    public GameObject view;

    public DifficultyFloat chanceOfGoingToPlayer = new DifficultyFloat();
    public DifficultyFloat speed = new DifficultyFloat();

    [HideInInspector]
    public Vector2 originalPosition = Vector2.zero;

    [HideInInspector]
    public bool isActive;

    float lastMovedTime;
    Vector2 lastPosition;
    public float maxIdleTime = 0.2f;

    public void RecievePath(List<PathPoint> pathPoints)
    {
        // Debug.Log("Path Recieved");
        // Debug.Log(pathPoints[pathPoints.Count - 1].location);
        points = CleanPathPoints(pathPoints);
        currentPoint = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GetComponent<Pathfinder>();
        points = new List<PathPoint>();
        OnPathComplete();

        originalPosition = GetComponent<RectTransform>().anchoredPosition;
        lastPosition = originalPosition;
        lastMovedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        MoveToNextPoint();
        DebugDrawPath();
        CheckIfIdle();
    }

    public void Reset()
    {
        if(originalPosition.SqrMagnitude() > 1)
        {
            GetComponent<RectTransform>().anchoredPosition = originalPosition;
            points.Clear();
            OnPathComplete();

            lastPosition = originalPosition;
            lastMovedTime = Time.time;
        }
    }

    void DebugDrawPath()
    {
        for(int i = 0; i < points.Count - 1; i++)
        {
            Debug.DrawLine(points[i].location, points[i + 1].location, Color.cyan, 1f);
        }
    }

    void MoveToNextPoint()
    {
        if (points.Count <= currentPoint)
        {
            return;
        }

        Vector3 nextPoint = points[currentPoint].location;

        if (Vector2.SqrMagnitude((Vector2)nextPoint - (Vector2)transform.position) 
            < (speed.Get() * Time.deltaTime) * (speed.Get() * Time.deltaTime))
        {
            currentPoint++;
            // Debug.Log(currentPoint);
            if (points.Count <= currentPoint)
            {
                points.Clear();
                OnPathComplete();
                return;
            }
        }

        Vector2 direction = ((Vector2)nextPoint - (Vector2)transform.position).normalized;
        Vector2 nextMoveDistance = speed.Get() * Time.deltaTime * direction;
        transform.Translate(nextMoveDistance);

        float rotation = MathFunctions.GetAngle(direction);
        Quaternion q = Quaternion.Euler(0, 0, rotation + 90);
        view.transform.rotation = q;
    }

    void OnPathComplete()
    {
        float rand = (float)Random.Range(0f, 1f);
        if(rand < chanceOfGoingToPlayer.Get())
        {
            // Debug.Log("Go To Player");
            pathfinder.FindPath(player.anchoredPosition);
        }
        else
        {
            // Debug.Log("Random Point");
            pathfinder.FindPath(pathfinder.GetRandomPoint());
        }
    }

    void CheckIfIdle()
    {
        Vector2 currentPosition = transform.position;
        if(Vector2.SqrMagnitude(currentPosition - lastPosition) > 0.5f)
        {
            lastMovedTime = Time.time;
        }

        float timeSinceLastMove = Time.time - lastMovedTime;
        if(timeSinceLastMove > maxIdleTime)
        {
            OnPathComplete();
        }

        lastPosition = currentPosition;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Dead");
            minigame.Die();
        }
    }

    List<PathPoint> CleanPathPoints(List<PathPoint> pathpoints)
    {
        Vector2 bearDelta = (Vector2)transform.position - pathpoints[0].location;
        if(Mathf.Abs(bearDelta.x) > 8 && Mathf.Abs(bearDelta.y) > 8)
        {
            pathpoints.Insert(0, new PathPoint(new Vector2(pathpoints[0].location.x, transform.position.y)));
        }

        for (int i = 0; i < pathpoints.Count - 1; i++)
        {
            Vector2 delta = pathpoints[i + 1].location - pathpoints[i].location;
            Debug.Log(delta);
            if(Mathf.Abs(delta.x) > 8 && Mathf.Abs(delta.y) > 8)
            {
                pathpoints.Insert(i, new PathPoint(new Vector2(pathpoints[i].location.x, pathpoints[i + 1].location.y)));
                // Debug.Log("Path Point Inserted");
            }
        }
        return pathpoints;
    }
}
