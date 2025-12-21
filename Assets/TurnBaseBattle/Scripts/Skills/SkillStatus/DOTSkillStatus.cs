using UnityEngine;


[CreateAssetMenu(fileName = "SkillStatus_DOT_", menuName = "TurnBaseBattle/Skills/SkillStatus/DOT")]
public class DOTSkillStatus : AbstractSkillStatus
{
    [SerializeField] private DiceDamageSource _damagePerTurn;

    public override void InternalApply(SkillStatusRuntime statusRuntime)
    {

    }

    public override void InternalRemove(SkillStatusRuntime statusRuntime)
    {

    }

    public override void InternalTurnEnd(SkillStatusRuntime statusRuntime, BattleLogger battleLogger)
    {

    }

    public override void InternalTurnStart(SkillStatusRuntime statusRuntime, BattleLogger battleLogger)
    {
        var damage = 0;

        for (int i = 0; i < statusRuntime.Stacks; i++)
        {
            damage += _damagePerTurn.GetDamage();
        }

        statusRuntime.Owner.TakeDamage(damage);

        battleLogger.Log($"{statusRuntime.Owner.GetName()} receive {damage} from {Description} ({statusRuntime.Stacks} stacks). {statusRuntime.RemainingDuration} turns remaining.");
    }
}
