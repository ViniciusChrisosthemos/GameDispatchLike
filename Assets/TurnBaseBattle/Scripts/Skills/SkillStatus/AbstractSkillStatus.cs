using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSkillStatus : ScriptableObject
{
    public Sprite Icon;
    public abstract void OnApply(SkillStatusRuntime statusRuntime);
    public abstract void OnTurnStart(SkillStatusRuntime statusRuntime);
    public abstract void OnTurnEnd(SkillStatusRuntime statusRuntime);
    public abstract void OnRemove(SkillStatusRuntime statusRuntime);
}
