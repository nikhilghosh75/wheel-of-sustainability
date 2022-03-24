using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScreen : MonoBehaviour
{
    public Text nameText;
    public Text easyScoreText;
    public Text mediumScoreText;
    public Text highScoreText;

    public void SetMinigame(IMinigame minigame)
    {
        nameText.text = minigame.GetName();
        easyScoreText.text = FormattingFunctions.NumberWithCommas(minigame.easyHighScore);
        mediumScoreText.text = FormattingFunctions.NumberWithCommas(minigame.mediumHighScore);
        highScoreText.text = FormattingFunctions.NumberWithCommas(minigame.hardHighScore);
    }
}
