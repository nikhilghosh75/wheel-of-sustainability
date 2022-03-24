using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicycle : MonoBehaviour
{
    int lane = 0;
    bool isSwitching = false;
    RectTransform rectTransform;

    bool queueSwitchLeft = false;
    bool queueSwitchRight = false;

    public List<float> lanePositions;

    public float switchTime;

    [HideInInspector]
    public MobilityMinigame minigame;

    [HideInInspector]
    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (lane != 0)
                {
                    StartCoroutine(DoSwitchLeft());
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (lane != lanePositions.Count - 1)
                {
                    StartCoroutine(DoSwitchRight());
                }
            }
        }
    }

    public void ResetBicycle()
    {
        queueSwitchLeft = false;
        queueSwitchRight = false;
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
        while (currentTime < switchTime)
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

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Coin"))
        {
            minigame.OnCoinCollected();
            Destroy(collider2D.gameObject);
        }
        else if(collider2D.CompareTag("Obstacle"))
        {
            minigame.OnObstacleHit();
            Destroy(collider2D.gameObject);
        }
        else if(collider2D.CompareTag("Bonus"))
        {
            BonusManager.currentBonuses++;
            Destroy(collider2D.gameObject);
        }
    }

    void HandleQueuedInput()
    {
        if (queueSwitchLeft)
        {
            StartCoroutine(DoSwitchLeft());
        }
        else if (queueSwitchRight)
        {
            StartCoroutine(DoSwitchRight());
        }

        queueSwitchLeft = false;
        queueSwitchRight = false;
    }
}
