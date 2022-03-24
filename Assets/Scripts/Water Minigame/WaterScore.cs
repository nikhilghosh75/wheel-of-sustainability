using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Variable Names are poor

public class WaterScore : MonoBehaviour
{
    public Text correctLabeled;
    public Text correctIgnored;
    public Text wildlifeRemoved;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    [HideInInspector]
    public WaterMinigame waterMinigame;

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
        correctLabeled.text = "0";
        correctIgnored.text = "0";
        wildlifeRemoved.text = "0";
        minigameScore.text = "0";
        totalScore.text = "0";

        wildlifeRemoved.color = Color.white;

        int correctLabeledScore = waterMinigame.correctlyCaught * 55;
        int correctIgnoredScore = waterMinigame.correctlyIgnored * 55;
        int wildlifeRemovedScore = waterMinigame.wildlifeRemoved * -110;

        int currentScore = 0;
        while (currentScore < correctLabeledScore)
        {
            currentScore += scoreSpeed;
            correctLabeled.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        correctLabeled.text = FormattingFunctions.NumberWithCommas(correctLabeledScore);

        currentScore = 0;
        while (currentScore < correctIgnoredScore)
        {
            currentScore += scoreSpeed;
            correctIgnored.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        correctIgnored.text = FormattingFunctions.NumberWithCommas(correctIgnoredScore);

        currentScore = 0;
        wildlifeRemoved.color = wildlifeRemovedScore < 0 ? Color.red : Color.white;
        while (currentScore < wildlifeRemovedScore)
        {
            currentScore += scoreSpeed;
            wildlifeRemoved.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        wildlifeRemoved.text = FormattingFunctions.NumberWithCommas(wildlifeRemovedScore);

        int waterMinigameScore = correctLabeledScore + correctIgnoredScore + wildlifeRemovedScore;
        currentScore = 0;
        while (currentScore < waterMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(waterMinigameScore);

        ScoreManager.AddScore(waterMinigameScore, Category.Water);
        waterMinigame.OnScoreReached(waterMinigameScore);

        currentScore = 0;
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
