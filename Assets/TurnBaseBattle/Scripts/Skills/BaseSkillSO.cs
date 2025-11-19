using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SkillTargetType
{
    Enemy,
    Ally
}

public enum SkillTargetAmount
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
    public SkillTargetAmount SkillTargetAmount;
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

    protected abstract string GetInternalDescription();
    public abstract void ApplySkill(IBattleCharacter user, List<IBattleCharacter> targets);

    public Sprite GetSprite()
    {
        return Art;
    }
}
