using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillResource_", menuName = "TurnBaseBattle/Skills/Skill Resource")]
public class SkillResourceSO : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int MaxAmount;
}



[Serializable]
public class ResourceBonus
{
    public int ResourceAmountRequired;
    public AbstractSkillBonus Bonus;
    public bool IsApplied;
}
