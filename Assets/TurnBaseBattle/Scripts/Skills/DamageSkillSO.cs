using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Damage_Skill_", menuName = "ScriptableObjects/TurnBasedBattle/Damage Skill")]
public class DamageSkillSO : BaseSkillSO
{
    public int DamageAmount;

    public override void ApplySkill(IBattleCharacter user, List<IBattleCharacter> targets)
    {
        var finalDamage = DamageAmount;

        foreach (var bonus in Bonus)
        {
            finalDamage += bonus.Type == BonusType.Flat ? Mathf.RoundToInt(bonus.BaseValue) : Mathf.RoundToInt(bonus.BaseValue * DamageAmount);
        }

        targets.ForEach(target => target.TakeDamage(finalDamage));
    }

    protected override string GetInternalDescription()
    {
        return string.Format(BaseDescription, DamageAmount);
    }
}
