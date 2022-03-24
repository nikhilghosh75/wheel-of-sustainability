using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyImpact : MonoBehaviour
{
    public Text topText;
    public Text fanText;
    public Text tvText;
    public Text gameText;
    public Text blenderText;

    [HideInInspector]
    public EnergyMinigame minigame;

    public void DisplayImpact()
    {
        string topString = "You collected <b><color=yellow>";
        topString += minigame.GetSunsCollected().ToString();
        topString += " suns</color></b>, which turned into <b>";
        topString += (minigame.GetSunsCollected() * 3).ToString();
        topString += " kJ</b> of energy, enough to power:";
        topText.text = topString;

        float fanMinutes = minigame.GetSunsCollected() / 0.8f;
        fanText.text = "A fan for " + fanMinutes.ToString("0.0#") + " minutes";

        float tvMinutes = minigame.GetSunsCollected() / 4f;
        tvText.text = "A new TV for " + tvMinutes.ToString("0.0#") + " minutes";

        float gameMinutes = minigame.GetSunsCollected() / 1.8f;
        gameText.text = "A game console for " + gameMinutes.ToString("0.0#") + " minutes";

        float blenderSeconds = minigame.GetSunsCollected() * 3;
        blenderText.text = "A blender for " + blenderSeconds.ToString("0.0#") + " seconds";
    }
}
