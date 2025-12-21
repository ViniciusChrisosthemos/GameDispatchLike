using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatusRuntime
{
    public AbstractSkillStatus StatusSO;
    public int RemainingDuration;
    public IBattleCharacter Owner;

    public SkillStatusRuntime(AbstractSkillStatus statusSO, IBattleCharacter owner)
    {
        StatusSO = statusSO;
        Owner = owner;
        Stacks = statusSO.InitialStacks;
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

    public void AddStack(int stack)
    {
        Stacks += stack;
    }

    public void ResetDuration()
    {
        RemainingDuration = StatusSO.Duration;
    }

    public int Stacks { get; private set; }
    public bool IsExpired { get; private set; } = false;
}
