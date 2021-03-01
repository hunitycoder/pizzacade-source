using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectFourTileHelper : TileHelperBase
{

    private Image displayImage;

    public override void Awake()
    {
        base.Awake();
        displayImage = GetComponent<Image>();
    }

    public override void UpdateTile(int state)
    {
        //base.UpdateTile(state);
        switch (state)
        {
            case 1:
                displayImage.DOColor(Color.black, .25f);
                break;
            case 2:
                displayImage.DOColor(Color.red, .25f);
                break;
            default:
                displayImage.DOColor(Color.white, .25f);
                break;
        }
        transform.DOPunchScale(Vector3.one* .1f, .25f, 0, .1f);
    }
}
