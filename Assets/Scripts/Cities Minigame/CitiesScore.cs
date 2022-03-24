using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitiesScore : MonoBehaviour
{
    public Text distanceDriven;
    public Text hospitalBonus;
    public Text timeBonus;
    public Text efficiencyBonus;
    public Text minigameScore;
    public Text totalScore;
    public Button continueButton;

    public DifficultyInt distanceScoreMultiplier;
    public DifficultyFloat parTime;

    [HideInInspector]
    public CitiesMinigame citiesMinigame;

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
        distanceDriven.text = "0";
        hospitalBonus.text = "0";
        timeBonus.text = "0";
        minigameScore.text = "0";
        totalScore.text = "0";

        bool completed = citiesMinigame.GetCurrentDistance() >= citiesMinigame.distanceToHospital.Get();

        int distanceDrivenScore = (int) (distanceScoreMultiplier.Get() * citiesMinigame.GetCurrentDistance() / citiesMinigame.distanceToHospital.Get());
        int hospitalBonusScore = completed ? 150 : 0;
        int timeBonusScore = -2 * (int)(citiesMinigame.GetCurrentTime() - parTime.Get()) + 225;
        int efficiencyBonusScore = citiesMinigame.GetEfficiencyScore();

        if (!completed)
            timeBonusScore = 0;

        int currentScore = 0;
        while (currentScore < distanceDrivenScore)
        {
            currentScore += scoreSpeed;
            distanceDriven.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        distanceDriven.text = FormattingFunctions.NumberWithCommas(distanceDrivenScore);

        currentScore = 0;
        while (currentScore < hospitalBonusScore)
        {
            currentScore += scoreSpeed;
            hospitalBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        hospitalBonus.text = FormattingFunctions.NumberWithCommas(hospitalBonusScore);

        currentScore = 0;
        while (currentScore < efficiencyBonusScore)
        {
            currentScore += scoreSpeed;
            efficiencyBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        efficiencyBonus.text = FormattingFunctions.NumberWithCommas(efficiencyBonusScore);

        currentScore = 0;
        while (currentScore < timeBonusScore)
        {
            currentScore += scoreSpeed;
            timeBonus.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        timeBonus.text = FormattingFunctions.NumberWithCommas(timeBonusScore);

        int citiesMinigameScore = timeBonusScore + distanceDrivenScore + hospitalBonusScore + efficiencyBonusScore;
        currentScore = 0;
        while (currentScore < citiesMinigameScore)
        {
            currentScore += totalScoreSpeed;
            minigameScore.text = FormattingFunctions.NumberWithCommas(currentScore);
            yield return null;
        }
        minigameScore.text = FormattingFunctions.NumberWithCommas(citiesMinigameScore);

        currentScore = 0;
        ScoreManager.AddScore(citiesMinigameScore, Category.Cities);
        citiesMinigame.OnScoreReached(citiesMinigameScore);
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
