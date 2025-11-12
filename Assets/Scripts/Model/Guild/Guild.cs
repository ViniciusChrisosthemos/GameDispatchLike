using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Guild
{
    private string _name;
    private int _balance;
    private int _reputation;
    private int _currentLevel;
    private int _currentExperience;
    private List<CharacterUnit> _allCharacters;

    public Guild(string name, int balance, int popularity, int currentLevel, int currentExperience, List<CharacterUnit> characters)
    {
        _name = name;
        _balance = balance;
        _reputation = popularity;
        _currentLevel = currentLevel;
        _currentExperience = currentExperience;
        _allCharacters = characters;
    }

    public void SetScheduledCharacter(CharacterUnit character, bool isScheduled)
    {
        character.SetScheduledCharater(isScheduled);
    }

    public void AddGold(int gold)
    {
        _balance += gold;
    }

    public void AddReputation(int reputation)
    {
        _reputation += reputation;
    }

    public void RmvReputation(int reputation)
    {
        _reputation -= reputation;
    }

    public LevelUPDescription HandleDayReport(DayReport dayReport, CharacterLevelDatabase levelDatabase)
    {
        var levelSO = levelDatabase.GetLevel(CurrentLevel);
        var oldExpPerc = _currentExperience / (float)levelSO.ExpToLevelUp;

        var totalExp = dayReport.MissionSucceded;
        var expLostByFailures = dayReport.MissionFailed;
        var expLostByMisses = dayReport.MissionMisses;

        var levelGained = 0;
        var expToAdd = Mathf.Max(0, totalExp - expLostByFailures - expLostByMisses);

        _currentExperience += expToAdd;
        Debug.Log($"Call {dayReport.TotalCalls}   Success={dayReport.MissionSucceded} Fail={dayReport.MissionFailed} Misses={dayReport.MissionMisses}");
        Debug.Log($"ExpToAdd={expToAdd}  Experience{_currentExperience}  {levelSO.ExpToLevelUp}  {_currentLevel}");

        while (_currentExperience >= levelSO.ExpToLevelUp)
        {
            _currentLevel++;
            _currentExperience -= levelSO.ExpToLevelUp;

            levelSO = levelDatabase.GetLevel(_currentLevel);
            levelGained++;
        }

        var currentPerc = _currentExperience / (float)levelSO.ExpToLevelUp;
        var successPerc = dayReport.MissionSucceded / (float)levelSO.ExpToLevelUp;
        var failPerc = dayReport.MissionFailed / (float)levelSO.ExpToLevelUp;
        var missPerc = dayReport.MissionMisses / (float)levelSO.ExpToLevelUp;

        var expWithSuccesPerc = currentPerc + failPerc + missPerc;
        var expWithoutFailuresPerc = Mathf.Max(0, expWithSuccesPerc - failPerc);
        var expWithoutMissesPerc = Mathf.Max(0, expWithoutFailuresPerc - missPerc);

        Debug.Log($"Old={oldExpPerc} CurrentPerc={currentPerc} SuccessPerc={successPerc} FailPerc={failPerc} MissPerc={missPerc}");
        Debug.Log($"expS={expWithSuccesPerc}  expWithoutFail={expWithoutFailuresPerc}  ExpWithoutMiss={expWithoutMissesPerc}");

        return new LevelUPDescription(oldExpPerc, expWithSuccesPerc, expWithoutFailuresPerc, expWithoutMissesPerc, levelGained, levelSO);
    }

    public string Name => _name;
    public int Balance => _balance;
    public int Reputation => _reputation;
    public int CurrentLevel => _currentLevel;
    public int CurrentExperience => _currentExperience;
    public List<CharacterUnit> AllCharacters => _allCharacters;
    public List<CharacterUnit> AvailableCharacters => _allCharacters.Where(c => !c.IsScheduled).ToList();
    public List<CharacterUnit> ScheduledCharacters => _allCharacters.Where(c => c.IsScheduled).ToList();

    public class LevelUPDescription
    {
        public float OldExpPerc { get; }
        public float ExpWithSuccess { get; }
        public float ExpWithSuccessWithoutFailures { get; }
        public float ExpWithSuccessWithoutFailuresAndMisses { get; }
        public int LevelGained { get; }
        public PlayerLevelSO LevelSO { get; }

        public LevelUPDescription(float oldExpPerc, float expWithSuccess, float expWithSuccessWithoutFailures, float expWithSuccessWithoutFailuresAndMisses, int levelGained, PlayerLevelSO levelSO)
        {
            OldExpPerc = oldExpPerc;
            ExpWithSuccess = expWithSuccess;
            ExpWithSuccessWithoutFailures = expWithSuccessWithoutFailures;
            ExpWithSuccessWithoutFailuresAndMisses = expWithSuccessWithoutFailuresAndMisses;
            LevelGained = levelGained;
            LevelSO = levelSO;
        }
    }
}
