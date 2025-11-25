using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SkillTargetType
{
    Enemy,
    Ally,
    Self
}

public enum SkillTargetAmountType
{
    SingleTarget,
    MultiTarget,
    AllTargets
}

public abstract class BaseSkillSO : ScriptableObject, IHasSprite
{
    public string Name;
    public string BaseDescription;
    public Sprite Art;
    public List<DiceValueSO> RequiredDiceValues;
    public List<SkillBonus> Bonus;
    public SkillTargetType SkillTargetType;
    public SkillTargetAmountType SkillTargetAmount;
    public int TargetAmount;
    public TMP_SpriteAsset SpriteAsset;

    public string GetDescription()
    {
        var finalString = string.Empty;

        finalString += GetInternalDescription();

        foreach (var bonus in Bonus)
        {
            finalString += ",  ";

            foreach (var diceValue in bonus.DicesRequired)
            {
                var spriteIndex = SpriteAsset.GetSpriteIndexFromName(diceValue.Art.name);
                finalString += $"<sprite={spriteIndex}>";
            }

            if (bonus.Type == BonusType.Flat)
            {
                finalString += $"{string.Format(bonus.Description, Mathf.RoundToInt(bonus.BaseValue))}";
            }
            else
            {
                finalString += string.Format(bonus.Description, $"{Mathf.RoundToInt(bonus.BaseValue * 100)}%");
            }
        }

        return finalString;
    }
    public Sprite GetSprite()
    {
        return Art;
    }

    public void ApplySkill(IBattleCharacter user, List<IBattleCharacter> targets)
    {
        ApplySkillInternal(user, targets);
    }

    protected abstract string GetInternalDescription();
    
    protected abstract void ApplySkillInternal(IBattleCharacter user, List<IBattleCharacter> targets);
}
