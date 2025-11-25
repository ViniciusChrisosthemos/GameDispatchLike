using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Heal_Skill_", menuName = "ScriptableObjects/TurnBasedBattle/Skill/Skill/Heal")]
public class HealSkillSO : BaseSkillSO
{
    public int HealAmount;

    protected override void ApplySkillInternal(IBattleCharacter user, List<IBattleCharacter> targets)
    {
        var finalHeal = HealAmount;

        foreach (var bonus in Bonus)
        {
            finalHeal = bonus.Type == BonusType.Flat ? Mathf.RoundToInt(finalHeal) : Mathf.RoundToInt(HealAmount * bonus.BaseValue);
        }

        targets.ForEach(target => target.TakeHeal(finalHeal));
    }

    protected override string GetInternalDescription()
    {
        return string.Format(BaseDescription, HealAmount);
    }
}
