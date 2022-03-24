using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public ConservationMinigame minigame;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float time;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void StartAnimal()
    {
        StartCoroutine(DoBunny());
    }

    public void StopAnimal()
    {
        StopAllCoroutines();
    }

    IEnumerator DoBunny()
    {
        rectTransform.anchoredPosition = startPosition;

        float currentTime = 0;
        while(currentTime < time)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;
    }
}
