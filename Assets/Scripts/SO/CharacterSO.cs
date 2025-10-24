using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Character")]
public class CharacterSO: ScriptableObject
{
    public string Name;
    public StatManager BaseStats;
    public Sprite Sprite;
    public LevelProgression LevelProgression;
}
