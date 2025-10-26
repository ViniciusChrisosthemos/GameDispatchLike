using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterUnit
{
    public enum CharacterStatus { Available, InMission, Resting  } 

    private CharacterSO _baseCharacter;
    private StatManager _statManager;
    private CharacterStatus _status;
    private int _currentXP;
    private int _expToLevelUp;
    private int _currentLevel;
    private int _availablePoints;

    public UnityEvent<CharacterUnit> OnCharacterInAvailable = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterInMission = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterInResting = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterLevelUP = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterEXPChanged = new UnityEvent<CharacterUnit>();
    public UnityEvent<CharacterUnit> OnCharacterStatChanged = new UnityEvent<CharacterUnit>();

    private float _startTime;

    public CharacterUnit(CharacterSO baseCharacter)
    {
        _baseCharacter = baseCharacter;

        _status = CharacterStatus.Available;
        _statManager = new StatManager(baseCharacter.BaseStats);

        _currentLevel = 1;
        _currentXP = _availablePoints = 0;

        _expToLevelUp = _baseCharacter.LevelProgression.GetXPForLevel(_currentLevel);
    }

    public string Name => _baseCharacter.Name;
    public Sprite FaceArt => _baseCharacter.FaceArt;
    public Sprite BodyArt => _baseCharacter.BodyArt;
    public Sprite FullArt => _baseCharacter.FullArt;

    public StatManager StatManager { get { return _statManager; } }

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

    public void HandleMissionCompleted(int exp, float currentTime)
    {
        _status = CharacterStatus.Resting;

        OnCharacterInResting?.Invoke(this);

        _startTime = currentTime;

        AddExp(exp);
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

    public bool IsInMission() => _status == CharacterStatus.InMission;
    public bool IsResting() => _status == CharacterStatus.Resting;
    public bool IsAvailable() => _status == CharacterStatus.Available;

    public CharacterStatus Status { get { return _status; } }
    
    public float NormalizedExp => _currentXP / (float)_expToLevelUp;
    public int Level => _currentLevel;
    public int AvailablePoints => _availablePoints;
}
