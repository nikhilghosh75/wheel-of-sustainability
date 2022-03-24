using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystem : MonoBehaviour
{
    [HideInInspector]
    public CitiesMinigame minigame;

    public GameObject bonusPrefab;

    public List<GameObject> vehicles;

    public List<float> lanePositions;

    Coroutine spawnCoroutine;

    bool isSpawning = false;

    public float spawnDistance;

    public float spawnYPosition;

    public float spawnSafeZone;

    float lastSpawnDistance;

    public DifficultyInt maxVehiclesOnRoad;
    public DifficultyFloat difficultyStepTime;

    [HideInInspector]
    public float minigameTime;

    int currentMaxVehiclesOnRoad;

    int endSafeLane = 0;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSpawning)
        {
            return;
        }

        float displacement = minigame.GetCurrentSpeed() * Time.deltaTime;
        lastSpawnDistance += displacement;

        if(lastSpawnDistance > spawnDistance && GetNumVehiclesOnRoad() < currentMaxVehiclesOnRoad)
        {
            DoSpawn();
            lastSpawnDistance = 0;
        }

        float distance = minigame.GetCurrentDistance();
        int percent = (int)((distance / minigame.distanceToHospital.Get()) * 100);
        if(percent > 95)
        {
            isSpawning = false;
        }
    }

    public void StartSpawning()
    {
        isSpawning = true;
        lastSpawnDistance = 0;

        endSafeLane = Random.Range(0, lanePositions.Count);

        currentMaxVehiclesOnRoad = 1;
        StartCoroutine(ControlMaxVehicles());

        if(ScreenSwitcher.switcher.minigameMode == MinigameMode.Wheel)
        {
            float randTime = Random.Range(0, minigameTime - 10);
            Invoke("SpawnBonus", randTime);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void ResetTraffic()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public int GetNumVehiclesOnRoad()
    {
        return transform.childCount;
    }

    void DoSpawn()
    {
        int randIndex = Random.Range(0, vehicles.Count);
        int laneIndex = Random.Range(0, lanePositions.Count);
        if (!CanSpawnInLane(laneIndex))
        {
            return;
        }

        GameObject spawnedVehicle = Instantiate(vehicles[randIndex], transform);
        Traffic traffic = spawnedVehicle.GetComponent<Traffic>();
        traffic.minigame = minigame;
        RectTransform rectTransform = spawnedVehicle.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(lanePositions[laneIndex], spawnYPosition);
    }

    bool CanSpawnInLane(int lane)
    {
        float distance = minigame.GetCurrentDistance();
        int percent = (int)((distance / minigame.distanceToHospital.Get()) * 100);
        if (percent > 80 && lane == endSafeLane)
        {
            return false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform childTransform = transform.GetChild(i).GetComponent<RectTransform>();
            int childLane = GetLaneOfXPosition(childTransform.anchoredPosition.x);
            if(childLane != lane)
            {
                continue;
            }

            if (childTransform.anchoredPosition.y > spawnSafeZone - spawnSafeZone)
                return false;
        }
        return true;
    }

    int GetLaneOfXPosition(float xPositon)
    {
        for(int i = 0; i < lanePositions.Count; i++)
        {
            if(Mathf.Approximately(lanePositions[i], xPositon))
            {
                return i;
            }
        }
        return 0;
    }

    void SpawnBonus()
    {
        int laneIndex = Random.Range(0, lanePositions.Count);

        GameObject spawnedVehicle = Instantiate(bonusPrefab, transform);
        Traffic traffic = spawnedVehicle.GetComponent<Traffic>();
        traffic.minigame = minigame;
        RectTransform rectTransform = spawnedVehicle.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(lanePositions[laneIndex], spawnYPosition);
    }

    IEnumerator ControlMaxVehicles()
    {
        for(int i = 2; i <= maxVehiclesOnRoad.Get(); i++)
        {
            yield return new WaitForSeconds(difficultyStepTime.Get());
            currentMaxVehiclesOnRoad = i;
        }
    }
}
