using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpScreen : MonoBehaviour
{
    public MinigameManager minigameManager;

    public Text titleText;
    public Text goalText;
    public Text scoringText;
    public Text controlText;

    public void SetText()
    {
        IMinigame currentMinigame = minigameManager.GetCurrentMinigame();

        titleText.text = currentMinigame.GetName();
        goalText.text = currentMinigame.GetDescription();
        scoringText.text = currentMinigame.GetScoring();
        controlText.text = currentMinigame.GetControls();
    }
}
