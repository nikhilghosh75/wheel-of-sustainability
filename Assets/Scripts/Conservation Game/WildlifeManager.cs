using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildlifeManager : MonoBehaviour
{
    [System.Serializable]
    public struct Wildlife
    {
        public GameObject gameObject;
        public int numToSpawnAt;
    }

    public List<Wildlife> wildlives;

    public void HideAnimals()
    {
        foreach(Wildlife wildlife in wildlives)
        {
            wildlife.gameObject.SetActive(false);
        }
    }

    public void OnNumPlanted(int numPlanted)
    {
        foreach(Wildlife wildlife in wildlives)
        {
            if(wildlife.numToSpawnAt == numPlanted)
            {
                wildlife.gameObject.SetActive(true);
                wildlife.gameObject.SendMessage("StartAnimal");
            }
        }
    }
}
