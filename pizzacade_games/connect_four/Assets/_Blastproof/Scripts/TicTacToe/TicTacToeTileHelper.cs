using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TicTacToeTileHelper : TileHelperBase
{
    private TMP_Text _displayText;

    public override void Awake()
    {
        base.Awake();
        _displayText = GetComponentInChildren<TMP_Text>();
    }

    public override void UpdateTile(int state)
    {
        switch (state)
        {
            case 0:
                _displayText.text = "";
                break;
            case 1:
                _displayText.color = Color.red;
                _displayText.text = "X";
                break;
            case 2:
                _displayText.color = Color.blue;
                _displayText.text = "O";
                break;
            default:
                _displayText.text = "";
                break;
        }
    }
}
