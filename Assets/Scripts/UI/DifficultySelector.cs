using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public List<DifficultySelectorButton> buttons;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].owningSelector = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].difficultyMode == DifficultyManager.currentDifficultyMode)
            {
                buttons[i].OnSelected();
            }
            else
            {
                buttons[i].OnDeselected();
            }
        }
    }

    public void SetDifficultyMode(DifficultyMode difficultyMode)
    {
        DifficultyManager.currentDifficultyMode = difficultyMode;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].difficultyMode == difficultyMode)
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
