using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSkillStatus : ScriptableObject
{
    [Header("Visuals")]
    public Sprite Icon;

    [Header("Paramters")]
    public int Duration;
    public int InitialStacks;
    public string Description;

    public void OnApply(SkillStatusRuntime statusRuntime)
    {
        statusRuntime.RemainingDuration = Duration;
        InternalApply(statusRuntime);
    }

    public void OnRemove(SkillStatusRuntime statusRuntime)
    {
        InternalRemove(statusRuntime);
    }

    public void OnTurnEnd(SkillStatusRuntime statusRuntime, BattleLogger battleLogger)
    {
        statusRuntime.RemainingDuration--;

        if (statusRuntime.RemainingDuration <= 0)
        {
            statusRuntime.RemoveStatus(statusRuntime);
        }

        InternalTurnEnd(statusRuntime, battleLogger);
    }

    public void OnTurnStart(SkillStatusRuntime statusRuntime, BattleLogger battleLogger)
    {
        InternalTurnStart(statusRuntime, battleLogger);
    }

    public abstract void InternalApply(SkillStatusRuntime statusRuntime);
    public abstract void InternalTurnStart(SkillStatusRuntime statusRuntime, BattleLogger battleLogger);
    public abstract void InternalTurnEnd(SkillStatusRuntime statusRuntime, BattleLogger battleLogger);
    public abstract void InternalRemove(SkillStatusRuntime statusRuntime);
}
