using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterUnit
{
    public enum CharacterStatus { Available, Going, InMission, Returning, Resting  } 

    private CharacterSO _baseCharacter;
    private StatManager _statManager;
    private CharacterStatus _status;
    private int _currentXP;
    private int _expToLevelUp;
    private int _currentLevel;
    private int _availablePoints;
    private bool _isScheduled;
    private DiceManager _diceManager;

    public UnityEvent<CharacterUnit> OnCharacterInAvailable = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterInMission = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterInResting = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterLevelUP = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterEXPChanged = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterStatChanged = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterGoingToMission = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterReturning = new UnityEvent<CharacterUnit>();

    private float _startTime;

    public CharacterUnit(CharacterSO baseCharacter)
    {
        _baseCharacter = baseCharacter;

        _status = CharacterStatus.Available;
        _statManager = new StatManager(baseCharacter.BaseStats);

        _currentLevel = 1;
        _currentXP = _availablePoints = 0;

        _expToLevelUp = _baseCharacter.LevelProgression.GetXPForLevel(_currentLevel);
        _isScheduled = false;

        _diceManager = new DiceManager(baseCharacter.SkillDicePrefab, baseCharacter.BaseSkillDicesAmount, baseCharacter.BaseDamageDicesAmount, baseCharacter.BaseCriticalDicesAmount);
    }

    public CharacterUnit(CharacterSO baseCharacter, int currentLevel, int currentXP, int availablePoints, StatManager statManager, bool isScheduled)
    {
        _baseCharacter = baseCharacter;

        _status = CharacterStatus.Available;
        _statManager = statManager;

        _currentLevel = currentLevel;
        _currentXP = currentXP;
        _availablePoints = availablePoints;

        _expToLevelUp = _baseCharacter.LevelProgression.GetXPForLevel(currentLevel);
        _isScheduled = isScheduled;

        _diceManager = new DiceManager(baseCharacter.SkillDicePrefab, baseCharacter.BaseSkillDicesAmount, baseCharacter.BaseDamageDicesAmount, baseCharacter.BaseCriticalDicesAmount);
    }

    public void SetStatusToAvailable()
    {
        _status = CharacterStatus.Available;

        OnCharacterInAvailable?.Invoke(this);
    }

    public void SetStatusToInMission()
    {
        _status = CharacterStatus.InMission;

        OnCharacterInMission?.Invoke(this);
    }

    public void SetStatusToInGoingToMission()
    {
        _status = CharacterStatus.Going;

        OnCharacterGoingToMission?.Invoke(this);
    }

    public void SetStatusToReturning()
    {
        _status = CharacterStatus.Returning;

        OnCharacterReturning?.Invoke(this);
    }

    public void HandleMissionCompleted(int exp, float currentTime)
    {
        _status = CharacterStatus.Returning;
        AddExp(exp);

        OnCharacterReturning?.Invoke(this);
    }

    public void SetCharacterResting(float currentTime)
    {
        _startTime = currentTime;
        _status = CharacterStatus.Resting;

        OnCharacterInResting?.Invoke(this);
    }

    public void AddPointInStat(StatManager.StatType stat)
    {
        if (_availablePoints == 0) return;

        _statManager.AddBonusToStat(stat, 1);

        _availablePoints = Math.Max(0, _availablePoints - 1);

        OnCharacterStatChanged?.Invoke(this);
    }


    public void UpdateCharacter(float currentTime)
    {
        if (IsResting())
        {
            if (currentTime - _startTime > _baseCharacter.TimeToRest)
            {
                SetStatusToAvailable();
            }
        }
    }

    public float GetNormalizedTimeToBeAvailable(float currentTime)
    {
        var timeDiff = currentTime - _startTime;

        switch (_status)
        {
            case CharacterStatus.Resting: return timeDiff / _baseCharacter.TimeToRest;
            default : return 0f;
        }
    }

    public void AddExp(int exp)
    {
        _currentXP += exp;

        if (_currentXP >= _expToLevelUp)
        {
            while (_currentXP >= _expToLevelUp)
            {
                _currentXP -= _expToLevelUp;
                
                _currentLevel += 1;
                _availablePoints += 1;

                _expToLevelUp = _baseCharacter.LevelProgression.GetXPForLevel(_currentLevel);
            }

            OnCharacterLevelUP?.Invoke(this);
        }

        OnCharacterEXPChanged?.Invoke(this);
    }

    public void SetScheduledCharater(bool isScheduled)
    {
        _isScheduled = isScheduled;
    }

    public Sprite GetArt(CharacterArtType characterArtType)
    {
        switch(characterArtType)
        {
            case CharacterArtType.Face: return FaceArt;
            case CharacterArtType.Body: return BodyArt;
            case CharacterArtType.FullBody: return FullArt;
            case CharacterArtType.MissionCompleted: return _baseCharacter.MissionCompletedArt;
            case CharacterArtType.Icon: return _baseCharacter.IconArt;
            default: return FaceArt;
        }
    }

    public List<AbstractKeywordSO> GetKeywords()
    {
        return _baseCharacter.Keywords;
    }

    public CharacterStatus Status { get { return _status; } }

    public bool IsInMission() => _status == CharacterStatus.InMission;
    public bool IsResting() => _status == CharacterStatus.Resting;
    public bool IsAvailable() => _status == CharacterStatus.Available;

    public float NormalizedExp => _currentXP / (float)_expToLevelUp;
    public int Level => _currentLevel;
    public int AvailablePoints => _availablePoints;
    public int CurrentXP => _currentXP;
    public RankSO Rank => _baseCharacter.Rank;
    public Color HeroBackgroundColor => _baseCharacter.ColorBackground;
    public string Name => _baseCharacter.Name;
    public Sprite FaceArt => _baseCharacter.FaceArt;
    public Sprite BodyArt => _baseCharacter.BodyArt;
    public Sprite FullArt => _baseCharacter.FullArt;
    public bool IsScheduled => _isScheduled;
    public CharacterSO BaseCharacterSO => _baseCharacter;
    public StatManager StatManager { get { return _statManager; } }
    public DiceManager DiceManager { get { return _diceManager; } }
}
