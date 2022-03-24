using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrogen : MonoBehaviour
{
    RectTransform rectTransform;
    public float deletePosition;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rectTransform.anchoredPosition.y < deletePosition)
        {
            ClimateMinigame.minigame.OnHit();
            Destroy(gameObject);
        }
    }
}
