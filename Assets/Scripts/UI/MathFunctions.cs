using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathFunctions
{
    public static Vector2 GetClosestPointToLine(Vector2 a, Vector2 b, Vector2 p)
    {
        Vector2 aToP = p - a;
        Vector2 aToB = b - a;

        float aToBSqr = Vector2.SqrMagnitude(aToB);
        float dot = Vector2.Dot(aToP, aToB);

        float t = dot / aToBSqr;

        return a + (b * t);
    }

    public static float GetAngle(Vector2 v)
    {
        return GetAngle(v.x, v.y);
    }

    public static float GetAngle(float x, float y)
    {
        if (x > 0)
        {
            if (y > 0)
            {
                return Mathf.Asin(y) * Mathf.Rad2Deg;
            }
            return (Mathf.Asin(y) * Mathf.Rad2Deg) + 360f;
        }
        if (y > 0)
        {
            return (Mathf.Acos(x) * Mathf.Rad2Deg);
        }
        return 180 - (Mathf.Asin(y) * Mathf.Rad2Deg);
    }
}
