using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum Direction
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    [HideInInspector]
    public bool isActive;

    bool isMoving = false;
    Soil soil = null;

    public float speed = 2;
    public GameObject view;
    public PathCreator pathCreator;
    public float threshold = 10;

    Vector3 nextPoint;
    Direction direction;

    [HideInInspector]
    public Vector2 originalPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        nextPoint = pathCreator.PointFromWorldLocation(transform.position).location;
        transform.position = nextPoint;

        originalPosition = GetComponent<RectTransform>().anchoredPosition;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        HandleInput();

        if(isMoving)
        {
            MoveToNextPoint();
        }
    }

    public void Reset()
    {
        if (originalPosition.SqrMagnitude() > 1)
        {
            GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
        direction = Direction.None;
        isMoving = false;
        nextPoint = pathCreator.PointFromWorldLocation(originalPosition).location;
    }

    void HandleInput()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            isMoving = false;
        }

        Direction newDirection = Direction.None;
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newDirection = Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            newDirection = Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            newDirection = Direction.Right;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            newDirection = Direction.Up;
        }

        if(IsOppositeOfCurrentDirection(newDirection))
        {
            nextPoint = FindPoint(newDirection);
        }

        if(newDirection != Direction.None)
        {
            isMoving = true;
            direction = newDirection;

            Vector3 nearestPathPoint = pathCreator.PointFromWorldLocation(transform.position).location;
            if (Vector3.SqrMagnitude(nearestPathPoint - transform.position) < threshold)
            {
                nextPoint = nearestPathPoint;
                nextPoint = FindPoint(newDirection);
            }
        }
    }

    void MoveToNextPoint()
    {
        float distance = speed * Time.deltaTime;
        if (Vector2.SqrMagnitude((Vector2)nextPoint - (Vector2)transform.position) < distance * distance)
        {
            Vector2 potentialNextPoint = FindPoint(direction);
            if(Vector2.SqrMagnitude(potentialNextPoint) < 1)
            {
                isMoving = false;
            }
            else
            {
                // Debug.Log("Found Next Point is " + potentialNextPoint.ToString());
                // Debug.Log("Difference is " + (potentialNextPoint - (Vector2)nextPoint).ToString());
                nextPoint = potentialNextPoint;
            }
            return;
        }

        Vector2 movementDirection = ((Vector2)nextPoint - (Vector2)transform.position).normalized;
        Vector2 nextMoveDistance = distance * movementDirection;
        transform.Translate(nextMoveDistance);

        float rotation = MathFunctions.GetAngle(movementDirection);
        Quaternion q = Quaternion.Euler(0, 0, rotation - 90);
        view.transform.rotation = q;
    }

    void OnPathComplete()
    {
        isMoving = false;

        if(soil != null)
        {
            soil.Plant();
        }
    }

    bool IsOppositeOfCurrentDirection(Direction inDirection)
    {
        switch(inDirection)
        {
            case Direction.None: return false;
            case Direction.Down: return direction == Direction.Up;
            case Direction.Left: return direction == Direction.Right;
            case Direction.Right: return direction == Direction.Left;
            case Direction.Up: return direction == Direction.Down;
        }
        return false;
    }

    Vector2 FindPoint(Direction direction)
    {
        if (direction == Direction.None) return Vector2.zero;

        Vector2 directionVector = GetDirectionVector(direction);
        PathPoint currentPoint = pathCreator.PointFromWorldLocation(nextPoint);

        foreach(int connection in currentPoint.connections)
        {
            PathPoint point = pathCreator.points[connection];
            Vector2 difference = point.location - currentPoint.location;
            float angle = Vector2.Angle(difference, directionVector);

            if(angle < 15)
            {
                return point.location;
            }
        }

        return Vector2.zero;
    }

    static Vector2 GetDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.None: return Vector2.zero;
            case Direction.Left: return Vector2.left;
            case Direction.Right: return Vector2.right;
            case Direction.Down: return Vector2.down;
            case Direction.Up: return Vector2.up;
        }
        return Vector2.zero;
    }
}
