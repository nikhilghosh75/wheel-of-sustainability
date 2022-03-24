using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitiesImpact : MonoBehaviour
{
    [HideInInspector]
    public CitiesMinigame minigame;

    public Text topText;

    public void DisplayImpact()
    {
        float totalGallons = minigame.GetTotalMPG() / 40000;
        float averageMPG = minigame.GetAverageMPG();

        // You used <b>1.5 gallons</b> of gas and had an average Miles Per Gallon of <b>45 MPG</b>.

        string topString = "You used <b>" + totalGallons.ToString("#.00") + " gallons</b>";
        topString += " of gas and had an average Miles Per Gallon of ";
        topString += "<b>" + ((int)averageMPG).ToString() + " MPG</b>";

        topText.text = topString;
    }
}
