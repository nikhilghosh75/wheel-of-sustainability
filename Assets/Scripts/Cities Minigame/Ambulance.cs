using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambulance : MonoBehaviour
{
    int lane = 0;
    bool isSwitching = false;

    public List<float> lanePositions;

    public float switchTime;

    [HideInInspector]
    public bool isActive = true;

    RectTransform rectTransform;

    [HideInInspector]
    public CitiesMinigame citiesMinigame;

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

    IEnumerator DoSwitchLeft()
    {
        if (isSwitching)
            yield break;

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
    }

    IEnumerator DoSwitchRight()
    {
        if (isSwitching)
            yield break;

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
    }

    static float EaseInOutSine(float f)
    {
        return -(Mathf.Cos(Mathf.PI * f) - 1) / 2;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Traffic"))
        {
            citiesMinigame.EndGame();
        }
        else if(collider.gameObject.CompareTag("Bonus"))
        {
            BonusManager.currentBonuses++;
            Destroy(collider.gameObject);
        }
    }
}
