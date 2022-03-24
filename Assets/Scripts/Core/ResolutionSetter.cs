using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    public int width;
    public int height;

    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(width, height, false);
    }
}
