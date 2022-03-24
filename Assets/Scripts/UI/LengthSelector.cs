using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LengthSelector : MonoBehaviour
{
    public List<LengthSelectorButton> buttons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].owningSelector = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].lengthToSet == BonusManager.currentLength)
            {
                buttons[i].OnSelected();
            }
            else
            {
                buttons[i].OnDeselected();
            }
        }
    }

    public void SetLength(int newLength)
    {
        BonusManager.currentLength = newLength;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].lengthToSet == BonusManager.currentLength)
            {
                buttons[i].OnSelected();
            }
            else
            {
                buttons[i].OnDeselected();
            }
        }
    }
}
