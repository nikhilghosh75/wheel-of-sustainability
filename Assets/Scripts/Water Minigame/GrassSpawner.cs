using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    public GameObject grassPrefab;
    RectTransform rectTransform;
    public float padding;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetGrass()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void AddGrasses(int numGrass)
    {
        for (int i = 0; i < numGrass; i++)
            AddGrass();
    }

    public void AddGrass()
    {
        if (PlayerSettings.simpleBackgrounds)
            return;

        Vector2 newPosition = new Vector2(Random.Range(padding, rectTransform.GetWidth() - padding), 
            Random.Range(padding, rectTransform.GetHeight() - padding));
        GameObject spawnedGrass = Instantiate(grassPrefab, transform);
        spawnedGrass.GetComponent<RectTransform>().anchoredPosition = newPosition;
    }
}
