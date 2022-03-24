using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyMode
{
    Easy,
    Medium,
    Hard
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyMode currentDifficultyMode;
}

[System.Serializable]
public class DifficultyFloat
{
    public float easyValue;
    public float mediumValue;
    public float hardValue;

    public float Get()
    {
        switch(DifficultyManager.currentDifficultyMode)
        {
            case DifficultyMode.Easy:
                return easyValue;
            case DifficultyMode.Medium:
                return mediumValue;
            case DifficultyMode.Hard:
                return hardValue;
        }
        return 0;
    }
}

[System.Serializable]
public class DifficultyInt
{
    public int easyValue;
    public int mediumValue;
    public int hardValue;

    public int Get()
    {
        switch (DifficultyManager.currentDifficultyMode)
        {
            case DifficultyMode.Easy:
                return easyValue;
            case DifficultyMode.Medium:
                return mediumValue;
            case DifficultyMode.Hard:
                return hardValue;
        }
        return 0;
    }
}