using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
    [HideInInspector]
    public bool isActive = false;

    public float downDistance;
    public float downTime;

    bool isDown = false;

    RectTransform rectTransform;
    BoxCollider2D collider2D;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive && !isDown)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine(GoDown());
            }
            if(!isDown)
            {
                float width = rectTransform.GetWidth() / 2;
                float height = rectTransform.GetHeight() / 2;
                rectTransform.anchoredPosition = new Vector2(
                    Mathf.Clamp(Input.mousePosition.x, width, (float)Screen.width - width),
                    Mathf.Clamp(Input.mousePosition.y, height, (float)Screen.height - height));
            }
        }
    }

    IEnumerator GoDown()
    {
        isDown = true;
        float downSpeed = downDistance / downTime;
        float currentTime = 0;

        while(currentTime < downTime)
        {
            rectTransform.anchoredPosition -= new Vector2(0, downSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        StampDown();

        currentTime = 0;
        while (currentTime < downTime)
        {
            rectTransform.anchoredPosition += new Vector2(0, downSpeed * Time.deltaTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        isDown = false;
    }

    void StampDown()
    {
        Collider2D[] colliders = new Collider2D[5];
        ContactFilter2D filter = new ContactFilter2D();
        int numColliders = collider2D.OverlapCollider(filter, colliders);

        for(int i = 0; i < numColliders; i++)
        {
            FoodItem item = colliders[i].GetComponent<FoodItem>();
            if(item != null)
            {
                item.LabelProcessed();
            }
        }
    }
}
