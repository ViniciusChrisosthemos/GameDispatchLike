using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterArtType
{
    Face,
    Body,
    FullBody
}

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Character/Character")]
public class CharacterSO: SavebleSO
{
    public string Name;
    public StatManager BaseStats;
    public Sprite FaceArt;
    public Sprite BodyArt;
    public Sprite FullArt;
    public Sprite MissionCompletedArt;
    public LevelProgression LevelProgression;
    public int TimeToRest;
    public List<KeywordSO> Keywords;
    public List<BaseSkillSO> Skills;
}
