using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public List<GameObject> foodObjects;

    public GameObject organicLabel;

    public float spawnXPosition;

    public float minSpawnYPosition;

    public float maxSpawnYPosition;

    public float maxXPosition;

    public float deleteXPosition;

    public float foodSpawnTime;

    [Header("Bonus")]
    public GameObject bonusObject;
    public float bonusYPosition;
    public ScreenSwitcher screenSwitcher;

    [HideInInspector]
    public float foodSpeed;

    [HideInInspector]
    public FoodMinigame minigame;

    Coroutine spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        FoodItem.organicPrefab = organicLabel;
    }

    // Update is called once per frame
    void Update()
    {
        int deleteIndex = -1;
        for(int i = 0; i < transform.childCount; i++)
        {
            RectTransform rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            rectTransform.anchoredPosition += new Vector2(foodSpeed * Time.deltaTime, 0);

            /*
            if(rectTransform.anchoredPosition.x > maxXPosition && rectTransform.anchoredPosition.x < maxXPosition + 10)
            {
                FoodItem foodItem = rectTransform.GetComponent<FoodItem>();
                foodItem.JudgeLabel(minigame);
            }
            */

            if(deleteIndex == -1 && rectTransform.anchoredPosition.x > deleteXPosition)
            {
                deleteIndex = i;
            }
        }

        if(deleteIndex != -1)
        {
            Destroy(transform.GetChild(deleteIndex).gameObject);
        }
    }

    public void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(DoSpawning());
        
        if(screenSwitcher.minigameMode == MinigameMode.Wheel)
        {
            float randTime = Random.Range(0, minigame.minigameTime.Get() - 10);
            Invoke("SpawnBonus", randTime);
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
        Debug.Log("STOP SPAWNING");

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            FoodItem foodItem = transform.GetChild(i).GetComponent<FoodItem>();
            foodItem.JudgeLabel(minigame);
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator DoSpawning()
    {
        while(true)
        {
            int spawnIndex = GetRandomIndex();
            GameObject spawnedObject = Instantiate(foodObjects[spawnIndex], transform);
            RectTransform spawnedTransform = spawnedObject.GetComponent<RectTransform>();
            spawnedTransform.anchoredPosition = new Vector2(spawnXPosition, Random.Range(minSpawnYPosition, maxSpawnYPosition));
            FoodItem foodItem = spawnedObject.GetComponent<FoodItem>();
            foodItem.maxXPosition = maxXPosition;

            yield return new WaitForSeconds(foodSpawnTime);
        }
    }

    int GetRandomIndex()
    {
        return (int)Random.Range(0, foodObjects.Count);
    }

    void SpawnBonus()
    {
        GameObject spawnedObject = Instantiate(bonusObject, transform);
        RectTransform spawnedTransform = spawnedObject.GetComponent<RectTransform>();
        spawnedTransform.anchoredPosition = new Vector2(spawnXPosition, bonusYPosition);
        FoodItem foodItem = spawnedObject.GetComponent<FoodItem>();
        foodItem.maxXPosition = maxXPosition;
    }
}
