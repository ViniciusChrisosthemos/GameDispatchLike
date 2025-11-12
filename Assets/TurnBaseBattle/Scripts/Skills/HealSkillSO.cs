using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Heal_Skill_", menuName = "ScriptableObjects/TurnBasedBattle/Heal Skill")]
public class HealSkillSO : BaseSkillSO
{
    public int HealAmount;

    public override void ApplySkill(IBattleCharacter user, List<IBattleCharacter> targets)
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
