using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public List<GameObject> objects;

    public GameObject bonusPrefab;

    public float spawnYPosition;

    public float minSpawnXPosition;

    public float maxSpawnXPosition;

    public float minYPosition;

    public float deleteYPosition;

    [HideInInspector]
    public float speed;

    public float bonusSpeed;

    [HideInInspector]
    public WaterMinigame minigame;

    [HideInInspector]
    public float minigameTime;

    public float spawnTime;

    public float tolerance;

    GameObject currentBonus = null;

    Coroutine spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBonus != null)
        {
            RectTransform rectTransform = currentBonus.GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(0, bonusSpeed * Time.deltaTime);
        }

        int deleteIndex = -1;
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);

            if (deleteIndex == -1 && rectTransform.anchoredPosition.y < deleteYPosition)
            {
                deleteIndex = i;
            }
        }

        if (deleteIndex != -1)
        {
            WaterItem item = transform.GetChild(deleteIndex).GetComponent<WaterItem>();
            
            if(!item.isBonus)
            {
                if (!item.isTrash)
                {
                    minigame.OnCorrectlyIgnored();
                }
                else
                {
                    minigame.OnTrashIgnored();
                }
            }
            
            Destroy(transform.GetChild(deleteIndex).gameObject);
        }
    }

    public void StartSpawning()
    {
        if(spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(DoSpawning());

            if(ScreenSwitcher.switcher.minigameMode == MinigameMode.Wheel)
            {
                float randTime = Random.Range(0, minigameTime - 10);
                Invoke("SpawnBonus", randTime);
            }
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public GameObject GetPickupAtY(float y)
    {
        if(currentBonus != null)
        {
            RectTransform rectTransform = currentBonus.GetComponent<RectTransform>();
            if (Mathf.Abs(rectTransform.anchoredPosition.y - y) < tolerance)
            {
                return currentBonus;
            }
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            RectTransform rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            if(Mathf.Abs(rectTransform.anchoredPosition.y - y) < tolerance)
            {
                return rectTransform.gameObject;
            }
        }
        return null;
    }

    IEnumerator DoSpawning()
    {
        while (true)
        {
            int spawnIndex = GetRandomIndex();
            GameObject spawnedObject = Instantiate(objects[spawnIndex], transform);
            RectTransform spawnedTransform = spawnedObject.GetComponent<RectTransform>();
            spawnedTransform.anchoredPosition = new Vector2(Random.Range(minSpawnXPosition, maxSpawnXPosition), spawnYPosition);

            yield return new WaitForSeconds(spawnTime);
        }
    }

    int GetRandomIndex()
    {
        return (int)Random.Range(0, objects.Count);
    }

    void SpawnBonus()
    {
        currentBonus = Instantiate(bonusPrefab, transform);
        RectTransform spawnedTransform = currentBonus.GetComponent<RectTransform>();
        spawnedTransform.anchoredPosition = new Vector2(Random.Range(minSpawnXPosition, maxSpawnXPosition), spawnYPosition);
    }
}
