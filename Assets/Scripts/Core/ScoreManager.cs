using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static int score = 0;

    public struct ScoreData
    {
        public int score;
        public Category type;

        public ScoreData(int score, Category type)
        {
            this.score = score;
            this.type = type;
        }
    }

    public static List<ScoreData> scoreDatas = new List<ScoreData>();

    public static int GetScore() { return score; }

    public static void AddScore(int inScore, Category category)
    {
        score += inScore;
        scoreDatas.Add(new ScoreData(inScore, category));
    }

    public static void ResetScore()
    {
        score = 0;
        scoreDatas.Clear();
    }
}
