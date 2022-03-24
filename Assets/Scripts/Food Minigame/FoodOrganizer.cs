using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodOrganizer : MonoBehaviour
{
    public FoodMinigame minigame;
    public Text text;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        FoodItem foodItem = collider2D.GetComponent<FoodItem>();
        if(foodItem != null)
        {
            string newString = foodItem.foodName.ToUpper() + "\n";
            if(foodItem.processed)
            {
                newString += "PROCESSED";
            }
            else
            {
                newString += "NOT PROCESSED";
            }
            text.text = newString;
            foodItem.JudgeLabel(minigame);
        }
    }
}
