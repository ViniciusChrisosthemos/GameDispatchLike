using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Stat(Stat stat)
    {
        BaseValue = stat.BaseValue;
        Bonus = new List<int>(stat.Bonus);
    }

    public int GetValue()
    {
        var value = BaseValue;

        Bonus.ForEach(x => value += x);

        return value;
    }

    public void AddBonus(int bonus)
    {
        Bonus.Add(bonus);
    }

    public void RmvBonus(int bonus)
    {
        Bonus.Remove(bonus);
    }
}
