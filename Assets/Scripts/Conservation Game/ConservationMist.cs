using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConservationMist : MonoBehaviour
{
    public Color startColor;
    public Color endColor;

    Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        if(image == null)
            image = GetComponent<Image>();

        if (PlayerSettings.simpleBackgrounds)
            image.color = new Color(0, 0, 0);
        else
            image.color = color;
    }
}
