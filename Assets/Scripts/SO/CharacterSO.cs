using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Character")]
public class CharacterSO: ScriptableObject
{
    public string Name;
    public StatManager BaseStats;
    public Sprite FaceArt;
    public Sprite BodyArt;
    public Sprite FullArt;
    public LevelProgression LevelProgression;
    public int TimeToRest;
}
