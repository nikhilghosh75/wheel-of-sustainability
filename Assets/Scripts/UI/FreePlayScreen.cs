using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreePlayScreen : MonoBehaviour
{
    public GameObject difficultySelector;
    public IntroScreen informationScreen;
    public HighScoreScreen highScoreScreen;

    void OnEnable()
    {
        difficultySelector.SetActive(false);
        informationScreen.gameObject.SetActive(false);
        highScoreScreen.gameObject.SetActive(false);
    }
}
