using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    public Text title;
    public Text description;
    public Text controls;
    public IntroImages introImages;
    public Image thumbnailImage;

    public MinigameManager manager;

    void Start()
    {
        // gameObject.SetActive(false);
    }

    public void ShowIntroScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(DoIntroScreen());
    }

    public void StartMinigame()
    {
        IMinigame minigame = manager.GetCurrentMinigame();
        minigame.StartMinigame();
        gameObject.SetActive(false);
    }

    IEnumerator DoIntroScreen()
    {
        Debug.Log("INTRO SCREEN");

        IMinigame minigame = manager.GetCurrentMinigame();
        title.text = minigame.GetName();
        description.text = minigame.GetDescription();
        controls.text = minigame.GetControls();
        introImages.SetIntroImages(minigame.GetIntroImageConfigs());

        if (thumbnailImage != null)
            thumbnailImage.sprite = minigame.thumbnailImage;

        yield break;
    }

    public void ShowIntroScreen(IMinigame minigame)
    {
        gameObject.SetActive(true);

        title.text = minigame.GetName();
        description.text = minigame.GetDescription();
        controls.text = minigame.GetControls();
        introImages.SetIntroImages(minigame.GetIntroImageConfigs());

        if (thumbnailImage != null)
            thumbnailImage.sprite = minigame.thumbnailImage;
    }
}
