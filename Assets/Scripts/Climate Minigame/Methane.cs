using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Methane : MonoBehaviour
{
    public GameObject hydrogenPrefab;

    public float transitionTime;

    public float waitTime;

    List<GameObject> children;

    // Start is called before the first frame update
    void Start()
    {
        if(children == null)
        {
            children = new List<GameObject>();
        }

        for(int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }
        StartCoroutine(StartShoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartShoot()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        for(int i = children.Count - 1; i >= 0 ; i--)
        {
            float currentTime = 0;
            float initialRotation = transform.rotation.eulerAngles.z;
            while(currentTime < transitionTime)
            {
                currentTime += Time.deltaTime;
                float currentRotation = Mathf.Lerp(initialRotation, initialRotation + 90, currentTime / transitionTime);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentRotation));
                yield return null;
            }

            Destroy(transform.GetChild(i).gameObject);
            GameObject spawnedHydrogen = Instantiate(hydrogenPrefab, ClimateMinigame.minigame.projectileParent.transform);
            RectTransform hydrogenTransform = spawnedHydrogen.GetComponent<RectTransform>();
            hydrogenTransform.anchoredPosition = rectTransform.anchoredPosition;

            yield return new WaitForSeconds(waitTime);
        }
    }
}
