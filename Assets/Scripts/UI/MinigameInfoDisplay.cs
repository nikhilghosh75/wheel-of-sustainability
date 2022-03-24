using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameInfoDisplay : MonoBehaviour
{
    public Image wheelImage;
    public Text category;
    public Text minigameName;
    public Text minigameDescription;

    public IntroScreen informationScreen;
    public HighScoreScreen highScoreScreen;

    IMinigame currentMinigame = null;

    public void Reset()
    {
        wheelImage.gameObject.SetActive(false);
        category.text = "";
        minigameName.text = "";
        minigameDescription.text = "";
    }

    public void Enable()
    {
        wheelImage.gameObject.SetActive(true);
    }

    public void SetMinigame(IMinigame minigame)
    {
        currentMinigame = minigame;
        highScoreScreen.SetMinigame(minigame);
    }

    public void ShowInformation()
    {
        informationScreen.ShowIntroScreen(currentMinigame);
    }
}
