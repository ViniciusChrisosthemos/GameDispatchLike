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

        var totalExp = dayReport.TotalCalls;
        var expLostByFailures = dayReport.MissionFailed;
        var expLostByMisses = dayReport.MissionMisses;

        var levelGained = 0;
        var expToAdd = totalExp - expLostByFailures - expLostByMisses;

        _currentExperience += expToAdd;
        Debug.Log($"ExpToAdd={expToAdd}  Experience{_currentExperience}  {levelSO.ExpToLevelUp}  {_currentLevel}");

        Debug.Log($"Current Level = {levelSO.LevelDescription}");

        while (_currentExperience >= levelSO.ExpToLevelUp)
        {
            _currentLevel++;
            _currentExperience -= levelSO.ExpToLevelUp;

            levelSO = levelDatabase.GetLevel(_currentLevel);
            levelGained++;

            Debug.Log($"New Level {levelSO.LevelDescription}");
        }

        Debug.Log($"New Level = {levelSO.LevelDescription}");

        var expLostByFailuresPerc = expLostByFailures / (float)levelSO.ExpToLevelUp;
        var expLostByMissesPerc = expLostByMisses / (float)levelSO.ExpToLevelUp;
        var totalExpPerc = Mathf.Min(_currentExperience / (float)levelSO.ExpToLevelUp + expLostByMissesPerc + expLostByFailuresPerc, 1f);


        Debug.Log($"{levelSO.ExpToLevelUp}");
        Debug.Log($"{expLostByFailures} {expLostByFailuresPerc}");
        Debug.Log($"{expLostByMisses} {expLostByMissesPerc}");
        Debug.Log($"{totalExpPerc}");

        return new LevelUPDescription(oldExpPerc, totalExpPerc, expLostByFailuresPerc, expLostByMissesPerc, levelGained, levelSO);
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
        public float TotalExpPerc { get; }
        public float ExpLostByFailures { get; }
        public float ExpLostByMissesPerc { get; }
        public int LevelGained { get; }
        public PlayerLevelSO LevelSO { get; }

        public LevelUPDescription(float oldExpPerc, float totalExpPerc, float expLostByFailuresPerc, float expLostByMissesPerc, int levelGained, PlayerLevelSO levelSO)
        {
            OldExpPerc = oldExpPerc;
            TotalExpPerc = totalExpPerc;
            ExpLostByFailures = expLostByFailuresPerc;
            ExpLostByMissesPerc = expLostByMissesPerc;
            LevelGained = levelGained;
            LevelSO = levelSO;
        }
    }
}
