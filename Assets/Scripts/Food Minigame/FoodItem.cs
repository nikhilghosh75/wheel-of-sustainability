using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public bool processed = false;

    public bool bonus = false;

    bool labeledProcessed = false;

    bool judged = false;

    public string foodName;

    RectTransform rectTransform;

    public static GameObject organicPrefab;

    [HideInInspector]
    public float maxXPosition;

    // Start is called before the first frame update
    void Start()
    {
        labeledProcessed = false;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LabelProcessed()
    {
        if (bonus)
        {
            BonusManager.currentBonuses++;
            Destroy(this.gameObject);
            return;
        }

        if (rectTransform.anchoredPosition.x > maxXPosition)
            return;

        labeledProcessed = true;

        Instantiate(organicPrefab, transform);
    }

    public void JudgeLabel(FoodMinigame minigame)
    {
        if (judged)
            return;
        judged = true;

        if(processed == labeledProcessed)
        {
            if(processed)
            {
                minigame.OnLabeledCorrectly();
                // Debug.Log("CORRECT: " + gameObject.name);
            }
            else
            {
                minigame.OnIgnoredCorrectly();
                // Debug.Log("INCORRECT: " + gameObject.name);
            }
        }
        else
        {
            minigame.OnDecidedWrongly();
        }
    }
}
