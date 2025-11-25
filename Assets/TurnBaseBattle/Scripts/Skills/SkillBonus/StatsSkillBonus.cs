using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsSkillBonus_", menuName = "TurnBaseBattle/Skills/SkillBonus/Stats")]
public class StatsSkillBonus : AbstractSkillBonus
{
    [SerializeField] private StatManager.StatType _statsType;
    [SerializeField] private int _bonus;

    public override void AddBonus(IBattleCharacter target)
    {
        var statManager = target.GetStatManager();

        statManager.AddBonusToStat(_statsType, _bonus);
    }

    public override void RmvBonus(IBattleCharacter target)
    {
        var statManager = target.GetStatManager();

        statManager.RmvBonusToStat(_statsType, _bonus);
    }
}
