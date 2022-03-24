using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 40;
    RectTransform rectTransform;

    [HideInInspector]
    public CloudSpawner cloudSpawner;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cloudSpawner.isPaused) return;

        Vector2 position = rectTransform.anchoredPosition;
        position.y -= speed * Time.deltaTime;
        rectTransform.anchoredPosition = position;
    }
}
