using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimateScore : MonoBehaviour
{
    public Text enemiesDefeated;
    public Text healthBonus;
    public Text projectilesConserved;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    public DifficultyInt maxShots;

    [HideInInspector]
    public ClimateMinigame minigame;

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
        enemiesDefeated.text = "0";
        projectilesConserved.text = "0";
        healthBonus.text = "0";
        minigameScore.text = "0";
        totalScore.text = "0";

        int enemiesDefeatedScore = minigame.currentDefeatedScore;
        int healthBonusScore = minigame.GetCurrentHealth() * 50;
        int projectilesConservedBonus = (int)(((maxShots.Get() - minigame.GetNumShot()) / 250f) * 400);
        if (projectilesConservedBonus < 0)
            projectilesConservedBonus = 0;

        int currentScore = 0;
        while (currentScore < enemiesDefeatedScore)
        {
            currentScore += scoreSpeed;
            enemiesDefeated.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        enemiesDefeated.text = FormattingFunctions.NumberWithCommas(enemiesDefeatedScore);

        currentScore = 0;
        while (currentScore < healthBonusScore)
        {
            currentScore += scoreSpeed;
            healthBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        healthBonus.text = FormattingFunctions.NumberWithCommas(healthBonusScore);

        currentScore = 0;
        while (currentScore < projectilesConservedBonus)
        {
            currentScore += scoreSpeed;
            projectilesConserved.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        projectilesConserved.text = FormattingFunctions.NumberWithCommas(projectilesConservedBonus);

        int climateMinigameScore = enemiesDefeatedScore + healthBonusScore + projectilesConservedBonus;
        currentScore = 0;
        while (currentScore < climateMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(climateMinigameScore);

        currentScore = 0;
        ScoreManager.AddScore(climateMinigameScore, Category.Climate);
        minigame.OnScoreReached(climateMinigameScore);
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
