using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int BaseValue;
    public List<int> Bonus;

    public Stat(int baseValue, List<int> bonus)
    {
        BaseValue = baseValue;
        Bonus = bonus;
    }
}
