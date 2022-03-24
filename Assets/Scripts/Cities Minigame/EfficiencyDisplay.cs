using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EfficiencyDisplay : MonoBehaviour
{
    public CitiesMinigame minigame;
    public int minConstantMPG;
    public int maxConstantMPG;
    public int accelerationMPG;

    public Color minColor;
    public Color maxColor;

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float t = (minigame.GetCurrentSpeed() - minigame.slowSpeed) / (minigame.boostSpeed - minigame.slowSpeed);
        int mpg = (int)Mathf.Lerp(minConstantMPG, maxConstantMPG, t);
        text.text = mpg.ToString();
        text.color = Color.Lerp(maxColor, minColor, t);

        minigame.AddMPG(mpg);
    }
}
