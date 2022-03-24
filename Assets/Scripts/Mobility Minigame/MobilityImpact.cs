using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobilityImpact : MonoBehaviour
{
    public Text topText;

    [HideInInspector]
    public MobilityMinigame minigame;

    public void DisplayImpact()
    {
        float distance = minigame.GetCurrentDistance() / 4000f;
        string topString = "You biked <b>" + distance.ToString(".0#") + " miles</b> around the park";
        topText.text = topString;
    }
}
