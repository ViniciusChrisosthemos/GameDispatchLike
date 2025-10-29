using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatData
{
    public int BaseValue;
    public List<int> Bonus;

    public StatData(Stat stat)
    {
        BaseValue = stat.BaseValue;
        Bonus = stat.Bonus;
    }
}
