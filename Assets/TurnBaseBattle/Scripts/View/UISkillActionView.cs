using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillActionView : UIItemController
{
    [SerializeField] private RectTransform _mainUIComponent;
    [SerializeField] private Transform _itemParent;
    [SerializeField] private UIIconView _iconPrefab;
    [SerializeField] private GameObject _separatorPrefab;
    [SerializeField] private UIDeleteItemView _uiDeleteItemPrefab;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    protected override void HandleInit(object obj)
    {
        var skillAction = obj as SkillAction;

        _itemParent.ClearChilds();

        var iconInstance = Instantiate(_iconPrefab, _itemParent);
        iconInstance.SetIcon(skillAction.Skill.Art);

        Instantiate(_separatorPrefab, _itemParent);

        foreach (var target in skillAction.Targets)
        {
            var targetIconInstance = Instantiate(_iconPrefab, _itemParent);
            targetIconInstance.SetIcon(target.BaseCharacter.FaceArt);
            
        }

        var componentSize = _gridLayoutGroup.cellSize;
        componentSize.y = _gridLayoutGroup.padding.vertical + _gridLayoutGroup.cellSize.y;
        componentSize.x = (int)(_gridLayoutGroup.cellSize.x * (skillAction.Targets.Count+2) + _gridLayoutGroup.spacing.x * (skillAction.Targets.Count + 1)) + _gridLayoutGroup.padding.horizontal;

        _mainUIComponent.sizeDelta = componentSize;

        var deleteItem = Instantiate(_uiDeleteItemPrefab, _itemParent);
        deleteItem.SetCallback(SelectItem);
        /*
        deleteItem.SetLayoutElementToIgnoreLayout(true);
        deleteItem.GetMainUIComponent().sizeDelta = componentSize;*/
    }
}
