using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobilityScore : MonoBehaviour
{
    public Text coinsCollected;
    public Text distanceTravelled;
    public Text livesBonus;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    public DifficultyInt coinConstant;

    [HideInInspector]
    public MobilityMinigame minigame;

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
        coinsCollected.text = "0";
        distanceTravelled.text = "0";
        livesBonus.text = "0";
        minigameScore.text = "0";
        totalScore.text = "0";

        int trashCollectedScore = minigame.GetCoinsCollected() * coinConstant.Get();
        int distanceTraveledScore = (int)(minigame.GetCurrentDistance() / 40);
        int livesBonusScore = minigame.GetNumLives() * 100;

        int currentScore = 0;
        while (currentScore < trashCollectedScore)
        {
            currentScore += scoreSpeed;
            coinsCollected.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        coinsCollected.text = FormattingFunctions.NumberWithCommas(trashCollectedScore);

        currentScore = 0;
        while (currentScore < distanceTraveledScore)
        {
            currentScore += scoreSpeed;
            distanceTravelled.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        distanceTravelled.text = FormattingFunctions.NumberWithCommas(distanceTraveledScore);

        currentScore = 0;
        while (currentScore < livesBonusScore)
        {
            currentScore += scoreSpeed;
            livesBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        livesBonus.text = FormattingFunctions.NumberWithCommas(livesBonusScore);

        int mobilityMinigameScore = trashCollectedScore + livesBonusScore + distanceTraveledScore;
        ScoreManager.AddScore(mobilityMinigameScore, Category.Mobility);
        minigame.OnScoreReached(mobilityMinigameScore);

        currentScore = 0;
        while (currentScore < mobilityMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(mobilityMinigameScore);

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
