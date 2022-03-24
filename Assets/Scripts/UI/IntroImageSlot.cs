using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroImageSlot : MonoBehaviour
{
    public Image image;
    public Text text;

    public void SetIntroImage(IntroImageConfig config)
    {
        image.sprite = config.sprite;
        text.text = config.text;
        image.GetComponent<ResizeOnSprite>().ResizeBasedOnSprite();
    }
}
