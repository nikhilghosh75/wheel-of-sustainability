using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelScreen : MonoBehaviour
{
    public Wheel wheel;
    public MinigameInfoDisplay infoDisplay;
    public GameObject settingsScreen;

    public MinigameManager minigameManager;

    public float scaleFactor;

    public List<Sprite> wheelIcons;
    public List<string> wheelCategoryNames;
    public List<Category> wheelCategories;

    void Start()
    {
        wheel.OnWheelStop.AddListener(DisplayCategory);
        infoDisplay.gameObject.SetActive(false);
    }

    public void OnDisplay()
    {
        infoDisplay.gameObject.SetActive(false);
        wheel.locked = false;

        bool isStarting = !ScreenSwitcher.switcher.wheelModeStarted;
        settingsScreen.gameObject.SetActive(isStarting);
    }

    public void OnSwipe(Swipe swipe)
    {
        if (gameObject.activeInHierarchy == false) return;

        BoxCollider2D wheelBox = wheel.GetComponent<BoxCollider2D>();
        if (wheelBox.OverlapPoint(swipe.start) && wheelBox.OverlapPoint(swipe.end))
        {
            DoWheelSpin(swipe);
        }
        else
        {
            Debug.Log("SWIPE NOT WITHIN");
        }
    }

    public void DisplayCategory()
    {
        int category = wheel.GetCurrentlySelectedCategory();

        infoDisplay.wheelImage.sprite = wheelIcons[category];
        infoDisplay.category.text = wheelCategoryNames[category];
        infoDisplay.minigameName.text = minigameManager.GetDefaultMinigameString(wheelCategories[category]);
        infoDisplay.minigameDescription.text = minigameManager.GetMinigameDescription(wheelCategories[category]);

        infoDisplay.gameObject.SetActive(true);

        minigameManager.currentCategory = wheelCategories[category];
    }

    public void StartWheelMode()
    {
        settingsScreen.gameObject.SetActive(false);

        ScreenSwitcher.switcher.wheelModeStarted = true;
        BonusManager.StartBonusMode();
    }

    public void StartMinigame()
    {
        minigameManager.PlayMinigame(minigameManager.GetDefaultMinigameString(minigameManager.currentCategory));
        BonusManager.currentBonuses--;
        Debug.Log("BONUS");
    }

    void DoWheelSpin(Swipe swipe)
    {
        Vector2 velocity = swipe.GetVelocity();
        float speed = velocity.magnitude;

        Debug.Log(speed);
        if(speed > 8f)
            wheel.Spin(speed * scaleFactor);
    }
}
