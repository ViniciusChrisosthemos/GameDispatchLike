using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeywordValue 
{
    Warrior,
    Mage,
    Cleric,
    Archer,
    Barbarian
}

[CreateAssetMenu(fileName = "Keyword_", menuName = "ScriptableObjects/Character/Keyword")]
public class KeywordSO : ScriptableObject
{
    public string Name;
    public KeywordValue KeywordValue;
    public Sprite Art;
}
