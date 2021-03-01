using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TileHelper : MonoBehaviour
{
    private TMP_Text displayText;
    
    void Awake()
    {
        displayText = GetComponentInChildren<TMP_Text>();
    }

    [Button]
    public void UpdateText(int state)
    {
        switch(state)
        {
            case 0:
                displayText.text = "";
                break;
            case 1:
                displayText.color = Color.red;
                displayText.text = "X";
                break;
            case 2:
                displayText.color = Color.blue;
                displayText.text = "O";
                break;
            default:
                displayText.text = "";
                break;
        }
    }
}
