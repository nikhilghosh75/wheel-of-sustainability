using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighScoreSaveData
{
    public string minigameName;
    public int easyScore;
    public int mediumScore;
    public int hardScore;
}

[System.Serializable]
public class SaveData
{
    public bool simpleBackgrounds;
    public List<HighScoreSaveData> highScores;

    public SaveData()
    {
        simpleBackgrounds = false;
        highScores = new List<HighScoreSaveData>();
    }
}
