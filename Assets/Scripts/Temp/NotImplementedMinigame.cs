using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotImplementedMinigame : IMinigame
{
    public override string GetName()
    {
        return "";
    }

    public override void StartMinigame()
    {
        Debug.LogWarning("Minigame is unimplemented");
    }
}
