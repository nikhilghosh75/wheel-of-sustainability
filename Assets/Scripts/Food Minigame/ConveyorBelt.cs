using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject conveyorBeltPrefab;

    [HideInInspector]
    public float speed;

    public float safeZone;

    public float beltWidth;

    const float width = 960;

    // Start is called before the first frame update
    void Start()
    {
        SpawnConveyorBelt();
    }

    // Update is called once per frame
    void Update()
    {
        float scrollDelta = speed * Time.deltaTime;
        float minPosition = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();

            child.anchoredPosition = new Vector2(child.anchoredPosition.x + scrollDelta, 0);
            if (child.anchoredPosition.x > width + safeZone + beltWidth)
            {
                Destroy(child.gameObject);
            }

            if (child.position.x < minPosition)
            {
                minPosition = child.GetComponent<RectTransform>().anchoredPosition.x;
            }
        }

        if (minPosition > 0 - safeZone)
        {
            SpawnConveyorBelt();
        }
    }

    void SpawnConveyorBelt()
    {
        GameObject spawnedBelt = Instantiate(conveyorBeltPrefab, transform);
        spawnedBelt.GetComponent<RectTransform>().anchoredPosition = GetPositionOfNewBelt();
    }

    Vector3 GetPositionOfNewBelt()
    {
        if (transform.childCount == 0)
        {
            return new Vector3(0, 0, 0);
        }

        Transform child = transform.GetChild(transform.childCount - 1);
        RectTransform rectTransform = child.GetComponent<RectTransform>();
        float minPosition = rectTransform.anchoredPosition.x - beltWidth;
        return new Vector3(minPosition, 0, 0);
    }
}
