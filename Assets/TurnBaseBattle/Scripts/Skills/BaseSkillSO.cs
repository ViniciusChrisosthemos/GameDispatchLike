using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    [Header("SFX Voice Line")]
    [SerializeField] private List<AudioClip> _sfxVoiceLines;

    [Header("Data")]
    public SkillDataSO DataSO;

    public string GetDescription(IBattleCharacter user)
    {
        var finalString = string.Empty;

        finalString += GetInternalDescription(user);

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

    public AudioClip GetVoiceLine()
    {
        return _sfxVoiceLines.GetRandomValue();
    }

    public bool HasVoiceLine() => _sfxVoiceLines.Count != 0;

    public Sprite GetSprite()
    {
        return Art;
    }

    public void ApplySkill(IBattleCharacter user, List<IBattleCharacter> targets, BattleLogger battleLogger)
    {
        ApplySkillInternal(user, targets, battleLogger);
    }

    protected abstract string GetInternalDescription(IBattleCharacter user);
    
    protected abstract void ApplySkillInternal(IBattleCharacter user, List<IBattleCharacter> targets, BattleLogger battleLogger);
}
