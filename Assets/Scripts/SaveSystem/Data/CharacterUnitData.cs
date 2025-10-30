using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterUnitData
{
    public string CharacterID;
    public int CurrentXP;
    public int CurrentLevel;
    public int AvailablePoints;
    public bool IsScheduled;
    public StatManagerData StatManagerData;

    public CharacterUnitData(CharacterUnit characterUnit)
    {
        CharacterID = characterUnit.BaseCharacterSO.ID;
        CurrentXP = characterUnit.CurrentXP;
        CurrentLevel = characterUnit.Level;
        AvailablePoints = characterUnit.AvailablePoints;
        IsScheduled = characterUnit.IsScheduled;
        StatManagerData = new StatManagerData(characterUnit.StatManager);
    }

    public CharacterUnitData() { }

    public override string ToString()
    {
        return $"{CharacterID};{CurrentXP};{CurrentLevel};{AvailablePoints};{IsScheduled}";
    }
}
