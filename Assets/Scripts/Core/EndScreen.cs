using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public GameObject scrollContent;
    public GameObject scorePrefab;
    public Scrollbar scrollbar;
    public Text finalScoreText;
    public MinigameManager minigameManager;
    public GameObject buttons;

    public List<Sprite> iconSprites;

    public int scoreSpeed = 41;
    public float scoreWidth;

    public void StartEndScreen()
    {
        StartCoroutine(DoEndScreen());
    }

    IEnumerator DoEndScreen()
    {
        buttons.SetActive(false);

        int currentScore = 0;
        while(currentScore < ScoreManager.GetScore())
        {
            currentScore += scoreSpeed;
            SetFinalScoreText(currentScore);
            yield return null;
        }

        SetFinalScoreText(ScoreManager.GetScore());

        for(int i = 0; i < ScoreManager.scoreDatas.Count; i++)
        {
            GameObject spawnedPrefab = Instantiate(scorePrefab, scrollContent.transform);

            EndSummary spawnedSummary = spawnedPrefab.GetComponent<EndSummary>();
            spawnedSummary.categoryName.text = ScoreManager.scoreDatas[i].type.ToString();
            spawnedSummary.icon.sprite = iconSprites[(int)ScoreManager.scoreDatas[i].type];
            spawnedSummary.minigameName.text = minigameManager.GetDefaultMinigameString(ScoreManager.scoreDatas[i].type);
            spawnedSummary.score.text = FormattingFunctions.NumberWithCommas(ScoreManager.scoreDatas[i].score);

            scrollContent.GetComponent<RectTransform>().SetHeight(i * scoreWidth);
        }
        scrollbar.size = Mathf.Min(3 / ScoreManager.scoreDatas.Count, 1);

        buttons.SetActive(true);
    }

    void SetFinalScoreText(int finalScore)
    {
        finalScoreText.text = "Final Score: <b>" + FormattingFunctions.NumberWithCommas(finalScore) + "</b>";
    }
}
