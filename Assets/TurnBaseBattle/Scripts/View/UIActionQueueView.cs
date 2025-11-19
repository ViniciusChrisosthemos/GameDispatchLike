using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActionQueueView : UIItemController
{
    [SerializeField] private Transform _itemParent;
    [SerializeField] private UISkillActionView _skillActionPrefab;

    protected override void HandleInit(object obj)
    {
        var skillAction = obj as SkillAction;


    }
}
