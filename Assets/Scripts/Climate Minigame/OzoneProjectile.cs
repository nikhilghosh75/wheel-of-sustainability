using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OzoneProjectile : MonoBehaviour
{
    public DifficultyFloat speed;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, speed.Get() * Time.deltaTime);
    }
}
