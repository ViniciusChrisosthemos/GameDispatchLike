using TMPro;
using UnityEngine;

public class UIStringItemController : UIItemController
{
    [SerializeField] private TextMeshProUGUI _txtValue;

    protected override void HandleInit(object obj)
    {
        var value = obj as string;

        _txtValue.text = value;
    }
}
