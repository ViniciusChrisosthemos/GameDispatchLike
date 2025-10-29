using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterUnitData
{
    public string ID;
    public int CurrentXP;
    public int CurrentLevel;
    public int AvailablePoints;
    public StatManagerData StatManagerData;

    public CharacterUnitData(CharacterUnit characterUnit)
    {
        ID = characterUnit.BaseCharacterSO.ID;
        CurrentXP = characterUnit.CurrentXP;
        CurrentLevel = characterUnit.Level;
        AvailablePoints = characterUnit.AvailablePoints;
        StatManagerData = new StatManagerData(characterUnit.StatManager);
    }

    public CharacterUnitData() { }
}
