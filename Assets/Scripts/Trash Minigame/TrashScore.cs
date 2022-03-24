using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashScore : MonoBehaviour
{
    public Text trashCollected;
    public Text livesBonus;
    public Text completionBonus;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    [HideInInspector]
    public TrashMinigame trashMinigame;

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
        trashCollected.text = "0";
        livesBonus.text = "0";
        completionBonus.text = "0";
        minigameScore.text = "0";
        totalScore.text = "0";

        int trashCollectedScore = trashMinigame.GetNumTrash() * 20;
        int livesBonusScore = trashMinigame.GetNumLives() * 40;
        int completionBonusScore = 200;
        
        if(trashMinigame.GetNumLives() == 0)
        {
            completionBonusScore = 0;
        }

        int currentScore = 0;
        while (currentScore < trashCollectedScore)
        {
            currentScore += scoreSpeed;
            trashCollected.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        trashCollected.text = FormattingFunctions.NumberWithCommas(trashCollectedScore);

        currentScore = 0;
        while (currentScore < livesBonusScore)
        {
            currentScore += scoreSpeed;
            livesBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        livesBonus.text = FormattingFunctions.NumberWithCommas(livesBonusScore);

        currentScore = 0;
        while (currentScore < completionBonusScore)
        {
            currentScore += scoreSpeed;
            completionBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        completionBonus.text = FormattingFunctions.NumberWithCommas(completionBonusScore);

        int trashMinigameScore = trashCollectedScore + livesBonusScore + completionBonusScore;
        ScoreManager.AddScore(trashMinigameScore, Category.Restoration);
        trashMinigame.OnScoreReached(trashMinigameScore);

        currentScore = 0;
        while (currentScore < trashMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(trashMinigameScore);

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
