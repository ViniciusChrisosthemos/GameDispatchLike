using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunSkillStatus_", menuName = "TurnBaseBattle/Skills/SkillStatus/Stun")]
public class StunSkillStatus : AbstractSkillStatus
{
    [SerializeField] private int StunDuration;

    public override void OnApply(SkillStatusRuntime statusRuntime)
    {
        statusRuntime.RemainingDuration = StunDuration;
        statusRuntime.Owner.AddActionBlocker();
    }

    public override void OnRemove(SkillStatusRuntime statusRuntime)
    {
        statusRuntime.Owner.RmvActionBlocker();
    }

    public override void OnTurnEnd(SkillStatusRuntime statusRuntime)
    {
        statusRuntime.RemainingDuration--;

        if (statusRuntime.RemainingDuration <= 0)
        {
            statusRuntime.Owner.RmvStatus(this);
        }
    }

    public override void OnTurnStart(SkillStatusRuntime statusRuntime)
    {

    }
}
