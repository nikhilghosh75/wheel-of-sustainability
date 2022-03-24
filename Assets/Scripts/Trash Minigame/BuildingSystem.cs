using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    enum Lane
    {
        Left,
        Right
    }

    public GameObject buildingPrefab;
    public float buildingWidth;

    public float leftLaneXPosition;
    public float rightLaneXPosition;

    public float safeZone;
    public float padding = 20;

    public List<Sprite> sprites;

    [HideInInspector]
    public float speed;

    [HideInInspector]
    public bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSettings.simpleBackgrounds)
            return;

        float maxLeftPosition = -500;
        float maxRightPosition = -500;
        int maxLeftObject = -1;
        int maxRightObject = -1;

        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            child.anchoredPosition -= new Vector2(0, speed * Time.deltaTime);
            
            if(child.anchoredPosition.y < -safeZone)
            {
                Destroy(child.gameObject);
            }

            Lane objectLane = GetLane(child.anchoredPosition.x);
            switch (objectLane)
            {
            case Lane.Left:
                if (child.anchoredPosition.y > maxLeftPosition)
                {
                    maxLeftPosition = child.anchoredPosition.y;
                    maxLeftObject = i;
                }
                break;
            case Lane.Right:
                if (child.anchoredPosition.y > maxRightPosition)
                {
                    maxRightPosition = child.anchoredPosition.y;
                    maxRightObject = i;
                }
                break;
            }
        }

        // Debug.Log(maxLeftPosition);

        if(maxLeftPosition < 960 + safeZone)
        {
            int spriteIndex = Random.Range(0, sprites.Count);
            float scaleFactor = buildingWidth / sprites[spriteIndex].rect.width;
            float spriteHeight = scaleFactor * sprites[spriteIndex].rect.height;

            float maxSpriteHeight = 0;
            if(maxLeftObject >= 0)
            {
                maxSpriteHeight = transform.GetChild(maxLeftObject).GetComponent<RectTransform>().GetHeight();
            }

            float spawnYPosition = maxLeftPosition + spriteHeight / 2 + maxSpriteHeight / 2 + padding;
            // Debug.Log(spawnYPosition);
            SpawnBuilding(spriteIndex, Lane.Left, spawnYPosition);
        }

        if (maxRightPosition < 960 + safeZone)
        {
            int spriteIndex = Random.Range(0, sprites.Count);
            float scaleFactor = buildingWidth / sprites[spriteIndex].rect.width;
            float spriteHeight = scaleFactor * sprites[spriteIndex].rect.height;

            float maxSpriteHeight = 0;
            if (maxRightObject >= 0)
            {
                maxSpriteHeight = transform.GetChild(maxRightObject).GetComponent<RectTransform>().GetHeight();
            }

            float spawnYPosition = maxRightPosition + spriteHeight / 2 + maxSpriteHeight / 2 + padding;
            // Debug.Log(spawnYPosition);
            SpawnBuilding(spriteIndex, Lane.Right, spawnYPosition);
        }
    }

    void SpawnBuilding(int spriteIndex, Lane lane, float spawnYPosition)
    {
        if (!isSpawning)
            return;

        float spawnXPosition = 0;

        switch (lane)
        {
            case Lane.Left: spawnXPosition = leftLaneXPosition; break;
            case Lane.Right: spawnXPosition = rightLaneXPosition; break;
        }

        GameObject spawnedObject = Instantiate(buildingPrefab, this.transform);
        RectTransform rectTransform = spawnedObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(spawnXPosition, spawnYPosition);

        Image image = spawnedObject.GetComponent<Image>();
        image.sprite = sprites[spriteIndex];

        ResizeOnSprite resize = spawnedObject.GetComponent<ResizeOnSprite>();
        resize.maxSize = new Vector2(buildingWidth, buildingWidth * 6);
        resize.ResizeBasedOnSprite();
    }

    Lane GetLane(float xPosition)
    {
        if (Mathf.Approximately(xPosition, leftLaneXPosition))
                return Lane.Left;

        return Lane.Right;
    }
}
