using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsBoolSetter : MonoBehaviour
{
    public enum BoolSettingType
    {
        SimpleBackgrounds,
        ShinyInteractables
    }

    public BoolSettingType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSetting(bool newSetting)
    {
        switch(type)
        {
            case BoolSettingType.SimpleBackgrounds:
                PlayerSettings.simpleBackgrounds = newSetting;
                break;
            case BoolSettingType.ShinyInteractables:
                PlayerSettings.shinyInteractables = newSetting;
                break;
        }
    }
}
