using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LengthSelectorButton : MonoBehaviour
{
    public int lengthToSet;
    public Text text;

    [HideInInspector]
    public LengthSelector owningSelector;

    [Header("Style")]
    public Color selectedImageColor;
    public Color deselectedImageColor;
    public Color selectedTextColor;
    public Color deselectedTextColor;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);

        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonClick()
    {
        owningSelector.SetLength(lengthToSet);
    }

    public void OnSelected()
    {
        if (image == null)
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
