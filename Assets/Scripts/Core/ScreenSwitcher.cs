using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MinigameMode
{
    Wheel,
    FreePlay
}

public class ScreenSwitcher : MonoBehaviour
{
    public MainMenuController mainMenu;
    public WheelScreen wheelScreen;
    public MinigameManager minigameManager;
    public EndScreen endScreen;

    public MinigameMode minigameMode = MinigameMode.Wheel;
    public bool wheelModeStarted = false;

    public static ScreenSwitcher switcher;

    // Awake is called before Start
    void Awake()
    {
        switcher = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchToMainMenu();
    }

    public void SwitchToMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
        wheelScreen.gameObject.SetActive(false);
        minigameManager.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);

        mainMenu.GoToMainMenu();

        wheelModeStarted = false;
    }

    public void SwitchToFreePlay()
    {
        mainMenu.gameObject.SetActive(true);
        wheelScreen.gameObject.SetActive(false);
        minigameManager.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);

        mainMenu.GoToFreePlay();
    }

    public void SwitchToWheelScreen()
    {
        mainMenu.gameObject.SetActive(false);
        wheelScreen.gameObject.SetActive(true);
        minigameManager.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);

        wheelScreen.OnDisplay();
    }

    public void SwitchToMinigame()
    {
        mainMenu.gameObject.SetActive(false);
        wheelScreen.gameObject.SetActive(false);
        minigameManager.gameObject.SetActive(true);
        endScreen.gameObject.SetActive(false);

        // minigameManager.PlayMinigame();
    }

    public void SwitchToEndScreen()
    {
        mainMenu.gameObject.SetActive(false);
        wheelScreen.gameObject.SetActive(false);
        minigameManager.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);

        endScreen.StartEndScreen();
    }

    public void SetMinigameMode(MinigameMode mode)
    {
        minigameMode = mode;
    }

    public void SetFreePlay()
    {
        minigameMode = MinigameMode.FreePlay;
    }

    public void SetWheel()
    {
        minigameMode = MinigameMode.Wheel;
        BonusManager.currentBonuses = 3;
    }

    public void SwitchFromMinigame()
    {
        switch(minigameMode)
        {
            case MinigameMode.FreePlay: SwitchToFreePlay(); break;
            case MinigameMode.Wheel:
                if(BonusManager.currentBonuses == 0)
                {
                    SwitchToEndScreen();
                    break;
                }
                SwitchToWheelScreen();
                break;
        }
    }
}
