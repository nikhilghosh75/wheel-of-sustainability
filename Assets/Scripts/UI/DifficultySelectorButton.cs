using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelectorButton : MonoBehaviour
{
    public DifficultyMode difficultyMode;
    public Text text;

    [HideInInspector]
    public DifficultySelector owningSelector;

    [Header("Style")]
    public Color selectedImageColor;
    public Color deselectedImageColor;
    public Color selectedTextColor;
    public Color deselectedTextColor;

    Image image;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);

        image = GetComponent<Image>();
    }

    void OnButtonClick()
    {
        owningSelector.SetDifficultyMode(difficultyMode);
    }

    public void OnSelected()
    {
        if(image == null)
            image = GetComponent<Image>();

        image.color = selectedImageColor;
        text.color = selectedTextColor;
    }

    public void OnDeselected()
    {
        if (image == null)
            image = GetComponent<Image>();

        image.color = deselectedImageColor;
        text.color = deselectedTextColor;
    }
}
