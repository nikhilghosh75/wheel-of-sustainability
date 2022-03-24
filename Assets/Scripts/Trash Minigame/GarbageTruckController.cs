using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageTruckController : MonoBehaviour
{
    int lane = 0;
    bool isSwitching = false;
    int numTrash = 0;

    bool queueSwitchLeft = false;
    bool queueSwitchRight = false;

    public List<float> lanePositions;

    public List<Vector2> garbagePositions;

    public GameObject trashPrefab;

    public float switchTime;

    [HideInInspector]
    public bool isActive = true;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if(lane != 0)
                {
                    StartCoroutine(DoSwitchLeft());
                }
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(lane != lanePositions.Count - 1)
                {
                    StartCoroutine(DoSwitchRight());
                }
            }
        }
    }

    public void ClearTrash()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        numTrash = 0;
    }

    public void ResetTruck()
    {
        queueSwitchLeft = false;
        queueSwitchRight = false;
    }

    public void AddTrash()
    {
        if (numTrash + 1 == garbagePositions.Count || garbagePositions.Count == 0)
            return;

        GameObject spawnedTrash = Instantiate(trashPrefab, this.transform);
        RectTransform spawnedTransform = spawnedTrash.GetComponent<RectTransform>();
        spawnedTransform.anchoredPosition = garbagePositions[numTrash];
        numTrash++;
    }

    IEnumerator DoSwitchLeft()
    {
        if (isSwitching)
        {
            queueSwitchLeft = true;
            queueSwitchRight = false;
            yield break;
        }

        isSwitching = true;

        float currentTime = 0;
        while (currentTime < switchTime)
        {
            float easePosition = EaseInOutSine(currentTime / switchTime);
            float xPosition = Mathf.Lerp(lanePositions[lane], lanePositions[lane - 1], easePosition);
            rectTransform.anchoredPosition = new Vector2(xPosition, rectTransform.anchoredPosition.y);
            currentTime += Time.deltaTime;
            yield return null;
        }

        lane = lane - 1;
        isSwitching = false;

        HandleQueuedInput();
    }

    IEnumerator DoSwitchRight()
    {
        if (isSwitching)
        {
            queueSwitchLeft = false;
            queueSwitchRight = true;
            yield break;
        }

        isSwitching = true;

        float currentTime = 0;
        while(currentTime < switchTime)
        {
            float easePosition = EaseInOutSine(currentTime / switchTime);
            float xPosition = Mathf.Lerp(lanePositions[lane], lanePositions[lane + 1], easePosition);
            rectTransform.anchoredPosition = new Vector2(xPosition, rectTransform.anchoredPosition.y);
            currentTime += Time.deltaTime;
            yield return null;
        }

        lane = lane + 1;
        isSwitching = false;

        HandleQueuedInput();
    }

    static float EaseInOutSine(float f)
    {
        return -(Mathf.Cos(Mathf.PI * f) - 1) / 2;
    }

    void HandleQueuedInput()
    {
        if(queueSwitchLeft)
        {
            StartCoroutine(DoSwitchLeft());
        }
        else if(queueSwitchRight)
        {
            StartCoroutine(DoSwitchRight());
        }

        queueSwitchLeft = false;
        queueSwitchRight = false;
    }
}
