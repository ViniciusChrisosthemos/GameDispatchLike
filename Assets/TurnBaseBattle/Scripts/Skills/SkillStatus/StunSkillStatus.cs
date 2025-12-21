using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillStatus_Stun_", menuName = "TurnBaseBattle/Skills/SkillStatus/Stun")]
public class StunSkillStatus : AbstractSkillStatus
{
    public override void InternalApply(SkillStatusRuntime statusRuntime)
    {
        statusRuntime.Owner.AddActionBlocker();
    }

    public override void InternalRemove(SkillStatusRuntime statusRuntime)
    {
        statusRuntime.Owner.RmvActionBlocker();
    }

    public override void InternalTurnEnd(SkillStatusRuntime statusRuntime, BattleLogger battleLogger)
    {

    }

    public override void InternalTurnStart(SkillStatusRuntime statusRuntime, BattleLogger battleLogger)
    {
        battleLogger.Log($"{statusRuntime.Owner.GetName()} is stunned. Pass turn");
    }
}
