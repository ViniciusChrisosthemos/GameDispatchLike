using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIIconView : UIItemController
{
    [SerializeField] private Image _imgIcon;

    public void SetIcon(Sprite sprite)
    {
        _imgIcon.sprite = sprite;
    }

    protected override void HandleInit(object obj)
    {
        var sprite = obj as IHasSprite;

        SetIcon(sprite.GetSprite());
    }
}
