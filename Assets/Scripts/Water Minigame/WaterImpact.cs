using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterImpact : MonoBehaviour
{
    public Text topText;

    [HideInInspector]
    public WaterMinigame minigame;

    public void DisplayImpact()
    {
        // You picked up <b>10 pieces of garbage</b> and ignored <b>8 animals</b>.
        string topString = "You picked up <b>";
        topString += minigame.correctlyCaught + " pieces of garbage";
        topString += "</b> and ignored <b>" + minigame.correctlyIgnored + " animals</b>";
        topText.text = topString;
    }
}
