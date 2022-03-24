using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    public List<Sprite> sprites;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        int randIndex = (int)Random.Range(0.0f, sprites.Count);
        image.sprite = sprites[randIndex];
    }
}
