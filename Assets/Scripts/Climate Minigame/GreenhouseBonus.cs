using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenhouseBonus : MonoBehaviour
{
    public float speed;
    public float deleteXPosition;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);
        if(rectTransform.anchoredPosition.x > deleteXPosition)
        {
            Destroy(gameObject);
        }
    }
}
