using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Damage_Skill_", menuName = "TurnBaseBattle/Skills/Skill/Damage")]
public class DamageSkillSO : BaseSkillSO
{
    public int DamageAmount;

    public List<ItemHolder<SkillResourceSO>> ResourcesToAdd;

    protected override void ApplySkillInternal(IBattleCharacter user, List<IBattleCharacter> targets)
    {
        var finalDamage = DamageAmount;

        foreach (var bonus in Bonus)
        {
            finalDamage += bonus.Type == BonusType.Flat ? Mathf.RoundToInt(bonus.BaseValue) : Mathf.RoundToInt(bonus.BaseValue * DamageAmount);
        }

        targets.ForEach(target => target.TakeDamage(finalDamage));

        Debug.Log($"DamgeSkillSO {name}  Add Resource {ResourcesToAdd.Count}");

        ResourcesToAdd.ForEach(item => user.AddResource(item.Item, item.Amount));
    }

    protected override string GetInternalDescription()
    {
        return string.Format(BaseDescription, DamageAmount);
    }
}
