using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMinigame : MonoBehaviour
{
    public Sprite thumbnailImage;

    [HideInInspector]
    public int easyHighScore = 0;

    [HideInInspector]
    public int mediumHighScore = 0;

    [HideInInspector]
    public int hardHighScore = 0;

    public virtual string GetName()
    {
        return "Unimplemented Minigame";
    }

    public virtual string GetDescription()
    {
        return "";
    }

    public virtual void LoadMinigame()
    {

    }

    public virtual void StartMinigame()
    {
        Debug.LogWarning("Minigame is unimplemented");
    }

    public virtual string GetScoring()
    {
        return "";
    }

    public virtual string GetControls()
    {
        return "";
    }

    public virtual void PauseMinigame()
    {

    }

    public virtual void UnpauseMinigame()
    {

    }

    public virtual List<IntroImageConfig> GetIntroImageConfigs()
    {
        return new List<IntroImageConfig>();
    }

    public void OnScoreReached(int newScore)
    {
        switch(DifficultyManager.currentDifficultyMode)
        {
            case DifficultyMode.Easy:
                if(newScore > easyHighScore)
                {
                    easyHighScore = newScore;
                }
                break;
            case DifficultyMode.Medium:
                if (newScore > mediumHighScore)
                {
                    mediumHighScore = newScore;
                }
                break;
            case DifficultyMode.Hard:
                if (newScore > hardHighScore)
                {
                    hardHighScore = newScore;
                }
                break;
        }

        ScreenSwitcher.switcher.minigameManager.SaveHighScores();
    }

    public HighScoreSaveData GetSaveData()
    {
        HighScoreSaveData saveData = new HighScoreSaveData();
        saveData.easyScore = easyHighScore;
        saveData.mediumScore = mediumHighScore;
        saveData.hardScore = hardHighScore;
        saveData.minigameName = GetName();

        return saveData;
    }
}
