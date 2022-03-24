using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public List<GameObject> planets;
    public DifficultyInt numToSpawn;

    List<RectTransform> spawnedPlanets = new List<RectTransform>();
    List<bool> isSpawned = new List<bool>();

    bool isSpawning = false;

    public float ySpawnPosition = -100f;
    public float minXSpawnPosition = 0;
    public float maxXSpawnPosition = 1980;
    public float speed = 20;

    [HideInInspector]
    public ClimateMinigame climateMinigame;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spawnedPlanets.Count; i++)
        {
            spawnedPlanets[i].anchoredPosition -= new Vector2(0, speed * Time.deltaTime);
        }
    }

    public void StartSpawning()
    {
        if (PlayerSettings.simpleBackgrounds)
            return;

        isSpawned.Clear();
        for(int i = 0; i < planets.Count; i++)
        {
            isSpawned.Add(false);
        }

        isSpawning = true;
        StartCoroutine(DoSpawn());
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    public void ResetPlanets()
    {
        spawnedPlanets.Clear();

        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator DoSpawn()
    {
        float minigameTime = climateMinigame.time.Get();
        for(int i = 0; i < numToSpawn.Get(); i++)
        {
            int randIndex = GetIndexToSpawnAt();

            GameObject spawnedCloud = Instantiate(planets[randIndex], this.transform);
            RectTransform spawnedRect = spawnedCloud.GetComponent<RectTransform>();
            spawnedRect.anchoredPosition = new Vector3(Random.Range(minXSpawnPosition, maxXSpawnPosition), ySpawnPosition, 0);
            spawnedPlanets.Add(spawnedRect);

            isSpawned[randIndex] = true;

            yield return new WaitForSeconds(minigameTime / numToSpawn.Get());
        }
    }

    int GetIndexToSpawnAt()
    {
        int randIndex = Random.Range(0, planets.Count);
        if (!isSpawned[randIndex])
            return randIndex;

        for(int i = 0; i < planets.Count; i++)
        {
            int index = (randIndex + i) % planets.Count;
            if (!isSpawned[index])
                return index;
        }

        return 0;
    }
}
