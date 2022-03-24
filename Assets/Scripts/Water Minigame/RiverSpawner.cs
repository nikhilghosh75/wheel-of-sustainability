using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiverSpawner : MonoBehaviour
{
    public GameObject riverPrefab;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public WaterMinigame minigame;

    public float safeZone;

    public float roadHeight;

    public float spawnXPosition;

    public Color startColor;
    public Color endColor;

    const float height = 540;

    // Start is called before the first frame update
    void Start()
    {
        SpawnRoad();
    }

    // Update is called once per frame
    void Update()
    {
        float scrollDelta = speed * Time.deltaTime;
        float maxPosition = 0;
        int maxIndex = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            child.GetComponent<Image>().color = Color.Lerp(startColor,
                endColor, (float)minigame.correctlyCaught / (float)7);

            child.anchoredPosition = new Vector2(spawnXPosition, child.anchoredPosition.y - scrollDelta);
            if (child.anchoredPosition.y < -safeZone - roadHeight)
            {
                Destroy(child.gameObject);
                continue;
            }

            if (child.anchoredPosition.y > maxPosition)
            {
                maxPosition = child.anchoredPosition.y;
                maxIndex = i;
            }
        }

        // Debug.Log(maxPosition);

        if (maxPosition < height + safeZone)
        {
            // Debug.Log(maxIndex + " " + maxPosition);
            SpawnRoad();
        }
    }

    void SpawnRoad()
    {
        GameObject spawnedBelt = Instantiate(riverPrefab, transform);
        spawnedBelt.GetComponent<RectTransform>().anchoredPosition = GetPositionOfNewRoad();
    }

    Vector3 GetPositionOfNewRoad()
    {
        if (transform.childCount == 0)
        {
            return new Vector3(0, 0, 0);
        }

        float maxPosition = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            if (child.anchoredPosition.y > maxPosition)
            {
                maxPosition = child.anchoredPosition.y;
            }
        }
        // Debug.Log(maxPosition);
        return new Vector3(0, maxPosition + roadHeight, 0);
    }
}
