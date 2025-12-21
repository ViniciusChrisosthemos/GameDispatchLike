using System;
using UnityEngine;

public interface IDamageSource
{
    public int GetDamage();
    public int GetDamage(DiceManager diceManager);
}
