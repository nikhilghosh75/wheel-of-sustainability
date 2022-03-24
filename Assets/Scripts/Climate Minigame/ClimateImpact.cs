using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimateImpact : MonoBehaviour
{
    public Text topText;

    [HideInInspector]
    public ClimateMinigame minigame;

    public void DisplayImpact()
    {
        string topString = "You defeated <b>";
        topString += minigame.numDefeated.ToString();
        topString += " greenhouse gasses</b>, which protected humans and the environment from the harmful effects of greenhouse gasses.";

        topText.text = topString;
    }
}
