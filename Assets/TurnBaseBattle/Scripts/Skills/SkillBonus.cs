using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusType { Perc, Flat };

[Serializable]
public class SkillBonus
{
    public string Name;
    public string Description;
    public float BaseValue;
    public BonusType Type;
    public List<DiceValueSO> DicesRequired; 

    public string GetDescription()
    {
        if (Type == BonusType.Perc) return string.Format(Description, $"{BaseValue:2F}%");
        
        return string.Format(Description, BaseValue);
    }
}
