using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour
{
    public float speed;

    [HideInInspector]
    public CitiesMinigame minigame;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = minigame.GetCurrentDistance();
        int percent = (int)((distance / minigame.distanceToHospital.Get()) * 100);
        if (percent > 93)
        {
            speed = 0;
        }

        float movementSpeed = minigame.GetCurrentSpeed() - speed;
        rectTransform.anchoredPosition -= new Vector2(0, movementSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Traffic Remover"))
        {
            Destroy(gameObject);
        }
    }
}
