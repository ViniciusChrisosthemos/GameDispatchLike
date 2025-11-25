using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunSkillStatus_", menuName = "TurnBaseBattle/Skills/SkillStatus/Stun")]
public class StunSkillStatus : AbstractSkillStatus
{
    [SerializeField] private int StunDuration;

    public override void OnApply(SkillStatusRuntime statusRuntime)
    {
        Duration = StunDuration;
    }

    public override void OnRemove(SkillStatusRuntime statusRuntime)
    {
        statusRuntime.Owner.AddActionBlocker();
    }

    public override void OnTurnEnd(SkillStatusRuntime statusRuntime)
    {
        Duration--;

        if (Duration <= 0)
        {
            statusRuntime.Owner.RmvActionBlocker();
        }
    }

    public override void OnTurnStart(SkillStatusRuntime statusRuntime)
    {

    }
}
