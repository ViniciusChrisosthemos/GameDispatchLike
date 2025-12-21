using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Damage_Skill_", menuName = "TurnBaseBattle/Skills/Skill/Damage")]
public class DamageSkillSO : BaseSkillSO
{
    public DiceDamageSource DamageSource;

    public List<ItemHolder<SkillResourceSO>> ResourcesToAdd;
    public List<ItemHolder<AbstractSkillStatus>> StatusToAddToEnemies;

    protected override void ApplySkillInternal(IBattleCharacter user, List<IBattleCharacter> targets, BattleLogger battleLogger)
    {
        var diceManager = user.GetDiceManager();

        var baseDamage = DamageSource.GetDamage(diceManager);
        var finalDamage = baseDamage;

        foreach (var bonus in Bonus)
        {
            finalDamage += bonus.Type == BonusType.Flat ? Mathf.RoundToInt(bonus.BaseValue) : Mathf.RoundToInt(bonus.BaseValue * baseDamage);
        }

        // Critical check
        int finalDamageReference = finalDamage;
        string damageString = string.Empty;

        for (int i = 0; i < DamageSource.DiceValues.Count; i++)
        {
            damageString += $"{DamageSource.DiceValues[i]}";

            if (i < DamageSource.DiceValues.Count - 1) damageString += " + ";
        }

        for (int i = 0; i < diceManager.GetCriticalDices(); i++)
        {
            if (UnityEngine.Random.Range(1, 11) == 10)
            {
                finalDamage *= 2;
                damageString = $"({damageString}) x 2 (critical)";
            }
        }

        damageString = $"({damageString}) = {finalDamage}";

        foreach (var target in targets)
        {
            target.TakeDamage(finalDamage);

            battleLogger.Log($"{user.GetName()} deals {damageString} damage to {target.GetName()}");
        }

        foreach (var resource in ResourcesToAdd)
        {
            user.AddResource(resource.Item, resource.Amount);
            battleLogger.Log($"{user.GetName()} gains {resource.Amount} of {resource.Item.Name}");
        }

        // Add status to Enemies
        foreach (var holder in StatusToAddToEnemies)
        {
            foreach (var target in targets)
            {
                target.AddStatus(holder.Item);

                battleLogger.Log($"{user.GetName()} applies {holder.Item.Description} to {target.GetName()}");
            }
        }

    }

    protected override string GetInternalDescription(IBattleCharacter user)
    {
        var finalAmount = DamageSource.GetDiceAmount(user.GetDiceManager());
        var criticalDicesAmount = user.GetDiceManager().GetCriticalDices();

        if (criticalDicesAmount == 0)
        {
            return string.Format(BaseDescription, $"{finalAmount}d{DamageSource.MaxDiceValue}");
        }
        else
        {
            return string.Format(BaseDescription, $"{finalAmount}d{DamageSource.MaxDiceValue} +{criticalDicesAmount} critical chance");
        }
    }
}
