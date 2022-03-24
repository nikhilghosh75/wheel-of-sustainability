using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterCan : MonoBehaviour
{
    public GameObject canPrefab;
    public float radius;

    public void AddObject(Sprite sprite)
    {
        GameObject spawnedPrefab = Instantiate(canPrefab, transform);
        spawnedPrefab.GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));
        spawnedPrefab.GetComponent<Image>().sprite = sprite;
        spawnedPrefab.GetComponent<ResizeOnSprite>().ResizeBasedOnSprite();
    }

    public void Clean()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
