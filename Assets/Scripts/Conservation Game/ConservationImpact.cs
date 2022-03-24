using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConservationImpact : MonoBehaviour
{
    public Text topText;

    [HideInInspector]
    public ConservationMinigame minigame;

    public void DisplayImpact()
    {
        // You planted <b><color=lime>12 trees</color></b>, which will produce <b>3,000 pounds</b> of oxygen per year.
        string topString = "You planted <b><color=lime>" + minigame.NumPlanted().ToString();
        topString += " trees</color></b>, which will produce <b>";
        topString += FormattingFunctions.NumberWithCommas(minigame.NumPlanted() * 250);
        topString += " pounds</b> of oxygen per year.";
        topText.text = topString;
    }
}
