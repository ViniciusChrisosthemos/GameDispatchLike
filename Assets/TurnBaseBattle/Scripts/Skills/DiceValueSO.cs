
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiceValyeType { Attack, Heal, Magic };

[CreateAssetMenu(fileName = "DiceValue_", menuName = "ScriptableObjects/TurnBasedBattle/Dice Value")]
public class DiceValueSO : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Art;
    public DiceValyeType Type;
}
