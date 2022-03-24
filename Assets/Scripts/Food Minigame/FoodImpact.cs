using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodImpact : MonoBehaviour
{
    public Text topText;

    [HideInInspector]
    public FoodMinigame minigame;

    public void DisplayImpact()
    {
        // You labeled<b>25 items </b> correctly. Every correct label allows consumers to make informed choices about the products they shop for.
        string topString = "You labeled <b>";
        topString += (minigame.correctIgnored + minigame.correctLabeled).ToString();
        topString += " items </b> correctly. Every correct label allows consumers to make informed choices about the products they shop for.";
        topText.text = topString;
    }
}
