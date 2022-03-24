using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

public class NToggle : MonoBehaviour
{
    public Sprite unSelectedImage;
    public Sprite selectedImage;
    public bool defaultValue = true;
    Image image;
    bool value = true;

    public BoolEvent OnValueChanged;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnButtonPressed);

        SetValue(defaultValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPressed()
    {
        SetValue(!value);
    }

    public void SetValue(bool newValue)
    {
        value = newValue;
        OnValueChanged.Invoke(value);

        if (value)
            image.sprite = selectedImage;
        else
            image.sprite = unSelectedImage;
    }

    public bool GetValue()
    {
        return value;
    }
}
