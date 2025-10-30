using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatManager;

public class FactoryCharacterUnit
{
    private CharacterDatabase _characterDatabase;

    public FactoryCharacterUnit(CharacterDatabase characterDatabase)
    {
        _characterDatabase = characterDatabase;
    }

    public CharacterUnit CreateCharacterUnit(CharacterUnitData data)
    {
        var baseCharacterSO = _characterDatabase.GetCharacterSO(data.CharacterID);
        var currentXP = data.CurrentXP;
        var currentLevel = data.CurrentLevel;
        var isScheduled = data.IsScheduled;

        var statManager = new StatManager();

        statManager.SetStat(StatType.Strengh, new Stat(data.StatManagerData.Strength.BaseValue, data.StatManagerData.Strength.Bonus));
        statManager.SetStat(StatType.Endurance, new Stat(data.StatManagerData.Endurance.BaseValue, data.StatManagerData.Endurance.Bonus));
        statManager.SetStat(StatType.Agility, new Stat(data.StatManagerData.Agility.BaseValue, data.StatManagerData.Agility.Bonus));
        statManager.SetStat(StatType.Charisma, new Stat(data.StatManagerData.Charisma.BaseValue, data.StatManagerData.Charisma.Bonus));
        statManager.SetStat(StatType.Intelligence, new Stat(data.StatManagerData.Intelligence.BaseValue, data.StatManagerData.Intelligence.Bonus));

        return new CharacterUnit(baseCharacterSO, currentXP, currentLevel, statManager, isScheduled);
    }

    public CharacterUnit CreateCharacterUnit(CharacterSO character)
    {
        return new CharacterUnit(character);
    }
}
