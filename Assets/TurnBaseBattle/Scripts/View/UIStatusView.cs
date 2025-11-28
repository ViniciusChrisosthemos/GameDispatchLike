using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusView : UIItemController
{
    [SerializeField] private TextMeshProUGUI _txtDuration;
    [SerializeField] private TextMeshProUGUI _txtStack;
    [SerializeField] private Image _imgStatusArt;

    protected override void HandleInit(object obj)
    {
        var status = obj as SkillStatusRuntime;

        _txtDuration.text = status.RemainingDuration.ToString();

        _imgStatusArt.sprite = status.StatusSO.Icon;
    }
}
