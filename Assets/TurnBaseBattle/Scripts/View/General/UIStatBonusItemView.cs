using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBonusItemView : UIItemController
{
    [SerializeField] private Image _imgIsOn;
    [SerializeField] private TextMeshProUGUI _txtStacksRequired;
    [SerializeField] private TextMeshProUGUI _txtDescription;

    [Header("Parameters")]
    [SerializeField] private Color _colorIsOn;
    [SerializeField] private Color _colorIsOff;

    protected override void HandleInit(object obj)
    {
        ResourceBonus resourceBonus = obj as ResourceBonus;

        _txtStacksRequired.text = $"{resourceBonus.ResourceAmountRequired}x Stacks";
        _txtDescription.text = resourceBonus.Bonus.Description;

        _imgIsOn.color = _colorIsOff;
    }

    public void SetActive(bool isApplied)
    {
        _imgIsOn.color = isApplied ? _colorIsOn : _colorIsOff;
    }
}
