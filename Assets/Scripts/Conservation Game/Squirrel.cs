using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : MonoBehaviour
{
    public ConservationMinigame minigame;
    public float startY;
    public float endY;
    public float time;
    public List<float> xPositions;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void StartAnimal()
    {
        StartCoroutine(DoSquirrel());
    }

    public void StopAnimal()
    {
        StopAllCoroutines();
    }

    IEnumerator DoSquirrel()
    {
        while(true)
        {
            float x = xPositions[Random.Range(0, xPositions.Count)];
            rectTransform.anchoredPosition = new Vector2(x, startY);
            float currentTime = 0;
            while(currentTime < time)
            {
                rectTransform.anchoredPosition = new Vector2(x, Mathf.Lerp(startY, endY, currentTime / time));
                currentTime += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = new Vector2(x, endY);

            yield return new WaitForSeconds(1);
        }
    }
}
