using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatManagerData
{
    public StatData Strength;
    public StatData Endurance;
    public StatData Agility;
    public StatData Charisma;
    public StatData Intelligence;
    
    public StatManagerData(StatManager statManager)
    {
        Strength = new StatData(statManager.GetStat(StatManager.StatType.Strengh));
        Endurance = new StatData(statManager.GetStat(StatManager.StatType.Endurance));
        Agility = new StatData(statManager.GetStat(StatManager.StatType.Agility));
        Charisma = new StatData(statManager.GetStat(StatManager.StatType.Charisma));
        Intelligence = new StatData(statManager.GetStat(StatManager.StatType.Intelligence));
    }

    public StatManagerData() { }
}
