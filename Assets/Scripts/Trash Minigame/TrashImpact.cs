using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashImpact : MonoBehaviour
{
    public Text topText;

    [HideInInspector]
    public TrashMinigame minigame;

    public void DisplayImpact()
    {
        // You picked up <b>54 pieces of trash</b>!
        string topString = "You picked up <b>" + minigame.GetNumTrash() + " pieces of trash</b>!";
        topText.text = topString;
    }
}
