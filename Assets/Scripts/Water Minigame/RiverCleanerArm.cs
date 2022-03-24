using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiverCleanerArm : MonoBehaviour
{
    public GameObject topArm;
    public GameObject bottomArm;

    public ResizeOnSprite pickedUpImage;

    public float armDisplacement = 23;

    public float grabTime;
    public float extensionTime;

    public float fullGrabWidth;
    public float partGrabWidth;

    public WaterSpawner waterSpawner;

    [HideInInspector]
    public RiverCleaner riverCleaner;

    public List<WaterCan> cans;

    bool isCurrentItemTrash;

    void Start()
    {
        pickedUpImage.gameObject.SetActive(false);
    }

    public void StartGrab()
    {
        StartCoroutine(DoGrab());
    }

    IEnumerator DoGrab()
    {
        float currentTime = 0;
        RectTransform topTransform = topArm.GetComponent<RectTransform>();
        RectTransform bottomTransform = bottomArm.GetComponent<RectTransform>();

        while (currentTime < grabTime)
        {
            topTransform.anchoredPosition = new Vector2(topTransform.anchoredPosition.x,
                Mathf.Lerp(armDisplacement, 0, currentTime / grabTime));
            bottomTransform.anchoredPosition = new Vector2(topTransform.anchoredPosition.x,
                Mathf.Lerp(-armDisplacement, 0, currentTime / grabTime));

            currentTime += Time.deltaTime;
            yield return null;
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        bool pickedUpObject = PickupObject();

        if(pickedUpObject)
        {
            currentTime = 0;
            while (currentTime < extensionTime)
            {
                rectTransform.SetWidth(Mathf.Lerp(fullGrabWidth, partGrabWidth, currentTime / extensionTime));

                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        currentTime = 0;
        while (currentTime < grabTime)
        {
            topTransform.anchoredPosition = new Vector2(topTransform.anchoredPosition.x,
                Mathf.Lerp(0, armDisplacement, currentTime / grabTime));
            bottomTransform.anchoredPosition = new Vector2(topTransform.anchoredPosition.x,
                Mathf.Lerp(0, -armDisplacement, currentTime / grabTime));

            currentTime += Time.deltaTime;
            yield return null;
        }

        pickedUpImage.gameObject.SetActive(false);

        if(pickedUpObject)
        {
            PutInCan();
            currentTime = 0;
            while (currentTime < extensionTime)
            {
                rectTransform.SetWidth(Mathf.Lerp(partGrabWidth, fullGrabWidth, currentTime / extensionTime));

                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    bool PickupObject()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        GameObject objectToPickUp = waterSpawner.GetPickupAtY(rectTransform.anchoredPosition.y);
        if (objectToPickUp == null)
        {
            // Debug.Log("Did not pick up an object");
            return false;
        }

        Image image = objectToPickUp.GetComponent<Image>();
        pickedUpImage.GetComponent<Image>().sprite = image.sprite;
        pickedUpImage.gameObject.SetActive(true);
        pickedUpImage.ResizeBasedOnSprite();

        WaterItem waterItem = objectToPickUp.GetComponent<WaterItem>();
        isCurrentItemTrash = waterItem.isTrash;

        if(waterItem.isBonus)
        {
            BonusManager.currentBonuses++;
            Destroy(objectToPickUp);
            return false;
        }

        Destroy(objectToPickUp);

        return true;
    }

    void PutInCan()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        WaterCan closestCan = GetCanAtY(rectTransform.anchoredPosition.y);
        closestCan.AddObject(pickedUpImage.GetComponent<Image>().sprite);

        riverCleaner.JudgeItem(isCurrentItemTrash);
    }

    WaterCan GetCanAtY(float y)
    {
        WaterCan currentCan = cans[0];
        float distance = Mathf.Abs(y - currentCan.GetComponent<RectTransform>().anchoredPosition.y);

        for(int i = 1; i < cans.Count; i++)
        {
            float tempDistance = Mathf.Abs(y - cans[i].GetComponent<RectTransform>().anchoredPosition.y);
            if(tempDistance < distance)
            {
                distance = tempDistance;
                currentCan = cans[i];
            }
        }

        return currentCan;
    }
}
