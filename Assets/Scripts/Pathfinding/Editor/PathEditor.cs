using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    List<int> selectedPoints;

    float pointRadius = 12f;

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        switch(guiEvent.type)
        {
            case EventType.MouseDown:
                if(guiEvent.control)
                {
                    Undo.RecordObject(creator, "Add Point");
                    creator.AddPoint(mousePos);
                    selectedPoints.Clear();
                }
                else if(guiEvent.shift)
                {
                    int mousePoint = MouseOverPoint(mousePos);
                    if(mousePoint != -1)
                    {
                        selectedPoints.Add(mousePoint);
                    }
                    else
                    {
                        selectedPoints.Clear();
                    }
                }
                else
                {
                    selectedPoints.Clear();
                }
                break;
            case EventType.KeyDown:
                if(guiEvent.keyCode == KeyCode.C)
                {
                    MakeConnections();
                }
                else if(guiEvent.keyCode == KeyCode.Backspace)
                {
                    BreakConnections();
                }
                break;
        }
    }

    void Draw()
    {
        for (int i = 0; i < creator.points.Count; i++)
        {
            Handles.color = Color.red;
            if(IsPointSelected(i))
            {
                Handles.color = Color.yellow;
            }
            Vector2 newPosition = Handles.FreeMoveHandle(creator.points[i].location, Quaternion.identity, pointRadius, Vector2.zero, Handles.CylinderHandleCap);
            Handles.Label(creator.points[i].location, new GUIContent(i.ToString()));
            if (Vector2.SqrMagnitude(newPosition - creator.points[i].location) > 0.01f)
            {
                Undo.RecordObject(creator, "Move Point");
                creator.points[i].MovePosition(newPosition);
            }

            for(int j = 0; j < creator.points[i].connections.Count; j++)
            {
                Handles.color = new Color(0, 1f, 0);
                Handles.DrawLine(creator.points[i].location, creator.points[creator.points[i].connections[j]].location);
            }
        }
    }

    void OnEnable()
    {
        creator = (PathCreator)target;
        if(creator.points == null)
        {
            creator.points = new List<PathPoint>();
            creator.AddPoint(Vector2.zero);
        }
        else if(creator.points.Count == 0)
        {
            creator.AddPoint(Vector2.zero);
        }
        selectedPoints = new List<int>();
    }

    int MouseOverPoint(Vector2 mousePos)
    {
        for(int i = 0; i < creator.points.Count; i++)
        {
            if(Vector2.SqrMagnitude(mousePos - creator.points[i].location) < pointRadius * pointRadius)
            {
                return i;
            }
        }
        return -1;
    }

    bool IsPointSelected(int pointIndex)
    {
        for(int i = 0; i < selectedPoints.Count; i++)
        {
            if(selectedPoints[i] == pointIndex)
            {
                return true;
            }
        }
        return false;
    }

    void MakeConnections()
    {
        for(int i = 0; i < selectedPoints.Count; i++)
        {
            for(int j = i + 1; j < selectedPoints.Count; j++)
            {
                creator.MakeConnection(selectedPoints[i], selectedPoints[j]);
            }
        }
        selectedPoints.Clear();
    }

    void BreakConnections()
    {
        for (int i = 0; i < selectedPoints.Count; i++)
        {
            for (int j = i + 1; j < selectedPoints.Count; j++)
            {
                creator.BreakConnection(selectedPoints[i], selectedPoints[j]);
            }
        }
        selectedPoints.Clear();
    }
}
