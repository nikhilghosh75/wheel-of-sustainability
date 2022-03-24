using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFractionDisplay : MonoBehaviour
{
    Text text;

    [HideInInspector]
    public int numerator;

    [HideInInspector]
    public int denominator;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = numerator.ToString() + "/" + denominator.ToString();
    }
}
