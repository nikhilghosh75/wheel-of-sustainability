using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public List<GameObject> plantPrefabs;
    public float plantTime = 4.0f;

    [HideInInspector]
    public ConservationMinigame conservationMinigame;

    bool planted = false;
    bool canPlant = false;
    float progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(planted)
        {
            // conservationMinigame.progressSlider.gameObject.SetActive(false);
            return;
        }

        if(canPlant && Input.GetKey(KeyCode.Space))
        {
            progress += Time.deltaTime / plantTime;
            conservationMinigame.progressSlider.gameObject.SetActive(true);
            conservationMinigame.progressSlider.value = progress;
            if(progress > 1.0f)
            {
                Plant();
            }
        }
        else if(canPlant)
        {
            conservationMinigame.progressSlider.gameObject.SetActive(false);
        }
    }

    public void Plant()
    {
        if (planted) return;

        int plantToSpawn = Random.Range(0, plantPrefabs.Count);
        GameObject spawnedPlant = Instantiate(plantPrefabs[plantToSpawn], transform);
        spawnedPlant.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        planted = true;
        canPlant = false;

        conservationMinigame.progressSlider.gameObject.SetActive(false);
        conservationMinigame.OnPlant();
    }

    public void Unplant()
    {
        planted = false;
        progress = 0.0f;

        for(int i = transform.childCount - 1; i >= 2; i--)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public bool IsPlanted() { return planted; }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            conservationMinigame.SetCurrentSoil(this);
            canPlant = !planted;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            conservationMinigame.SetCurrentSoil(null);
            canPlant = false;
        }
    }
}
