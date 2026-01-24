using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionResult
{
    public BattleCharacter Source;
    public BaseSkillSO Skill;
    public List<BattleCharacter> Targets;
    public bool IsBattleEnded = false;

    public SkillActionResult(BattleCharacter source, BaseSkillSO skill, List<BattleCharacter> targets, bool isBattleEnded)
    {
        Source = source;
        Skill = skill;
        Targets = targets;
        IsBattleEnded = isBattleEnded;
    }
}
