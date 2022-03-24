using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static int currentBonuses = 0;
    public static int currentLength = 3;

    public static void StartBonusMode()
    {
        currentBonuses = currentLength;
    }

    public static void Reset()
    {
        currentBonuses = 0;
    }
}
