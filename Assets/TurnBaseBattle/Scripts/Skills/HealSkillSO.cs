using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Heal_Skill_", menuName = "TurnBaseBattle/Skills/Skill/Heal")]
public class HealSkillSO : BaseSkillSO
{
    public int HealAmount;

    protected override void ApplySkillInternal(IBattleCharacter user, List<IBattleCharacter> targets, BattleLogger battleLogger)
    {
        var finalHeal = HealAmount;

        foreach (var bonus in Bonus)
        {
            finalHeal = bonus.Type == BonusType.Flat ? Mathf.RoundToInt(finalHeal) : Mathf.RoundToInt(HealAmount * bonus.BaseValue);
        }

        targets.ForEach(target => target.TakeHeal(finalHeal));
    }

    protected override string GetInternalDescription(IBattleCharacter user)
    {
        return string.Format(BaseDescription, HealAmount);
    }
}
