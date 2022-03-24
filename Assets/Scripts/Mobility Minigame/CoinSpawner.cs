using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;

    public GameObject bonusPrefab;

    public float spawnTime = 1.0f;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public float minigameTime;

    public float safeZone;

    public float spawnYPosition;

    public List<float> lanePositions;

    // public List<GameObject> obstacles;
    public GameObject shortObstacle;
    public GameObject longObstacle;
    public float shortProbability;
    public DifficultyFloat obstacleRerollProbability;

    Coroutine spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scrollDelta = speed * Time.deltaTime;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            RectTransform rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(0, scrollDelta);

            if (rectTransform.anchoredPosition.y < -safeZone)
            {
                Destroy(rectTransform.gameObject);
            }
        }
    }

    public void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(SpawnCoins());

        if(ScreenSwitcher.switcher.minigameMode == MinigameMode.Wheel)
        {
            float randTime = Random.Range(0, minigameTime - 10);
            Invoke("SpawnBonus", randTime);
        }
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
    }

    public void Clean()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void DoSpawn(bool spawnShort)
    {
        int laneForCoin = Random.Range(0, lanePositions.Count);
        GameObject spawnedCoin = Instantiate(coinPrefab, transform);
        RectTransform rectTransform = spawnedCoin.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(lanePositions[laneForCoin], spawnYPosition);

        int laneForObstacle = Random.Range(0, lanePositions.Count);
        if(laneForObstacle == laneForCoin)
        {
            bool resetObstacle = Random.Range(0f, 1f) < obstacleRerollProbability.Get();
            if(resetObstacle)
            {
                laneForObstacle = Random.Range(0, 2) == 0 ? ((laneForCoin + 1) % 3) : ((laneForCoin + 2) % 3);
            }
        }

        if(laneForObstacle != laneForCoin)
        {
            GameObject spawnedObstacle = Instantiate(spawnShort ? shortObstacle : longObstacle, transform);
            rectTransform = spawnedObstacle.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(lanePositions[laneForObstacle], spawnYPosition);
        }
    }

    void SpawnBonus()
    {
        int laneForBonus = Random.Range(0, lanePositions.Count);
        GameObject spawnedBonus = Instantiate(bonusPrefab, transform);
        RectTransform rectTransform = spawnedBonus.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(lanePositions[laneForBonus], spawnYPosition);
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            bool spawnShort = Random.Range(0f, 1f) < shortProbability;
            if (!spawnShort)
                yield return new WaitForSeconds(0.06f);
            DoSpawn(spawnShort);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
