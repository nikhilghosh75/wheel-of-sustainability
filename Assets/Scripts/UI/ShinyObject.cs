using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIShiny))]
public class ShinyObject : MonoBehaviour
{
    UIShiny shiny;

    // Start is called before the first frame update
    void Start()
    {
        shiny = GetComponent<UIShiny>();
        if(PlayerSettings.shinyInteractables)
        {
            shiny.enabled = true;
            shiny.Play();
        }
        else
        {
            shiny.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
