using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeOnSprite : MonoBehaviour
{
    public Vector2 maxSize;

    RectTransform rectTransform;

    public void ResizeBasedOnSprite()
    {
        if(rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (maxSize.x < 0.01 && maxSize.y < 0.01)
            return;

        Sprite sprite = GetComponent<Image>().sprite;
        Vector2 spriteSize = new Vector2(sprite.rect.width, sprite.rect.height);

        Vector2 scaledToWidth = spriteSize * (maxSize.x / spriteSize.x);
        Vector2 scaledToHeight = spriteSize * (maxSize.y / spriteSize.y);

        if (scaledToWidth.y <= maxSize.y)
        {
            rectTransform.SetWidth(maxSize.x);
            rectTransform.SetHeight(scaledToWidth.y);
        }
        else if (scaledToHeight.x <= maxSize.x)
        {
            rectTransform.SetWidth(scaledToHeight.x);
            rectTransform.SetHeight(maxSize.y);
        }
    }

    public static Vector2 ScaledSize(Vector2 spriteSize, Vector2 maxSize)
    {
        Vector2 scaledToWidth = spriteSize * (maxSize.x / spriteSize.x);
        Vector2 scaledToHeight = spriteSize * (maxSize.y / spriteSize.y);

        Vector2 returnVector = spriteSize;
        if (scaledToWidth.y <= maxSize.y)
        {
            returnVector = new Vector2(maxSize.x, scaledToWidth.y);
        }
        else if (scaledToHeight.x <= maxSize.x)
        {
            returnVector = new Vector2(scaledToHeight.x, maxSize.y);
        }

        return returnVector;
    }
}
