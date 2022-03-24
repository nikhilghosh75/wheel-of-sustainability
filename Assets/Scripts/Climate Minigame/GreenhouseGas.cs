using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenhouseGas : MonoBehaviour
{
    public float ySpeed;

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition -= new Vector2(0, ySpeed * Time.deltaTime);
    }
}
