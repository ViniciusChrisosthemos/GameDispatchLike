using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillStatusRuntime
{
    public AbstractSkillStatus StatusSO;
    public int RemainingDuration;
    public IBattleCharacter Owner;

    public SkillStatusRuntime(AbstractSkillStatus statusSO, int duration, IBattleCharacter owner)
    {
        StatusSO = statusSO;
        RemainingDuration = duration;
        Owner = owner;
    }
}
