using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct PickupInfo
    {
        public GameObject gameObject;
        public float probability;
        public DifficultyFloat timeLimitBeforeEnd;
    }

    public ScreenSwitcher screenSwitcher;

    public IMinigame owningMinigame;
    public List<PickupInfo> pickupInfos;

    public GameObject bonusPrefab;

    public float ySpawnPosition = -100f;
    public float minXSpawnPosition = 0;
    public float maxXSpawnPosition = 1980;

    public DifficultyFloat minSpawnTime = new DifficultyFloat();
    public float maxSpawnTime = 2.0f;
    public float spawnTimeRange = 0.15f;
    public float endTime = 8f;

    [HideInInspector]
    public float minigameTime;

    [HideInInspector]
    public bool isSpawning = false;

    [HideInInspector]
    public bool isPaused = false;

    float minigameStartTime = 0;

    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnPickups());

        if(screenSwitcher.minigameMode == MinigameMode.Wheel)
        {
            float randTime = Random.Range(0, minigameTime - 10);
            Invoke("SpawnBonus", randTime);
        }

        minigameStartTime = Time.time;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void ResetObjects()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator SpawnPickups()
    {
        while (isSpawning)
        {
            if (!isPaused)
            {
                int randomIndex = GetRandomIndex();
                if(CanSpawnPickup(pickupInfos[randomIndex]))
                {
                    GameObject spawnedObject = Instantiate(pickupInfos[randomIndex].gameObject, this.transform);
                    spawnedObject.transform.position = new Vector3(Random.Range(minXSpawnPosition, maxXSpawnPosition), ySpawnPosition, 0);

                    MinigameTracker tracker = spawnedObject.GetComponent<MinigameTracker>();
                    if (tracker != null)
                    {
                        tracker.minigame = owningMinigame;
                    }
                }
            }

            yield return new WaitForSeconds(GetRandomWaitTime());
        }
    }

    int GetRandomIndex()
    {
        float randomFloat = Random.Range(0.0f, 1.0f);
        Debug.Log(randomFloat);
        float currentFloat = 0;
        for(int i = 0; i < pickupInfos.Count; i++)
        {
            if(currentFloat + pickupInfos[i].probability > randomFloat)
            {
                return i;
            }
            currentFloat += pickupInfos[i].probability;
        }

        return 0;
    }

    float GetRandomWaitTime()
    {
        float currentMinigameTime = Time.time - minigameStartTime;
        float clampedMinigameTime = Mathf.Clamp(currentMinigameTime, 0, minigameTime - endTime);
        float initialTime = Mathf.Lerp(maxSpawnTime, minSpawnTime.Get(), clampedMinigameTime / (minigameTime - endTime));
        float randTime = Random.Range(-spawnTimeRange, spawnTimeRange);

        Debug.Log("Spawn Time: " + initialTime + " Rand Time: " + randTime);

        return Mathf.Clamp(initialTime + randTime, minSpawnTime.Get(), maxSpawnTime);
    }

    void SpawnBonus()
    {
        GameObject spawnedObject = Instantiate(bonusPrefab, this.transform);
        spawnedObject.transform.position = new Vector3(Random.Range(minXSpawnPosition, maxXSpawnPosition), ySpawnPosition, 0);

        MinigameTracker tracker = spawnedObject.GetComponent<MinigameTracker>();
        if (tracker != null)
        {
            tracker.minigame = owningMinigame;
        }
    }

    bool CanSpawnPickup(PickupInfo info)
    {
        float currentMinigameTime = Time.time - minigameStartTime;
        float timeLeftInMinigame = minigameTime - currentMinigameTime;

        if (timeLeftInMinigame < info.timeLimitBeforeEnd.Get())
            return false;
        return true;
    }
}
