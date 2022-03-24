using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string minigameName;
    public Sprite themeSprite;
    public string themeName;
    public Color color;

    public GameObject difficultySelector;

    string description;

    public MinigameInfoDisplay infoDisplay;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartMinigame);
        description = ScreenSwitcher.switcher.minigameManager.GetMinigameDescription(minigameName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartMinigame()
    {
        // ScreenSwitcher.switcher.SwitchToMinigame();
        ScreenSwitcher.switcher.minigameManager.QueueMinigame(minigameName);
        difficultySelector.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoDisplay.Enable();
        infoDisplay.SetMinigame(ScreenSwitcher.switcher.minigameManager.GetMinigameByName(minigameName));

        infoDisplay.category.text = themeName;
        infoDisplay.category.color = color;
        infoDisplay.wheelImage.sprite = themeSprite;
        infoDisplay.minigameName.text = minigameName;
        infoDisplay.minigameDescription.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // infoDisplay.Reset();
    }
}
