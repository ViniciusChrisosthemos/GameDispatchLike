using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillActionView : UIItemController
{
    [SerializeField] private Transform _itemParent;
    [SerializeField] private UIIconView _iconPrefab;
    [SerializeField] private GameObject _separatorPrefab;

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
    }
}
