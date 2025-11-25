
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DiceValue_", menuName = "ScriptableObjects/TurnBasedBattle/Dice Value")]
public class DiceValueSO : ScriptableObject, IHasSprite
{
    public string Name;
    public string Description;
    public Sprite Art;
    public int Priority;

    public Sprite GetSprite()
    {
        return Art;
    }
}
