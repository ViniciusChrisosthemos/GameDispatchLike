using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIKeywordView : UIItemController
{
    [SerializeField] private RectTransform _mainTransform;
    [SerializeField] private Image _imgKeywordBackground;
    [SerializeField] private Image _imgKeywordIcon;
    [SerializeField] private RectTransform _iconTransform;
    [SerializeField] private TextMeshProUGUI _txtKeywordName;
    [SerializeField] private float _paddingHorizontal = 10f;

    public AbstractKeywordSO KeywordSO;

    protected override void HandleInit(object obj)
    {
        var keywordSO = obj as AbstractKeywordSO;

        _txtKeywordName.text = keywordSO.Name;
        _imgKeywordIcon.sprite = keywordSO.Art;
        _imgKeywordBackground.color = keywordSO.Color;

        _txtKeywordName.ForceMeshUpdate();

        _mainTransform.sizeDelta = new Vector2(_txtKeywordName.preferredWidth + _paddingHorizontal + _iconTransform.sizeDelta.x, 0);
    }
}
