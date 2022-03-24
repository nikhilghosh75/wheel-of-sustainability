using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloud;
    int currentBonus;

    public float minScale = 0.25f;
    public float maxScale = 1.2f;

    public float ySpawnPosition = -100f;
    public float minXSpawnPosition = 0;
    public float maxXSpawnPosition = 1980;

    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 2.0f;

    [HideInInspector]
    public float minigameTime;

    [HideInInspector]
    public bool isSpawning = false;

    [HideInInspector]
    public bool isPaused = false;

    public void StartSpawning()
    {
        if (PlayerSettings.simpleBackgrounds)
            return;

        isSpawning = true;
        StartCoroutine(SpawnClouds());
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void ResetObjects()
    {
        Debug.Log("Reset Objects");
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    IEnumerator SpawnClouds()
    {
        while(isSpawning)
        {
            GameObject spawnedCloud = Instantiate(cloud, this.transform);
            spawnedCloud.transform.position = new Vector3(Random.Range(minXSpawnPosition, maxXSpawnPosition), ySpawnPosition, 0);
            float newScale = Random.Range(minScale, maxScale);
            spawnedCloud.transform.localScale = new Vector3(newScale, newScale, 1);
            spawnedCloud.GetComponent<Cloud>().cloudSpawner = this;
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
}
