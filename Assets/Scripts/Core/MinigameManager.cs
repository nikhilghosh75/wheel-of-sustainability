using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Category
{
    Cities,
    Climate,
    Conservation,
    Energy,
    Food,
    Mobility,
    Restoration,
    Water,
    None
}

public class MinigameManager : MonoBehaviour
{
    public List<IMinigame> minigames;

    public HelpScreen helpScreen;

    public IntroScreen introScreen;

    [HideInInspector]
    public Category currentCategory;

    int currentMinigame = 0;

    bool paused = false;

    string currentlyQueuedMinigame = "";

    void Start()
    {
        introScreen.manager = this;
        LoadSaveData();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (paused)
        {
            minigames[currentMinigame].UnpauseMinigame();
            Time.timeScale = 1f;
            helpScreen.gameObject.SetActive(false);
        }
        else
        {
            minigames[currentMinigame].PauseMinigame();
            Time.timeScale = 0f;
            helpScreen.gameObject.SetActive(true);
            helpScreen.SetText();
        }
        paused = !paused;
    }

    public void PlayMinigame()
    {
        PlayMinigame(minigames[currentMinigame].GetName());
    }

    public void PlayMinigame(string minigameName)
    {
        helpScreen.gameObject.SetActive(false);
        Debug.Log(minigameName);
        for(int i = 0; i < minigames.Count; i++)
        {
            if(minigames[i].GetName() == minigameName)
            {
                Debug.Log("FOUND");

                minigames[i].gameObject.SetActive(true);
                minigames[i].LoadMinigame();
                currentMinigame = i;
                introScreen.ShowIntroScreen();
            }
            else
            {
                minigames[i].gameObject.SetActive(false);
            }
        }
    }

    public IMinigame GetCurrentMinigame()
    {
        return minigames[currentMinigame];
    }

    public IMinigame GetMinigameByName(string minigameName)
    {
        for (int i = 0; i < minigames.Count; i++)
        {
            if (minigames[i].GetName() == minigameName)
            {
                return minigames[i];
            }
        }
        return null;
    }

    public string GetDefaultMinigameString(Category category)
    {
        switch(category)
        {
            case Category.Cities: return "Get to the Office";
            case Category.Climate: return "Protect the Ozone Layer";
            case Category.Conservation: return "Plant Trees";
            case Category.Energy: return "Collect That Solar Energy";
            case Category.Food: return "Label Things Processed";
            case Category.Mobility: return "Bicycle Game";
            case Category.Restoration: return "Pick Up Trash";
            case Category.Water: return "Clean the River";
        }
        return "";
    }

    public string GetMinigameDescription(Category category)
    {
        string minigameName = GetDefaultMinigameString(category);
        for (int i = 0; i < minigames.Count; i++)
        {
            if(minigames[i].GetName() == minigameName)
            {
                return minigames[i].GetDescription();
            }
        }
        return "";
    }

    public string GetMinigameDescription(string minigameName)
    {
        for (int i = 0; i < minigames.Count; i++)
        {
            if (minigames[i].GetName() == minigameName)
            {
                return minigames[i].GetDescription();
            }
        }
        return "";
    }

    public void QueueMinigame(string minigameToQueue)
    {
        currentlyQueuedMinigame = minigameToQueue;
    }

    public void PlayQueuedMinigame()
    {
        PlayMinigame(currentlyQueuedMinigame);
        currentlyQueuedMinigame = "";
    }

    public void LoadSaveData()
    {
        SaveData saveData = SaveSystem.Load();
        if(saveData != null)
        {
            PlayerSettings.simpleBackgrounds = saveData.simpleBackgrounds;

            for(int i = 0; i < saveData.highScores.Count; i++)
            {
                IMinigame minigame = GetMinigameByName(saveData.highScores[i].minigameName);
                minigame.easyHighScore = saveData.highScores[i].easyScore;
                minigame.mediumHighScore = saveData.highScores[i].mediumScore;
                minigame.hardHighScore = saveData.highScores[i].hardScore;
            }
        }
    }

    public void SaveHighScores()
    {
        SaveData saveData = new SaveData();
        saveData.simpleBackgrounds = PlayerSettings.simpleBackgrounds;

        for(int i = 0; i < minigames.Count; i++)
        {
            saveData.highScores.Add(minigames[i].GetSaveData());
        }

        SaveSystem.Save(saveData);
    }

    public static Color GetCategoryColor(Category category)
    {
        switch(category)
        {
            case Category.Cities: return new Color(205f / 255f, 26f / 255f, 235f / 255f);
            case Category.Climate: return Color.white;
            case Category.Conservation: return new Color(139f / 255f, 69f / 255f, 0);
            case Category.Energy: return new Color(1, 207f / 255f, 1f / 255f);
            case Category.Food: return new Color(7f / 255f, 232f / 255f, 105f / 255f);
            case Category.Mobility: return new Color(242f / 255f, 152f / 255f, 7f / 255f);
            case Category.Restoration: return Color.black;
            case Category.Water: return new Color(26f / 255f, 98f / 255f, 219f / 255f);
        }
        return Color.black;
    }
}
