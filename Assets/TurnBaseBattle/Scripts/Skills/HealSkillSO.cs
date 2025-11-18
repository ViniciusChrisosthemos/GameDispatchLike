using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class HealSkillLevel : SkillLevel
{
    public int BaseValue;
    public List<SkillBonus> Bonus;

    public override string GetDescription()
    {
        return string.Format(BaseDescription, BaseValue);
    }
}

[CreateAssetMenu(fileName = "Heal_Skill_", menuName = "ScriptableObjects/TurnBasedBattle/Heal Skill")]
public class HealSkillSO : BaseSkillSO
{
    public int HealAmount;
    public List<HealSkillLevel> Levels;

    public override void ApplySkill(IBattleCharacter user, List<IBattleCharacter> targets)
    {
        var currentBaseHeal = 0;
        var currentBonus = new List<SkillBonus>();

        foreach (var level in Levels)
        {
            currentBaseHeal += level.BaseValue;
            currentBonus.AddRange(level.Bonus);
        }

        var finalHeal = currentBaseHeal;

        foreach (var bonus in currentBonus)
        {
            finalHeal = bonus.Type == BonusType.Flat ? Mathf.RoundToInt(finalHeal) : Mathf.RoundToInt(currentBaseHeal * bonus.BaseValue);
        }

        targets.ForEach(target => target.TakeHeal(finalHeal));
    }

    public override List<SkillLevel> GetLevels()
    {
        return Levels.Select(level => (SkillLevel) level).ToList();
    }

    protected override string GetInternalDescription()
    {
        return string.Format(BaseDescription, HealAmount);
    }
}
