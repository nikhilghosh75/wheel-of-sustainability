using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenhouseSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct GreenhouseTroop
    {
        public GameObject prefab;
        public int spawnChance;
        public float timeLimitBeforeEnd;
    }

    public List<GreenhouseTroop> troops;

    public GameObject bonusPrefab;

    [HideInInspector]
    public ClimateMinigame climateMinigame;

    public float minSpawnX;
    public float maxSpawnX;
    public float spawnY;
    public Vector2 bonusSpawn;
    public float deleteY;

    public DifficultyFloat minSpawnTime = new DifficultyFloat();
    public DifficultyFloat maxSpawnTime = new DifficultyFloat();
    public float spawnTimeRange = 0.15f;
    public float endTime = 8f;

    [HideInInspector]
    public float minigameTime;

    int sum;

    float minigameStartTime = 0;

    Coroutine spawnCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        ComputeSum();
    }

    // Update is called once per frame
    void Update()
    {
        int deleteIndex = -1;
        for(int i = 0; i < transform.childCount; i++)
        {
            RectTransform rectTransform = transform.GetChild(i).GetComponent<RectTransform>();

            if(rectTransform.anchoredPosition.y < deleteY)
            {
                deleteIndex = i;
            }
        }

        if(deleteIndex != -1)
        {
            GameObject childObject = transform.GetChild(deleteIndex).gameObject;

            if(childObject.name[0] == 'B')
            {
                BonusManager.currentBonuses++;
            }
            else
            {
                climateMinigame.OnHit();
            }

            Destroy(childObject);
        }
    }

    public void StartSpawning()
    {
        // InvokeRepeating("DoSpawn", 0.1f, spawnTime);
        spawnCoroutine = StartCoroutine(SpawnGreenhouse());

        if(ScreenSwitcher.switcher.minigameMode == MinigameMode.Wheel)
        {
            float randTime = Random.Range(0, minigameTime - 10);
            Invoke("SpawnBonus", randTime);
        }

        minigameStartTime = Time.time;
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void DoSpawn()
    {
        int randIndex = Random.Range(0, sum);
        int spawnIndex = GetSpawnIndex(randIndex);

        if (!CanSpawnTroop(troops[spawnIndex]))
            return;

        GameObject spawnedGas = Instantiate(troops[spawnIndex].prefab, transform);
        RectTransform spawnedTransform = spawnedGas.GetComponent<RectTransform>();
        spawnedTransform.anchoredPosition = new Vector2(Random.Range(minSpawnX, maxSpawnX), spawnY);
        spawnedGas.GetComponent<GreenhouseHealth>().minigame = climateMinigame;
    }

    int GetSpawnIndex(int randIndex)
    {
        int currentSum = 0;
        for(int i = 0; i < troops.Count; i++)
        {
            currentSum += troops[i].spawnChance;
            if(currentSum > randIndex)
            {
                return i;
            }
        }
        return 0;
    }

    void SpawnBonus()
    {
        GameObject spawnedBonus = Instantiate(bonusPrefab, transform);
        RectTransform spawnedTransform = spawnedBonus.GetComponent<RectTransform>();
        spawnedTransform.anchoredPosition = bonusSpawn;
        spawnedBonus.GetComponent<GreenhouseHealth>().minigame = climateMinigame;
    }

    void ComputeSum()
    {
        sum = 0;
        for (int i = 0; i < troops.Count; i++)
        {
            sum += troops[i].spawnChance;
        }
    }

    IEnumerator SpawnGreenhouse()
    {
        while(true)
        {
            DoSpawn();
            yield return new WaitForSeconds(GetRandomWaitTime());
        }
    }

    float GetRandomWaitTime()
    {
        float currentMinigameTime = Time.time - minigameStartTime;
        float clampedMinigameTime = Mathf.Clamp(currentMinigameTime, 0, minigameTime - endTime);
        float initialTime = Mathf.Lerp(maxSpawnTime.Get(), minSpawnTime.Get(), clampedMinigameTime / (minigameTime - endTime));
        float randTime = Random.Range(-spawnTimeRange, spawnTimeRange);

        Debug.Log("Spawn Time: " + initialTime + " Rand Time: " + randTime);

        return Mathf.Clamp(initialTime + randTime, minSpawnTime.Get(), maxSpawnTime.Get());
    }

    bool CanSpawnTroop(GreenhouseTroop troop)
    {
        float currentMinigameTime = Time.time - minigameStartTime;
        float timeLeftInMinigame = minigameTime - currentMinigameTime;

        if (timeLeftInMinigame < troop.timeLimitBeforeEnd)
            return false;
        return true;
    }
}
