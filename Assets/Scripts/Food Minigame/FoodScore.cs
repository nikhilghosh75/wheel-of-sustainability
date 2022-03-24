using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodScore : MonoBehaviour
{
    public Text correctLabeled;
    public Text correctIgnored;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    [HideInInspector]
    public FoodMinigame foodMinigame;

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
        minigameScore.text = "0";
        totalScore.text = "0";

        int correctLabeledScore = foodMinigame.correctLabeled * 45;
        int correctIgnoredScore = foodMinigame.correctIgnored * 45;

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

        int foodMinigameScore = correctLabeledScore + correctIgnoredScore;
        currentScore = 0;
        while (currentScore < foodMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(foodMinigameScore);

        ScoreManager.AddScore(foodMinigameScore, Category.Food);
        foodMinigame.OnScoreReached(foodMinigameScore);

        currentScore = 0;
        // Debug.Log(ScoreManager.GetScore());
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
