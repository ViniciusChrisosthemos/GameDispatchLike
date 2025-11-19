using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAction
{
    public BattleCharacter Source;
    public BaseSkillSO Skill;
    public List<BattleCharacter> Targets;

    public SkillAction(BattleCharacter source, BaseSkillSO skill, List<BattleCharacter> targets)
    {
        Source = source;
        Skill = skill;
        Targets = targets;
    }
}
