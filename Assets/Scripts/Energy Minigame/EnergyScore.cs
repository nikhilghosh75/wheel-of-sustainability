using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyScore : MonoBehaviour
{
    public Text energyCollected;
    public Text livesBonus;
    public Text completionBonus;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    [HideInInspector]
    public EnergyMinigame energyMinigame;

    [Header("Speed")]
    public int scoreSpeed;
    public int totalScoreSpeed;

    public void StartScore()
    {
        StartCoroutine(DoScore());
    }

    IEnumerator DoScore()
    {
        continueButton.gameObject.SetActive(false);
        energyCollected.text = "0";
        livesBonus.text = "0";
        completionBonus.text = "0";
        minigameScore.text = "0";
        totalScore.text = "0";

        int energyMinigameScore = energyMinigame.GetSunsCollectedScore();

        int currentScore = 0;
        while(currentScore < energyMinigame.GetSunsCollectedScore())
        {
            currentScore += scoreSpeed;
            energyCollected.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        energyCollected.text = FormattingFunctions.NumberWithCommas(energyMinigame.GetSunsCollectedScore());

        int livesScore = energyMinigame.GetLives() * 40;
        energyMinigameScore += livesScore;

        currentScore = 0;
        while (currentScore < livesScore)
        {
            currentScore += scoreSpeed;
            livesBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        livesBonus.text = FormattingFunctions.NumberWithCommas(livesScore);


        if (energyMinigame.GotToEnd())
        {
            energyMinigameScore += 200;
            currentScore = 0;
            while (currentScore < 300)
            {
                currentScore += scoreSpeed;
                completionBonus.text = currentScore.ToString();
                yield return null;
            }
            completionBonus.text = "300";
        }

        currentScore = 0;
        while (currentScore < energyMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(energyMinigameScore);

        currentScore = 0;
        Debug.Log(ScoreManager.GetScore());
        while (currentScore < ScoreManager.GetScore())
        {
            currentScore += totalScoreSpeed;
            totalScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        totalScore.text = FormattingFunctions.NumberWithCommas(ScoreManager.GetScore());

        continueButton.gameObject.SetActive(true);
    }
}
