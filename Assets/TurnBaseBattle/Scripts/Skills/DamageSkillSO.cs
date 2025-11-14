using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DamageSkillLevel : SkillLevel
{
    public int BaseValue;
    public List<SkillBonus> Bonus;

    public override string GetDescription()
    {
        return string.Format(BaseDescription, BaseValue);
    }
}


[CreateAssetMenu(fileName = "Damage_Skill_", menuName = "ScriptableObjects/TurnBasedBattle/Damage Skill")]
public class DamageSkillSO : BaseSkillSO
{
    public int DamageAmount;
    public List<DamageSkillLevel> Levels;

    public override void ApplySkill(int skillLevel, IBattleCharacter user, List<IBattleCharacter> targets)
    {
        var currentBaseDamage = 0;
        var currentBonus = new List<SkillBonus>();

        foreach (var level in Levels)
        {
            currentBaseDamage += level.BaseValue;
            currentBonus.AddRange(level.Bonus);
        }

        var finalDamage = currentBaseDamage;

        foreach (var bonus in currentBonus)
        {
            finalDamage += bonus.Type == BonusType.Flat ? Mathf.RoundToInt(bonus.BaseValue) : Mathf.RoundToInt(bonus.BaseValue * currentBaseDamage);
        }

        targets.ForEach(target => target.TakeDamage(finalDamage));
    }

    public override List<SkillLevel> GetLevels()
    {
        return Levels.Select(level => (SkillLevel) level).ToList();
    }

    protected override string GetInternalDescription()
    {
        return string.Format(BaseDescription, DamageAmount);
    }
}
