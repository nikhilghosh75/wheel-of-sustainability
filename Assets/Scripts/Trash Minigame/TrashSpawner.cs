using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [HideInInspector]
    public float speed;

    [Header("Trash Prefabs")]
    public List<GameObject> trashPrefabs;

    [Header("Obstacle Prefabs")]
    public List<GameObject> obstaclePrefabs;

    public GameObject bonusPrefab;

    public List<float> lanePositions;

    Coroutine spawnCoroutine;

    public float spawnTime;

    public float spawnYPosition;

    public float safeZone;

    [HideInInspector]
    public float minigameTime;

    void Update()
    {
        float scrollDelta = speed * Time.deltaTime;
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            RectTransform rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            rectTransform.anchoredPosition -= new Vector2(0, scrollDelta);

            if(rectTransform.anchoredPosition.y < -safeZone)
            {
                Destroy(rectTransform.gameObject);
            }
        }
    }

    public void StartSpawning()
    {
        // InvokeRepeating("DoSpawn", 0, spawnTime);
        StartCoroutine(DoSpawn());

        if (ScreenSwitcher.switcher.minigameMode == MinigameMode.Wheel)
        {
            float randTime = Random.Range(0, minigameTime - 10);
            Invoke("SpawnBonus", randTime);
        }
    }

    public void StopSpawning()
    {
        // CancelInvoke("DoSpawn");
        StopAllCoroutines();
    }

    public void Reset()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void Spawn()
    {
        int trashSpawnLane = Random.Range(0, 3);
        int noSpawnLane = Random.Range(0, 3);

        for (int i = 0; i < 4; i++)
        {
            if (i == trashSpawnLane)
            {
                SpawnTrash(i);
            }
            else if (i != noSpawnLane)
            {
                SpawnObstacle(i);
            }
        }
    }

    void SpawnTrash(int lane)
    {
        // GameObject prefabToSpawn = trashPrefabs[0];
        GameObject spawnedTrash = Instantiate(trashPrefabs[0], transform);
        spawnedTrash.GetComponent<RectTransform>().anchoredPosition = new Vector2(lanePositions[lane], spawnYPosition);
    }

    void SpawnObstacle(int lane)
    {
        // GameObject prefabToSpawn = obstaclePrefabs[0];
        GameObject spawnedObstacle = Instantiate(obstaclePrefabs[0], transform);
        spawnedObstacle.GetComponent<RectTransform>().anchoredPosition = new Vector2(lanePositions[lane], spawnYPosition);
    }

    void SpawnBonus()
    {
        int spawnLane = Random.Range(0, 3);

        GameObject spawnedBonus = Instantiate(bonusPrefab, transform);
        spawnedBonus.GetComponent<RectTransform>().anchoredPosition = new Vector2(lanePositions[spawnLane], spawnYPosition);
    }

    IEnumerator DoSpawn()
    {
        while(true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
