using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatManager
{
    private const string EXPCETION_MESSAGE_STAT_NOT_FOUND = "Stat '{0}' not found in method call '{1}'";

    public enum StatType
    {
        Strengh,
        Endurance,
        Agility,
        Charisma,
        Intelligence
    }

    [SerializeField] private Stat _strengh;
    [SerializeField] private Stat _endurance;
    [SerializeField] private Stat _agility;
    [SerializeField] private Stat _charisma;
    [SerializeField] private Stat _intelligence;

    [SerializeField] private int _maxStatValue = 10;

    public StatManager()
    {
        _strengh = new Stat(0, new List<int>());
        _endurance = new Stat(0, new List<int>());
        _agility = new Stat(0, new List<int>());
        _charisma = new Stat(0, new List<int>());
        _intelligence = new Stat(0, new List<int>());
    }

    public StatManager(StatManager statManager)
    {
        _strengh = new Stat(statManager._strengh);
        _endurance = new Stat(statManager._endurance);
        _agility = new Stat(statManager._agility);
        _charisma = new Stat(statManager._charisma);
        _intelligence = new Stat(statManager._intelligence);
    }

    public Stat GetStat(StatType statType)
    {
        return statType switch
        {
            StatType.Strengh => _strengh,
            StatType.Endurance => _endurance,
            StatType.Agility => _agility,
            StatType.Charisma => _charisma,
            StatType.Intelligence => _intelligence,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, string.Format(EXPCETION_MESSAGE_STAT_NOT_FOUND, statType, "GetStat"))
        };
    }

    public void ExpendStats(StatManager stats)
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            var currentValue = GetStat(statType).BaseValue;
            var currentBonus = GetStat(statType).Bonus;

            var statToAddValue = stats.GetStat(statType).BaseValue;
            var statToAddBonus = stats.GetStat(statType).Bonus;

            var newBaseValue = currentValue + statToAddValue;
            var newBonus = new List<int>();
            newBonus.AddRange(currentBonus);
            newBonus.AddRange(statToAddBonus);

            var newStat = new Stat(newBaseValue, newBonus);
            
            SetStat(statType, newStat);
        }
    }

    public void SetStat(StatType statType, Stat newStat)
    {
        switch (statType)
        {
            case StatType.Strengh: _strengh = newStat; break;
            case StatType.Endurance: _endurance = newStat; break;
            case StatType.Agility: _agility = newStat; break;
            case StatType.Charisma: _charisma = newStat; break;
            case StatType.Intelligence: _intelligence = newStat; break;
            default: throw new ArgumentOutOfRangeException(nameof(statType), statType, string.Format(EXPCETION_MESSAGE_STAT_NOT_FOUND, statType, "SetStat"));
        }
    }

    public List<float> GetValues()
    {
        var order = new List<StatType>() { StatType.Strengh, StatType.Endurance, StatType.Agility, StatType.Charisma, StatType.Intelligence };
        
        var values = new List<float>();
        order.ForEach(stat => values.Add(GetStat(stat).GetValue() / (float)_maxStatValue));

        return values;
    }

    public void AddBonusToStat(StatType statType, int bonus)
    {
        var stat = GetStat(statType);

        stat.AddBonus(bonus);
    }

    public void RmvBonusToStat(StatType statType, int bonus)
    {
        var stat = GetStat(statType);

        stat.RmvBonus(bonus);
    }


    public override string ToString()
    {
        return $"S:{_strengh.GetValue()};E:{_endurance.GetValue()};A:{_agility.GetValue()};C:{_charisma.GetValue()};I:{_intelligence.GetValue()}";
    }
}