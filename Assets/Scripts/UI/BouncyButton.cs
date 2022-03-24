using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BouncyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 raisedPosition;
    public float transitionTime;
    Vector2 originalPosition;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        rectTransform.anchoredPosition = originalPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(DoTransitionTo(raisedPosition));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(DoTransitionTo(originalPosition));
    }

    IEnumerator DoTransitionTo(Vector2 newPosition)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;

        float currentTime = 0;
        while(currentTime < transitionTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, newPosition, currentTime / transitionTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = newPosition;
    }
}
