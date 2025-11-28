using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatusRuntime
{
    public AbstractSkillStatus StatusSO;
    public int RemainingDuration;
    public IBattleCharacter Owner;
    public bool IsExpired { get; private set; }

    public SkillStatusRuntime(AbstractSkillStatus statusSO, IBattleCharacter owner)
    {
        StatusSO = statusSO;
        Owner = owner;
    }

    public void RemoveStatus(SkillStatusRuntime statusRuntime)
    {
        StatusSO.OnRemove(statusRuntime);
        IsExpired = true;
    }

    public void ApplyStatus(SkillStatusRuntime statusRuntime)
    {
        StatusSO.OnApply(statusRuntime);
    }
}
