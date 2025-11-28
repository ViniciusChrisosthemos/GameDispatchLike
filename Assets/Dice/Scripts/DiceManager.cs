using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceType
{
    Skill,
    Damage,
    Critical
}

public class DiceManager
{
    private DiceController _skillDicePrefab;
    private Dictionary<DiceType, DiceInfo> _dices;

    public DiceManager(DiceController skillDicePrefab, int skillDices, int damageDices, int criticalDices)
    {
        _skillDicePrefab = skillDicePrefab;

        _dices = new Dictionary<DiceType, DiceInfo>
        {
            { DiceType.Skill, new DiceInfo(DiceType.Skill, new Stat(skillDices, new List<int>())) },
            { DiceType.Damage, new DiceInfo(DiceType.Damage, new Stat(damageDices, new List<int>())) },
            { DiceType.Critical, new DiceInfo(DiceType.Critical, new Stat(criticalDices, new List < int >())) }
        };
    }

    public int GetSkillDices() => _dices[DiceType.Skill].Stat.GetValue();

    public int GetDamageDices() => _dices[DiceType.Damage].Stat.GetValue();

    public int GetCriticalDices() => _dices[DiceType.Critical].Stat.GetValue();

    public void AddDices(DiceType type, int amount)
    {
        if (_dices.ContainsKey(type))
        {
            _dices[type].AddAmount(amount);
        }
    }

    public void RmvDices(DiceType type, int amount)
    {
        if (_dices.ContainsKey(type))
        {
            _dices[type].RmvAmount(amount);
        }
    }

    public DiceController GetSkillDicePrefab()
    {
        return _skillDicePrefab;
    }

    private class DiceInfo
    {
        public DiceType Type;
        public Stat Stat;

        public DiceInfo(DiceType type, Stat stat)
        {
            Type = type;
            Stat = stat;
        }

        public void AddAmount(int amount)
        {
            Stat.AddBonus(amount);
        }

        public void RmvAmount(int amount)
        {
            Stat.RmvBonus(amount);
        }
    }
}
