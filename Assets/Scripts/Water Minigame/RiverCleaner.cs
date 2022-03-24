using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiverCleaner : MonoBehaviour
{
    [HideInInspector]
    public WaterMinigame minigame;

    public RiverCleanerArm arm;

    public float maxY;
    public float minY;

    public Text text;

    [HideInInspector]
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        arm.riverCleaner = this;

        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        Vector2 mousePosition = Input.mousePosition;
        RectTransform armTransform = arm.GetComponent<RectTransform>();
        armTransform.anchoredPosition = new Vector2(armTransform.anchoredPosition.x,
            Mathf.Clamp(mousePosition.y, minY, maxY));

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Start Grab");
            arm.StartGrab();
        }
    }

    public void JudgeItem(bool isTrash)
    {
        if(isTrash)
        {
            text.text = "Trash";
            minigame.OnCorrectlyCaught();
        }
        else
        {
            text.text = "Wildlife";
            minigame.OnWildlifeRemoved();
        }
    }

    public void Clear()
    {
        text.text = "";

        for(int i = 0; i < arm.cans.Count; i++)
        {
            arm.cans[i].Clean();
        }
    }
}
