using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class DiceDamageSource : IDamageSource
{
    public int DiceAmount;
    public int MinDiceValue;
    public int MaxDiceValue;

    public List<int> DiceValues = new List<int>();

    public int GetDamage()
    {
        return InternalGetDamage(DiceAmount, MinDiceValue, MaxDiceValue);
    }

    public int GetDamage(DiceManager diceManager)
    {
        return InternalGetDamage(GetDiceAmount(diceManager), MinDiceValue, MaxDiceValue);
    }

    private int InternalGetDamage(int amount, int minDiceValue, int minMaxDiceValue)
    {
        var result = 0;
        DiceValues = new List<int>();

        for (int i = 0; i < amount; i++)
        {
            var diceValue = Random.Range(MinDiceValue, MaxDiceValue);

            DiceValues.Add(diceValue);

            result += diceValue;
        }

        return result;
    }

    public int GetDiceAmount(DiceManager diceManager)
    {
        return DiceAmount + diceManager.GetDamageDices();
    }
}
