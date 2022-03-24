using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]
public struct Swipe
{
    public Vector2 start;
    public Vector2 end;
    public float time;

    public Vector2 GetVelocity()
    {
        return (end - start) / time;
    }

    public bool Within(Rect rect)
    {
        return rect.Contains(start) && rect.Contains(end);
    }
}

[System.Serializable]
public class SwipeEvent : UnityEvent<Swipe> { }

public class SwipeController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SwipeEvent OnSwipe;

    public static SwipeController controller;

    Vector2 startSwipePosition;
    float lastSwipeTime;

    void Awake()
    {
        controller = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastSwipeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startSwipePosition = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Swipe swipe;
        swipe.start = startSwipePosition;
        swipe.end = Input.mousePosition;
        swipe.time = Time.time - lastSwipeTime;

        OnSwipe.Invoke(swipe);
        lastSwipeTime = Time.time;
    }
}
