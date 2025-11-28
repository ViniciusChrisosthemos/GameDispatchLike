using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyStatus_Skill_", menuName = "TurnBaseBattle/Skills/Skill/Apply Status")]
public class ApplyStatusSkillSO : BaseSkillSO
{
    [SerializeField] private List<AbstractSkillStatus> _statusToAdd;

    protected override void ApplySkillInternal(IBattleCharacter user, List<IBattleCharacter> targets)
    {
        foreach (var target in targets)
        {
            foreach (var status in _statusToAdd)
            {
                target.AddStatus(status);
            }
        }
    }

    protected override string GetInternalDescription()
    {
        return BaseDescription;
    }
}
