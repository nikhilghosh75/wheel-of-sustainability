using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeDisplay : MonoBehaviour
{
    [HideInInspector]
    public float currentTime;

    public bool minisecondsDisplayed;

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = FormattingFunctions.TimeToText(currentTime, minisecondsDisplayed);
    }
}
