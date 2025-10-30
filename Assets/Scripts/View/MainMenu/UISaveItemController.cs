using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISaveItemController : UIItemController
{
    [SerializeField] private TextMeshProUGUI _txtSaveName;

    protected override void HandleInit(object obj)
    {
        var save = obj as string;

        _txtSaveName.text = save.Replace(".json", "").Replace("_", " ");
    }
}
