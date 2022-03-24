using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject freePlay;
    public GameObject options;
    public GameObject buttons;

    // Start is called before the first frame update
    void Start()
    {
        // options.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToFreePlay()
    {
        mainMenu.SetActive(false);
        freePlay.SetActive(true);
    }

    public void GoToMainMenu()
    {
        mainMenu.SetActive(true);
        freePlay.SetActive(false);
    }

    public void ShowOptionsScreen()
    {
        options.SetActive(true);
        buttons.SetActive(false);
    }

    public void HideOptionsScreen()
    {
        options.SetActive(false);
        buttons.SetActive(true);
    }
}
