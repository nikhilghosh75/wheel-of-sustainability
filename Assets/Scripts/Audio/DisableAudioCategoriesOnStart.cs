using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAudioCategoriesOnStart : MonoBehaviour
{
    public List<string> categoriesToDisable;

    // Start is called before the first frame update
    void Start()
    {
        foreach(string category in categoriesToDisable)
        {
            AudioManager.instance.DisableCategory(category);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
