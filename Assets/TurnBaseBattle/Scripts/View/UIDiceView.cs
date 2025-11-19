using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDiceView : UIItemController
{
    [SerializeField] private Image _imgDice;
    [SerializeField] private GameObject _diceLockedOverlay;

    protected override void HandleInit(object obj)
    {
        var spriteHolder = obj as IHasSprite;

        _imgDice.sprite = spriteHolder.GetSprite();

        _diceLockedOverlay.SetActive(false);
    }

    public void SetDiceLocked(bool isLocked)
    {
        _diceLockedOverlay.SetActive(isLocked);
    }
}
