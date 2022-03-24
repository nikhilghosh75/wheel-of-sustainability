using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILivesDisplay : MonoBehaviour
{
    public GameObject livesPrefab;
    public float livesWidth;

    [HideInInspector]
    public int lives;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int numLivesDisplayed = transform.childCount;

        if(numLivesDisplayed != lives)
        {
            if(numLivesDisplayed > lives)
            {
                for(int i = 0; i < numLivesDisplayed - lives; i++)
                {
                    Destroy(transform.GetChild(transform.childCount - 1).gameObject);
                }
            }
            else
            {
                for (int i = 0; i < lives - numLivesDisplayed; i++)
                {
                    GameObject tempObject = Instantiate(livesPrefab, this.transform);
                    RectTransform rectTransform = tempObject.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector3(livesWidth * (numLivesDisplayed + i + 0.5f), 0, 0);
                }
            }
        }
    }
}
