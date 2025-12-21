using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterArtType
{
    Face,
    Body,
    FullBody,
    MissionCompleted,
    Icon,
    BattleScreen1,
    BattleScreen2,
    BattleScreen3,
    BattleScreen4
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
    public Sprite BattleScreen1Art;
    public Sprite BattleScreen2Art;
    public Sprite BattleScreen3Art;
    public Sprite BattleScreen4Art;
    public LevelProgression LevelProgression;
    public int TimeToRest;
    public List<AbstractKeywordSO> Keywords;
    public List<BaseSkillSO> Skills;
    public float BaseMoveSpeed;

    [Header("Dices")]
    public DiceController SkillDicePrefab;
    public int BaseSkillDicesAmount;
    public int BaseDamageDicesAmount;
    public int BaseCriticalDicesAmount;

    [Header("Individuality")]
    public AbstractIndividuality Individuality;

    [Header("SFX Voice Lines")]
    public List<AudioClip> CharacterTurnVoiceLines;

    public Sprite GetArt(CharacterArtType type)
    {
        switch (type)
        {
            case CharacterArtType.Face: return FaceArt;
            case CharacterArtType.Body: return BodyArt;
            case CharacterArtType.FullBody: return FullArt;
            case CharacterArtType.MissionCompleted: return MissionCompletedArt;
            case CharacterArtType.Icon: return IconArt;
            case CharacterArtType.BattleScreen1: return BattleScreen1Art;
            case CharacterArtType.BattleScreen2: return BattleScreen2Art;
            case CharacterArtType.BattleScreen3: return BattleScreen3Art;
            case CharacterArtType.BattleScreen4: return BattleScreen4Art;
            default: return FaceArt;
        }
    }

    public bool HasTurnVoiceLines() => CharacterTurnVoiceLines.Count != 0;

    public AudioClip GetTurnVoiceLine() => CharacterTurnVoiceLines.GetRandomValue();
}
