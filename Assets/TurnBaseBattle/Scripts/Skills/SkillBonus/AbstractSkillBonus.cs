using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AbstractSkillBonus : ScriptableObject
{
    public abstract void AddBonus(IBattleCharacter target);
    public abstract void RmvBonus(IBattleCharacter target);
}
