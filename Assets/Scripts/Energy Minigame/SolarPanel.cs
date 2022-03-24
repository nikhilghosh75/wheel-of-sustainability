using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    RectTransform rectTransform;
    Collider2D panelCollider;

    [HideInInspector]
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        panelCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            float width = rectTransform.GetWidth() / 2;
            rectTransform.anchoredPosition = new Vector2(
                Mathf.Clamp(Input.mousePosition.x, width, (float)Screen.width - width), 
                rectTransform.anchoredPosition.y);
        }
    }
}
