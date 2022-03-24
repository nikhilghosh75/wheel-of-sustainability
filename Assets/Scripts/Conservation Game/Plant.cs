using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    [System.Serializable]
    public struct PlantStage
    {
        public float lifetime;
        public Sprite sprite;
    }

    public List<PlantStage> plantStages;
    public List<GameObject> plantExtensions;

    Image image;
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        StartCoroutine(DoPlantLifetime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnExtension()
    {
        if (plantExtensions.Count == 0)
            return;

        if(Random.Range(0.0f, (float)1.0f) < 0.45f)
        {
            int randIndex = Random.Range(0, plantExtensions.Count);
            Instantiate(plantExtensions[randIndex], transform.parent);
        }
    }

    IEnumerator DoPlantLifetime()
    {
        float width = rectTransform.GetWidth();

        for(int i = 0; i < plantStages.Count; i++)
        {
            image.sprite = plantStages[i].sprite;

            float imageRatio = plantStages[i].sprite.rect.height / plantStages[i].sprite.rect.width;
            rectTransform.SetHeight(imageRatio * width);

            yield return new WaitForSeconds(plantStages[i].lifetime);
        }
        SpawnExtension();
    }
}
