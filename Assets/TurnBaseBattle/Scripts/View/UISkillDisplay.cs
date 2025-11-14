using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillDisplay : UIItemController
{
    [SerializeField] private TextMeshProUGUI _txtSkillDescription;
    [SerializeField] private RectTransform _diceValueParent;
    [SerializeField] private Image _diceValuePrefab;
    [SerializeField] private RectTransform _layoutGroup;
    [SerializeField] private int _horizontalSizeOffset;

    private BaseSkillSO _skillSO;


    public BaseSkillSO SkillSO;

    private void Start()
    {
        HandleInit(SkillSO as object);
    }

    protected override void HandleInit(object obj)
    {
        _skillSO = obj as BaseSkillSO;

        _txtSkillDescription.text = _skillSO.GetDescription();

        _diceValueParent.ClearChilds();
        var imgSize = 0f;

        foreach (var diceSO in _skillSO.RequiredDiceValues)
        {
            var img = Instantiate(_diceValuePrefab, _diceValueParent);
            img.sprite = diceSO.Art;
            img.rectTransform.sizeDelta = Vector2.one * img.rectTransform.sizeDelta.y;
            imgSize = img.rectTransform.sizeDelta.y;
        }

        _diceValueParent.sizeDelta = new Vector2(imgSize * _skillSO.RequiredDiceValues.Count, _diceValueParent.sizeDelta.y);

        var containerSize = _layoutGroup.GetComponent<RectTransform>().sizeDelta.x + _horizontalSizeOffset;
        _txtSkillDescription.rectTransform.sizeDelta = new Vector2(containerSize - _diceValueParent.sizeDelta.x, _txtSkillDescription.rectTransform.sizeDelta.y);
    }
}
