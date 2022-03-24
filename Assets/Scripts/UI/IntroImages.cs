using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroImageConfig
{
    public Sprite sprite;
    public string text;

    public IntroImageConfig(Sprite sprite, string text)
    {
        this.sprite = sprite;
        this.text = text;
    }
}

public class IntroImages : MonoBehaviour
{
    public GameObject prefab;

    public void SetIntroImages(List<IntroImageConfig> configs)
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < configs.Count; i++)
        {
            GameObject spawnedPrefab = Instantiate(prefab, transform);
            spawnedPrefab.GetComponent<IntroImageSlot>().SetIntroImage(configs[i]);
        }
    }
}
