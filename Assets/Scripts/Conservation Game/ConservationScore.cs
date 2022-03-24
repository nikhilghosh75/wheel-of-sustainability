using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConservationScore : MonoBehaviour
{
    public Text statusText;
    public Text treesPlanted;
    public Text survivalBonus;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    public ConservationMinigame minigame;

    public DifficultyInt treeMultiplier;

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
        treesPlanted.text = "0";
        survivalBonus.text = "0";
        minigameScore.text = "0";
        totalScore.text = "0";

        int treePlantedScore = minigame.NumPlanted() * treeMultiplier.Get();
        int survivalScore = 300;
        if(minigame.IsDead())
        {
            survivalScore = 0;
            statusText.text = "You Died";
            statusText.color = Color.red;
        }
        else
        {
            statusText.text = "You Planted All Trees";
            statusText.color = Color.green;
        }
        int conservationMinigameScore = treePlantedScore + survivalScore;

        int currentScore = 0;
        while (currentScore < treePlantedScore)
        {
            currentScore += scoreSpeed;
            treesPlanted.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        treesPlanted.text = FormattingFunctions.NumberWithCommas(treePlantedScore);

        currentScore = 0;
        while (currentScore < survivalScore)
        {
            currentScore += scoreSpeed;
            survivalBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        survivalBonus.text = FormattingFunctions.NumberWithCommas(survivalScore);

        ScoreManager.AddScore(conservationMinigameScore, Category.Conservation);
        minigame.OnScoreReached(conservationMinigameScore);
        currentScore = 0;
        while (currentScore < conservationMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(conservationMinigameScore);

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
