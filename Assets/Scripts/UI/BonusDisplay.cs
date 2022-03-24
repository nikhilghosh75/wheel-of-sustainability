using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDisplay : MonoBehaviour
{
    UILivesDisplay display;

    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<UILivesDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        display.lives = BonusManager.currentBonuses;
    }
}
