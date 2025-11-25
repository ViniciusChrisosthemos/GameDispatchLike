using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterArtType
{
    Face,
    Body,
    FullBody,
    MissionCompleted,
    Icon
}

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Character/Character")]
public class CharacterSO: SavebleSO
{
    public string Name;
    public RankSO Rank;
    public Color ColorBackground;
    public StatManager BaseStats;
    public Sprite FaceArt;
    public Sprite BodyArt;
    public Sprite FullArt;
    public Sprite IconArt;
    public Sprite MissionCompletedArt;
    public LevelProgression LevelProgression;
    public int TimeToRest;
    public List<AbstractKeywordSO> Keywords;
    public List<BaseSkillSO> Skills;
    public float BaseMoveSpeed;

    [Header("Dices")]
    public int BaseSkillDicesAmount;
    public int BaseDamageDicesAmount;
    public int BaseCriticalDicesAmount;

    [Header("Individuality")]
    public AbstractIndividuality Individuality;
    public AbstractIndividualityView IndividualityView;
}
