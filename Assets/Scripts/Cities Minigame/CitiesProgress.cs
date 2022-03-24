using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitiesProgress : MonoBehaviour
{
    public Slider progressBar;
    public Text percentageText;

    [HideInInspector]
    public CitiesMinigame minigame;

    public void DisplayProgress(float distance)
    {
        if(minigame == null)
        {
            progressBar.value = 0;
            percentageText.text = "0%";
            return;
        }

        progressBar.value = distance / minigame.distanceToHospital.Get();
        int percent = (int) ((distance / minigame.distanceToHospital.Get()) * 100);
        percentageText.text = percent.ToString() + "%";
    }
}
