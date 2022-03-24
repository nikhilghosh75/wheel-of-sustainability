using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkSystem : MonoBehaviour
{
    public List<GameObject> parkObjects;

    public float minXPosition;
    public float maxXPosition;
    public float spacing;

    public float height;
    public float safeZone;

    [HideInInspector]
    public bool isActive;

    [HideInInspector]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
            return;

        if (PlayerSettings.simpleBackgrounds)
            return;

        float maxPosition = -safeZone;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            child.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

            if (child.anchoredPosition.y < -safeZone)
            {
                Destroy(child.gameObject);
                continue;
            }

            float upPosition = child.anchoredPosition.y + child.GetHeight();
            if(maxPosition < upPosition)
            {
                maxPosition = upPosition;
            }
        }

        if(maxPosition < height + safeZone)
        {
            int randIndex = (int)Random.Range(0f, (float)parkObjects.Count);
            GameObject spawnedObject = Instantiate(parkObjects[randIndex], transform);
            RectTransform rectTransform = spawnedObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(Random.Range(minXPosition, maxXPosition), 
                maxPosition + (rectTransform.GetHeight() / 2) + spacing);
        }
    }

    public void Clean()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
