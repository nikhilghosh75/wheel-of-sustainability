using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hospital : MonoBehaviour
{
    [HideInInspector]
    public CitiesMinigame minigame;

    public float endYPosition;

    RectTransform rectTransform;
    Vector2 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceLeft = minigame.distanceToHospital.Get() - minigame.GetCurrentDistance();
        if(distanceLeft < 300)
        {
            rectTransform.anchoredPosition = new Vector2(0, endYPosition + distanceLeft);
        }
    }

    public void Reset()
    {
        if(originalPosition != Vector2.zero)
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
